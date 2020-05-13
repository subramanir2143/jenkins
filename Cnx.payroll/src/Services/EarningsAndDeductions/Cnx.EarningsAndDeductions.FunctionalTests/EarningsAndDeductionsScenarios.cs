using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Cnx.EarningsAndDeductions.FunctionalTests.Extensions;
using Cnx.EarningsAndDeductions.API.Application.Commands;
using System.Linq;

namespace Cnx.EarningsAndDeductions.FunctionalTests
{
    public class EarningsAndDeductionsScenarios : EarningsAndDeductionsScenarioBase
    {
        [Fact]
        public async Task Get_downloadTemplate()
        {
            using (var edServer = new EarningsAndDeductionsScenarioBase().CreateServer())
            {
                var edClient = edServer.CreateIdempotentClient();
                //var employeeDetails = GetNewEmployee();

                var res = await edClient.GetAsync(EarningsAndDeductionsScenarioBase.Get.DownloadTemplate);

                // after insert the status code should be 200
                Assert.Equal(System.Net.HttpStatusCode.OK, res.StatusCode);
            }
        }

        [Fact]
        public async Task Add_DuplicateED_ReturnsFailure()
        {
            using (var edServer = new EarningsAndDeductionsScenarioBase().CreateServer())
            {
                var edClient = edServer.CreateIdempotentClient();

                var fakesingleManualEDCommand = this.GetManualEDCommandFake(isSingleAdd: true, isValidData: true);
                await edClient.PostAsync(EarningsAndDeductionsScenarioBase.Post.ManualED,
                    new StringContent(JsonConvert.SerializeObject(fakesingleManualEDCommand), UTF8Encoding.UTF8, "application/json"));

                var res = await edClient.PostAsync(EarningsAndDeductionsScenarioBase.Post.ManualED,
                    new StringContent(JsonConvert.SerializeObject(fakesingleManualEDCommand), UTF8Encoding.UTF8, "application/json"));

                // after insert the status code should be 200
                Assert.Equal(System.Net.HttpStatusCode.OK, res.StatusCode);
            }
        }

        [Fact]
        public async Task Add_MultipleED_ReturnsSuccess()
        {
            using (var edServer = new EarningsAndDeductionsScenarioBase().CreateServer())
            {
                var edClient = edServer.CreateIdempotentClient();

                var fakesingleManualEDCommand = this.GetManualEDCommandFake(isSingleAdd: false, isValidData: true, isAdd: true);

                var res = await edClient.PostAsync(EarningsAndDeductionsScenarioBase.Post.ManualED,
                    new StringContent(JsonConvert.SerializeObject(fakesingleManualEDCommand), UTF8Encoding.UTF8, "application/json"));

                // after insert the status code should be 200
                Assert.Equal(System.Net.HttpStatusCode.OK, res.StatusCode);
            }
        }

        [Fact]
        public async Task Add_MultipleED_With_InvalidData_ReturnsFailure()
        {
            using (var edServer = new EarningsAndDeductionsScenarioBase().CreateServer())
            {
                var edClient = edServer.CreateIdempotentClient();

                var fakesingleManualEDCommand = this.GetManualEDCommandFake(isSingleAdd: false, isValidData: false, isAdd: true);

                var res = await edClient.PostAsync(EarningsAndDeductionsScenarioBase.Post.ManualED,
                    new StringContent(JsonConvert.SerializeObject(fakesingleManualEDCommand), UTF8Encoding.UTF8, "application/json"));

                // after insert the status code should be 400
                Assert.Equal(System.Net.HttpStatusCode.BadRequest, res.StatusCode);
            }
        }

        [Fact]
        public async Task Update_ed_returns_success()
        {
            using (var edServer = new EarningsAndDeductionsScenarioBase().CreateServer())
            {
                var edClient = edServer.CreateIdempotentClient();
                var fakesingleManualEDCommand = this.GetManualEDCommandFake(isSingleAdd: false, isValidData: true, isAdd: true);

                await edClient.PostAsync(EarningsAndDeductionsScenarioBase.Post.ManualED,
                    new StringContent(JsonConvert.SerializeObject(fakesingleManualEDCommand), UTF8Encoding.UTF8, "application/json"));

                var records = await edClient.GetAsync(EarningsAndDeductionsScenarioBase.Get.GetRecords);
                var recordToBeUpdatedList = records.Content.ReadAsAsync<List<ManualEDCommand.EarningsAndDeductions>>().Result;

                var recordToBeUpdated = recordToBeUpdatedList.Skip(1).FirstOrDefault();
                recordToBeUpdated.PayDateTo = recordToBeUpdated.PayDateTo.AddDays(10);
                edClient = edServer.CreateIdempotentClient();

                var res = await edClient.PostAsync(EarningsAndDeductionsScenarioBase.Post.ManualED,
                    new StringContent(JsonConvert.SerializeObject(GetEarningsAndDeduction_Type2(recordToBeUpdated)), UTF8Encoding.UTF8, "application/json"));

                // after update the status code should be 200
                Assert.Equal(System.Net.HttpStatusCode.OK, res.StatusCode);
            }
        }

