using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

    private bool _canMove = true;


    // Dash
    private float _dashLength = 6f;
    private float _dashDuration = 1f;
    private float _dashSmallCD = 0.5f;
    private float _dashLargeCD = 1f;
    private float _timerCD = 0f;
    private int _dashCount = 0;
    private bool _canDash = true;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _mouseScreenPosition = Input.mousePosition;
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(_mouseScreenPosition);
        _currentFramePosition = transform.position;
        _nextFramePosition = _currentFramePosition + new Vector2(_horizontal * _runSpeed, _vertical * _runSpeed);
        if (_canMove)
        {
            if (Input.GetKeyDown(KeyCode.Space) && _canDash && _dashCount <= 2)
            {
                DoDash();
            }
        }
        Debug.Log(_canMove);
        
    }

    private void DoDash()
    {
        Vector2 dashDirection = (_mouseWorldPosition - _currentFramePosition).normalized;
        Vector2 dashTarget = _currentFramePosition + dashDirection * _dashLength;

        StopAllCoroutines();
        Coroutine dashAnimation = StartCoroutine(AnimateDash(dashTarget));
        Coroutine startTimer = StartCoroutine(StartTimer());
    }

    IEnumerator AnimateDash(Vector2 target)
    {
        float elapsedTime = 0f;
        while (Vector2.Distance(_currentFramePosition, target) > 0.1f)
        {
            _canMove = false;
            elapsedTime += Time.deltaTime;
            transform.position = Vector2.Lerp(_currentFramePosition, target, elapsedTime / _dashDuration);
            yield return null;
        }
        transform.position = target;
        _canMove = true;
    }
    IEnumerator StartTimer()
    {
        _timerCD = 0;
        _dashCount += 1;
        while (true)
        {
            _timerCD += Time.deltaTime;
            if (_timerCD <= _dashSmallCD)
            {
                _canDash = false;
            }
            else _canDash = true;
            if (_timerCD >= _dashLargeCD)
            {
                _dashCount = 0;
                _timerCD = 0;
                break;
            }
            yield return null;
        }
    }

    void FixedUpdate()
    {
        if (_canMove)
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
        }
        else
        {
            _horizontal = 0;
            _vertical = 0;
        }


        if (_horizontal != 0 && _vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            _horizontal *= _moveLimiter;
            _vertical *= _moveLimiter;
        }

        

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
