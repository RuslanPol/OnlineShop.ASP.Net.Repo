namespace OnlineShop.Models.Responses
    // ReSharper disable once ArrangeNamespaceBody
{
    public record LogInResponse(string AccountName, string AccountEmail, string Token);
}