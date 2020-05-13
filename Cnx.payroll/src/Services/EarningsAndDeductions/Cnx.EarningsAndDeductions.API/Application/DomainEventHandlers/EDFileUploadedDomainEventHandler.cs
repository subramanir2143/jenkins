using Cnx.EarningsAndDeductions.API.Infrastructure.Extensions;
using Cnx.EarningsAndDeductions.Domain;
using Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate;
using Cnx.EarningsAndDeductions.Domain.AggregatesModel.Interfaces;
using Cnx.EarningsAndDeductions.Domain.Events;
using Cnx.EarningsAndDeductions.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cnx.EarningsAndDeductions.API.Application.DomainEventHandlers
{
    public class EDFileUploadedDomainEventHandler
        : INotificationHandler<EDFileUploadedDomainEvent>
    {
        private readonly ILogger<EDFileUploadedDomainEventHandler> _logger;
        private readonly FileSettings _settings;
        private readonly IDbSession _session;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EDFileUploadedDomainEventHandler(ILogger<EDFileUploadedDomainEventHandler> logger,
            IOptions<FileSettings> settings, IDbSession session, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _settings = settings.Value;
            _session = session;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task Handle(EDFileUploadedDomainEvent notification, CancellationToken cancellationToken)
        {
            string path = Path.Combine(_hostingEnvironment.ContentRootPath, notification.FileName);

            int employeeId;
            decimal amount;
            bool isValid;
            string message = string.Empty;
            int invalidCount = 0;
            List<EarningsAndDeductionItem> earningsAndDeductionItems = null;
            if (notification.FileName.EndsWith(".xlsx"))
            {
                earningsAndDeductionItems = ExcelToList(path);
            }
            else
            {
                earningsAndDeductionItems = CsvToList(path);
            }

            int row = 0;
            int fileId = notification.FileName.GetHashCode();
            foreach(var request in earningsAndDeductionItems)
            {
                isValid = true;
                row++;

                if (!EDConstants.ValidFileActions.ToList().Contains(request.Action))
                {
                    message = EDConstants.InvalidAction;

                    isValid = false;
                }

                else if (!int.TryParse(request.EmployeeId, out employeeId))
                {
                    message = EDConstants.InvalidEmployeeId;
                    isValid = false;
                }

                else if (!decimal.TryParse(request.Amount, out amount))
                {
                    message = EDConstants.InvalidAmount;
                    isValid = false;
                }

                else if (request.PayDateFrom.Date > request.PayDateTo.Date)
                {
                    message = EDConstants.InvalidPayDates;
                    isValid = false;
                }
                else
                {
                    //TODO#: Check for Duplicates
                }

                //TODO#: Uncomment after merge 

                //if (isValid)
                //{
                //    switch (request.Action)
                //    {
                //        case "A":
                //            //var earningsAndDeductions = new EarningsAndDeduction(Guid.NewGuid(),
                //            //    Convert.ToInt32(request.EmployeeId), request.PayCode, request.PayDateFrom,
                //            //    request.PayDateTo, request.CoverageDateFrom, request.CoverageDateTo,
                //            //    Convert.ToDecimal(request.Amount), request.Remarks, fileId);
                //            //await _session.Add(earningsAndDeductions);
                //            break;

                //    }

                //}
                //else
                //{
                //    invalidCount++;
                //    var earningsAndDeductionsFile = await _session.
                //    Get<EarningsAndDeductionsFile>(notification.Id);
                //    earningsAndDeductionsFile.InvalidRecord(request.EmployeeId, request.PayCode,
                //        request.PayDateFrom.ToShortDateString(), request.PayDateTo.ToShortDateString(),
                //        request.CoverageDateFrom.ToShortDateString(),
                //        request.CoverageDateTo.ToShortDateString(), request.Amount, request.Remarks, request.Action,
                //        message, (int)row);
                //}

                //await _session.Commit();
            }

            var earningsAndDeductionsFileFinal = await _session.
                        Get<EarningsAndDeductionsFile>(notification.Id);
            earningsAndDeductionsFileFinal.FileProcessed(EDConstants.UploadStatusMessage);
        }

        /// <summary>
        /// Convert excel items to List of earnings and deductions
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <returns>Collection of earnings and deductions</returns>
        private List<EarningsAndDeductionItem> ExcelToList(string filePath)
        {
            BlockingCollection<EarningsAndDeductionItem> earningsAndDeductionItems = new BlockingCollection<EarningsAndDeductionItem>();
            FileInfo fileInfo = new FileInfo(filePath);
            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;
                Parallel.For(2, rowCount, row =>
                {
                    var request = new EarningsAndDeductionItem
                    {
                        EmployeeId = worksheet.Cells[row, 1].Value.ToString().Trim(),
                        PayCode = worksheet.Cells[row, 2].Value.ToString().Trim(),
                        PayDateFrom = DateTime.FromOADate((double)worksheet.Cells[row, 3].Value),
                        PayDateTo = DateTime.FromOADate((double)worksheet.Cells[row, 4].Value),
                        CoverageDateFrom = DateTime.FromOADate((double)worksheet.Cells[row, 5].Value),
                        CoverageDateTo = DateTime.FromOADate((double)worksheet.Cells[row, 6].Value),
                        Amount = worksheet.Cells[row, 7].Value.ToString().Trim(),
                        Remarks = worksheet.Cells[row, 8].Value != null ?
                        worksheet.Cells[row, 8].Value.ToString().Trim() : string.Empty,
                        Action = worksheet.Cells[row, 9].Value.ToString().Trim(),
                        TransactionId = worksheet.Cells[row, 10].Value != null ?
                        worksheet.Cells[row, 10].Value.ToString().Trim() : string.Empty,
                        SequenceNumber = row
                    };

                    earningsAndDeductionItems.Add(request);
                });
                
            }

            return earningsAndDeductionItems.OrderBy(x => x.SequenceNumber).ToList();
        }

        private List<EarningsAndDeductionItem> CsvToList(string filePath)
        {
            var earningsAndDeductionItems = File.ReadLines(filePath)
               .Skip(1)
               .Select(row => row.Split(','))
               .SelectTry(column => GetEarningsAndDeductions(column))
               .OnCaughtException(x => { return null; })
               .Where(x => x != null);
            return earningsAndDeductionItems.ToList();
        }

        private EarningsAndDeductionItem GetEarningsAndDeductions(string[] column)
        {
            return new EarningsAndDeductionItem
            {
                EmployeeId = column[0].Trim(),
                PayCode = column[1].Trim(),
                PayDateFrom = Convert.ToDateTime(column[2].Trim()),
                PayDateTo = Convert.ToDateTime(column[3].Trim()),
                CoverageDateFrom = Convert.ToDateTime(column[4].Trim()),
                CoverageDateTo = Convert.ToDateTime(column[5].Trim()),
                Amount = column[6].Trim(),
                Remarks = column[7] != null ?
                        column[7].Trim() : string.Empty,
                Action = column[8] != null ?
                        column[8].Trim() : string.Empty,
                TransactionId = column[9] != null ?
                        column[9].Trim() : string.Empty
            };
        }
    }

    internal class EarningsAndDeductionItem
    {
        public string EmployeeId { get; set; }

        public string PayCode { get; set; }

        public DateTime PayDateFrom { get; set; }

        public DateTime PayDateTo { get; set; }

        public DateTime CoverageDateFrom { get; set; }

        public DateTime CoverageDateTo { get; set; }

        public string Amount { get; set; }

        public string Remarks { get; set; }

        public string Action { get; set; }

        public string TransactionId { get; set; }

        public int SequenceNumber { get; set; }
    }
}
