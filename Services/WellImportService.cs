using EFCore.BulkExtensions;
using WellsAPI.Data;
using System.Text;
using WellsAPI.Entities;

namespace WellsAPI.Services
{
    public class WellImportService
    {
        private readonly AppDbContext _context;
        public WellImportService(AppDbContext context)
        {
            _context = context;
        }
        public async Task ImportWellsFromFile(string filePath)
        {
            // Lê as Linahs do arquivo
            var lines = await File.ReadAllLinesAsync(filePath, Encoding.Latin1);

            List<Well> wells = new List<Well>();

            var bulkConfig = new BulkConfig
            {
                UpdateByProperties = new List<string> { nameof(Well.Name), nameof(Well.OrganizationNumber) }
            };

            for (int i = 1; i < lines.Length; i++)
            {
                var fields = lines[i].Split(';');

                var well = new Well
                {
                    //Mapemamento dos campos do Excel para a entidade Well
                    Name = fields[0], //POCO
                    OrganizationNumber = fields[1], //CADASTRO
                    Operator = fields[2],//OPERADOR
                    NameWellOperator = fields[3],//OPERADOR
                    State = fields[4],//ESTADO
                    Basin = fields[5],//BACIA
                    Classification = fields[13],//RECLASSIFICACAO
                    Status = fields[14],//SITUACAO
                    Latitude = decimal.TryParse(fields[21], out var lat) ? lat : null,//LATITUDE_BASE_DD
                    Longitude = decimal.TryParse(fields[22], out var lng) ? lng : null,//LONGITUDE_BASE_DD
                    CreatedAt = DateTime.UtcNow
                };

                wells.Add(well);


                if (wells.Count >= 1000)
                {
                    //Evitar Duplicados usando BulkInsertOrUpdateAsync com UpdateByProperties
                    await _context.BulkInsertOrUpdateAsync(wells, bulkConfig);
                    wells.Clear();
                }
            }

            //Evitar Duplicados usando BulkInsertOrUpdateAsync com UpdateByProperties
            await _context.BulkInsertOrUpdateAsync(wells, bulkConfig);
        }
    }
}
