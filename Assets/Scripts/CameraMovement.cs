using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector3 _circleCenter;
    private Vector3 _mouseScreenPosition;
    private Vector3 _mouseWorldPosition;

    private float _circleRadius = 1.5f;
    private float _smoothSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _circleCenter = transform.parent.localPosition;
        _mouseScreenPosition = Input.mousePosition;
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(_mouseScreenPosition);


        Vector3 v = _mouseWorldPosition - _circleCenter;
        v = Vector3.ClampMagnitude(v, _circleRadius);
        _mouseWorldPosition = _circleCenter + v;

        transform.position = Vector3.Lerp(transform.position, _mouseWorldPosition, Time.deltaTime * _smoothSpeed);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(_circleCenter.x, _circleCenter.y, 0), _circleRadius);
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y, 0), 0.3f);
    }
}
