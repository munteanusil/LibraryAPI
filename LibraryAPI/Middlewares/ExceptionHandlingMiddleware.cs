using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibraryAPI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
           _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                FormatGlobalException(context,ex);
            }
          

        }

        public async Task FormatGlobalException(HttpContext contex,Exception ex)
        {
            var traceId = Guid.NewGuid().ToString();
            contex.Response.Headers.Add("X-Trace-Id", traceId);
            var (status,titlu) = MapException(ex);
           
            var problem = new
            {
                Status = status,
                Message = titlu,
                Details = "Adresativa la administrator!"
            };

            contex.Response.ContentType = "application/json";
            var json = JsonSerializer.Serialize(problem);
            await contex.Response.WriteAsync(json); 
        }

        private (HttpStatusCode ,string title) MapException(Exception ex)
        {
            switch (ex)
            {
                case KeyNotFoundException x: return (HttpStatusCode.NotFound, " Resource not found!");
                case NullReferenceException x: return (HttpStatusCode.InternalServerError, "Somethings gone wrong!");
                case UnauthorizedAccessException x: return (HttpStatusCode.Unauthorized, "you are not autorized to make this");
                default:
                    return (HttpStatusCode.InternalServerError, "Something went wrong!");
            }
        }
        

        
    }
}
