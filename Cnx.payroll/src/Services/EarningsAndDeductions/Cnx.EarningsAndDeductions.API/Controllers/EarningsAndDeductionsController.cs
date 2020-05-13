using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cnx.EarningsAndDeductions.API.Application.Commands;
using Cnx.EarningsAndDeductions.API.Application.Queries;
using Cnx.EarningsAndDeductions.API.Infrastructure.Extensions;
using Cnx.EarningsAndDeductions.API.Infrastructure.Services;
using Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate;
using Cnx.EarningsAndDeductions.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cnx.EarningsAndDeductions.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EarningsAndDeductionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EarningsAndDeductionsController> _logger;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IEDQueries _edQueries;

        public EarningsAndDeductionsController(IMediator mediator, 
            ILogger<EarningsAndDeductionsController> logger,
            IHostingEnvironment hostingEnvironment,
            IEDQueries edQueries)            
        {
            _mediator = mediator;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _edQueries = edQueries;
        }   

        [HttpPost]
        [Route("Upload")]
        public async Task<ActionResult> UploadExcelAsync(IFormFile file, [FromHeader(Name = "x-requestid")] string requestId)
        {
            Unit commandResult;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty/* && file?.Length > 0*/)
            {
                await new UploadService().UploadFileAsync(file, _hostingEnvironment.ContentRootPath);
                var command = new EDUploadCommand(file.FileName, "UploaderName"); //TODO#: replace with logged in user name
                var requestUploadED = new IdentifiedCommand<EDUploadCommand, Unit>(command, guid);
                requestUploadED.Command.Id = Guid.NewGuid();
                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    requestUploadED.GetGenericTypeName(),
                    nameof(requestUploadED.Command.FileName),
                    requestUploadED.Command.FileName,
                    requestUploadED);

                commandResult = await _mediator.Send(requestUploadED);
            }

            if (!commandResult.Equals(Unit.Value))
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("save")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ManualED([FromBody]ManualEDCommand command, [FromHeader(Name = "x-requestid")] string requestId)
        {
            bool commandResult = false;

            if (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty)
            {
                var mannualEDCommandRequest = new IdentifiedCommand<ManualEDCommand, bool>(command, guid);
                mannualEDCommandRequest.Command.Id = Guid.NewGuid();

                _logger.LogInformation($"----- Sending command: {mannualEDCommandRequest.GetGenericTypeName()} - {nameof(mannualEDCommandRequest.Command)}: {command}");

                commandResult = await _mediator.Send(mannualEDCommandRequest);
            }

            if (commandResult)
                return Ok();

            return BadRequest();
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            List<EDViewModel> edList = _edQueries.ReadEDViewModelList();

            return Ok(edList);
        }

        [HttpGet]
        [Route("paycodes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetPayCodeList()
        {
            List<PayCodeViewModel> payCodeList = _edQueries.ReadPayCodeViewModelList();

            return Ok(payCodeList);
        }

        [HttpGet]
        [Route("employees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetEmployeeList()
        {
            List<EmployeeViewModel> employeeList = _edQueries.ReadEmployeeViewModelList();

            return Ok(employeeList);
        }
    }
}
