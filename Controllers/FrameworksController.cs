using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AIApi.Data;
using AIApi.Models;
using AIApi.DTOs;

namespace AIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrameworksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FrameworksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FrameworkDto>>> GetFrameworks()
        {
            var frameworks = await _context.Frameworks
                .Select(f => new FrameworkDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Description = f.Description
                })
                .ToListAsync();
            return Ok(frameworks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FrameworkDto>> GetFramework(int id)
        {
            var framework = await _context.Frameworks.FindAsync(id);
            if (framework == null)
                return NotFound();

            return Ok(new FrameworkDto
            {
                Id = framework.Id,
                Name = framework.Name,
                Description = framework.Description
            });
        }

        [HttpPost]
        public async Task<ActionResult<FrameworkDto>> CreateFramework(FrameworkCreateDto dto)
        {
            var framework = new AIFramework
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.Frameworks.Add(framework);
            await _context.SaveChangesAsync();

            var resultDto = new FrameworkDto
            {
                Id = framework.Id,
                Name = framework.Name,
                Description = framework.Description
            };

            return CreatedAtAction(nameof(GetFramework), new { id = framework.Id }, resultDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFramework(int id, FrameworkUpdateDto dto)
        {
            var framework = await _context.Frameworks.FindAsync(id);
            if (framework == null)
                return NotFound();

            framework.Name = dto.Name;
            framework.Description = dto.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFramework(int id)
        {
            var framework = await _context.Frameworks.FindAsync(id);
            if (framework == null)
                return NotFound();

            _context.Frameworks.Remove(framework);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}