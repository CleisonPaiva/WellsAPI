using Microsoft.EntityFrameworkCore;
using WellsAPI.Data;
using WellsAPI.DTOs;

namespace WellsAPI.Services
{
    public class WellService
    {
        private readonly AppDbContext _context;
        public WellService(AppDbContext context)
        {
            _context = context;
        }

        //Retorna com paginação e filtro
        public async Task<PaginatedResponseDto<WellResponseDto>> GetAllAsync(WellFilterDto filterDto)
        {
            var query = _context.Wells.AsQueryable();

            if (!string.IsNullOrEmpty(filterDto.Name))
            {
                query = query.Where(c => c.Name.Contains(filterDto.Name));
            }

            if (!string.IsNullOrEmpty(filterDto.NameWellOperator))
            {
                query = query.Where(c => c.NameWellOperator.Contains(filterDto.NameWellOperator));
            }

            if (!string.IsNullOrEmpty(filterDto.OrganizationNumber))
            {
                query = query.Where(c => c.OrganizationNumber.Contains(filterDto.OrganizationNumber));
            }

            if (!string.IsNullOrEmpty(filterDto.Operator))
            {
                query = query.Where(c => c.Operator.Contains(filterDto.Operator));
            }

            if (!string.IsNullOrEmpty(filterDto.State))
            {
                query = query.Where(c => c.State.Contains(filterDto.State));
            }

            if (!string.IsNullOrEmpty(filterDto.Basin))
            {
                query = query.Where(c => c.Basin.Contains(filterDto.Basin));
            }

            if (!string.IsNullOrEmpty(filterDto.Status))
            {
                query = query.Where(c => c.Status.Contains(filterDto.Status));
            }

            if (!string.IsNullOrEmpty(filterDto.Classification))
            {
                query = query.Where(c => c.Classification.Contains(filterDto.Classification));
            }

            /* WellFilterDto → entrada — o que o usuário manda na requisição
             * PaginatedResponseDto → saída — o que a API devolve
             * O usuário precisa mandar page e pageSize para a API saber qual fatia retornar. 
             * E a API devolve page e pageSize junto com os dados para o frontend saber em qual página está.
             */
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filterDto.Page - 1) * filterDto.PageSize)
                .Take(filterDto.PageSize)
                .Select(w => new WellResponseDto
                {
                    Name = w.Name,
                    NameWellOperator = w.NameWellOperator,
                    OrganizationNumber = w.OrganizationNumber,
                    Operator = w.Operator,
                    State = w.State,
                    Basin = w.Basin,
                    Status = w.Status,
                    Classification = w.Classification,
                    Latitude = w.Latitude,
                    Longitude = w.Longitude,
                })
                .ToListAsync();

            //Monta os dados paginados para devolver para o frontend
            return new PaginatedResponseDto<WellResponseDto>
            {
                Data = items,
                TotalCount = totalCount,
                Page = filterDto.Page,
                PageSize = filterDto.PageSize
            };
            
        }

        public async Task<List<WellMapDto>> GetAllForMapAsync(WellFilterDto filterDto)
        {
            if (!filterDto.MinLatitude.HasValue || !filterDto.MaxLatitude.HasValue ||
                !filterDto.MinLongitude.HasValue || !filterDto.MaxLongitude.HasValue)
            {
                throw new ArgumentException("Os filtros de bounding box são obrigatórios.");
            }

            var query = _context.Wells.AsQueryable();

            if (!string.IsNullOrEmpty(filterDto.Name))
            {
                query = query.Where(c => c.Name.Contains(filterDto.Name));
            }

            if (!string.IsNullOrEmpty(filterDto.NameWellOperator))
            {
                query = query.Where(c => c.NameWellOperator.Contains(filterDto.NameWellOperator));
            }

            if (!string.IsNullOrEmpty(filterDto.OrganizationNumber))
            {
                query = query.Where(c => c.OrganizationNumber.Contains(filterDto.OrganizationNumber));
            }

            if (!string.IsNullOrEmpty(filterDto.Operator))
            {
                query = query.Where(c => c.Operator.Contains(filterDto.Operator));
            }

            if (!string.IsNullOrEmpty(filterDto.State))
            {
                query = query.Where(c => c.State.Contains(filterDto.State));
            }

            if (!string.IsNullOrEmpty(filterDto.Basin))
            {
                query = query.Where(c => c.Basin.Contains(filterDto.Basin));
            }

            if (!string.IsNullOrEmpty(filterDto.Status))
            {
                query = query.Where(c => c.Status.Contains(filterDto.Status));
            }

            if (!string.IsNullOrEmpty(filterDto.Classification))
            {
                query = query.Where(c => c.Classification.Contains(filterDto.Classification));
            }

            /*
             *----- abordagem Bounding Box
             Usuário move o mapa → frontend pega as coordenadas visíveis → 
            chama API com minLat, minLng, maxLat, maxLng → 
            backend retorna só os poços daquela área
            */
            if (filterDto.MinLatitude.HasValue && filterDto.MaxLatitude.HasValue)
            {
                query = query.Where(c => c.Latitude >= filterDto.MinLatitude && c.Latitude <= filterDto.MaxLatitude);
            }

            if (filterDto.MinLongitude.HasValue && filterDto.MaxLongitude.HasValue)
            {
                query = query.Where(c => c.Longitude >= filterDto.MinLongitude && c.Longitude <= filterDto.MaxLongitude);
            }

            /*
             *  Zoom 1-5 → visão ampla
                Zoom 6-9 → estado
                Zoom 10+ → cidade
             */
            int limit = filterDto.Zoom switch
            {
                <= 5 => 200,
                <= 9 => 500,
                _ => 2000
            };

            return await query
                .Take(limit)
                .Select(w => new WellMapDto
                {
                    Name = w.Name,
                    Status = w.Status,
                    Latitude = w.Latitude,
                    Longitude = w.Longitude,
                })
                .ToListAsync();

        }

        public async Task<WellResponseDto?> GetByNameAsync(string name)
        {
            var query = _context.Wells.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.Name == name);
            }
            return await query
                .Select(w => new WellResponseDto
                {
                    Name = w.Name,
                    NameWellOperator = w.NameWellOperator,
                    OrganizationNumber = w.OrganizationNumber,
                    Operator = w.Operator,
                    State = w.State,
                    Basin = w.Basin,
                    Status = w.Status,
                    Classification = w.Classification,
                    Latitude = w.Latitude,
                    Longitude = w.Longitude,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<WellFiltersDto> GetFiltersAsync()
        {
            var states = await _context.Wells
                .Where(w => w.State != null)
                .Select(w => w.State!)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();

            var basins = await _context.Wells
                .Where(w => w.Basin != null)
                .Select(w => w.Basin!)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();

            var status = await _context.Wells
                .Where(w => w.Status != null)
                .Select(w => w.Status!)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();


            return new WellFiltersDto
            {
                States = states,
                Basins = basins,
                Statuses = status,
            };
        }
    }
}
