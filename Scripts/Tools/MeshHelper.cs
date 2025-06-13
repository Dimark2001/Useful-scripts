using UnityEngine;

public class MeshHelper
{
    public static Vector3 GetRandomPointOnMesh(Mesh mesh)
    {
        var sizes = GetTriSizes(mesh.triangles, mesh.vertices);
        var cumulativeSizes = new float[sizes.Length];
        float total = 0;

        for (var i = 0; i < sizes.Length; i++)
        {
            total += sizes[i];
            cumulativeSizes[i] = total;
        }


        var randomsample = Random.value* total;

        var triIndex = -1;
        
        for (var i = 0; i < sizes.Length; i++)
        {
            if (randomsample <= cumulativeSizes[i])
            {
                triIndex = i;
                break;
            }
        }

        if (triIndex == -1)
        {
            Debug.LogError("triIndex should never be -1");
        }

        var a = mesh.vertices[mesh.triangles[triIndex * 3]];
        var b = mesh.vertices[mesh.triangles[triIndex * 3 + 1]];
        var c = mesh.vertices[mesh.triangles[triIndex * 3 + 2]];


        var r = Random.value;
        var s = Random.value;

        if(r + s >=1)
        {
            r = 1 - r;
            s = 1 - s;
        }
        var pointOnMesh = a + r*(b - a) + s*(c - a);
        return pointOnMesh;

    }

    private static float[] GetTriSizes(int[] tris, Vector3[] verts)
    {
        var triCount = tris.Length / 3;
        var sizes = new float[triCount];
        for (var i = 0; i < triCount; i++)
        {
            sizes[i] = 0.5f*Vector3.Cross(verts[tris[i*3 + 1]] - verts[tris[i*3]], verts[tris[i*3 + 2]] - verts[tris[i*3]]).magnitude;
        }
        return sizes;
    }
}