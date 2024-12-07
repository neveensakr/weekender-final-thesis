using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public enum GameMode {
    StoryMode,
    InteractiveMode
}

public class IntroManager : MonoBehaviour
{
    [SerializeField] private GameObject _introUI;
    [SerializeField] private PlayableDirector _introSequence;
    [SerializeField] private GameObject _playerCamera;
    private GameMode _currentGameMode;
    
    private void Awake()
    {
        _introUI.SetActive(false);
        _playerCamera.SetActive(false);
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
        EndIntroSequence();
    }
    
    public void ActivateInteractiveMode()
    {
        _currentGameMode = GameMode.InteractiveMode;
        Debug.Log("[IntroManager] Interactive Mode Activated.");
        _introUI.SetActive(false);
        FindObjectOfType<Movement>().EnablePlayerMovement();
        _playerCamera.SetActive(true);
        EndIntroSequence();
    }

    private void EndIntroSequence()
    {
        _introSequence.Stop();
    }
}
