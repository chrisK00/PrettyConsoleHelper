using System;
using Microsoft.Extensions.DependencyInjection;

namespace PrettyConsoleHelper
{
    public static class ServicesExtensions
    {
        public static void AddPrettyConsoleHelper(this IServiceCollection services, Action<PrettyConsoleOptions> consoleOptions)
        {
            if (consoleOptions != null)
            {
                services.Configure(consoleOptions);
            }
            services.AddSingleton<IPrettyConsole, PrettyConsole>();
            services.AddSingleton<IInputHelper, InputHelper>();
        }

        public static void AddPrettyConsoleHelper(this IServiceCollection services)
        {
            services.AddSingleton<IPrettyConsole, PrettyConsole>();
            services.AddSingleton<IInputHelper, InputHelper>();
        }
    }
}
