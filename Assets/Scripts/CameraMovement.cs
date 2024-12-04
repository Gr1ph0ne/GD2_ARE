using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject _mainCursorObject;
    [SerializeField] private GameObject _secondCursorObject;

    private Vector3 _cameraPositon;
    private Vector2 _cameraLimit;
    private Vector2 _secondCursorLimit;
    private Vector2 _circleCenter;
    private Vector2 _mouseScreenPosition;
    private Vector2 _mouseWorldPosition;
    private GameObject _mainCursor;
    private GameObject _secondCursor;
    //public Vector2 _cursorHotspot = Vector2.zero;

    private float _cameraHeight;
    private float _circleRadius = 1.5f;
    private float _smoothSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCursor = Instantiate(_mainCursorObject);
        _secondCursor = Instantiate(_secondCursorObject);
        _cameraHeight = transform.position.z;

        //GameObject.Instantiate(_mainCursorTexture, Vector3.zero, Quaternion.identity);
        //GameObject.Instantiate(_secondCursorTexture, Vector3.zero, Quaternion.identity);

        //Cursor.SetCursor(_mainCursorObject, _cursorHotspot, CursorMode.Auto);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _circleCenter = transform.parent.localPosition;
        _mouseScreenPosition = Input.mousePosition;
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(_mouseScreenPosition);


        _mainCursor.transform.position = _mouseWorldPosition;


        Vector2 playerToMouse = _mouseWorldPosition - _circleCenter;
        playerToMouse = Vector2.ClampMagnitude(playerToMouse, _circleRadius);
        _cameraLimit = _circleCenter + playerToMouse;

        _cameraPositon = Vector2.Lerp(transform.position, _cameraLimit, Time.deltaTime * _smoothSpeed);
        _cameraPositon.z = _cameraHeight;
        transform.position = _cameraPositon;


        Vector2 mouseToPlayer = _circleCenter - _mouseWorldPosition;
        mouseToPlayer = Vector2.ClampMagnitude(mouseToPlayer, _circleRadius);
        _secondCursorLimit = _mouseWorldPosition + mouseToPlayer;

        _secondCursor.transform.position = _secondCursorLimit;
        //_mainCursorObject.transform.localPosition =  _mouseScreenPosition;
        //_secondCursorObject.transform.localPosition = (_mouseScreenPosition - _circleCenter).normalized;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(_circleCenter.x, _circleCenter.y, 0), _circleRadius);
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y, 0), 0.3f);
    }
}
