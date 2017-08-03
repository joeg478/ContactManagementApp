using Autofac;
using ContactManagementApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagementApp
{
    public class IocProvider
    {
        public static IContainer Container { get; private set; }

        public static TService Resolve<TService>()
        {
            return Container.Resolve<TService>();
        }

        public static void BuildContainer()
        {
            if (Container == null)
            {
                var builder = new ContainerBuilder();
                //builder.RegisterType<MocWebServices>().As<IWebServices>();
                builder.RegisterType<NodeJsContactWebServices>().As<IContactWebServices>();
                Container = builder.Build();
            }
        }
    }
}
