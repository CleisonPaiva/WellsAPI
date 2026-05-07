using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using WellsAPI.DTOs;
using WellsAPI.Services;

namespace WellsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WellsController : ControllerBase
    {
        private readonly WellService _wellService;

        public WellsController(WellService wellService)
        {
            _wellService = wellService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] WellFilterDto request)
        {
            var result = await _wellService.GetAllAsync(request);
            return Ok(result);
        }

        /*
         * 1. Usuário aplica filtros
            2. Mapa atualiza com os pontos filtrados
            3. Usuário clica num ponto
            4. Popup abre com as informações daquele poço
         */
        [HttpGet("map")]
        public async Task<IActionResult> GetAllMap([FromQuery] WellFilterDto request)
        {
            try
            {
                var result = await _wellService.GetAllForMapAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            var result = await _wellService.GetByNameAsync(name);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);

        }
    }
}
