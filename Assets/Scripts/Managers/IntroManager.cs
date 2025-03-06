using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Playables;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private GameObject _introUI;
    [SerializeField] private PlayableDirector _introSequence;
    [SerializeField] private PlayableDirector _loungeSequence;
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private GameObject _UICamera;
    [SerializeField] private Rig _objectInteractionRig;
    
    public static IntroManager Instance;
    public GameMode CurrentGameMode { get; private set; }

    private void Awake()
    {
        Instance = this;
        CurrentGameMode = GameMode.Default;
        _introUI.SetActive(false);
        _playerCamera.SetActive(false);
    }

    private void Update()
    {
        if (InputManager.Instance.InputActivated && CurrentGameMode == GameMode.Default)
        {
            if (Input.GetKeyDown(KeyCode.A)) ActivateInteractiveMode();
            if (Input.GetKeyDown(KeyCode.B)) ActivateStoryMode();
        }
    }

    public void ActivateIntroUI()
    {
        _introUI.SetActive(true);
        _UICamera.SetActive(false);
        _UICamera.SetActive(true);
        InputManager.Instance.EnableInput();
    }

    public void ActivateStoryMode()
    {
        CurrentGameMode = GameMode.StoryMode;
        InputManager.Instance.DisableInput();
        EndIntroSequence();
        _loungeSequence.Play();
    }
    
    public void ActivateInteractiveMode()
    {
        CurrentGameMode = GameMode.InteractiveMode;
        _playerCamera.SetActive(true);
        _objectInteractionRig.weight = 0;
        EndIntroSequence();
    }

    private void EndIntroSequence()
    {
        _introUI.SetActive(false);
        _introSequence.Stop();
        Debug.Log("[IntroManager] Current Game Mode: " + CurrentGameMode);
    }
}
