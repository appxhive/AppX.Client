using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI(); //added AddMicrosoftIdentityUI() = adb2c policy microsoft identity

//Configuration for ADB2C - see appsettings
//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));

////redirect to specific page when signout is clicked - below code redirect to home page
//builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApp(option =>
//    {
//        builder.Configuration.Bind("AzureAdB2C", option);

//        option.Events.OnSignedOutCallbackRedirect = context =>
//        {
//            context.Response.Redirect("/");
//            context.HandleResponse();
//            return Task.CompletedTask;
//        };
//    }); //default identity config

//using oidc
builder.Services.AddAuthentication(options =>
{

});

//fixed infinite loop when signout is clicked
builder.Services.AddRazorPages();

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

//Added
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//fixed infinite loop when signout is clicked
app.MapRazorPages();

app.Run();
