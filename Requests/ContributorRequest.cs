using Contribute.Models;

namespace Contribute.Requests;
public class ContributorRequest
{
    public required string Reference { get; set; }
    public required string Name { get; set; }
    public int Amount { get; set; }
    public int ReceipientId { get; set; }
    public bool IsAnonymous { get; set; }

    public Contributor ToContributor()
    {
        return new Contributor
        {
            Reference = Reference,
            Name = Name,
            Amount = Amount,
            ReceipientId = ReceipientId,
            IsAnonymous = IsAnonymous
        };
    }
}