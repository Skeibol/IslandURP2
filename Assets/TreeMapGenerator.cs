using UnityEngine;
using Unity.Mathematics;
public static class TreeMapGenerator 
{
    public static float GetSample(float x, float y)
    {
        var sample = noise.cellular(new float2(x,y));
        return sample.x;
    }
}

