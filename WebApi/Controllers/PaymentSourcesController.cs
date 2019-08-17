using Application;
using Microsoft.AspNetCore.Mvc;
using Queries;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("payment-sources")]
    public class PaymentSourcesController : ControllerBase
    {
        private readonly Commands commands;
        private readonly Queries queries;

        public PaymentSourcesController(Commands commands, Queries queries)
        {
            this.commands = commands;
            this.queries = queries;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                ModelState.AddModelError(nameof(id), "The id must be provided.");
                return BadRequest(ModelState);
            }

            var result = await commands.SendAsync(new DeletePaymentSource(id));

            if (result.IsSuccess)
                return NoContent();

            return StatusCode(500, result.Error);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return Content("Payment Source with Id " + id.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(PaymentSourceInputModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await commands.SendAsync(model.ToAddCommand());

                if (result.IsSuccess)
                    return Created(Url.Action("Get", new { id = model.Id }), null);

                return StatusCode(500, result.Error);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(PaymentSourceInputModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await commands.SendAsync(model.ToChangeNameCommand());

                if (result.IsSuccess)
                    return NoContent();

                return StatusCode(500, result.Error);
            }

            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> Summary(DateTime? start, DateTime? end)
        {
            if (start == null)
                start = DateHelpers.MonthStart();

            if (end == null)
                end = DateHelpers.MonthEnd();

            var result = await queries.RunAsync(new ListSourcesWithAmounts(start.Value, end.Value));
            return Ok(result);
        }
    }

    public class PaymentSourceInputModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        public AddPaymentSource ToAddCommand() => new AddPaymentSource(Id, Name);
        public ChangePaymentSourceName ToChangeNameCommand() => new ChangePaymentSourceName(Id, Name);
    }
}