using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;
using WebApp.Server.Database;
using WebApp.Shared;

namespace WebApp.Server
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddRazorPages();

			#region Abstract
			services.AddTransient<IFileReader, FileReader>();
			services.AddSingleton<IJsonConvert, JsonConvert>();
			services.AddSingleton<IFileReader, FileReader>();
			services.AddScoped<IDevLog, DevLog>();
			#endregion

			#region Database
			services.AddTransient<ISqlRunner, SqlRunner>();
			services.AddTransient(context =>
			{
				string connectionString = Configuration.GetConnectionString("Demo:SqlDatabase")
					?? Configuration.GetConnectionString("SqlDatabase")
					?? Configuration.GetValue<string>("ConnectionStrings:Demo:SqlDatabase")
					?? Configuration.GetValue<string>("Demo:SqlDatabase")
					?? Configuration.GetValue<string>("SqlDatabase")
					;
				SqlConnection DataConnection = new(connectionString);
				return DataConnection;
			});
			#endregion

			#region Crypto
			services.AddTransient<ICSVLoader, CSVLoader>();
			services.AddTransient<ICryptoIngestion, CryptoIngestion>();
			services.AddTransient<ICryptoStorage, CryptoStorage>();
			#endregion
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseWebAssemblyDebugging();
			}
			else
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseBlazorFrameworkFiles();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapRazorPages();
				endpoints.MapControllers();
				endpoints.MapFallbackToFile("index.html");
			});
		}
	}
}
