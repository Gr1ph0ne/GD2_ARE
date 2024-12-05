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
    private float _runSpeed = 7.0f;

    private bool _canMove = true;


    // Dash
    private float _dashLength = 6f;
    private float _dashDuration = 1f;
    private float _dashSmallCD = 0.5f;
    private float _dashLargeCD = 1f;
    private float _timerDashCD = 0f;
    private int _dashCount = 0;
    private bool _canDash = true;

    //Sword
    [SerializeField] private GameObject _swordObject;
    private float _hitDuration = 0.2f;
    private float _swordDashLength = 1f;
    private float _swordHitAmplitude = 50f;
    private int _hitCount = 0;
    private float _hitLargeCD = 0.5f;
    private float _timerHitCD = 0f;
    private bool _canHit = true;

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
            if (Input.GetMouseButtonDown(0) && _canHit && _hitCount <= 2)
            {
                Hit();
            }
        }
        
    }

    private void Hit()
    {
        Vector2 hitDirection = (_mouseWorldPosition - _currentFramePosition).normalized;

        StopAllCoroutines();
        Coroutine swordAnimation = StartCoroutine(AnimateSword(hitDirection));
        Coroutine startTimer = StartCoroutine(HitTimer());

    }

    IEnumerator AnimateSword(Vector2 direction)
    {
        _swordObject.SetActive(true);

        float angleInRadians = Mathf.Atan2(direction.y, direction.x);
        float angleInDegrees = Mathf.Rad2Deg * angleInRadians;
        float position1 = angleInDegrees - _swordHitAmplitude;
        float position2 = angleInDegrees + _swordHitAmplitude;

        Vector2 target = _currentFramePosition + direction * _swordDashLength;

        float elapsedTime = 0f;
        while (elapsedTime <= _hitDuration)
        {
            _canMove = false;
            _canHit = false;
            _canDash = false;
            elapsedTime += Time.deltaTime;
            if (_hitCount == 0 || _hitCount == 2) 
            _swordObject.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, position1), Quaternion.Euler(0, 0, position2), elapsedTime / _hitDuration);
            else _swordObject.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, position2), Quaternion.Euler(0, 0, position1), elapsedTime / _hitDuration);
            transform.position = Vector2.Lerp(_currentFramePosition, target, elapsedTime / _hitDuration);
            yield return null;
        }
        _dashCount = 0;
        _timerDashCD = 0;
        _swordObject.SetActive(false);
        _canMove = true;
        _canHit = true;
        _canDash = true;

    }

    IEnumerator HitTimer()
    {
        _timerHitCD = 0;
        _hitCount += 1;
        while (true)
        {
            _timerHitCD += Time.deltaTime;
            if (_timerHitCD >= _hitLargeCD)
            {
                _hitCount = 0;
                _timerHitCD = 0;
                break;
            }
            yield return null;
        }
    }

    private void DoDash()
    {
        Vector2 dashDirection = (_mouseWorldPosition - _currentFramePosition).normalized;
        Vector2 dashTarget = _currentFramePosition + dashDirection * _dashLength;

        StopAllCoroutines();
        Coroutine dashAnimation = StartCoroutine(AnimateDash(dashTarget));
        Coroutine startTimer = StartCoroutine(DashTimer());
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
        _hitCount = 0;
        _timerHitCD = 0;
        transform.position = target;
        _canMove = true;
    }
    IEnumerator DashTimer()
    {
        _timerDashCD = 0;
        _dashCount += 1;
        while (true)
        {
            _timerDashCD += Time.deltaTime;
            if (_timerDashCD <= _dashSmallCD)
            {
                _canDash = false;
            }
            else _canDash = true;
            if (_timerDashCD >= _dashLargeCD)
            {
                _dashCount = 0;
                _timerDashCD = 0;
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
