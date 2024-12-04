using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    private float _horizontal;
    private float _vertical;
    private float _moveLimiter = 0.7f;

    private float _runSpeed = 10.0f;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (_horizontal != 0 && _vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            _horizontal *= _moveLimiter;
            _vertical *= _moveLimiter;
        }

        _rigidBody.linearVelocity = new Vector2(_horizontal * _runSpeed, _vertical * _runSpeed);
    }
}
