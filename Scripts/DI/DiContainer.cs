using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public static class DiContainer
{
    private static readonly Dictionary<Type, object> Instances = new();
    private static bool _isAutoRegistered;

    public static void RegisterDependencies()
    {
        if (_isAutoRegistered && false) 
            return;
        
        var registeredTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.GetCustomAttribute<RegisterAttribute>() != null);

        foreach (var type in registeredTypes)
        {
            if (type.IsAbstract || type.IsInterface || typeof(MonoBehaviour).IsAssignableFrom(type))
                continue;

            var instance = Activator.CreateInstance(type);
            RegisterInstance(instance);
        }

        RegisterSceneMonoBehaviours();

        _isAutoRegistered = true;
    }

    private static void RegisterSceneMonoBehaviours()
    {
        var monoBehaviours = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        
        foreach (var mb in monoBehaviours)
        {
            var type = mb.GetType();
            
            if (type.GetCustomAttribute<RegisterAttribute>() != null)
            {
                RegisterInstance(mb);
                
                foreach (var interfaceType in type.GetInterfaces())
                {
                    RegisterInstance(interfaceType, mb);
                }
            }
        }
    }

    public static void RegisterInstance(object instance)
    {
        Instances[instance.GetType()] = instance;
    }

    public static void RegisterInstance(Type type, object instance)
    {
        Instances[type] = instance;
    }

    public static object Resolve(Type type)
    {
        if (Instances.TryGetValue(type, out var instance))
            return instance;
        
        throw new Exception($"Dependency of type {type} not registered");
    }

    public static T Resolve<T>() where T : class
    {
        return (T)Resolve(typeof(T));
    }

    public static void ClearSceneBindings()
    {
        Instances.Clear();
        _isAutoRegistered = false;
    }
}