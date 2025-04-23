using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(MonoBehaviour), true)]
public class ButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var mono = (MonoBehaviour)target;

        var methods = mono.GetType().GetMethods(
            System.Reflection.BindingFlags.Instance |
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.NonPublic);

        foreach (var method in methods)
        {
            var buttonAttribute = (ButtonAttribute)Attribute.GetCustomAttribute(
                method, typeof(ButtonAttribute));

            if (buttonAttribute != null)
            {
                if (GUILayout.Button(method.Name))
                {
                    method.Invoke(mono, null);
                }
            }
        }
    }
}
#endif

[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : Attribute { }