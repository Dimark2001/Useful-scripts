using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public static class DiFabric
{
    public static T Create<T>(params object[] args) where T : class
    {
        var type = typeof(T);
        var instance = Activator.CreateInstance(type, args) as T;
        
        // Автоматическая регистрация если есть атрибут [Register]
        if (type.GetCustomAttribute<RegisterAttribute>() != null)
        {
            DiContainer.RegisterInstance(instance);
        }
        
        Injector.Inject(instance);
        return instance;
    }

    public static T InstantiatePrefab<T>(T prefab, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : Object
    {
        var instance = Object.Instantiate(prefab, position, rotation, parent);
        var type = instance.GetType();
        
        if (type.GetCustomAttribute<RegisterAttribute>() != null)
        {
            DiContainer.RegisterInstance(instance);
        }
        
        switch (instance)
        {
            case GameObject gameObject:
                InjectDependenciesInGameObject(gameObject);
                break;
            case Component component:
                InjectDependenciesInGameObject(component.gameObject);
                break;
        }
        
        return instance;
    }

    private static void InjectDependenciesInGameObject(GameObject gameObject)
    {
        var components = gameObject.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (var component in components)
        {
            var componentType = component.GetType();
            if (componentType.GetCustomAttribute<RegisterAttribute>() != null)
            {
                DiContainer.RegisterInstance(componentType, component);
            }
            
            Injector.Inject(component);
        }
    }
}