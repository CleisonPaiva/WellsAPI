using Microsoft.AspNetCore.Mvc;
using WellsAPI.Services;

namespace WellsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WellsImportController : ControllerBase
    {
        private readonly WellImportService _wellImportService;

        public WellsImportController(WellImportService wellImportService)
        {
            _wellImportService = wellImportService;
        }

        [HttpPost]
        public async Task<IActionResult> Import(IFormFile file)
        {
            // 1. cria um caminho temporário
            var tempPath = Path.GetTempFileName();

            // 2. salva o arquivo nesse caminho
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 3. chama o service passando o caminho do arquivo temporário
            await _wellImportService.ImportWellsFromFile(tempPath);

            // 4. deleta o arquivo temporário
            System.IO.File.Delete(tempPath);

            return Ok(new { message = "Importação concluída com sucesso!" });
        }
    }
}
