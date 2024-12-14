using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using TaskManagerAPI.Services;

namespace TaskManagerAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configurações do MongoDB a partir do appsettings.json
            services.Configure<MongoDbSettings>(_configuration.GetSection("MongoDbSettings"));
            // Configurações do MongoDB a partir do appsettings.json

            // Registra os serviços de Projeto e Tarefa
            services.AddSingleton<ProjectService>();
            services.AddSingleton<TaskService>();

            // Adiciona o suporte a controllers
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Middleware para usar roteamento e autorização
            app.UseRouting();
            app.UseAuthorization();

            // Mapeia os controllers da API
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
