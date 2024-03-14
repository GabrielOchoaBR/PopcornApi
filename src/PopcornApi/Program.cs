using Application;
using Application.Validations;
using Infrastructure;
using Infrastructure.Context;
using Infrastructure.Logging;
using PopcornApi.Configuration;
using PopcornApi.Middlewares;
using PopcornApi.Model.Settings;

var builder = WebApplication.CreateBuilder(args);

AppSettings appSettings = AppSettingsConfiguration.GetSettings();

builder.Services.AddInfrastructureConfiguration(new AppDbContextConfiguration()
{
    ConnectionString = appSettings.DbContext.ConnectionString,
    DatabaseName = appSettings.DbContext.DatabaseName
},
    new CustomLoggerProviderConfig()
    {
        LogLevel = appSettings.Logger.LogLevel,
        EventId = appSettings.Logger.EventId,
        FileName = appSettings.Logger.FileName
    });
builder.Services.AddApplicationConfiguration(new PasswordValidation()
{
    RegexPattern = appSettings.UserConfiguration.PasswordValidation,
    ErrorMessage = appSettings.UserConfiguration.PasswordValidationMessage

});
builder.Services.AddPopcornApiConfiguration(appSettings.Authentication);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();
app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization();
app.MapGraphQL("/graphql");

app.Run();
