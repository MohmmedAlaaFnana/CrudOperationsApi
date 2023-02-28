using CrudOperationsApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudOperationsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly BrandContext brandContext;

        public BrandController(BrandContext brandContext)
        {
            brandContext = brandContext;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrand(int id)
        {
            if (brandContext.Brands == null)
            {
                return NotFound();
            }

            var brand=await brandContext.Brands.FindAsync(id);
            if(brand == null)
            {
                return NotFound();
            }
            return brand ;
        }

        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            brandContext.Brands.Add(brand); 
            await brandContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBrand), new { id = brand.Id }, brand);
        }

        [HttpPut]

        public async Task<IActionResult>PutBrand(int Id, Brand brand)
        {
            if (id != brand.ID)

            {
                return BadRequest();
            }
            brandContext.Entry(brand).State = EntityState.Modified;

            try
            {
                await brandContext.SaveChangesAsync();
            }

            catch(DbUpdateConcurrencyException)
            {
                if (!BrandAvailable(Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        private bool BrandAvailable(int id)
        {
            return (brandContext.Brands?.Any(x => x.Id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteBrand(int id)
        {
            if (brandContext.Brands == null)
            {
                return NotFound();
            }

            var brand = brandContext.Brands.FindAsync(id);
            if(brand == null)
            {
                return NotFound();
            }

            brandContext.Brands.Remove(Brand);

            await brandContext.SaveChangesAsync();

            return Ok();
        }
        
    }
}
