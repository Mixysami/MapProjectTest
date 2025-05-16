using Core.Interfaces;
using Core.Models;

namespace BLL
{
    public class MapService : Base<IMapManager>, IMapManager
    {
        public async Task<IEnumerable<Map>> Get()
        {
            return await _data.Get();
        }
    }
}