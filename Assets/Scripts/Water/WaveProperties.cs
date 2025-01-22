using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    [Range(0f, 0.5f)]
    public float Amplitude;
    [Range(0f, 3f)]
    public float WaveLength;
    public Vector2 WaveDirection;
}

public class WaveProperties : MonoBehaviour
{
    public Wave[] waves;

    void Update()
    {
        float[] amplitudes = new float[waves.Length];
        float[] wavelengths = new float[waves.Length];
        Vector4[] waveDirections = new Vector4[waves.Length];
        
        for (int i = 0; i < waves.Length; i++)
        {
            amplitudes[i] = waves[i].Amplitude;
            wavelengths[i] = waves[i].WaveLength;
            waveDirections[i] = new Vector4(waves[i].WaveDirection.x, waves[i].WaveDirection.y, 0, 0);
        }

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetFloat("_WaveCount", waves.Length);
        renderer.material.SetFloatArray("_Amplitudes", amplitudes);
        renderer.material.SetFloatArray("_WaveLengths", wavelengths);
        renderer.material.SetVectorArray("_WaveDirections", waveDirections);
    }
}
