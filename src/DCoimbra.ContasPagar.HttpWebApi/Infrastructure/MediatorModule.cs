using System.Reflection;
using Autofac;
using DCoimbra.ContasPagar.Dominio;
using DCoimbra.Shared.MediatRBehavior;
using MediatR;

namespace DCoimbra.ContasPagar.HttpWebApi.Infrastructure;

public class MediatorModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var domainAssembly = typeof(Ambiente).GetTypeInfo().Assembly;
            builder
                .RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder
                .RegisterAssemblyTypes(domainAssembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>))
                .InstancePerDependency();
            builder
                .RegisterAssemblyTypes(domainAssembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));
            builder
                .Register<ServiceFactory>(context =>
                {
                    var componentContext = context.Resolve<IComponentContext>();

                    return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
                });
            
            builder
                .RegisterGeneric(typeof(EnrichLogContextBehavior<,>))
                .As(typeof(IPipelineBehavior<,>))
                .InstancePerDependency();

            // TODO : Verificar necessidade dos behaviors
            /*
             builder
                .RegisterGeneric(typeof(RetryDbConcurrencyBehavior<,>))
                .As(typeof(IPipelineBehavior<,>))
                .InstancePerDependency();

            builder
                .RegisterGeneric(typeof(UnitOfWorkBehavior<,>))
                .As(typeof(IPipelineBehavior<,>))
                .InstancePerDependency();

            builder
                .RegisterGeneric(typeof(TransactionIsolationLevelSerializableBehavior<,>))
                .As(typeof(IPipelineBehavior<,>))
                .InstancePerDependency();
            */
        }
    }