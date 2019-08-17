using Application;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Data;
using Queries;

namespace WebApi
{
    public class Startup
    {
        private const string connectionStringName = "Accounting";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            AddMediator(services);
            AddPersistence(services);
            AddQueries(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void AddPersistence(IServiceCollection services)
        {
            services.AddDbContext<AccountingContext>(options => options.UseSqlite(Configuration.GetConnectionString(connectionStringName)));
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPaymentSourceRepository, PaymentSourceRepository>();
        }

        private void AddQueries(IServiceCollection services)
        {
            services.AddSingleton(new AccountingConnectionString(Configuration.GetConnectionString(connectionStringName)));
            services.AddTransient<ConnectionFactory>();
        }

        private void AddMediator(IServiceCollection services)
        {
            services.AddMediatR(new[]
            {
                typeof(RegisterPayment).Assembly,       // Application
                typeof(Payment).Assembly,               // Domain
                typeof(PaymentRepository).Assembly,     // Persistence
                typeof(ListPayments).Assembly           // Queries
            });
            services.AddTransient<Commands>();
            services.AddTransient<Queries>();
        }
    }
}
