using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AIApi.Data;
using AIApi.Models;
using AIApi.DTOs;

namespace AIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatasetsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DatasetsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DatasetDto>>> GetDatasets()
        {
            var datasets = await _context.Datasets
                .Select(d => new DatasetDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Source = d.Source,
                    SizeInMb = d.SizeInMb
                })
                .ToListAsync();
            return Ok(datasets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DatasetDto>> GetDataset(int id)
        {
            var dataset = await _context.Datasets.FindAsync(id);
            if (dataset == null) return NotFound();

            return Ok(new DatasetDto
            {
                Id = dataset.Id,
                Name = dataset.Name,
                Source = dataset.Source,
                SizeInMb = dataset.SizeInMb
            });
        }

        [HttpPost]
        public async Task<ActionResult<DatasetDto>> CreateDataset(DatasetCreateDto dto)
        {
            var dataset = new Dataset
            {
                Name = dto.Name,
                Source = dto.Source,
                SizeInMb = dto.SizeInMb
            };

            _context.Datasets.Add(dataset);
            await _context.SaveChangesAsync();

            var result = new DatasetDto
            {
                Id = dataset.Id,
                Name = dataset.Name,
                Source = dataset.Source,
                SizeInMb = dataset.SizeInMb
            };

            return CreatedAtAction(nameof(GetDataset), new { id = dataset.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDataset(int id, DatasetUpdateDto dto)
        {
            var dataset = await _context.Datasets.FindAsync(id);
            if (dataset == null) return NotFound();

            dataset.Name = dto.Name;
            dataset.Source = dto.Source;
            dataset.SizeInMb = dto.SizeInMb;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDataset(int id)
        {
            var dataset = await _context.Datasets.FindAsync(id);
            if (dataset == null) return NotFound();

            _context.Datasets.Remove(dataset);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}