using UnityEngine;

public static class HeightMapGenerator 
{
    public static float GetSample(float x, float y, float scale, int octaves,float persistence, float lacunarity, int seed)
    {
        float amplitude = 1f;
        float frequency = 1f;
        float noiseHeight = 0;
        float maxNoiseHeight = 1f;
        float minNoiseHeight = 0f;
        float sample = 0f;

        for (int i = 0; i < octaves; i++) {
            float sampleX = x / scale * frequency;
            float sampleY = y / scale * frequency;
            
            sample = Mathf.PerlinNoise(sampleX + seed,sampleY + seed) * 2 - 1;
            noiseHeight += sample * amplitude;

            amplitude *= persistence;
            frequency *= lacunarity;
        }
     

        if (noiseHeight > maxNoiseHeight) {
            noiseHeight = maxNoiseHeight;
        } else if (noiseHeight < minNoiseHeight) {
            noiseHeight = minNoiseHeight;
        }
        return noiseHeight;
    }
}
