using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetBehaviour : MonoBehaviour {

    public Transform target;
    public float RotationSpeed = 100f;
    public float OrbitDegrees = 1f;

    CircleZone _CurrentCircleZone;
    private Vector3 Delta;
    // Use this for initialization
    void Start () {
        InputManager.MouseDown += _OnMouseDown;
        InputManager.MouseDrag += _OnDrag;
        InputManager.MouseUp += _OnMouseUp;
	}

    // Update is called once per frame

    void Update()
    {
        if (target)
        {
            transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
            transform.RotateAround(target.position, Vector3.up, OrbitDegrees);
        }  
    }
    void _OnMouseDown(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject)
        {
            _CurrentCircleZone = CircleZone.CreateNewZone(transform, hit.point).GetComponent<CircleZone>();
            Delta = Vector3.zero;
        }
    }
    void _OnDrag(RaycastHit hit)
    {
        if (hit.transform.gameObject == this.gameObject && _CurrentCircleZone!=null)
        {
            Delta = _CurrentCircleZone.transform.position - hit.point;
            _CurrentCircleZone.UpdateCircleZone(Mathf.Sqrt(Delta.x * Delta.x + Delta.y * Delta.y + Delta.z * Delta.z));
        }
    }
    void _OnMouseUp()
    {
        _CurrentCircleZone = null;
    }
}
