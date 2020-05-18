using MediatR;
using Moq;
using Microsoft.Extensions.Logging;
using Cnx.EarningsAndDeductions.API.Controllers;
using Xunit;
using System.Threading.Tasks;
using Cnx.EarningsAndDeductions.API.Application.Commands;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using Cnx.EarningsAndDeductions.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate;
using Cnx.EarningsAndDeductions.API.Application.Queries;

namespace Cnx.EarningsAndDeductions.UnitTests.Application
{
    public class EarningsAndDeductionsWebApiTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<ILogger<EarningsAndDeductionsController>> _loggerMock;
        private readonly Mock<IEDQueries> _edQueries;
        private readonly Mock<IHostingEnvironment> _hostingEnvironment;

        public EarningsAndDeductionsWebApiTest()
        {
            _mediatorMock = new Mock<IMediator>();
            _loggerMock = new Mock<ILogger<EarningsAndDeductionsController>>();
            _edQueries = new Mock<IEDQueries>();
            _hostingEnvironment = new Mock<IHostingEnvironment>();
        }

        [Fact]
        public async Task Add_SingleED_With_Duplicate_RequestId_ReturnsFailure()
        {
            //Arrange
            var requestId = Guid.NewGuid().ToString();
            var fakesingleManualEDCommand = this.GetManualEDCommandFake(isSingleAdd: true, isValidData: true);
            _mediatorMock.Setup(x => x.Send(It.IsAny<IRequest<bool>>(), default(System.Threading.CancellationToken)))
               .Returns(Task.FromResult(false));

            //Act
            var edController = new EarningsAndDeductionsController(_mediatorMock.Object, _loggerMock.Object, _hostingEnvironment.Object, _edQueries.Object);
            await edController.ManualED(fakesingleManualEDCommand, requestId);
            var actionResult = await edController.ManualED(fakesingleManualEDCommand, requestId);

            //Assert
            Assert.Equal((actionResult as BadRequestResult).StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Add_DuplicateED_ReturnsFailure()
        {
            //Arrange
            var fakesingleManualEDCommand = this.GetManualEDCommandFake(isSingleAdd: true, isValidData: true);
            _mediatorMock.Setup(x => x.Send(It.IsAny<IRequest<bool>>(), default(System.Threading.CancellationToken)))
               .Returns(Task.FromResult(false));

            //Act
            var edController = new EarningsAndDeductionsController(_mediatorMock.Object, _loggerMock.Object, _hostingEnvironment.Object, _edQueries.Object);
            var actionResult = await edController.ManualED(fakesingleManualEDCommand, Guid.NewGuid().ToString());
            var secondActionResult = await edController.ManualED(fakesingleManualEDCommand, Guid.NewGuid().ToString());

            //Assert
            Assert.Equal((secondActionResult as OkResult).StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Add_MultipleED_ReturnsSuccess()
        {
            //Arrange
            var fakeMultipleManualEDCommand = this.GetManualEDCommandFake(isSingleAdd: false, isValidData: true);
            _mediatorMock.Setup(x => x.Send(It.IsAny<IRequest<bool>>(), default(System.Threading.CancellationToken)))
               .Returns(Task.FromResult(true));

            //Act
            var edController = new EarningsAndDeductionsController(_mediatorMock.Object, _loggerMock.Object, _hostingEnvironment.Object, _edQueries.Object);
            var actionResult = await edController.ManualED(fakeMultipleManualEDCommand, Guid.NewGuid().ToString());

            //Assert
            Assert.Equal((actionResult as OkResult).StatusCode, (int)System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Add_MultipleED_With_InvalidData_ReturnsFailure()
        {
            //Arrange
            var fakeMultipleManualEDCommand = this.GetManualEDCommandFake(isSingleAdd: false, isValidData: false);
            _mediatorMock.Setup(x => x.Send(It.IsAny<IRequest<bool>>(), default(System.Threading.CancellationToken)))
               .Returns(Task.FromResult(false));

            //Act
            var edController = new EarningsAndDeductionsController(_mediatorMock.Object, _loggerMock.Object, _hostingEnvironment.Object, _edQueries.Object);
            var actionResult = await edController.ManualED(fakeMultipleManualEDCommand, Guid.NewGuid().ToString());

            //Assert
            Assert.Equal((actionResult as BadRequestResult).StatusCode, (int)System.Net.HttpStatusCode.BadRequest);
        }

        private ManualEDCommand GetManualEDCommandFake(bool isSingleAdd, bool isValidData)
        {
            List<ManualEDCommand.EarningsAndDeductions> addedList = isSingleAdd ? this.GetSingleEarningsAndDeductionFake() : this.GetMultipleEarningsAndDeductionsFake(isValidData);
            List<ManualEDCommand.EarningsAndDeductions> updatedList = new List<ManualEDCommand.EarningsAndDeductions>();
            List<ManualEDCommand.EarningsAndDeductions> deletedList = new List<ManualEDCommand.EarningsAndDeductions>();

            return new ManualEDCommand(addedList, updatedList, deletedList);
        }

        private List<ManualEDCommand.EarningsAndDeductions> GetSingleEarningsAndDeductionFake()
        {
            return new List<ManualEDCommand.EarningsAndDeductions>()
            {
                this.GetEarningsAndDeduction(true)
            };
        }

        private List<ManualEDCommand.EarningsAndDeductions> GetMultipleEarningsAndDeductionsFake(bool isValidData)
        {
            List<ManualEDCommand.EarningsAndDeductions> list = new List<ManualEDCommand.EarningsAndDeductions>();

            for (int i = 0; i < 5; i++)
            {
                if (isValidData)
                    list.Add(GetEarningsAndDeduction(true));
                else
                    list.Add(GetEarningsAndDeduction(i % 2 == 0));
            }

            return list;
        }

        private ManualEDCommand.EarningsAndDeductions GetEarningsAndDeduction(bool isValidData)
        {
            return new ManualEDCommand.EarningsAndDeductions()
            {
                Amount = 25,
                CoverageDateFrom = DateTime.Now,
                CoverageDateTo = isValidData ? DateTime.Now.AddDays(new Random().Next(100)) : DateTime.Now.AddDays(-1),
                EmployeeId = 1,
                FirstName = "Rahul",
                LastName = "K",
                PayCode = "PayCode",
                PayCodeDescription = "PayCodeDesciption",
                PayDateFrom = DateTime.Now,
                PayDateTo = DateTime.Now.AddDays(new Random().Next(100)),
                Remarks = "remarks"
            };
        }
    }
}
