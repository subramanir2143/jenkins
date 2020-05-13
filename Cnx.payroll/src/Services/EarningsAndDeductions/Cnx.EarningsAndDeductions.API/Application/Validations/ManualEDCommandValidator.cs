using Cnx.EarningsAndDeductions.API.Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Cnx.EarningsAndDeductions.API.Application.Commands.ManualEDCommand;

namespace Cnx.EarningsAndDeductions.API.Application.Validations
{
    public class ManualEDCommandValidator : AbstractValidator<ManualEDCommand>
    {
        public ManualEDCommandValidator()
        {
            RuleFor(command => command.Added).SetCollectionValidator(new EarningsAndDeductionValidator());            
        }
    }

    public class EarningsAndDeductionValidator : AbstractValidator<ManualEDCommand.EarningsAndDeductions>
    {
        public EarningsAndDeductionValidator()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.PayCodeDescription).NotEmpty();
            RuleFor(x => x.PayDateFrom).NotEmpty().LessThan(x => x.PayDateTo);
            RuleFor(x => x.PayDateTo).NotEmpty();
            RuleFor(x => x.CoverageDateTo).NotEmpty();
            RuleFor(x => x.CoverageDateFrom).NotEmpty().LessThan(x => x.CoverageDateTo);
            RuleFor(x => x.Amount).NotEmpty().Must(BeLessThanOrEqualToTwoDecimalPlaces).WithMessage("Amount has a maximum of two decimal places.");
        }

        private bool BeLessThanOrEqualToTwoDecimalPlaces(decimal amount)
        {
            int count = BitConverter.GetBytes(decimal.GetBits(amount)[3])[2];

            return count <= 2;
        }
    }
}
