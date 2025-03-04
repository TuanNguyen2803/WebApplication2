using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.DTO;
using WebApplication2.Service;

namespace WebApplication2.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    //private readonly NumberInitializationService numberInitializationService;
    public class NumberInitializationController : ControllerBase
    {
        //[HttpPost("initialize-schedule")]
        //public IActionResult InitializeAndSchedule([FromBody] CronInitializationRequest request)
        //{
        //    try
        //    {
        //        // Lên lịch job để khởi tạo số liệu theo biểu thức cron
        //        RecurringJob.AddOrUpdate<NumberInitializationService>(
        //            "initialize-numbers",
        //            service => service.InitializeNumbers(),
        //            request.CronExpression
        //        );

        //        return Ok($" {request.CronExpression}");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Error scheduling number initialization: {ex.Message}");
        //    }
        //}
        [HttpPost("initialize-schedule")]
        public IActionResult InitializeAndSchedule([FromQuery] CronRequest request)
        {
            try
            {
                // Chuyển đổi request thành cron expression
                string cronExpression = ConvertToCron(request);

                // Lên lịch job với cron expression
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
            string minute = request.Minute.HasValue ? request.Minute.Value.ToString() : "*";
            string hour = request.Hour.HasValue ? request.Hour.Value.ToString() : "*";
            string day = request.Day.HasValue ? request.Day.Value.ToString() : "*";
            string month = request.Month.HasValue ? request.Month.Value.ToString() : "*";
            string dayOfWeek = request.DayOfWeek.HasValue ? (request.DayOfWeek.Value == 0 ? "7" : request.DayOfWeek.Value.ToString()) : "?"; // 0 hoặc 7 là Chủ Nhật
            string year = request.Year.HasValue ? request.Year.Value.ToString() : "*";

            if (dayOfWeek == "?" && day == "*")
            {
                dayOfWeek = "*";
            }

            return $"{minute} {hour} {day} {month} {dayOfWeek} {year}";
        }
    }
}
