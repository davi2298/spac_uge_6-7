using Microsoft.AspNetCore.Mvc;
using Lagersystem.Entitys;
using Lagersystem.Utilitys;
using Microsoft.EntityFrameworkCore;

namespace Lagersystem.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private LagerContext LagerContext { get; set; }

        public ItemsController(LagerContext context)
        {
            LagerContext = context;
        }

        // GET: api/Items
        [HttpGet]
        public ActionResult<IEnumerable<Item>> GetItems()
        {
            return LagerContext.Items.ToList();
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(string id)
        {
            var item = LagerContext.Items.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // POST: api/Items
        [HttpPost]
        public ActionResult<Item> PostItem(Item item)
        {
            LagerContext.Items.Add(item);
            LagerContext.SaveChanges();

            return CreatedAtAction(nameof(GetItem), new { id = item.ItemId }, item);
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public IActionResult PutItem(string id, Item item)
        {
            if (id != item.ItemId)
            {
                return BadRequest();
            }

            LagerContext.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                LagerContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LagerContext.Items.Any(e => e.ItemId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public IActionResult DeleteItem(string id)
        {
            var item = LagerContext.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            LagerContext.Items.Remove(item);
            LagerContext.SaveChanges();

            return NoContent();
        }
    }
}
