using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Contribute.Models;

public class Contributor
{
    [Key]
    [MaxLength(50)]
    public required string Reference { get; set; }
    [MaxLength(100)]
    public required string Name { get; set; }
    public int Amount { get; set; }
    public bool IsAnonymous { get; set; }

    [ForeignKey(nameof(Receipient))]
    public int ReceipientId { get; set; }
    public Receipient Receipient { get; set; } = null!;
    public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
}