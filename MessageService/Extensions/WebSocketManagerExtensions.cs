using MessageService.Middleware;
using MessageService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace MessageService.Extensions
{
    public static class WebSocketManagerExtensions
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            services.AddTransient<WebSocketService>();
            services.AddSingleton<WebSocketConnectionManager>();

            return services;
        }

        public static IApplicationBuilder MapWebSocketManager(this IApplicationBuilder app,
            PathString path,
            WebSocketService service)
        {
            return app.Map(path, appBuilder => appBuilder.UseMiddleware<WebSocketManagerMiddleware>(service));
        }
    }
}
