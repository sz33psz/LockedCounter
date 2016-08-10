using Autofac;
using LockedCounter.Model;
using LockedCounter.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockedCounter
{
    public class DI
    {
        private static IContainer _container;

        public static void Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<TimeCounter>()
                .AsSelf()
                .PropertiesAutowired()
                .SingleInstance();

            builder.RegisterType<StatisticsViewModel>()
                .AsSelf()
                .PropertiesAutowired()
                .SingleInstance();

            builder.RegisterType<StateDurationRepository>()
                .As<BaseRepository<StateDuration>>()
                .SingleInstance()
                .OnActivating(async h => await h.Instance.Initialize());

            _container = builder.Build();
        }

        public static T Get<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
