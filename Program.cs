using Employee_System.DAL;
using Employee_System.Map;
using Employee_System.Models;
using Employee_System.Repository;
using Employee_System.Repository.GenericRepository;
using Employee_System.Services;
using Employee_System.Services.Interface;
using Employee_System.Strategy_Pattern;
using Employee_System.Strategy_Pattern.Interface;
using Employee_System.Unit_of_work;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FluentValidation;
using Employee_System.Validator;
using FluentValidation.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    // Enable ReferenceHandler.Preserve for JSON serialization to handle circular references
//    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
//});

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories and services
builder.Services.AddScoped<IGenericRepository<Employee>, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IAttendanceTrackingStrategy, DailyAttendanceTrackingStrategy>();
builder.Services.AddScoped<IAttendanceTrackingStrategy, HourlyAttendanceTrackingStrategy>();

builder.Services.AddValidatorsFromAssembly(typeof(EmployeeValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(LeaveRequestValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(AttendanceValidator).Assembly);

// Enable FluentValidation auto-validation
builder.Services.AddFluentValidationAutoValidation();

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Configure Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
if (key.Length < 32)
{
    throw new ArgumentException("JWT Key must be at least 32 bytes long.");
}
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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Register MVC controllers
builder.Services.AddControllers(); // Add this line to register controllers

// Register Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Employee Management API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("HRPolicy", policy => policy.RequireRole("HR"));
    options.AddPolicy("EmployeePolicy", policy => policy.RequireRole("Employee"));
});

var app = builder.Build();

// Seed roles on application startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedRolesAsync(roleManager);
}

//async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
//{
//    string[] roleNames = { "Admin", "HR", "Employee" };

//    foreach (var roleName in roleNames)
//    {
//        if (!await roleManager.RoleExistsAsync(roleName))
//        {
//            await roleManager.CreateAsync(new IdentityRole(roleName));
//        }
//    }
//}

async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
{
    // Define roles with custom Ids
    var roles = new List<IdentityRole>
    {
        new IdentityRole { Id = "Admin", Name = "Admin", NormalizedName = "ADMIN" },
        new IdentityRole { Id = "HR", Name = "HR", NormalizedName = "HR" },
        new IdentityRole { Id = "Employee", Name = "Employee", NormalizedName = "EMPLOYEE" }
    };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role.Name))
        {
            await roleManager.CreateAsync(role);
        }
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers(); // This line remains unchanged
app.Run();
