using AutoMapper;
using Genealogy.Models;
using Genealogy.Repository.Abstract;
using Genealogy.Repository.Concrete;
using Genealogy.Service.Astract;
using Genealogy.Service.Concrete;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MintPlayer.AspNetCore.Hsts;
using MintPlayer.AspNetCore.SpaServices.Prerendering;
using MintPlayer.AspNetCore.SpaServices.Routing;
using MintPlayer.AspNetCore.SubDirectoryViews;
using System.Text;
using WebMarkupMin.AspNetCore7;

var builder = WebApplication.CreateBuilder(args);
var appSettings = builder.Configuration.GetRequiredSection("AppSettings").GetChildren().AsEnumerable();
builder.Services.AddControllersWithViews();
builder.Services.AddSpaStaticFiles(options =>
{
	options.RootPath = "ClientApp/dist";
});
builder.Services.AddSpaPrerenderingService<genealogy_ssr.Services.SpaPrerenderingService>();
builder.Services.AddWebMarkupMin(options =>
{
	options.DisablePoweredByHttpHeaders = true;
	options.AllowMinificationInDevelopmentEnvironment = true;
	options.AllowCompressionInDevelopmentEnvironment = true;
	options.DisablePoweredByHttpHeaders = false;
}).AddHttpCompression(options =>
{
}).AddHtmlMinification(options =>
{
	options.MinificationSettings.RemoveEmptyAttributes = true;
	options.MinificationSettings.RemoveRedundantAttributes = true;
	options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
	options.MinificationSettings.RemoveHttpsProtocolFromAttributes = false;
	options.MinificationSettings.MinifyInlineJsCode = true;
	options.MinificationSettings.MinifyEmbeddedJsCode = true;
	options.MinificationSettings.MinifyEmbeddedJsonData = true;
	options.MinificationSettings.WhitespaceMinificationMode = WebMarkupMin.Core.WhitespaceMinificationMode.Aggressive;
});

builder.Services.AddHttpContextAccessor();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GenealogyContext>(options => options.UseNpgsql(connection));
builder.Services.AddHostedService<PurchaseManageService>();

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IGenealogyService, GenealogyService>();

builder.Services.AddScoped<DbContext, GenealogyContext>();


builder.Services.ConfigureViewsInSubfolder("Server");

// Auto Mapper Configurations
var mappingConfig = new MapperConfiguration(cfg =>
{
	cfg.AddProfile<AutoMapperProfile>();
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var secretKey = appSettings.FirstOrDefault(i => i.Key == "Secret").Value;
var key = Encoding.ASCII.GetBytes(appSettings.FirstOrDefault(i => i.Key == "Secret").Value);
builder.Services.AddAuthentication(opt =>
{
	opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
	x.RequireHttpsMetadata = false;
	x.SaveToken = true;
	x.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key),
		ValidateIssuer = false,
		ValidateAudience = false
	};
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Error");
}

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseImprovedHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller}/{action=Index}/{id?}");
});

if (!app.Environment.IsDevelopment())
{
	app.UseSpaStaticFiles();
}

app.UseSpa(spa =>
{
	spa.Options.SourcePath = "ClientApp";

	spa.UseSpaPrerendering(options =>
	{
		// Disable below line, and run "npm run build:ssr" or "npm run dev:ssr" manually for faster development.
		options.BootModuleBuilder = app.Environment.IsDevelopment() ? new AngularPrerendererBuilder(npmScript: "build:ssr") : null;
		options.BootModulePath = $"{spa.Options.SourcePath}/dist/ClientApp/server/main.js";
		options.ExcludeUrls = new[] { "/sockjs-node" };
	});

	app.UseWebMarkupMin();

	if (app.Environment.IsDevelopment())
	{
		spa.UseAngularCliServer(npmScript: "start");
	}
});

app.Run();
