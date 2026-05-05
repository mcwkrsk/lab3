using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AIApi.Data;
using AIApi.Models;
using AIApi.DTOs;

namespace AIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ModelsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/models
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelDto>>> GetModels()
        {
            var models = await _context.Models
                .Include(m => m.Framework)
                .Include(m => m.ModelDatasets)
                    .ThenInclude(md => md.Dataset)
                .ToListAsync();

            var result = models.Select(m => new ModelDto
            {
                Id = m.Id,
                Name = m.Name,
                Version = m.Version,
                Description = m.Description,
                FrameworkId = m.FrameworkId,
                FrameworkName = m.Framework?.Name ?? "",
                DatasetNames = m.ModelDatasets.Select(md => md.Dataset.Name).ToList()
            });

            return Ok(result);
        }

        // GET: api/models/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelDto>> GetModel(int id)
        {
            var model = await _context.Models
                .Include(m => m.Framework)
                .Include(m => m.ModelDatasets)
                    .ThenInclude(md => md.Dataset)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
                return NotFound();

            var result = new ModelDto
            {
                Id = model.Id,
                Name = model.Name,
                Version = model.Version,
                Description = model.Description,
                FrameworkId = model.FrameworkId,
                FrameworkName = model.Framework?.Name ?? "",
                DatasetNames = model.ModelDatasets.Select(md => md.Dataset.Name).ToList()
            };

            return Ok(result);
        }

        // POST: api/models
        [HttpPost]
        public async Task<ActionResult<ModelDto>> CreateModel(ModelCreateDto dto)
        {
            // Проверка существования фреймворка
            var framework = await _context.Frameworks.FindAsync(dto.FrameworkId);
            if (framework == null)
                return BadRequest("Framework not found");

            var model = new AIModel
            {
                Name = dto.Name,
                Version = dto.Version,
                Description = dto.Description,
                FrameworkId = dto.FrameworkId
            };

            // Добавление many-to-many связей с датасетами
            foreach (var dsId in dto.DatasetIds.Distinct())
            {
                var dataset = await _context.Datasets.FindAsync(dsId);
                if (dataset != null)
                {
                    model.ModelDatasets.Add(new ModelDataset { Model = model, Dataset = dataset });
                }
            }

            _context.Models.Add(model);
            await _context.SaveChangesAsync();

            // Загружаем связанные данные для ответа
            await _context.Entry(model).Reference(m => m.Framework).LoadAsync();
            await _context.Entry(model).Collection(m => m.ModelDatasets).Query().Include(md => md.Dataset).LoadAsync();

            var result = new ModelDto
            {
                Id = model.Id,
                Name = model.Name,
                Version = model.Version,
                Description = model.Description,
                FrameworkId = model.FrameworkId,
                FrameworkName = model.Framework?.Name ?? "",
                DatasetNames = model.ModelDatasets.Select(md => md.Dataset.Name).ToList()
            };

            return CreatedAtAction(nameof(GetModel), new { id = model.Id }, result);
        }

        // PUT: api/models/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModel(int id, ModelUpdateDto dto)
        {
            var model = await _context.Models
                .Include(m => m.ModelDatasets)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
                return NotFound();

            // Проверка фреймворка
            var framework = await _context.Frameworks.FindAsync(dto.FrameworkId);
            if (framework == null)
                return BadRequest("Framework not found");

            // Обновление основных полей
            model.Name = dto.Name;
            model.Version = dto.Version;
            model.Description = dto.Description;
            model.FrameworkId = dto.FrameworkId;

            // Обновление many-to-many связей
            var existingDatasetIds = model.ModelDatasets.Select(md => md.DatasetId).ToHashSet();
            var newDatasetIds = dto.DatasetIds.ToHashSet();

            // Удалить те, которых нет в новом списке
            foreach (var md in model.ModelDatasets.Where(md => !newDatasetIds.Contains(md.DatasetId)).ToList())
                _context.ModelDatasets.Remove(md);

            // Добавить новые связи
            foreach (var dsId in newDatasetIds.Except(existingDatasetIds))
            {
                var dataset = await _context.Datasets.FindAsync(dsId);
                if (dataset != null)
                    model.ModelDatasets.Add(new ModelDataset { ModelId = model.Id, DatasetId = dsId });
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/models/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModel(int id)
        {
            var model = await _context.Models.FindAsync(id);
            if (model == null)
                return NotFound();

            _context.Models.Remove(model);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}