using UnityEngine;
using System.Collections;

public delegate void MouseMoved(float xMovement, float yMovement);
public delegate void MouseDown(RaycastHit hit);
public delegate void MouseDrag(RaycastHit hit);
public delegate void MouseUp();
public delegate void MouseWheel(float ScrollV);
public class InputManager : MonoBehaviour
{
    #region Private References
    private float _xMovement;
    private float _yMovement;
    #endregion
    #region Private properties
    private bool clicked;
    #endregion
    #region Events
    public static event MouseMoved MouseMoved;
    public static event MouseDown MouseDown;
    public static event MouseDrag MouseDrag;
    public static event MouseUp MouseUp;
    public static event MouseWheel MouseWheel;
    #endregion
    #region Event Invoker Methods
    private static void OnMouseMoved(float xmovement, float ymovement)
    {
        var handler = MouseMoved;
        if (handler != null) handler(xmovement, ymovement);
    }
    private static void OnMouseDown(RaycastHit hit)
    {
        var handler = MouseDown;
        if (handler != null) handler(hit);
    }
    private static void OnMouseDrag(RaycastHit hit)
    {
        var handler = MouseDrag;
        if (handler != null) handler(hit);
    }
    private static void OnMouseUp()
    {
        var handler = MouseUp;
        if (handler != null) handler();
    }
    private static void OnMouseScroll(float ScrollV)
    {
        var handeler = MouseWheel;
        if (handeler != null) handeler(ScrollV);

    }
    #endregion
    #region Private Methods
    private void InvokeActionOnInput()
    {
        if (Input.GetMouseButton(1))
        {
            _xMovement = Input.GetAxis("Mouse X");
            _yMovement = Input.GetAxis("Mouse Y");
            OnMouseMoved(_xMovement, _yMovement);
        }
        if (Input.GetMouseButton(0))

            if (clicked)
            {
                //Drag
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                    OnMouseDrag(hit);
            }
            else
            {
                //clicked
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                {
                    OnMouseDown(hit);
                    clicked = true;
                }
            }
        else
        {
            clicked = false;
            OnMouseUp();
        }

        float d = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(d) > 0.001f)
        {
            // scroll
            OnMouseScroll(d);
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