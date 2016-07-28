using UnityEngine;
using System.Collections;
public class CameraController : MonoBehaviour
{
    public GameObject PlanetTarget;
    
    #region Private References      
    [SerializeField, Range(0.0f, 1.0f)]
    private float _lerpRate;
    private float _xRotation;
    private float _yYRotation;
    float RotationSpeed = 1f;
    float OrbitDegrees = 1f;
    public float Distance = 2;
    public float ZoomSpeed=2;
    #endregion

    #region Private Methods
    private void Rotate(float xMovement, float yMovement)
    {
        _xRotation += xMovement;
        _yYRotation += yMovement;
    }
    private void Zoom(float ScrollV)
    {
        Distance -= ScrollV*4;
    }
    #endregion

    #region Unity CallBacks
    void Start()
    {
        InputManager.MouseMoved += Rotate;
        InputManager.MouseWheel += Zoom;
    }

    void FixedUpdate()
    {
        _xRotation = Mathf.Lerp(_xRotation, 0, _lerpRate);
        _yYRotation = Mathf.Lerp(_yYRotation, 0, _lerpRate);
        //transform.eulerAngles += new Vector3(-_yYRotation,_xRotation, 0);
        //transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, PlanetTarget.transform.position+(transform.position-PlanetTarget.transform.position).normalized*Distance, Time.deltaTime* ZoomSpeed);
        transform.RotateAround(PlanetTarget.transform.position, new Vector3(-_yYRotation, _xRotation, 0), OrbitDegrees*RotationSpeed);
        transform.LookAt(PlanetTarget.transform);
    }

    void OnDestroy()
    {
        InputManager.MouseMoved -= Rotate;
        InputManager.MouseWheel -= Zoom;
    }
    #endregion
}