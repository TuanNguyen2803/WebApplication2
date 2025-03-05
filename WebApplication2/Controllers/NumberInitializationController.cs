using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Configuration;
using WebApplication2.DTO;
using WebApplication2.Service;

namespace WebApplication2.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NumberInitializationController : ControllerBase
    {
        private readonly ISystemJobSettings _jobSettings;
        public NumberInitializationController(ISystemJobSettings jobSettings)
        {
            _jobSettings = jobSettings;
        }
        [HttpPost("initialize-schedule")]
        public IActionResult InitializeAndSchedule([FromQuery] CronRequest request)
        {
            try
            {

                string cronExpression = ConvertToCron(request);


                RecurringJob.AddOrUpdate<NumberInitializationService>(
                    "initialize-numbers",
                    service => service.InitializeNumbers(),
                    cronExpression
                );

                return Ok($"Scheduled with cron expression: {cronExpression}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error scheduling number initialization: {ex.Message}");
            }
        }
        private string ConvertToCron(CronRequest request)
        {
            if (request.Minute.HasValue)
            {
                return $"*/{request.Minute.Value} * * * *"; 
            }
            else if (request.Hour.HasValue)
            {
                return $"0 {request.Hour.Value} * * *"; 
            }
            else if (request.Day.HasValue)
            {
                return $"0 0 */{request.Day.Value} * *"; 
            }
            else if (request.Month.HasValue)
            {
                return $"0 0 1 */{request.Month.Value} *"; 
            }
            else if (request.DayOfWeek.HasValue)
            {
                return $"0 0 * * {request.DayOfWeek.Value}"; 
            }
            else if (request.Year.HasValue)
            {
                return $"0 0 1 1 */{request.Year.Value}"; 
            }
            else
            {
                return "* * * * *";
            }
        }
        [HttpPut("schedule-update-chunks")]
        public IActionResult ScheduleUpdateNumbersInChunks([FromQuery] CronRequest request)
        {
            try
            {
                var cronExpression = ConvertToCron(request);

                RecurringJob.AddOrUpdate<NumberInitializationService>(
                    "update-numbers-in-chunks",
                    service => service.UpdateNumbersInChunks(),
                    cronExpression);

                return Ok($"Scheduled chunked update with cron expression: {cronExpression}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error scheduling chunked update: {ex.Message}");
            }
        }

    }
}
