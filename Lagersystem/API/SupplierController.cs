using Lagersystem.Entitys;
using Lagersystem.Utilitys;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lagersystem.API;

[ApiController]
[Route("api/[controller]")]
public class SupplierController : ControllerBase
{
    private LagerContext LagerContext { get; set; }
    public SupplierController(LagerContext lagerContext)
    {
        LagerContext = lagerContext;
    }

    [HttpGet()]
    public IEnumerable<Supplier> GetAll()
    {
        return LagerContext.Suppliers
          .AsNoTracking()
          .AsEnumerable();
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var supplier = LagerContext.Suppliers.Find(id);
        if (supplier != null)
            return Ok(supplier);

        return NotFound($"No supplier with the id {id}");
    }

    [HttpPut("Create")]
    public async Task<IActionResult> Create(Supplier supplier)
    {
        try
        {
            LagerContext.Suppliers.Add(supplier);
            await LagerContext.SaveChangesAsync();
            return Created();
        }
        catch (Exception ex)
        {
            //todo som loging of ex
            return BadRequest("Supplier allready exsists");
        }
    }

    [HttpPost("Update/{id}")]
    public async Task<IActionResult> Update(Supplier supplier, string id)
    {
        try
        {
            var supplierToUpdate = LagerContext.Suppliers.Find(id);
            if (supplierToUpdate == null) { return BadRequest($"No Supplier with id: {id}"); }
            supplierToUpdate = supplier;
            await LagerContext.SaveChangesAsync();
            return Ok();
        }
        catch
        {
            return Problem($"Culd not update the supplier with id: {id}");
        }
    }
    [HttpPost("AddItem/{SupplierId}/{ItemId}")]
    public async Task<IActionResult> Update(string SupplierId, string ItemId)
    {
        try
        {
            var supplier = LagerContext.Suppliers.Find(SupplierId);
            var item = LagerContext.Items.Find(ItemId);
            if (supplier == null) { return BadRequest($"No Supplier with id: {SupplierId}"); }
            if (item == null) { return BadRequest($"No Item with id: {ItemId}"); }
            supplier.Items.Add(item);
            await LagerContext.SaveChangesAsync();
            return Ok();
        }
        catch
        {
            return Problem($"Culd not update the supplier with id: {SupplierId}");
        }
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var supplier = LagerContext.Suppliers.Find(id);
            if (supplier == null) { return BadRequest($"No supplier with the id: {id}"); }
            LagerContext.Suppliers.Remove(supplier);
            await LagerContext.SaveChangesAsync();
            return Ok();
        }
        catch
        {
            return Problem($"Could not delete the supplier with id: {id}");

        }
    }

}
