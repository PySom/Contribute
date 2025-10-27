using Contribute;
using Contribute.Requests;
using Contribute.Responses;
using Contribute.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var corsPolicyName = "AllowAllOrigins";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicyName,
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies");

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminPolicy", policy => policy.RequireClaim("IsAdmin", "true")
);


builder.Services.AddDbContext<ContributionContext>(
            dbContextOptions => dbContextOptions
                .UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.Parse("10.11.13-mariadb")
                )
        );

builder.Services.AddSingleton<SecretService>();

var secret = builder.Configuration.GetValue<string>("Secret");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseCors(corsPolicyName);
app.UseAuthorization();


app.MapGet("/contributors", async (ContributionContext db) =>
{
    return await db.Contributors
        .OrderByDescending(contributor => contributor.CreatedAt)
        .Select(contributor => ContributorResponse.FromContributor(contributor))
        .ToListAsync();
}).WithName("GetContributors").WithOpenApi();

app.MapGet("/contributors/{reference}", async (string reference, ContributionContext db) =>
{
    return await db.Contributors
        .Where(contributor => contributor.Reference == reference)
        .OrderByDescending(contributor => contributor.CreatedAt)
        .Select(contributor => ContributorResponse.FromContributor(contributor))
        .FirstOrDefaultAsync()
            is ContributorResponse contributor
                ? Results.Ok(contributor)
                : Results.NotFound();
}).WithName("GetContributorByReference").WithOpenApi();

app.MapPost("admin/contributors", async (ContributorRequest request, ContributionContext db) =>
{
    db.Contributors.Add(request.ToContributor());
    await db.SaveChangesAsync();

    return Results.Created($"/contributors/{request.Reference}", request);
}).WithName("AdminCreateContributor").WithOpenApi().RequireAuthorization("AdminPolicy");

app.MapPost("/contributors", async (ContributorRequest request, ContributionContext db) =>
{
    // naive retry mechanism to verify transaction status from Payments model
    var isVerified = false;
    for (int i = 0; i < 5; i++)
    {
        isVerified = await db.Payments
            .AnyAsync(p => p.TransactionRef == request.Reference);
        if (isVerified)
        {
            break;
        }
        await Task.Delay(2000);
    }
    if (!isVerified)
    {
        return Results.BadRequest(new { Message = "Transaction could not be verified." });
    }

    db.Contributors.Add(request.ToContributor());
    await db.SaveChangesAsync();
    // TODO: send notification to whatsapp group

    return Results.Created($"/contributors/{request.Reference}", request);
}).WithName("CreateContributor").WithOpenApi();


app.MapPost("/receipients", async (ReceipientRequest request, ContributionContext db) =>
{
    var model = request.ToReceipient();
    db.Receipients.Add(model);
    await db.SaveChangesAsync();

    var slug = model.Name.ToLower().Replace(" ", "-") + "-" + model.Id;
    model.Slug = slug;
    await db.SaveChangesAsync();

    return Results.Created($"/receipients/{{slug}}", request);
}).WithName("CreateReceipient").WithOpenApi().RequireAuthorization("AdminPolicy");

app.MapGet("/receipients", async (ContributionContext db) =>
{
    var now = DateTime.UtcNow;
    return await db.Receipients
        .Where(r => r.ExpiredAt > now)
        .OrderByDescending(r => r.CreatedAt)
        .Select(receipient => ReceipientResponse.FromReceipient(receipient))
        .ToListAsync();
}).WithName("GetReceipients").WithOpenApi();

app.MapGet("/receipients/{slug}", async (ContributionContext db, string slug) =>
{
    return await db.Receipients
        .Where(r => r.Slug!.ToLower() == slug.ToLower())
        .Include(r => r.Contributors.OrderByDescending(c => c.CreatedAt))
        .OrderByDescending(r => r.CreatedAt)
        .Select(receipient => ReceipientWithContributorResponse.FromReceipient(receipient))
        .FirstOrDefaultAsync()
            is ReceipientWithContributorResponse receipient
                ? Results.Ok(receipient)
                : Results.NotFound();
}).WithName("GetReceipientsWithSlug").WithOpenApi();

// basic authentication for admin login
app.MapPost("/login", async (LoginRequest request, ContributionContext db, SecretService secretService, HttpContext context) =>
{
    if (!secretService.VerifyHash(request.Secret, secret))
    {
        return Results.Unauthorized();
    }
    List<Claim> claims = [new("IsAdmin", "true")];
    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

    await context.SignInAsync("Cookies", claimsPrincipal);

    return Results.Ok(new { Message = "Login successful." });
}).WithName("Login").WithOpenApi();

app.MapPost("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync("Cookies");
    return Results.Ok(new { Message = "Logout successful." });
}).WithName("Logout").WithOpenApi();

app.MapPost("/webhook", async (VPayBankTransactionResponse request, ContributionContext db) =>
{
    if(string.IsNullOrEmpty(request.TransactionRef))
    {
        return Results.BadRequest();
    }
    var hasExistingPayment = await db.Payments
        .AnyAsync(p => p.TransactionRef == request.TransactionRef);
    if (hasExistingPayment) return Results.Ok();
    var model = request.ToPayment();
    db.Payments.Add(model);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();