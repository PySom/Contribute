using Contribute.Models;
namespace Contribute.Requests;
public class ReceipientRequest
{
    public required string Name { get; set; }
    public DateTime ExpiredAt { get; set; }

    public Receipient ToReceipient()
    {
        return new Receipient
        {
            Name = Name,
            ExpiredAt = ExpiredAt
        };
    }
}
