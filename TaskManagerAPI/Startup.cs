using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskManagerAPI.Services;
using TaskManagerAPI.Models;

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

            // Registra o IMongoClient para ser usado para conectar ao MongoDB
            services.AddSingleton<IMongoClient>(sp =>
            {
                var mongoDbSettings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(mongoDbSettings.ConnectionString);
            });

            // Registra o IMongoCollection<TaskItem> com ciclo de vida Scoped, pois depende da requisição
            services.AddScoped<IMongoCollection<TaskItem>>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var database = client.GetDatabase(_configuration.GetValue<string>("MongoDbSettings:DatabaseName"));
                return database.GetCollection<TaskItem>("Tasks");
            });

            // Registra o IMongoCollection<TaskItem> com ciclo de vida Scoped, pois depende da requisição
            services.AddScoped<IMongoCollection<Project>>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var database = client.GetDatabase(_configuration.GetValue<string>("MongoDbSettings:DatabaseName"));
                return database.GetCollection<Project>("Projects");
            });

            services.AddSwaggerGen(c =>
            {
                // Configuração básica do Swagger
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "TaskManager API",
                    Version = "v1",
                    Description = "API para gerenciamento de tarefas de projetos",
                });

                // Adiciona a documentação de autenticação, se houver (opcional)
                // c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                // {
                //     Description = "JWT Authorization header using the Bearer scheme",
                //     In = ParameterLocation.Header,
                //     Name = "Authorization",
                //     Type = SecuritySchemeType.ApiKey
                // });
                //
                // c.AddSecurityRequirement(new OpenApiSecurityRequirement
                // {
                //     {
                //         new OpenApiSecurityScheme
                //         {
                //             Reference = new OpenApiReference
                //             {
                //                 Type = ReferenceType.SecurityScheme,
                //                 Id = "Bearer"
                //             }
                //         },
                //         new string[] {}
                //     }
                // });
            });

            // Registra o TaskService e ProjectService como Scoped
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IProjectService, ProjectService>();

            // Adiciona o suporte a controllers
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            // Habilita a interface do Swagger UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API v1");
                c.RoutePrefix = string.Empty;  // Deixa o Swagger UI acessível diretamente na raiz
            });

            // Middleware para usar roteamento e autorização
            app.UseRouting();

            // Mapeia os controllers da API
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
