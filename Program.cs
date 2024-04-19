using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineTechShop.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TechOnlineShopContext>(options =>
 options.UseSqlServer(builder.Configuration["Server"])
 );
 
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

            // JWT Token Generation from Server Side.  
            builder.Services.AddMvc();  
            // Enable Swagger   
            builder.Services.AddSwaggerGen(swagger =>  
            {  
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo  
                {   
                    Version= "v1",   
                    Title = "JWT Token Authentication API",  
                    Description="ASP.NET Core 3.1 Web API" });  
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()  
                {  
                    Name = "Authorization",  
                    Type = SecuritySchemeType.ApiKey,  
                    Scheme = "Bearer",  
                    BearerFormat = "JWT",  
                    In = ParameterLocation.Header,  
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",  
                });  
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement  
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
            
            builder.Services.AddAuthentication(option =>  
            {  
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  
  
            }).AddJwtBearer(options =>  
            {  
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters  
                {  
                    ValidateIssuer = true,  
                    ValidateAudience = true,  
                    ValidateLifetime = false,  
                    ValidateIssuerSigningKey = true,  
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],  
                    ValidAudience = builder.Configuration["Jwt:Audience"],  
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };  
            });  

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Logging.AddConsole();
builder.Logging.AddDebug();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();  
app.UseAuthorization();

app.MapControllers();

app.Run();