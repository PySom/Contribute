using System.Text.Json.Serialization;
using Contribute.Models;

namespace Contribute.Responses;

public class VPayBankTransactionResponse
{
    [JsonPropertyName("reference")]
    public string? Reference { get; set; }

    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }

    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    [JsonPropertyName("fee")]
    public int Fee { get; set; }

    [JsonPropertyName("account_number")]
    public string? AccountNumber { get; set; }

    [JsonPropertyName("originator_account_number")]
    public string? OriginatorAccountNumber { get; set; }

    [JsonPropertyName("originator_account_name")]
    public string? OriginatorAccountName { get; set; }

    [JsonPropertyName("originator_bank")]
    public string? OriginatorBank { get; set; }

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("transactionref")]
    public string? TransactionRef { get; set; }

    public Payment ToPayment()
    {
        return new Payment
        {
            Reference = Reference,
            SessionId = SessionId ?? string.Empty,
            Amount = Amount,
            Fee = Fee,
            Email = Email,
            TransactionRef = TransactionRef,
            Timestamp = Timestamp
        };
    }
}

