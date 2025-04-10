using System.Net;
using System.Threading.Tasks;
using Lagersystem.Entitys;
using Lagersystem.Utilitys;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
// using System.

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
    [HttpGet()]//, EnableCors("")]
    public IActionResult GetAll()
    {
        Response.Headers.Add("Access-Control-Allow-Credentials", "true");
        return Ok(LagerContext.Warehouses.Include(w => w.ItemLocations).AsEnumerable());
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(string id)
    {
        try
        {
            var warehouse = await LagerContext.Warehouses.Include(w => w.ItemLocations).Where(w => w.WarehouseId == id).FirstAsync();
            Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            return Ok(warehouse);
        }
        catch (Exception)
        {
            return NotFound($"No warehouse with the id {id}");
        }
    }
    [HttpPost("AddNewItem/{WarehouseId}")]
    public async Task<IActionResult> AddItem(Location location, string WarehouseId)
    {
        return NotFound(); // todo
    }
    [HttpPost("Create")]
    public async Task<IActionResult> Create(Warehouse warehouse)
    {
        try
        {
            LagerContext.Warehouses.Add(warehouse);
            await LagerContext.SaveChangesAsync();
            Response.Headers.Add("Access-Control-Allow-Credentials", "true");

            return Created();
        }
        catch (Exception ex)
        {
            Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            return BadRequest("Warehouse allready exsists");
        }
    }
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(Warehouse warehouse, string id)
    {
        try
        {
            var warehouseToUpdate = LagerContext.Warehouses.Find(id);
            if (warehouseToUpdate == null) { return BadRequest($"No Warehouse with id: {id}"); }
            warehouseToUpdate = warehouse;
            await LagerContext.SaveChangesAsync();
            Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            return Ok();
        }
        catch
        {
            Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            return Problem($"Culd not update the warehouse with id: {id}");
        }
    }
    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var warehouse = LagerContext.Warehouses.Find(id);
            if (warehouse == null) { return BadRequest($"No warehouse with the id: {id}"); }
            LagerContext.Warehouses.Remove(warehouse);

            foreach (var location in warehouse.ItemLocations)
            {
                // if (location.Item != null) location.Item.Location = null;
                // location.Item = null;
                // location.Warehouse = null;
                LagerContext.Locations.Remove(location);
            }
            // warehouse.ItemLocations = null;
            await LagerContext.SaveChangesAsync();
            LagerContext.Warehouses.Where(w => w.WarehouseId == id).ExecuteDelete();
            await LagerContext.SaveChangesAsync();
            Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            return Ok();
        }
        catch
        {
            Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            return Problem($"Culd not delete the warehouse with id: {id}");
        }
    }
}
