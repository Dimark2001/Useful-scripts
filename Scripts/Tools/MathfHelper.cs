using UnityEngine;

public static class MathfHelper
{
    public static Vector3 Clamp(Vector3 value, float min, float max)
    {
        return new Vector3(
            Mathf.Clamp(value.x, min, max),
            Mathf.Clamp(value.y, min, max),
            Mathf.Clamp(value.z, min, max)
        );
    }

    public static Vector3 FindClosest(Vector3 value, Vector3[] values)
    {
        var minDistance = float.MaxValue;
        var minIndex = 0;
        for (var i = 0; i < values.Length; i++)
        {
            var distance = Vector3.Distance(value, values[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                minIndex = i;
            }
        }

        return values[minIndex];
    }
    
    public static Vector3 FindClosest(Vector3 value, List<Vector3> values)
    {
        var minDistance = float.MaxValue;
        var minIndex = 0;
        for (var i = 0; i < values.Count; i++)
        {
            var distance = Vector3.Distance(value, values[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                minIndex = i;
            }
        }

        return values[minIndex];
    }
}