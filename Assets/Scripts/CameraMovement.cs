using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Vector3 _cameraPositon;
    private float _cameraHeight;
    private Vector2 _cameraLimit;
    private Vector2 _cameraCircleCenter;
    private float _cameraCircleRadius = 1.5f;
    private float _cameraSmoothSpeed = 5f;

    private Vector2 _mouseScreenPosition;
    private Vector2 _mouseWorldPosition;

    [SerializeField] private GameObject _playerObject;
    private Vector2 _playerPosition;

    [SerializeField] private GameObject _mainCursorObject;
    [SerializeField] private GameObject _secondCursorObject;
    private GameObject _mainCursor;
    private GameObject _secondCursor;
    private Vector2 _secondCursorLimit;
    private float _cursorCircleRadius = 1.5f;


    void Start()
    {
        _mainCursor = Instantiate(_mainCursorObject);
        _secondCursor = Instantiate(_secondCursorObject);
        _cameraHeight = transform.position.z;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _cameraCircleCenter = transform.parent.localPosition;
        _mouseScreenPosition = Input.mousePosition;
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(_mouseScreenPosition);


        _mainCursor.transform.position = _mouseWorldPosition;


        Vector2 playerToMouse = _mouseWorldPosition - _cameraCircleCenter;
        playerToMouse = Vector2.ClampMagnitude(playerToMouse, _cameraCircleRadius);
        _cameraLimit = _cameraCircleCenter + playerToMouse;

        _cameraPositon = Vector2.Lerp(transform.position, _cameraLimit, Time.deltaTime * _cameraSmoothSpeed);
        _cameraPositon.z = _cameraHeight;
        transform.position = _cameraPositon;

        _playerPosition = _playerObject.transform.position;
        Vector2 mouseToPlayer = _playerPosition - _mouseWorldPosition;
        mouseToPlayer = Vector2.ClampMagnitude(mouseToPlayer, _cursorCircleRadius);
        _secondCursorLimit = _mouseWorldPosition + mouseToPlayer;

        _secondCursor.transform.position = _secondCursorLimit;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(_cameraCircleCenter.x, _cameraCircleCenter.y, 0), _cameraCircleRadius);
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y, 0), 0.3f);
    }
}
