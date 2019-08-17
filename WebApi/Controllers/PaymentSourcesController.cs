using Application;
using Microsoft.AspNetCore.Mvc;
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

        public PaymentSourcesController(Commands commands)
        {
            this.commands = commands;
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