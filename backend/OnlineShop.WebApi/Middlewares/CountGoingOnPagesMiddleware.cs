using System.Collections.Concurrent;

namespace OnlineShop.WebApi.Middlewares
{
    public class CountGoingOnPagesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ConcurrentDictionary<string, int> _transition = new();

        public CountGoingOnPagesMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var requestPath = context.Request.Path.ToString();
            if (requestPath.EndsWith("/metrics"))
            {
                await context.Response.WriteAsJsonAsync(_transition);
            }
            else
            {
                _transition.AddOrUpdate(context.Request.Path, //метод ConcurrentDictionary
                    _ => 1, //добавить
                    (_, currentCount) => currentCount + 1 //изменить
                );
                await _next(context);
            }
        }
    }
}