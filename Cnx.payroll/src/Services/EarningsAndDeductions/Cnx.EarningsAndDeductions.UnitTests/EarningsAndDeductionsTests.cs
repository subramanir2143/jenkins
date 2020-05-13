using Cnx.EarningsAndDeductions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Cnx.EarningsAndDeductions.Domain.Events;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate;
using Cnx.EarningsAndDeductions.API.Application.DomainEventHandlers;

namespace Cnx.EarningsAndDeductions.UnitTests
{
    public class EarningsAndDeductionsTests
    {
        private readonly Mock<EDModelContext> _eDModelContext;
       
        private readonly Mock<IEDRepository> _repository;
        public EarningsAndDeductionsTests()
        {
            _eDModelContext = new Mock<EDModelContext>();
            _repository = new Mock<IEDRepository>();
        }
        [Fact]
        public void EDAddedDomainEventHandlerTest()
        {
            //_eDModelContext.Setup(m => m.SaveChanges()).Verifiable();
            //_eDModelContext.SetupGet(m => m.Database).Returns(_database.Object);
            //_eDModelContext.Setup(m => m.Database.EnsureCreated()).Verifiable();

            EDAddedDomainEvent eDAddedDomainEvent = new EDAddedDomainEvent(Guid.NewGuid(), 1, "", "", "", "", new DateTime(), new DateTime(), new DateTime(), new DateTime(), 1, "");
            EDAddedDomainEventHandler eDAddedDomainEventHandler = new EDAddedDomainEventHandler(_repository.Object);
            eDAddedDomainEventHandler.Handle(eDAddedDomainEvent, System.Threading.CancellationToken.None);

        }
    }
}
