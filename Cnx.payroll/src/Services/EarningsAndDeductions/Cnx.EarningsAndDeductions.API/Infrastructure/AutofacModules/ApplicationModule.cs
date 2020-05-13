using Autofac;
using Cnx.EarningsAndDeductions.API.Application.Queries;
using Cnx.EarningsAndDeductions.API.Services;
using Cnx.EarningsAndDeductions.Domain.AggregateModel.EarningsAndDeductionAggegate;
using Cnx.EarningsAndDeductions.Domain.Interfaces;
using Cnx.EarningsAndDeductions.Infrastructure.Idempotency;
using Cnx.EarningsAndDeductions.Infrastructure.Repository;

namespace Cnx.Payroll.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RequestManager>()
              .As<IRequestManager>()
              .InstancePerLifetimeScope();

            builder.RegisterType<EDDomainService>()
               .As<IEDDomainService>()
               .InstancePerLifetimeScope();

            builder.RegisterType<EDRepository>()
               .As<IEDRepository>()
               .InstancePerLifetimeScope();

            builder.RegisterType<DuplicateEDRepository>()
               .As<IDuplicateEDRepository>()
               .InstancePerLifetimeScope();

            builder.RegisterType<EDQueries>()
                .As<IEDQueries>()
                .InstancePerLifetimeScope();

        }
    }
}
