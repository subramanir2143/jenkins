using System.Threading;
using System.Threading.Tasks;
using Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate;
using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces;
using Cnx.EarningsAndDeductions.Infrastructure.Idempotency;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Cnx.EarningsAndDeductions.API.Application.Commands
{
    public class EDUploadCommandHandler : IRequestHandler<EDUploadCommand, Unit>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EDUploadCommandHandler> _logger;
        private readonly IDbSession _session;

        public EDUploadCommandHandler(IMediator mediator,
            ILogger<EDUploadCommandHandler> logger,
            IDbSession session)
        {
            _mediator = mediator;
            _logger = logger;
            _session = session;
        }

        public async Task<Unit> Handle(EDUploadCommand request, CancellationToken cancellationToken)
        {
            var uploadedFile = new EarningsAndDeductionsFile(request.Id, request.FileName, request.Uploader);
            _logger.LogInformation($"Uploading File: {uploadedFile}");

            await _session.Add(uploadedFile, cancellationToken);
            await _session.Commit(cancellationToken);
            return Unit.Value; 
        }
    }


    // Use for Idempotency in Command process
    public class EDUploadIdentifiedCommandHandler : IdentifiedCommandHandler<EDUploadCommand, Unit>
    {
        public EDUploadIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager,
            ILogger<IdentifiedCommandHandler<EDUploadCommand, Unit>> logger)
            : base(mediator, requestManager, logger)
        {
        }

        protected override Unit CreateResultForDuplicateRequest()
        {
            return Unit.Value;                // Ignore duplicate requests for creating order.
        }
    }
}
