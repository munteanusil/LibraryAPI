using System.Diagnostics;

namespace LibraryAPI.Middlewares
{
    public class RequestResponseLogginMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLogginMiddleware(RequestDelegate next)
        {
           _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            Console.WriteLine("Handling request: {0},{1}",context.Request.Method,context.Request.Path);
            var stopWatch = Stopwatch.StartNew();
            await _next(context);
            stopWatch.Stop();
            Console.WriteLine($"Finised handling request. Status code: {context.Response.StatusCode}.Time taken:{stopWatch.ElapsedMilliseconds}");

        }
    }
}
