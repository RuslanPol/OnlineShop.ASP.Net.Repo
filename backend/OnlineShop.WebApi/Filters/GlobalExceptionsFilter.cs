using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineShop.Domain.Services;

namespace OnlineShop.WebApi.Filters;

public  class GlobalExceptionsFilter : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var message = TryUserMessageFromExeptions(context);
        if (message != null)
        {
            context.Result = new ObjectResult(new ErrorResponse(message));
            context.ExceptionHandled = true;
        }
    }

    private string? TryUserMessageFromExeptions(ExceptionContext context)
    {
        return context.Exception switch
        {
            EmailNotFoundExeption => "Учетная запись с таким Email не найдена",
            IncorrectPasswordExeption => "Неверный пароль",
            DuplicationOfAccountException=>"Такой аккаунт уже зарегистрирован",
            _ => null
        };
    }
}