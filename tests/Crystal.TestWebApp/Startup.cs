using Crystal.DbProviders;
using Crystal.Ioc;
using Crystal.Middlewares;
using Crystal.Models;
using Crystal.TestWebApp.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using Crystal.TestWebApp.Interfaces;
using Crystal.TestWebApp.Models;

namespace Crystal.TestWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RegisterCrystal(services);

            services.AddScoped<AppContext>();
            services.AddTransient<IGenericRepository<Entity>, GenericRepository<Entity>>();
            services.AddControllers();
        }

        private void RegisterCrystal(IServiceCollection services)
        {
            var shards = new List<Shard<long>>
            {
                new Shard<long>(1, "Server=localhost;Database=OrgA;Trusted_Connection=True;"),
                new Shard<long>(2, "Server=localhost;Database=OrgB;Trusted_Connection=True;")
            };

            services.UseCrystal<long, SqlServerDbProvider>(options =>
                {
                    options.KeyExtractorDelegate = httpContext =>
                    {
                        var tenantId = httpContext.Request.Query["TenantId"];
                        return long.TryParse(tenantId, out var parsedId) ? parsedId : 0;
                    };
                    options.ShardStorageType = ShardStorageType.List;
                    options.Shards = shards;
                    options.RouteExceptions = new List<string>
                    {
                        "/api/admin"
                    };
                })
                .WithShardMigrator<long, AppContext>(
                    (shardManager, dbProvider) => new AppContext(shardManager, dbProvider));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<CrystalMiddleware<long>>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
