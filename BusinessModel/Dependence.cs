using Core.Interfaces;
using DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    /// <summary>
    /// Зависимости
    /// </summary>
    internal static class Dependence
    {
        /// <summary>
        /// Сервисы
        /// </summary>
        public static ServiceProvider Services { get; private set; }

        static Dependence()
        {
            Services = new ServiceCollection()
                .AddScoped<IMapManager, MapRepository>()
            .BuildServiceProvider();
        }
    }
}