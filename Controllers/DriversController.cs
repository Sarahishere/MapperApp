using AutoMapper;
using MapperApp.Models;
using MapperApp.Models.DTOs.Incoming;
using MapperApp.Models.DTOs.Outgoing;
using Microsoft.AspNetCore.Mvc;

namespace MapperApp.Controllers;

[ApiController]
[Route("[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<DriversController> _logger;
    private readonly IMapper _mapper;
    private static List<Driver> drivers = new List<Driver>();

    public DriversController(ILogger<DriversController> logger,IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;

    }

    //get
    [HttpGet]
    public IActionResult GetAllDrivers()
    {
        var allDrivers = drivers.Where(x => x.Status == 1).ToList();
        var outDrivers = _mapper.Map<IEnumerable<DriverForReturnDto>>(allDrivers);
        return Ok(outDrivers);
    }

    //post
    [HttpPost]
    public IActionResult CreateDriver(DriverForCreationDto data)
    {
        if(ModelState.IsValid)
        {
            var newDriver = _mapper.Map<Driver>(data);
            drivers.Add(newDriver);
            var outDriver = _mapper.Map<DriverForReturnDto>(newDriver);
            
            _logger.LogInformation("New driver created");
            return CreatedAtAction("GetDriver",new {outDriver.Id},outDriver);
        }

        return new JsonResult("Something went wrong"){StatusCode = 500};
    }

    [HttpGet("{id}")]
    public IActionResult GetDriver(Guid id)
    {

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
    public IActionResult UpdateDriver(Guid id, Driver data)
    {
        if (id != data.Id)
        {
            return BadRequest();
        }

        var existingDriver = drivers.FirstOrDefault(x => x.Id == data.Id);
        if (existingDriver == null)
          return NotFound();
         
        existingDriver.DriverNumber = data.DriverNumber;
        existingDriver.FirstName = data.FirstName;
        existingDriver.LastName = data.LastName;
        existingDriver.WorldChampionships = data.WorldChampionships;
        _logger.LogInformation("Driver updated");

        return NoContent();
    }

    [HttpDelete("{id}")]
    
    public IActionResult DeleteDriver(Guid id)
    {
        var item = drivers.FirstOrDefault(x => x.Id == id);
        if (item == null)
        {
            return NotFound();
        }

        item.Status = 0;
        _logger.LogInformation("Driver deactivated");
        return NoContent();
    }
    
}
