using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

/// <summary>
/// API контроллер для управления сотрудниками
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[SwaggerTag("Управление данными карт")]
public class MapController : ControllerBase
{
    private readonly IMapManager _service;

    public MapController(IMapManager service)
    {
        _service = service;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Получить все карты",
        Description = "Возвращает полный список карт")]
    [ProducesResponseType(typeof(IEnumerable<Map>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get()
    {
        try
        {
            var maps = await _service.Get();
            return Ok(maps);
        }
        catch (Exception ex) 
        {
            Log.Error($"Ошибка: {ex.Message} получения карт"); 
            return BadRequest(ex.Message); 
        }
    }
}