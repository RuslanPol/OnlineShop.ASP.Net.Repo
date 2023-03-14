namespace OnlineShop.Models.Responses;

public record RegisterResponse(Guid AccountId,string AccountName, string AccountEmail, string Token);