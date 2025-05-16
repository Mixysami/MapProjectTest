using Microsoft.Extensions.DependencyInjection;

namespace BLL
{
    /// <summary>
    /// Базовый сервис
    /// </summary>
    public class Base<I> where I : class
    {
        protected readonly I _data;

        public Base()
        {
            _data = Dependence.Services.GetRequiredService<I>();
        }
    }
}
