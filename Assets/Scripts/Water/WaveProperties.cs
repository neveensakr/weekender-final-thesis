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

    public Transform Light;
    // Start is called before the first frame update
    void Update()
    {
        float[] amplitudes = new float[waves.Length];
        float[] wavelengths = new float[waves.Length];
        Vector4[] wavedirections = new Vector4[waves.Length];
        
        for (int i = 0; i < waves.Length; i++)
        {
            amplitudes[i] = waves[i].Amplitude;
            wavelengths[i] = waves[i].WaveLength;
            wavedirections[i] = new Vector4(waves[i].WaveDirection.x, waves[i].WaveDirection.y, 0, 0);
        }
        
        GetComponent<Renderer>().material.SetFloat("_WaveCount", waves.Length);
        GetComponent<Renderer>().material.SetFloatArray("_Amplitudes", amplitudes);
        GetComponent<Renderer>().material.SetFloatArray("_WaveLengths", wavelengths);
        GetComponent<Renderer>().material.SetVectorArray("_WaveDirections", wavedirections);
        GetComponent<Renderer>().material.SetVector("_LightPoint", Light.position);
        
    }
}
