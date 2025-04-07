using System.Net;
using System.Threading.Tasks;
using Lagersystem.Entitys;
using Lagersystem.Utilitys;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;

namespace Lagersystem.API;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private LagerContext LagerContext { get; set; }
    public WarehouseController(LagerContext lagerContext)
    {
        LagerContext = lagerContext;
    }
    [HttpGet()]
    public IEnumerable<Warehouse> GetAll()
    {
        return LagerContext.Warehouses.Include(w => w.ItemLocations).AsEnumerable();
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        try
        {
            var warehouse = await LagerContext.Warehouses.Include(w => w.ItemLocations).Where(w => w.WarehouseId == id).FirstAsync();
            return Ok(warehouse);
        }
        catch (Exception)
        {
            return NotFound($"No warehouse with the id {id}");
        }
    }
    [HttpPut("Create")]
    public async Task<IActionResult> Create(Warehouse warehouse)
    {
        try
        {
            LagerContext.Warehouses.Add(warehouse);
            await LagerContext.SaveChangesAsync();
            return Created();
        }
        catch (Exception ex)
        {
            return BadRequest("Warehouse allready exsists");
        }
    }
    [HttpPost("Update/{id}")]
    public async Task<IActionResult> Update(Warehouse warehouse, string id)
    {
        try
        {
            var warehouseToUpdate = LagerContext.Warehouses.Find(id);
            if (warehouseToUpdate == null) { return BadRequest($"No Warehouse with id: {id}"); }
            warehouseToUpdate = warehouse;
            await LagerContext.SaveChangesAsync();
            return Ok();
        }
        catch
        {
            return Problem($"Culd not update the warehouse with id: {id}");
        }
    }
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var warehouse = LagerContext.Warehouses.Find(id);
            if (warehouse == null) { return BadRequest($"No warehouse with the id: {id}"); }
            LagerContext.Warehouses.Remove(warehouse);
            await LagerContext.SaveChangesAsync();
            return Ok();
        }
        catch
        {
            return Problem($"Culd not delete the warehouse with id: {id}");

        }
    }
}
