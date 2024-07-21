using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Station.Data;
using Station.dto;
using Station.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Station.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReaderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/Reader/Get/{subName}
        [HttpGet("GetSubscriber/{subName}")]
        public async Task<IActionResult> GetSubscriber(string subName)
        {
            var subscribers = await _context.Subscribers.Where(s => s.FullName == subName).Include(a => a.Meter).ToListAsync();

            if (subscribers == null)
            {
                return NotFound(new { message = "Subscriber not found" });
            }

            List<SearchSubscriberDto> searchSubscriberListResult = new();

            foreach (var subscriber in subscribers)
            {
                searchSubscriberListResult.Add(
                    new SearchSubscriberDto
                    {
                        SubscriberName = subscriber.FullName,
                        MeterNumber = subscriber.Meter.numberMeter,
                        MeterId = subscriber.MeterId
                    }
                );
            }

            return Ok(searchSubscriberListResult);
        }



        [HttpPost("GetMeterDetails")]
        public async Task<IActionResult> GetMeterDetails([FromBody] int meterId)
        {
            var meter = await _context.Meters
                .Include(i => i.Item).Include(r => r.reading)
                .Include(s => s.Subscriber)
                .ThenInclude(s => s.Subscription).FirstOrDefaultAsync(m => m.Id == meterId);

            var subscriberDetailsDto = new SubscriberDetailsDto()
            {
                SubscriberName = meter.Subscriber.FullName,
                MeterNumber = meter.numberMeter,
                MeterId = meter.Id,
                ItemName = meter.Item.Name,
                ItemId = meter.Item.Id,
                SubscriptionType = meter.Subscriber.Subscription.Type,
                PreviousReading = meter.reading.OrderByDescending(d => d.dateTime).Select(r => r.CurrentRead).FirstOrDefault()
            };

            return Ok(subscriberDetailsDto);
        }

        [HttpPost("PostReading")]
        public async Task<IActionResult> PostReading([FromBody] MeterReadingDto meterReadingDto)
        {
            var reading = new Reading
            {
                CurrentRead = meterReadingDto.CurrentReading,
                MeterId = meterReadingDto.MeterId,
                ItemId = meterReadingDto.ItemId,
                dateTime = DateTime.Now
            };

            var result = await _context.Readings.AddAsync(reading);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

