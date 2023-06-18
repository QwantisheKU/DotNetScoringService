using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DotNetScoringService.Data;
using DotNetScoringService.Repositories;
using DotNetScoringService.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using DotNetScoringService.Repositories.Interfaces;
using DotNetScoringService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using DotNetScoringService.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EducativeAppContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("EducativeAppContext") ?? throw new InvalidOperationException("Connection string 'EducativeAppContext' not found.")));

builder.Services.AddDefaultIdentity<ApplicationUser>().AddEntityFrameworkStores<EducativeAppContext>().AddDefaultTokenProviders();

builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
// Add services to the container.
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddSession();
builder.Services.AddSwaggerGen(c =>
{
    //c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ScoringAPI", Version = "v1" });
});

// Repositories
builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IProfileRepository, ProfileRepository>();
builder.Services.AddTransient<ICalculationRepository, CalculationRepository>();
builder.Services.AddTransient<ICalculationResultRepository, CalculationResultRepository>();
builder.Services.AddTransient<IFeedbackRepository, FeedbackRepository>();

// Services
builder.Services.AddTransient<ICalculationService, CalculationService>();
builder.Services.AddTransient<IProfileService, ProfileService>();
builder.Services.AddTransient<IFeedbackService, FeedbackService>();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
