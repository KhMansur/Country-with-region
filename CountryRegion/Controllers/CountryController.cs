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
    public class CountryController : ControllerBase
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;


        public CountryController(AppDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CountryCreationDTO countryDto)
        {
            if (countryDto is not null)
            {
                var country = mapper.Map<Country>(countryDto);
                await context.Countries.AddAsync(country);
                await context.SaveChangesAsync();
                return Ok(country);
            }
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var countries = await context.Countries.Include(p => p.Regions).ToListAsync();
            return Ok(countries);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var country = await context.Countries.Include("Regions").FirstOrDefaultAsync(p => p.Id == Id);
            if(country == null)
                return NotFound("Country is not found");
            return Ok(country);
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(int id, [FromForm] CountryCreationDTO country)
        {
            if (country is not null)
            {
                var ucountry = await context.Countries.FirstOrDefaultAsync(p => p.Id == id);
                if (ucountry is null)
                    return NotFound("not found");
                mapper.Map(country, ucountry);
                context.Countries.Attach(ucountry);
                context.Entry(ucountry).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(ucountry);
            }
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var country = await context.Countries.FirstOrDefaultAsync(p => p.Id == id);
            if (country is null)
                return NotFound("not found");
            context.Countries.Remove(country);
            await context.SaveChangesAsync();
            return NoContent();
        }


    }
}
