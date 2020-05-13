using System.Linq;
using System.Reflection;
using Autofac;
using Cnx.EarningsAndDeductions.API.Application.Behaviors;
using Cnx.EarningsAndDeductions.API.Application.Commands;
using Cnx.EarningsAndDeductions.API.Application.DomainEventHandlers;
using Cnx.EarningsAndDeductions.API.Application.Validations;
using FluentValidation;
using MediatR;

namespace Cnx.Payroll.API.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
               .AsImplementedInterfaces();

            // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
            builder.RegisterAssemblyTypes(typeof(ManualEDCommand).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            // Register the Command's Validators (Validators based on FluentValidation library)
            builder.RegisterAssemblyTypes(typeof(ManualEDCommandValidator).GetTypeInfo().Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            // Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
            builder.RegisterAssemblyTypes(typeof(EDAddedDomainEventHandler).GetTypeInfo().Assembly)
              .AsClosedTypesOf(typeof(INotificationHandler<>));

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            //builder.RegisterGeneric(typeof(IdentifiedCommandHandler<,>)).As(typeof(IRequestHandler<,>));
            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}
