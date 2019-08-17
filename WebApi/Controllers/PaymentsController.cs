using Application;
using Microsoft.AspNetCore.Mvc;
using Queries;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly Commands commands;
        private readonly Queries queries;

        public PaymentsController(Commands commands, Queries queries)
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

            var result = await commands.SendAsync(new DeletePayment(id));

            if (result.IsSuccess)
                return NoContent();

            return StatusCode(500, result.Error);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            return Content("Payment with Id " + id.ToString());
        }

        [HttpGet()]
        public async Task<IActionResult> ListAsync(DateTime? start = null, DateTime? end = null, int startIndex = 0, int pageSize = 30)
        {
            startIndex = Math.Max(startIndex, 0);
            pageSize = Math.Min(pageSize, 100);

            if (start == null)
                start = DateTime.Today.AddDays(-30);

            if (end == null)
                end = DateTime.Today.AddDays(1);

            var query = new ListPayments(start.Value, end.Value, new Paging(startIndex, pageSize));
            var result = await queries.RunAsync(query);

            return Ok(result);
        }

        public async Task<IActionResult> PostAsync(PaymentInputModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await commands.SendAsync(model.ToRegisterCommand());

                if (result.IsSuccess)
                    return Created(Url.Action("Get", new { id = model.Id }), null);

                return StatusCode(500, result.Error);
            }

            return BadRequest(ModelState);
        }
    }

    public class PaymentInputModel
    {
        public Guid Id { get; set; }

        public DateTimeOffset DateTime { get; set; }

        [Range(0.01, 999_999_999.99)]
        public decimal Amount { get; set; }

        public string Description { get; set; }

        public RegisterPayment ToRegisterCommand()
        {
            throw new NotImplementedException();
        }
    }
}