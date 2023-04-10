using Application.Behaviors;
using FluentValidation;
using Infrastructure.Contexts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.

builder.Services.Scan(selector => selector
    .FromAssemblies(
        Infrastructure.AssemblyReference.Assembly)
    .AddClasses(false)
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddMediatR(Application.AssemblyReference.Assembly);
builder.Services.AddAutoMapper(Application.AssemblyReference.Assembly);

#region Fluent Validation
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
builder.Services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly,
    includeInternalTypes: true);
#endregion

builder.Services.AddControllers()
    .AddApplicationPart(Presentation.AssemblyReference.Assembly);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

#region Swagger/OpenAPI Service
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

#region API Versioning
// Add API Versioning to the Project
builder.Services.AddApiVersioning(config =>
{
    // Specify the default API Version as 1.0
    config.DefaultApiVersion = new ApiVersion(1, 0);
    // If the client hasn't specified the API version in the request, use the default API version number 
    config.AssumeDefaultVersionWhenUnspecified = true;
    // Advertise the API versions supported for the particular endpoint
    config.ReportApiVersions = true;
});
#endregion

#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline.

#region Use Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#endregion

#region Culture Select Middleware
app.Use((context, next) =>
{
    var userLangs = context.Request.Headers["Accept-Language"].ToString();
    var lang = userLangs.Split(',').FirstOrDefault();

    //If no language header was provided, then default to english.
    if (string.IsNullOrEmpty(lang))
    {
        lang = "en";
    }

    //You could set the environment culture based on the language.
    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(lang);
    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

    //you could save the language preference for later use as well.
    context.Items["Culture"] = lang;
    context.Items["ClientCulture"] = Thread.CurrentThread.CurrentUICulture.Name;


    return next();
});
#endregion

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
