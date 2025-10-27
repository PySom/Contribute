using Contribute.Models;

namespace Contribute.Responses;
public class ContributorResponse
{
    public required string Name { get; set; }
    public int Amount { get; set; }
    public bool IsAnonymous { get; set; }

    public static ContributorResponse FromContributor(Contributor contributor)
    {
        return new ContributorResponse
        {
            Name = contributor.Name,
            Amount = contributor.Amount,
            IsAnonymous = contributor.IsAnonymous
        };
    }
}