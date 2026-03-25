using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShipmentLearning;
using ShipmentLearning.Api.Data;

namespace ShipmentLearning.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParcelsController : ControllerBase
    {
        private readonly ShipmentContext _context;

        public ParcelsController(ShipmentContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetParcels(
            [FromQuery] string? search,
            [FromQuery] bool? delivered,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (page < 1 || pageSize < 1 || pageSize > 100) return BadRequest("Invalid pagination parameters.");

            IQueryable<Parcel> query = _context.Parcels.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var terms = search.Trim().ToLower();
                query = query.Where(p => p.Category.ToLower().Contains(terms) || p.Id.ToString().Contains(terms));
            }

            if (delivered.HasValue)
                query = query.Where(p => p.IsDelivered == delivered.Value);

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                total,
                page,
                pageSize,
                pages = (int)Math.Ceiling(total / (double)pageSize),
                payload = items
            };
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Parcel>> GetParcel(int id)
        {
            var parcel = await _context.Parcels.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (parcel == null)
                return NotFound();

            return parcel;
        }

        [HttpPost]
        public async Task<ActionResult<Parcel>> CreateParcel(Parcel parcel)
        {
            if (parcel.Id <= 0 || parcel.Length <= 0 || parcel.Breadth <= 0 || parcel.Height <= 0 || parcel.Weight <= 0 || parcel.Value < 0)
                return BadRequest("Parcel fields must be positive and value cannot be negative.");

            if (await _context.Parcels.AnyAsync(p => p.Id == parcel.Id))
                return Conflict(new { message = $"Parcel with Id {parcel.Id} already exists." });

            parcel.Dimensions = parcel.Length * parcel.Breadth * parcel.Height;

            _context.Parcels.Add(parcel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParcel), new { id = parcel.Id }, parcel);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateParcel(int id, Parcel updatedParcel)
        {
            if (id != updatedParcel.Id)
                return BadRequest();

            if (updatedParcel.Length <= 0 || updatedParcel.Breadth <= 0 || updatedParcel.Height <= 0 || updatedParcel.Weight <= 0 || updatedParcel.Value < 0)
                return BadRequest("Parcel fields must be positive and value cannot be negative.");

            var parcel = await _context.Parcels.FindAsync(id);
            if (parcel == null)
                return NotFound();

            parcel.Length = updatedParcel.Length;
            parcel.Breadth = updatedParcel.Breadth;
            parcel.Height = updatedParcel.Height;
            parcel.Weight = updatedParcel.Weight;
            parcel.Value = updatedParcel.Value;
            parcel.Category = updatedParcel.Category;
            parcel.IsDelivered = updatedParcel.IsDelivered;
            parcel.Dimensions = updatedParcel.Length * updatedParcel.Breadth * updatedParcel.Height;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteParcel(int id)
        {
            var parcel = await _context.Parcels.FindAsync(id);
            if (parcel == null)
                return NotFound();

            _context.Parcels.Remove(parcel);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("summary")]
        public async Task<ActionResult<object>> GetSummary()
        {
            var total = await _context.Parcels.CountAsync();
            var deliveredCount = await _context.Parcels.CountAsync(p => p.IsDelivered);
            var totalWeight = await _context.Parcels.SumAsync(p => (double?)p.Weight) ?? 0;
            var totalValue = await _context.Parcels.SumAsync(p => (double?)p.Value) ?? 0;

            return new
            {
                total,
                deliveredCount,
                pendingCount = total - deliveredCount,
                totalWeight,
                totalValue,
                deliveredPercent = total == 0 ? 0 : (double)deliveredCount * 100 / total
            };
        }
    }
}
