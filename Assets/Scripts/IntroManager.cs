using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode {
    StoryMode,
    InteractiveMode
}

public class IntroManager : MonoBehaviour
{
    [SerializeField] private GameObject _introUI;
    private GameMode _currentGameMode;

    private void Awake()
    {
        _introUI.SetActive(false);
    }

    public void ActivateIntroUI()
    {
        _introUI.SetActive(true);
    }

    public void ActivateStoryMode()
    {
        _currentGameMode = GameMode.StoryMode;
        Debug.Log("[IntroManager] Story Mode Activated.");
        _introUI.SetActive(false);
    }
    
    public void ActivateInteractiveMode()
    {
        _currentGameMode = GameMode.InteractiveMode;
        Debug.Log("[IntroManager] Interactive Mode Activated.");
        _introUI.SetActive(false);
    }
}
