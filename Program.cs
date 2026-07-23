using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<CountriesProject.DAL.UserDAL>();
builder.Services.AddScoped<CountriesProject.DAL.CountryDAL>();
builder.Services.AddScoped<CountriesProject.BL.AuthBL>();
builder.Services.AddScoped<CountriesProject.DAL.CurrencyDAL>();
builder.Services.AddScoped<CountriesProject.DAL.LanguageDAL>();
builder.Services.AddScoped<CountriesProject.DAL.ContinentDAL>();
builder.Services.AddScoped<CountriesProject.DAL.PreferencesDAL>();
builder.Services.AddScoped<CountriesProject.BL.PreferencesBL>();
builder.Services.AddScoped<CountriesProject.DAL.UserCountryListDAL>();
builder.Services.AddScoped<CountriesProject.BL.UserListsBL>();
builder.Services.AddScoped<CountriesProject.DAL.ShareDAL>();
builder.Services.AddScoped<CountriesProject.BL.ShareBL>();
builder.Services.AddScoped<CountriesProject.DAL.QuizDAL>();
builder.Services.AddScoped<CountriesProject.BL.QuizBL>();
builder.Services.AddScoped<CountriesProject.DAL.AdminDAL>();
builder.Services.AddScoped<CountriesProject.BL.AdminBL>();

builder.Services.AddHttpClient<CountriesProject.BL.Services.RestCountriesService>(client =>
{
    client.BaseAddress = new Uri("https://countries.dev/");
}); builder.Services.AddScoped<CountriesProject.BL.CountryBL>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//---------- jwt stuff

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            new string[] {}
        }
    });
});

builder.Services.AddScoped<CountriesProject.BL.Services.JwtService>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };
});

//---------------------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();