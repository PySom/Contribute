using Contribute.Models;
using Microsoft.EntityFrameworkCore;

namespace Contribute;

public partial class ContributionContext : DbContext
{
    public ContributionContext()
    {
    }

    public ContributionContext(DbContextOptions<ContributionContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public DbSet<Contributor> Contributors { get; set; }
    public DbSet<Receipient> Receipients { get; set; }
    public DbSet<Payment> Payments { get; set; }
}
