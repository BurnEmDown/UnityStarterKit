using System;
using System.Collections.Generic;

namespace Core.Services
{
    public static class Services
    {
        private static readonly Dictionary<Type, object> services = new();

        public static void Register<T>(T serviceInstance)
        {
            services[typeof(T)] = serviceInstance;
        }

        public static T Get<T>()
        {
            return (T)services[typeof(T)];
        }
    }
}