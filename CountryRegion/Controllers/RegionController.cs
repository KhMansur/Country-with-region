using AutoMapper;
using Data.Context;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.DTO;

namespace CountryRegion.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;


        public RegionController(AppDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] RegionCreationDTO regionDto)
        {
            if (regionDto is not null)
            {
                var country = await context.Countries.FirstOrDefaultAsync(p => p.Id == regionDto.CountryId);
                if (country == null)
                    return NotFound("Country is not found");
                var region = mapper.Map<Region>(regionDto);
                await context.Regions.AddAsync(region);
                await context.SaveChangesAsync();
                return Ok(region);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await context.Regions.ToListAsync();
            return Ok(regions);
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(int id, [FromForm] RegionCreationDTO region)
        {
            if (region is not null)
            {
                var uregion = await context.Regions.FirstOrDefaultAsync(p => p.Id == id);
                if (uregion is null)
                    return NotFound("not found");
                mapper.Map(region, uregion);
                context.Regions.Attach(uregion);
                context.Entry(uregion).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(uregion);
            }
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var region = await context.Regions.FirstOrDefaultAsync(p => p.Id == id);
            if (region is null)
                return NotFound("Country did not found");
            context.Regions.Remove(region);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
