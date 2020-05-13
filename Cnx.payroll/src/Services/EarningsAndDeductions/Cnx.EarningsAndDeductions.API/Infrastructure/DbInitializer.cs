using Cnx.EarningsAndDeductions.Domain.Model;
using Cnx.EarningsAndDeductions.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Infrastructure
{
    public class DbInitializer
    {
        public async Task SeedAsync(EDModelContext _context,ILogger<DbInitializer> logger, IHostingEnvironment env, int retries = 3)
        {
            var policy = CreatePolicy(retries, logger, nameof(EDModelContext));
            var contentRootPath = env.ContentRootPath;

            await policy.ExecuteAsync(async () =>
            {
                _context.Database.EnsureCreated();

                if (!_context.PayCodeModel.Any())

                {
                    await _context.PayCodeModel.AddRangeAsync(GetDefinedPayCodesFromFile(contentRootPath, logger));
                    await _context.PayPeriodModel.AddRangeAsync(GetPayPeriodListForDataSeeding());
                    await _context.EmployeeDataModel.AddRangeAsync(GetEmployeeDataList());
                    await _context.SaveChangesAsync();
                }
            });
        }

        private PayCodeModel[] GetDefinedPayCodesFromFile(string contentRootPath, ILogger<DbInitializer> logger)
        {
            string paycodeFilePath = Path.Combine(contentRootPath, "Setup", "Paycode_Incentives.xlsx");
            BlockingCollection<PayCodeModel> payCodeList = new BlockingCollection<PayCodeModel>();

            try
            {
                FileStream stream = File.Open(paycodeFilePath, FileMode.Open, FileAccess.Read);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    Parallel.For(2, rowCount, row =>
                    {
                        var paycode = new PayCodeModel
                        {
                            PayCode = worksheet.Cells[row, 1].Value.ToString().Trim(),
                            PayCodeDescription = worksheet.Cells[row, 2].Value.ToString().Trim(),
                            IsActive = true
                        };

                        payCodeList.Add(paycode);
                    });
                }

                return payCodeList.ToArray();
               
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occured while seeding the paycode - {ex}");

                return payCodeList.ToArray();
            }
        }

        private List<PayPeriodModel> GetPayPeriodListForDataSeeding()
        {
            List<PayPeriodModel> modelList = new List<PayPeriodModel>();

            modelList.Add(new PayPeriodModel()
            {
                PayPeriodId = Guid.NewGuid(),
                PayDateFrom = DateTime.Parse("June 30 2019"),
                PayDateTo = DateTime.Parse("July 13 2019"),
                IsLocked = true
            });
            modelList.Add(new PayPeriodModel()
            {
                PayPeriodId = Guid.NewGuid(),
                PayDateFrom = DateTime.Parse("July 14 2019"),
                PayDateTo = DateTime.Parse("July 27 2019"),
                IsLocked = false
            });
            modelList.Add(new PayPeriodModel()
            {
                PayPeriodId = Guid.NewGuid(),
                PayDateFrom = DateTime.Parse("July 28 2019"),
                PayDateTo = DateTime.Parse("August 10 2019"),
                IsLocked = false
            });

            return modelList;
        }

        private List<EmployeeDataModel> GetEmployeeDataList()
        {
            List<EmployeeDataModel> modelList = new List<EmployeeDataModel>();

            modelList.Add(new EmployeeDataModel()
            {
                EmployeeId = 101221,
                FirstName = "Joseph",
                LastName = "Michael",
                IsActive = true
            });
            modelList.Add(new EmployeeDataModel()
            {
                EmployeeId = 111223,
                FirstName = "JM",
                LastName = "Mamaril",
                IsActive = true
            });
            modelList.Add(new EmployeeDataModel()
            {
                EmployeeId = 111673,
                FirstName = "Gre",
                LastName = "Taguran",
                IsActive = true
            });

            return modelList;
        }

        private AsyncRetryPolicy CreatePolicy(int retries, ILogger<DbInitializer> logger, string prefix)
        {
            return Policy.Handle<MySqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }
    }
}