        [Fact]
        public async Task Update_ed_duplicate_returns_failure()
        {
            using (var edServer = new EarningsAndDeductionsScenarioBase().CreateServer())
            {
                var edClient = edServer.CreateIdempotentClient();
                var fakesingleManualEDCommand = this.GetManualEDCommandFake(isSingleAdd: false, isValidData: true, isAdd: true);

                await edClient.PostAsync(EarningsAndDeductionsScenarioBase.Post.ManualED,
                    new StringContent(JsonConvert.SerializeObject(fakesingleManualEDCommand), UTF8Encoding.UTF8, "application/json"));

                var records = await edClient.GetAsync(EarningsAndDeductionsScenarioBase.Get.GetRecords);
                var recordToBeUpdatedList = records.Content.ReadAsAsync<List<ManualEDCommand.EarningsAndDeductions>>().Result;

                var recordToBeUpdated = recordToBeUpdatedList.Skip(1).FirstOrDefault();

                edClient = edServer.CreateIdempotentClient();

                var res = await edClient.PostAsync(EarningsAndDeductionsScenarioBase.Post.ManualED,
                    new StringContent(JsonConvert.SerializeObject(GetEarningsAndDeduction_Type2(recordToBeUpdated)), UTF8Encoding.UTF8, "application/json"));

                // after update the status code should be 200
                Assert.Equal(System.Net.HttpStatusCode.OK, res.StatusCode);
            }
        }

        private ManualEDCommand GetManualEDCommandFake(bool isAdd = false, bool isUpdate = false, bool isDelete = false, bool isSingleAdd = false, bool isSingleUpdate = false, bool isSingleDelete = false, bool isValidData = false)
        {
            List<ManualEDCommand.EarningsAndDeductions> addedList = isAdd && isSingleAdd ? this.GetSingleEarningsAndDeductionFake() : this.GetMultipleEarningsAndDeductionsFake(isValidData);
            List<ManualEDCommand.EarningsAndDeductions> updatedList = new List<ManualEDCommand.EarningsAndDeductions>();//isUpdate && isSingleUpdate;
            List<ManualEDCommand.EarningsAndDeductions> deletedList = new List<ManualEDCommand.EarningsAndDeductions>();

            return new ManualEDCommand(addedList, updatedList, deletedList);
        }

        private List<ManualEDCommand.EarningsAndDeductions> GetSingleEarningsAndDeductionFake()
        {
            return new List<ManualEDCommand.EarningsAndDeductions>()
            {
                this.GetEarningsAndDeduction_Type1(true)
            };
        }

        private List<ManualEDCommand.EarningsAndDeductions> GetMultipleEarningsAndDeductionsFake(bool isValidData)
        {
            List<ManualEDCommand.EarningsAndDeductions> list = new List<ManualEDCommand.EarningsAndDeductions>();

            for (int i = 0; i < 5; i++)
            {
                if (isValidData)
                    list.Add(GetEarningsAndDeduction_Type1(true));
                else
                    list.Add(GetEarningsAndDeduction_Type1(i % 2 == 0));
            }

            return list;
        }

        private ManualEDCommand.EarningsAndDeductions GetEarningsAndDeduction_Type1(bool isValidData)
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

        private ManualEDCommand GetEarningsAndDeduction_Type2(ManualEDCommand.EarningsAndDeductions earningsAndDeductions)
        {
            return new ManualEDCommand(updated: new List<ManualEDCommand.EarningsAndDeductions>()
            {
                earningsAndDeductions
            }, added: new List<ManualEDCommand.EarningsAndDeductions>(),
            deleted: new List<ManualEDCommand.EarningsAndDeductions>());
        }
    }
}
