using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Chess : Interactable
{
    [SerializeField] private GameObject _promptUI;
    [SerializeField] private GameObject _gameEndedUI;
    [SerializeField] private GameObject _hudUI;
    [SerializeField] private Image _backProgress;
    [SerializeField] private GameObject _chessCamera;
    [SerializeField] private GameObject _closedChessBox;
    [SerializeField] private PlayableDirector _sequence;
    private bool _initialPromptShown;
    private bool _gameInprogress;
    private bool _expectPlayerInput;
    private bool _gameEnded;

    private bool _startedExitTimer;
    private float _exitTime = 3f;
    private float _exitTimer = 0;

    private void Awake()
    {
        _promptUI.SetActive(false);
        _gameEndedUI.SetActive(false);
        _hudUI.SetActive(false);
        _initialPromptShown = false;
        _chessCamera.SetActive(false);
    }

    private void Update()
    {
        if (_expectPlayerInput)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                // If the player is in the initial prompt, start the game
                if (_initialPromptShown && !_gameEnded)
                {
                    ChessGameManager.Instance.StartGame();
                    _promptUI.SetActive(false);
                    _hudUI.SetActive(true);
                    _gameInprogress = true;
                    _expectPlayerInput = false;
                }
                // If the player is in the replay prompt, restart the game
                else if (_gameEnded)
                {
                    ChessGameManager.Instance.ResetGame();
                    ChessGameManager.Instance.StartGame();
                    _gameEndedUI.SetActive(false);
                    _gameInprogress = true;
                    _expectPlayerInput = false;
                }
            } else if (Input.GetKeyDown(KeyCode.B)) ExitInteraction();
        }

        if (_gameInprogress)
        {
            // If the player presses back in game, start the exit timer
            if (Input.GetKeyDown(KeyCode.B) && !_startedExitTimer) _startedExitTimer = true;
            // Update the exit timer as the player presses the B key
            if (Input.GetKey(KeyCode.B)) _exitTimer += Time.deltaTime;
            // If the player lets go of the B key, gradually reset the timer
            if (Input.GetKeyUp(KeyCode.B)) _startedExitTimer = false;
            if (!_startedExitTimer && _exitTimer > 0) _exitTimer -= Time.deltaTime * 2;
            // Update Back Progress bar
            _backProgress.fillAmount = (_exitTimer / _exitTime);
            // If the player held down the B key long enough, exit the interaction
            if (_exitTimer >= _exitTime) ExitInteraction();
            // If the game ended, show the game ended UI
            if (ChessGameManager.Instance.GameEnded)
            {
                _gameEndedUI.SetActive(true);
                _gameEnded = true;
                _gameInprogress = false;
                _expectPlayerInput = true;
            }
        }
    }

    public override void StartInteraction(GameObject player)
    {
        Debug.Log("[Chess] Starting Interaction...");
        _chessCamera.SetActive(true);
        _sequence.Play();
    }

    public override void ExitInteraction()
    {
        Debug.Log("[Chess] Exiting Interaction...");
        
        _promptUI.SetActive(false);
        _gameEndedUI.SetActive(false);
        _hudUI.SetActive(false);
        _chessCamera.SetActive(false);
        _closedChessBox.SetActive(true);
        if (_gameEnded || _gameInprogress) ChessGameManager.Instance.ResetGame();
        _startedExitTimer = false;
        _exitTimer = 0;
        _gameInprogress = false;
        _gameEnded = false;
        _expectPlayerInput = false;
        _initialPromptShown = false;
        
        onEnd.Invoke();
    }

    public void ActivatePrompt()
    {
        _promptUI.SetActive(true);
        _closedChessBox.SetActive(false);
        _initialPromptShown = true;
        _expectPlayerInput = true;
    }
}
