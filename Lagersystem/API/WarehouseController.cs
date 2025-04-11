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
    [HttpPost("AddNewItem/{WarehouseId}")]
    public async Task<IActionResult> AddItem(Location location, string WarehouseId)
    {
        var warehouse = LagerContext.Warehouses.Include(w => w.ItemLocations).Where(w => w.WarehouseId == WarehouseId).First();
        if (warehouse == null) return NotFound($"Warehouse with {WarehouseId} not found");
        if (location.Item == null) return Problem("No item in the locathions's data");
        location.Warehouse = warehouse;
        warehouse.ItemLocations.Add(location);
        return Created();
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
            var warehouse = LagerContext.Warehouses.Include(w => w.ItemLocations).Where(w => w.WarehouseId == id).First();
            if (warehouse == null) { return BadRequest($"No warehouse with the id: {id}"); }
            // Here we dont await because we can remove the wairhouse while this removes all the referenses to the warehouse
            // The await coms latere
            if (warehouse.ItemLocations.Any())
                await LagerContext.Items.Where(i => i.Location != null && i.Location.WarehouseId == warehouse.WarehouseId).Include(i => i.Location).ForEachAsync(i => i.Location = null);
            warehouse.ItemLocations.Clear();
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
        catch (Exception e)
        {
            Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            return Problem($"Culd not delete the warehouse with id: {id}");
        }
    }
}
