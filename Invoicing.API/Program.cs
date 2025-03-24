using FluentValidation;
using Invoicing.API.Setup.MediatR;
using Invoicing.API.Setup.Middleware;
using Invoicing.Features.Billing;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var featureAssemblies = AppDomain.CurrentDomain
    .GetAssemblies()
    .Where(a => a.FullName != null && a.FullName.Contains(".Features.")) // Filter only feature assemblies
    .ToArray();

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblies(featureAssemblies);
})
.AddValidatorsFromAssemblies(featureAssemblies)
.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.InjectBillingDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapBillingEndpoints();
app.AddExceptionMiddleware();
app.UseHttpsRedirection();

app.Run();
