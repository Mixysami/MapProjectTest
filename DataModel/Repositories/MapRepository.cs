using Core.Interfaces;
using Core.Models;
using DAL.Base;

namespace DAL.Repositories
{
    public class MapRepository : IMapManager
    {

        public async Task<IEnumerable<Map>> Get()
        {
            return await SqlConnector.GetAsync<Map>("[dbo].[Map.Get]");
        }
    }
}