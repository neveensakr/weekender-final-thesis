using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioFade : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    public float crossFadeTime = 1.0f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject.transform.parent);
    }

    public void TransitionToLounge()
    {
        _audioMixer.FindSnapshot("Lounge").TransitionTo(crossFadeTime);
    }
    
    public void TransitionToEnteringLounge()
    {
        _audioMixer.FindSnapshot("EnteringLounge").TransitionTo(crossFadeTime);
    }
    
    public void TransitionToOcean()
    {
        _audioMixer.FindSnapshot("Ocean").TransitionTo(crossFadeTime);
    }
}
