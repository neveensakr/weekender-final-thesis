using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public bool InputActivated { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void EnableInput()
    {
        InputActivated = true;
    }
    
    public void DisableInput()
    {
        InputActivated = false;
    }
}
