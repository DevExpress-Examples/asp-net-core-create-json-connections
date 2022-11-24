using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Excel;
using DevExpress.DataAccess.Sql;
using DevExpress.DataAccess.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreDashboard {
    public class Startup {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment) {
            Configuration = configuration;
            FileProvider = hostingEnvironment.ContentRootFileProvider;
            DashboardExportSettings.CompatibilityMode = DashboardExportCompatibilityMode.Restricted;
        }

        public IFileProvider FileProvider { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services
                .AddMvc();

            services.AddScoped<DashboardConfigurator>((IServiceProvider serviceProvider) => {
                DashboardConfigurator configurator = new DashboardConfigurator();
                // Use the ConnectionStringProvider class to allow end users to use only connections created at runtime.
                //configurator.SetConnectionStringsProvider(new ConnectionStringProvider());

                // Use the ConnectionStringProviderEx class to allow end users to use connections created at runtime in addition to predefined connection strings.
                configurator.SetConnectionStringsProvider(new ConnectionStringsProviderEx(new DashboardConnectionStringsProvider(Configuration)));

                DashboardFileStorage dashboardFileStorage = new DashboardFileStorage(FileProvider.GetFileInfo("Data/Dashboards").PhysicalPath);
                configurator.SetDashboardStorage(dashboardFileStorage);

                DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();

                // Registers an SQL data source.
                DashboardSqlDataSource sqlDataSource = new DashboardSqlDataSource("SQL Data Source", "nwindConnectionString");
                sqlDataSource.DataProcessingMode = DataProcessingMode.Client;
                SelectQuery query = SelectQueryFluentBuilder
                    .AddTable("Categories")
                    .Join("Products", "CategoryID")
                    .SelectAllColumns()
                    .Build("Products_Categories");
                sqlDataSource.Queries.Add(query);
                dataSourceStorage.RegisterDataSource("sqlDataSource", sqlDataSource.SaveToXml());

                configurator.SetDataSourceStorage(dataSourceStorage);

                return configurator;
            });

            services.AddDevExpressControls();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseDevExpressControls();
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints => {
                endpoints.MapRazorPages();
                // Maps the dashboard route.
                EndpointRouteBuilderExtension.MapDashboardRoute(endpoints, "api/dashboards", "DefaultDashboard");
            });
        }
    }
    
    // Allows you to save and use connections created at runtime.
    public class ConnectionStringProvider: IDataSourceWizardConnectionStringsStorage {
        static readonly Dictionary<string, DataConnectionParametersBase> storage = new Dictionary<string, DataConnectionParametersBase>();
        public Dictionary<string, string> GetConnectionDescriptions() {
            
            return storage.ToDictionary(p=>p.Key, p=>p.Key);
        }

        public DataConnectionParametersBase GetDataConnectionParameters(string name) {
            return storage[name];
        }

        public void SaveDataConnectionParameters(string name, DataConnectionParametersBase connectionParameters, bool saveCredentials) {
            storage[name] = connectionParameters;
        }
    }

    // Allows you to save and use connections created at runtime in addition to predefined connection strings.
    public class ConnectionStringsProviderEx : IDataSourceWizardConnectionStringsStorage {
        readonly IDataSourceWizardConnectionStringsProvider provider;
        static readonly Dictionary<string, DataConnectionParametersBase> storage = new Dictionary<string, DataConnectionParametersBase>();

        public ConnectionStringsProviderEx(IDataSourceWizardConnectionStringsProvider provider) {
            this.provider = provider;
        }
        
        public Dictionary<string, string> GetConnectionDescriptions() {
            var result = provider.GetConnectionDescriptions();
            foreach(var pair in storage) {
                if(!result.ContainsKey(pair.Key))
                    result[pair.Key] = pair.Key;
            }
            return result;
        }

        public DataConnectionParametersBase GetDataConnectionParameters(string name) {
            if(provider.GetConnectionDescriptions().ContainsKey(name))
                return provider.GetDataConnectionParameters(name);
            DataConnectionParametersBase fromStorage;
            storage.TryGetValue(name, out fromStorage);
            return fromStorage;
        }

        public void SaveDataConnectionParameters(string name, DataConnectionParametersBase connectionParameters, bool saveCredentials) {
            storage[name] = connectionParameters;
        }
    }
}