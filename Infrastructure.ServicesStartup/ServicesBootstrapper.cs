using System;
using Autofac;
using Cmas.Infrastructure.ErrorHandler.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Bootstrappers.Autofac;
using Nancy.Responses.Negotiation;

namespace Cmas.Infrastructure.ServicesStartup
{
    internal class ServicesBootstrapper : AutofacNancyBootstrapper
    {
        private readonly IServiceProvider _serviceProvider = null;

        public ServicesBootstrapper(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during application startup.
        }

        protected override void ConfigureApplicationContainer(ILifetimeScope existingContainer)
        {
            base.ConfigureApplicationContainer(existingContainer);
        }

        protected override void ConfigureRequestContainer(ILifetimeScope container, NancyContext context)
        {
            // Perform registrations that should have a request lifetime
        }

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            // No registrations should be performed in here, however you may
            // resolve things that are needed during request startup.
            var handler = new ErrorHandler.Web.ErrorHandler(container.Resolve<ILoggerFactory>());
            handler.Enable(pipelines, container.Resolve<IResponseNegotiator>());

            //CORS Enable
            pipelines.AfterRequest.AddItemToEndOfPipeline(ctx =>
            {
                ctx.Response
                    .WithHeader("Access-Control-Allow-Origin", "*")
                    .WithHeader("Access-Control-Allow-Methods", "POST,GET,PUT,DELETE")
                    .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");

            });            
        }

        protected override ILifetimeScope GetApplicationContainer()
        {
            return _serviceProvider.GetService<ILifetimeScope>();
        }

        protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(config => {
                    config.StatusCodeHandlers = new[] { typeof(StatusCodeHandler404), typeof(StatusCodeHandler500) };
                    config.ResponseProcessors = new[] { typeof(JsonProcessor) };
                });
            }
        }
    }
}