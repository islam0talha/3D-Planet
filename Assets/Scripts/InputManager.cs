﻿using UnityEngine;
using System.Collections;

public delegate void MouseMoved(float xMovement, float yMovement);
public class InputManager : MonoBehaviour
{
    #region Private References
    private float _xMovement;
    private float _yMovement;
    #endregion
    #region Events
    public static event MouseMoved MouseMoved;
    #endregion
    #region Event Invoker Methods
    private static void OnMouseMoved(float xmovement, float ymovement)
    {
        var handler = MouseMoved;
        if (handler != null) handler(xmovement, ymovement);
    }
    #endregion
    #region Private Methods
    private void InvokeActionOnInput()
    {
        if (Input.GetMouseButton(0))
        {
            _xMovement = Input.GetAxis("Mouse X");
            _yMovement = Input.GetAxis("Mouse Y");
            OnMouseMoved(_xMovement, _yMovement);
        }
        if (Input.GetMouseButton(1))
        {

        }
    }
    #endregion
    #region Unity CallBacks
    void Update()
    {
        InvokeActionOnInput();
    }
    #endregion
}