using Core.Models;

namespace Core.Interfaces
{
    public interface IMapManager
    {
        Task<IEnumerable<Map>> Get();
    }
}
