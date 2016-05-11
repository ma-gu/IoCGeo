using Autofac;
using Autofac.Features.Variance;
using BCJobs.Analytics.Geocoding;
using Infra.IoC;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().TestIoC();
        }

        void TestIoC()
        {
            var builder = new ContainerBuilder();
            RegisterClassesConventionally(builder);

            var container = builder.Build();
        }

        void RegisterClassesConventionally(ContainerBuilder builder)
        {
            builder.RegisterSource(new ContravariantRegistrationSource());
            RegisterPerRequest(builder, "Services");
        }

        void RegisterPerRequest(ContainerBuilder builder, string nameSpace)
        {
            RegisterAll(nameSpace, a =>
            {
                builder
                    .RegisterType(a)
                    .AsImplementedInterfaces()
                    .InstancePerRequest();
            });
        }

        void RegisterAll(string nameSpace, Action<Type> action)
        {
            Types.Referenced.KindOf(nameSpace)
                .Classes()
                .ForAll(action);
        }
    }
}
