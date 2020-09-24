using System.Collections;
using UnityEngine;

public static class Noise 
{
	// Lacunarity : frequency
	// Persistence: amplitude
	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
	{

		System.Random prng = new System.Random(seed);
		Vector2[] octaveOffsets = new Vector2[octaves];
		for (int i = 0; i < octaves; i++)
		{
			float offsetx = prng.Next(-100000,100000) + offset.x;
			float offsety = prng.Next(-100000,100000) + offset.y;
			octaveOffsets[i] = new Vector2(offsetx,offsety);
		}

		float[,] noiseMap = new float[mapWidth,mapHeight];
		if(scale <= 0) scale = 0.00001f;

		float halfWidth = mapWidth/2f;
		float halfHeight = mapHeight/2f;

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		for (int y = 0; y < mapHeight; y++){
			for (int x = 0; x < mapWidth; x++){
				
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++)
				{
					float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
					float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise(sampleX,sampleY) * 2 - 1; // *2 - 1 makes it go between -1 and 1
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;
				}
				if(noiseHeight > maxNoiseHeight) 
					maxNoiseHeight = noiseHeight;
				else if(noiseHeight < minNoiseHeight) 
					minNoiseHeight = noiseHeight;
				noiseMap[x,y] = noiseHeight;
			}
		}
		for (int y = 0; y < mapHeight; y++){
			for (int x = 0; x < mapWidth; x++){
				noiseMap[x,y] = Mathf.InverseLerp(minNoiseHeight,maxNoiseHeight,noiseMap[x,y]);
			}
		}
		return noiseMap;
	}

}
