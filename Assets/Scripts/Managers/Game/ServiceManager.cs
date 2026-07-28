using System;
using System.Collections.Generic;

public static class ServiceManager
{
    private static Dictionary<Type, object> services = new Dictionary<Type, object>();

    public static void RegisterService<T>(T service)
    {
        services[typeof(T)] = service;
    }

    public static T GetService<T>()
    {
        if (services.TryGetValue(typeof(T), out object service))
            return (T)service;

        return default;
    }
}
