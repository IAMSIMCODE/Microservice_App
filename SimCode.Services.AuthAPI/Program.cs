using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimCode.Services.AuthAPI.Configuration.AppConfig;
using SimCode.Services.AuthAPI.Data;
using SimCode.Services.AuthAPI.Models.User;
using SimCode.Services.AuthAPI.Services;
using SimCode.Services.AuthAPI.Services.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
#region Demo auth config
////I ADDED THIS FOR DEMO PURPOSES
//builder.Services.Configure<IdentityOptions>(options =>
//{
//    options.Password.RequireNonAlphanumeric = true;
//    options.Password.RequireDigit = true;
//    options.Password.RequiredLength = 2;
//    options.Password.RequireLowercase = true;
//    options.Password.RequireUppercase = true;

//    options.SignIn.RequireConfirmedPhoneNumber = true;
//    options.SignIn.RequireConfirmedEmail = true;
//    options.SignIn.RequireConfirmedAccount = true;

//    options.Lockout.AllowedForNewUsers = true;
//    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
//    options.Lockout.MaxFailedAccessAttempts = 5;

//    options.User.RequireUniqueEmail = true;
//    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqretuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-._@+";


//});




#endregion


builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
ApplyMigration();

app.Run();

//This method will check my code for any pending migration whenever my app starts and automatically apply it.
//To use it you will have to invoke it in the middleware pipeline 
void ApplyMigration()
{
    using var scope = app.Services.CreateScope();
    var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (_db.Database.GetPendingMigrations().Any())
    {
        _db.Database.Migrate();
    }
}
