using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private GameObject _interactUI;
    [SerializeField] private GameObject _player;

    public static InteractionManager Instance;
    public Interactable CurrentInteractable { get; private set; }
    private bool _interactionStarted;
    private Vector3 _originalPlayerPosition;

    private void Awake()
    {
        Instance = this;
        _interactUI.SetActive(false);
    }

    private void Update()
    {
        if (CurrentInteractable != null)
        {
            if (Input.GetKeyDown(KeyCode.X) && !_interactionStarted)
            {
                _interactUI.SetActive(false);
                _originalPlayerPosition = _player.transform.position;
                InputManager.Instance.DisableInput();
                CurrentInteractable.StartInteraction(_player);
                _interactionStarted = true;
            }
        }
    }

    public void SetInteractUIVisibility(bool visible)
    {
        _interactUI.SetActive(visible);
    }

    public void SetCurrentInteractable(Interactable interactable)
    {
        CurrentInteractable = interactable;
        if (CurrentInteractable == null) _interactionStarted = false;
        else CurrentInteractable.onEnd.AddListener(InteractionEnded);
    }

    private void InteractionEnded()
    {
        _player.transform.position = _originalPlayerPosition;
        CurrentInteractable = null;
        _interactionStarted = false;
        _interactUI.SetActive(false);
        InputManager.Instance.EnableInput();
    }
}
