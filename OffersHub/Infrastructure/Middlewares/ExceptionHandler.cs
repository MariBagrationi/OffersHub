using OffersHub.Application.Exceptions.Categories;
using OffersHub.Application.Exceptions.Companies;
using OffersHub.Application.Exceptions.Offers;
using OffersHub.Application.Exceptions.Orders;
using OffersHub.Application.Exceptions.Products;
using OffersHub.Application.Exceptions.Users;

namespace OffersHub.API.Infrastructure.Middlewares
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await Handle(context, ex);
            }
        }

        public async Task Handle(HttpContext context, Exception ex)
        {

            int statusCode = ex switch
            {
                CategoryDoesNotExist => StatusCodes.Status404NotFound,
                CompanyDoesNotExist => StatusCodes.Status404NotFound,
                OrderDoesNotExist => StatusCodes.Status404NotFound,
                OfferDoesNotExist => StatusCodes.Status404NotFound,
                UserDoesNotExist => StatusCodes.Status404NotFound,

                CategoryAlreadyExists => StatusCodes.Status409Conflict,
                CompanyAlreadyExist => StatusCodes.Status409Conflict,
                OfferAlreadyExists => StatusCodes.Status409Conflict,

                CompanyIsNotAuthorized => StatusCodes.Status401Unauthorized,

                UnauthorizedException => StatusCodes.Status401Unauthorized,
                BadRequestException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            //Log.Error($"An error occurred: {ex.Message}");

            var response = new
            {
                StatusCode = statusCode,
                Details = ex.StackTrace, //for dev only
                Message = ex.Message // optional
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }


    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message) { }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
