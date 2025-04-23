using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

public static class Injector
{
    public static void InjectDependencies()
    {
        var monoBehaviours = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var monoBehaviour in monoBehaviours)
        {
            Inject(monoBehaviour);
        }
    }

    public static void Inject(object target)
    {
        var type = target.GetType();

        // Inject fields
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var field in fields)
        {
            if (field.GetCustomAttribute<InjectAttribute>() == null)
                continue;

            var dependency = DiContainer.Resolve(field.FieldType);
            field.SetValue(target, dependency);
        }

        // Inject properties
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<InjectAttribute>() == null)
                continue;

            if (property.CanWrite == false)
                continue;

            var dependency = DiContainer.Resolve(property.PropertyType);
            property.SetValue(target, dependency);
        }
    }

    public static void InjectStatic(Type staticType)
    {
        // Инжект в статические поля
        var fields = staticType.GetFields(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static
        );

        foreach (var field in fields)
        {
            if (field.GetCustomAttribute<InjectAttribute>() == null)
                continue;

            var dependency = DiContainer.Resolve(field.FieldType);
            field.SetValue(null, dependency); // null для статики
        }

        // Инжект в статические свойства
        var properties = staticType.GetProperties(
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static
        );

        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<InjectAttribute>() == null)
                continue;

            if (!property.CanWrite)
                continue;

            var dependency = DiContainer.Resolve(property.PropertyType);
            property.SetValue(null, dependency); // null для статики
        }
    }
}