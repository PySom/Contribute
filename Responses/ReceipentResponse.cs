using Contribute.Models;
namespace Contribute.Responses;

public class ReceipientResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Slug { get; set; }

    public static ReceipientResponse FromReceipient(Receipient receipient)
    {
        return new ReceipientResponse
        {
            Id = receipient.Id,
            Name = receipient.Name,
            Slug = receipient.Slug
        };
    }
}

public class ReceipientWithContributorResponse
{
    public required ReceipientResponse Receipient { get; set; }
    public List<ContributorResponse> Contributors { get; set; } = [];

    public static ReceipientWithContributorResponse FromReceipient(Receipient receipient)
    {
        return new ReceipientWithContributorResponse
        {
            Receipient = ReceipientResponse.FromReceipient(receipient),
            Contributors = [.. receipient.Contributors.Select(ContributorResponse.FromContributor)]
        };
    }
}