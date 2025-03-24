using Invoicing.Core.Exceptions;

namespace Invoicing.API.Setup.Middleware
{
    public static class ExceptionMiddleware
    {
        public static void AddExceptionMiddleware(this WebApplication app)
        {
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (NotFoundException ex)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, trace = ex.StackTrace });
                }
                catch (FluentValidation.ValidationException)
                {
                    // Allow fluent validation to throw a better error
                    throw;
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message, trace = ex.StackTrace });
                }
            });
        }

    }
}