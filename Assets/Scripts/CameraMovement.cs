using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 _circleCenter;
    private Vector3 _mousePosition;

    private float _circleRadius = 1.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _circleCenter = transform.parent.localPosition;
        _mousePosition = Input.mousePosition;
        
        Vector3 v = _mousePosition - _circleCenter;
        v = Vector3.ClampMagnitude(v, _circleRadius);
        _mousePosition = _circleCenter + v;

        transform.position = _mousePosition;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(10, 10, 10));
    }
}
