using System;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    private Vector2 _mouseScreenPosition;
    private Vector2 _mouseWorldPosition;

    private Vector2 _currentFramePosition;
    private Vector2 _nextFramePosition;
    private float _horizontal;
    private float _vertical;

    private float _moveLimiter = 0.7f;
    private float _runSpeed = 10.0f;
    private float _raycastDistance = 1f;

    private bool _canMove = true;


    // Dash
    private float _dashLength = 5f;
    private float _dashDuration = 1f;
    private float _dashSmallCD = 1f;
    private float _dashLargeCD = 3f;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _mouseScreenPosition = Input.mousePosition;
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(_mouseScreenPosition);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoDash();
        }
        
    }

    private void DoDash()
    {
        _canMove = false;
        Vector2 dashDirection = (_mouseWorldPosition - _currentFramePosition).normalized;
        transform.position = Vector2.Lerp(_currentFramePosition, dashDirection * _dashLength, Time.deltaTime * _dashDuration);
        _canMove = true;
    }

    void FixedUpdate()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if (_horizontal != 0 && _vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            _horizontal *= _moveLimiter;
            _vertical *= _moveLimiter;
        }

        _currentFramePosition = transform.position;
        _nextFramePosition = _currentFramePosition + new Vector2(_horizontal * _runSpeed, _vertical * _runSpeed);
        if (_canMove)
        {
            _rigidBody.linearVelocity = new Vector2(_horizontal * _runSpeed, _vertical * _runSpeed);
        }
        else
        {
            _rigidBody.linearVelocity = Vector2.zero;
        }

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        //this.transform.position = _nextFramePosition;
        if (other.CompareTag("Field"))
        {
            _canMove = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //this.transform.position = _nextFramePosition;
        if (other.CompareTag("Field"))
        {
            _canMove = false;
        }
    }


}
