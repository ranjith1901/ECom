
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Clarion.Ecom.API.Extensions;
using System.Reflection;
using System.Text;
//using Clarion.Ecom.API.Models;
using Clarion.Ecom.API.Services;
using Clarion.Ecom.API.Middleware;
using Microsoft.AspNetCore.Mvc;
using Clarion.Ecom.API.Utilities;
using Microsoft.Extensions.Configuration;
using Clarion.Ecom.API.IRepository;
using Clarion.Ecom.API.NonEntity;
using Clarion.Ecom.API.Repository;
using Clarion.Ecom.API.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ClarionECOMDBContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("ClarionECOMCS") ??
   throw new InvalidOperationException("Connection string 'ClarionECOMDBContext' not found.")));

// Add Serilog configurations
var _logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration)
.Enrich.FromLogContext()
//.MinimumLevel.Information()
//.WriteTo.File("logs\\log-.txt", rollingInterval: RollingInterval.Day)
.CreateLogger();
builder.Logging.AddSerilog(_logger);

//Enabling CORS
builder.Services.AddCors(options => options.AddPolicy(name: "ApiCorsPolicy",
    policy =>
    {
        policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }));

builder.Services.AddHttpContextAccessor(); // Add this line

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DictionaryKeyPolicy = null;
    });

// Add services to the container.   
builder.Services.AddTransient<JwtServices>();
builder.Services.APIServices();

//Email configuration
//var configuration = builder.Configuration;
//var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfigModel>();
//builder.Services.AddSingleton(emailConfig);

//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
SwaggerControllerOrder<ControllerBase> swaggerControllerOrder = new SwaggerControllerOrder<ControllerBase>(Assembly.GetEntryAssembly()!);
builder.Services.AddSwaggerGen(option =>
{
    option.OrderActionsBy((apiDesc) => $"{swaggerControllerOrder.SortKey(apiDesc.ActionDescriptor.RouteValues["controller"]!)}");
    option.SwaggerDoc("v1", new OpenApiInfo { Title = " Clarion.Ecom.API", Version = "v1" });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please Enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                new string[]{}
            }
    });
    //option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

var signingKey = Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"]!);
var encKey = Encoding.UTF8.GetBytes(builder.Configuration["JWT:EncryptionKey"]!);
var symmetricSigningKey = new SymmetricSecurityKey(signingKey);
var symmetricEncKey = new SymmetricSecurityKey(encKey);

//Add autentication schema .
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidAudience = builder.Configuration["JWT:Audience"],
                IssuerSigningKey = symmetricSigningKey,
                TokenDecryptionKey = symmetricEncKey,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();

app.UseRouting();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.UseHttpsRedirection();

app.Run();

