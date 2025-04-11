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

            return Created();
        }

        // PUT: api/Items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemAsync(string id, Item item)
        {
            try
            {
                var itemToUpdate = LagerContext.Items.Find(id);
                if (itemToUpdate == null) { return BadRequest($"No Supplier with id: {id}"); }
                itemToUpdate = item;
                await LagerContext.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return Problem($"Culd not update the supplier with id: {id}");
            }
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
