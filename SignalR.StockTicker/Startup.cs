using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Ninject;
using Owin;

[assembly: OwinStartup(typeof(SignalR.StockTicker.Startup))]

namespace SignalR.StockTicker
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            // Ninject container.
            var kernel = new StandardKernel();

            var resolver = new NinjectSignalRDependencyResolver(kernel);

            // Whenever the application needs an IStockTicker, kernel creates an instance of StockTicker.
            // StockTicker is created as a Singleton object i.e., one instance of the object is created and the same instace
            // is returned for each request.
            kernel.Bind<IStockTicker>()
                .To<StockTicker>() // Binding to StockTicker.
                .InSingletonScope(); // Making it a singleton object.

            // Anonymous function returns IHubConnetion.
            // WhenInjectedInto indicates usage of this function only when creating StockTicker instances.
            // Limit to StockTicker and not tamper with how SignalR creates IHubConnectionContext.
            kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
                                resolver.Resolve<IConnectionManager>().GetHubContext<StockTickerHub>().Clients).WhenInjectedInto<IStockTicker>();

            // Any connection or hub wire up and configuration should go in here.
            var config = new HubConfiguration();
            config.Resolver = resolver;
            ConfigureSignalR(app, config);
        }

        public static void ConfigureSignalR(IAppBuilder app, HubConfiguration config)
        {
            app.MapSignalR(config);
        }
    }
}
