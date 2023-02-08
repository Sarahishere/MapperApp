using AutoMapper;
using MapperApp.Core;
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
    private readonly IUnitOfWork _unitOfWork;

    public DriversController(ILogger<DriversController> logger,IMapper mapper,IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _mapper = mapper;
        _unitOfWork = unitOfWork;

    }

    //get
    [HttpGet]
    public IActionResult GetAllDrivers()
    {
        var allDrivers = _unitOfWork.Drivers.All();
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
            _unitOfWork.Drivers.Add(newDriver);
            _unitOfWork.CompleteChange();
            var outDriver = _mapper.Map<DriverForReturnDto>(newDriver);
            
            _logger.LogInformation("New driver created");
            return CreatedAtAction("GetDriver",new {outDriver.Id},outDriver);
        }

        return new JsonResult("Something went wrong"){StatusCode = 500};
    }

    [HttpGet]
    [Route("GetDriver")]
    public  IActionResult GetDriver(Guid id)
    {
        var item = _unitOfWork.Drivers.GetById(id);
        var outDriver = _mapper.Map<DriverForReturnDto>(item);
        if(item ==null)
        {
            return NotFound();
        }
        _logger.LogInformation("Driver found");
        return Ok(outDriver);
    }

    [HttpPatch]
    public IActionResult UpdateDriver( Driver data)
    {
        try{
            var update =_unitOfWork.Drivers.Update(data);
         _unitOfWork.CompleteChange();
        _logger.LogInformation("Driver updated");

        return NoContent();
        }
        catch(Exception e)
        {
            _logger.LogInformation($"{e}");
            throw;
        }
         
    }

    [HttpDelete]
    [Route("DeleteDriver")]
    
    public IActionResult DeleteDriver(Guid id)
    {
        var item = _unitOfWork.Drivers.GetById(id);
        if (item == null)
        {
            return NotFound();
        }

        _unitOfWork.Drivers.Delete(item);
        _unitOfWork.CompleteChange();
        _logger.LogInformation("Driver deactivated");
        return NoContent();
    }
    
}
