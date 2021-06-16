using Microsoft.Extensions.DependencyInjection;

namespace PrettyConsoleHelper
{
    public static class ServicesExtensions
    {
        public static void AddPrettyConsoleHelper(this IServiceCollection services)
        {
            services.AddSingleton<IPrettyConsole, PrettyConsole>();
            services.AddSingleton<InputHelper>();
        }
    }
}
