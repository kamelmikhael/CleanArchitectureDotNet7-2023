using Api.Configurations;
using Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .InstallServices(builder.Configuration, typeof(IServiceInstaller).Assembly);

var app = builder.Build();

#region Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
