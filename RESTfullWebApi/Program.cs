using CustomerRecord.Data.Data.IRepository;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RESTfullWebApi;
using RESTfullWebApi.Data;
using RESTfullWebApi.Data.IRepository;
using RESTfullWebApi.Data.Repository;
using RESTfullWebApi.Model.Dto.UserDto;
using RESTfullWebApi.Services.Services.IOperation;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = "v1",
    Title = "Weacther Forecast",
    Description = "An ASP.NET Core Web API for Weather forecast ",
    TermsOfService = new Uri("https://example.com/terms"),
    Contact = new OpenApiContact
    {
      Name = "Example Contact",
      Url = new Uri("https://example.com/contact")
    },
    License = new OpenApiLicense
    {
      Name = "Example License",
      Url = new Uri("https://example.com/license")
    }
  });
  options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
  {
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
       "Example: \"Bearer 12345abcdef\"",
  });
  options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
 }
});
});
var tokenValidationParameters = new TokenValidationParameters
{

  RequireExpirationTime = true,
  RequireSignedTokens = false,
  ValidateIssuerSigningKey = true,
  ValidateIssuer = true,
  ValidIssuer = "8d708afe-2966-40b7-918c-a39551625958",
  ValidateAudience = true,
  ValidAudience = "https://sts.windows.net/a1d50521-9687-4e4d-a76d-ddd53ab0c668/",
  ValidateLifetime = false,
  ClockSkew = TimeSpan.Zero
};
//Database
builder.Services.AddDbContext<DataContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

//Automapper
builder.Services.AddAutoMapper(typeof(UserRequestMappingConfig));

//Repositry
builder.Services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Valiadtor
builder.Services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
builder.Services.AddScoped<IValidator<UserLoginDto>, UserLoginDtoValidator>();
builder.Services.AddScoped<IValidator<AddUserDto>, AddUserDtoValidator>();

//Services
builder.Services.AddScoped<IUserService, UserService>();


// Adding Authentication
builder.Services.AddAuthentication(options =>
{
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
  options.SaveToken = true;
  options.RequireHttpsMetadata = false;
  options.TokenValidationParameters = new TokenValidationParameters()
  {
    ValidateIssuerSigningKey = true,
    ValidateLifetime = true,
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidAudience =builder.Configuration["Identity:ValidAudience"],
    ValidIssuer = builder.Configuration["Identity:ValidIssuer"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Identity:key"]))
  };
  options.Events = new JwtBearerEvents
  {
    OnAuthenticationFailed = context =>
    {
      if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
      {
        context.Response.Headers.Add("Token-Expired", "true");
      }
      return Task.CompletedTask;
    },
    OnChallenge = context =>
    {
      context.HandleResponse();
      context.Response.StatusCode = StatusCodes.Status401Unauthorized;
      context.Response.ContentType = "application/json";

      // Ensure we always have an error and error description.
      if (string.IsNullOrEmpty(context.Error))
        context.Error = "invalid_token";
      if (string.IsNullOrEmpty(context.ErrorDescription))
        context.ErrorDescription = "This request requires a valid JWT access token to be provided";

      // Add some extra context for expired tokens.
      if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
      {
        var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
        context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
        context.ErrorDescription = $"The token expired on {authenticationException.Expires.ToString("o")}";
      }

      return context.Response.WriteAsync(JsonSerializer.Serialize(new
      {
        error = context.Error,
        error_description = context.ErrorDescription,
        statusCode = StatusCodes.Status401Unauthorized,
      }));
    },
  };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();



app.MapControllers();

app.Run();
