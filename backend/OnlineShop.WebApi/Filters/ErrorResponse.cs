namespace OnlineShop.WebApi.Filters;

public record ErrorResponse(string Message)
{
    public override string ToString()
    {
        return $"{{ Message={Message} }}";
    }
}