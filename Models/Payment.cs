using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace Contribute.Models;

[Index(nameof(SessionId), IsUnique = true)]
public class Payment
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public required string SessionId { get; set; }

    [MaxLength(50)]
    public string? Reference { get; set; }

    public int Amount { get; set; }

    public int Fee { get; set; }

    [MaxLength(100)]
    public string? TransactionRef { get; set; }

    public DateTime Timestamp { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }
}