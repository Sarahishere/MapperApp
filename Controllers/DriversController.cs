using AutoMapper;
using MapperApp.Data;
using MapperApp.Models;
using MapperApp.Models.DTOs.Incoming;
using MapperApp.Models.DTOs.Outgoing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MapperApp.Controllers;

[ApiController]
[Route("[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<DriversController> _logger;
    private readonly IMapper _mapper;
    private readonly ApiDbContext _context;

    public DriversController(ILogger<DriversController> logger,IMapper mapper,ApiDbContext context)
    {
        _logger = logger;
        _mapper = mapper;
        _context = context;

    }

    //get
    [HttpGet]
    public async Task<IActionResult> GetAllDrivers()
    {
        var drivers = await _context.Drivers.ToListAsync();
        var allDrivers = drivers.Where(x => x.Status == 1).ToList();
        var outDrivers = _mapper.Map<IEnumerable<DriverForReturnDto>>(allDrivers);
        return Ok(outDrivers);
    }

    //post
    [HttpPost]
    public async Task<IActionResult> CreateDriver(DriverForCreationDto data)
    {
        if(ModelState.IsValid)
        {
            var newDriver = _mapper.Map<Driver>(data);
            _context.Drivers.Add(newDriver);
            await _context.SaveChangesAsync();
            var outDriver = _mapper.Map<DriverForReturnDto>(newDriver);
            
            _logger.LogInformation("New driver created");
            return CreatedAtAction("GetDriver",new {outDriver.Id},outDriver);
        }

        return new JsonResult("Something went wrong"){StatusCode = 500};
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDriver(Guid id)
    {
        var drivers = await _context.Drivers.ToListAsync();
        var item = drivers.FirstOrDefault(x => x.Id == id);
        var outDriver = _mapper.Map<DriverForReturnDto>(item);
        if(item ==null)
        {
            return NotFound();
        }
        _logger.LogInformation("Driver found");
        return Ok(outDriver);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDriver(Guid id, Driver data)
    {
        if (id != data.Id)
        {
            return BadRequest();
        }

        var drivers = await _context.Drivers.ToListAsync();
        var existingDriver = drivers.FirstOrDefault(x => x.Id == data.Id);
        if (existingDriver == null)
          return NotFound();
         
        existingDriver.DriverNumber = data.DriverNumber;
        existingDriver.FirstName = data.FirstName;
        existingDriver.LastName = data.LastName;
        existingDriver.WorldChampionships = data.WorldChampionships;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Driver updated");

        return NoContent();
    }

    [HttpDelete("{id}")]
    
    public async Task<IActionResult> DeleteDriver(Guid id)
    {
        var drivers = await _context.Drivers.ToListAsync();
        var item = drivers.FirstOrDefault(x => x.Id == id);
        if (item == null)
        {
            return NotFound();
        }

        item.Status = 0;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Driver deactivated");
        return NoContent();
    }
    
}
