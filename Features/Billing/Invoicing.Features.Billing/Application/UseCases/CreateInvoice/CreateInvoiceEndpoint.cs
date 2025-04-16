using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Invoicing.Features.Billing.Application.UseCases.CreateInvoice
{
    public class CreateInvoiceEndpoint
    {
        public static void MapEndpoint(WebApplication app)
        {
            app.MapPost("/api/invoices", async (
                CreateInvoiceCommand command,
                IMediator mediator) =>
            {
                var id = await mediator.Send(command);
                return Results.Created($"/api/invoices/{id}", new { id });
            })
            .WithName("CreateInvoice")
            .WithOpenApi();
        }
    }
}