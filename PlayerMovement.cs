using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : NetworkBehaviour, IMovable
{
    public Action OnGroundCollision;
    public float MaxSpeed => _maxSpeed;
    public float MinSpeed => _minSpeed;
    public float CurrentSpeed => _currentSpeed;
    public float JumpForce => _jumpForce;
    [SerializeField]private float _jumpForce;
    [SerializeField]private float _maxSpeed;
    [SerializeField]private float _minSpeed;
    [SerializeField]private float _currentSpeed;
    private Rigidbody2D _rigidbody;
    private bool _isMoving = false;
    private float _horizontal;
    private bool _onGround = false;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        if(!isLocalPlayer)return;
        if(_isMoving){
            _rigidbody.velocity = new Vector2(_horizontal * _currentSpeed,_rigidbody.velocity.y);
            Flip();
        }
    }

    private void Flip(){
        if(_rigidbody.velocity.x < 0){
            Quaternion rot = transform.rotation;
            rot.y = 180;
            transform.rotation = rot;
        }
        if(_rigidbody.velocity.x > 0){
            Quaternion rot = transform.rotation;
            rot.y = 0;
            transform.rotation = rot;
        }
    }

    public void Jump(InputAction.CallbackContext _callback){
        if(!isLocalPlayer)return;
        if(!_onGround)return;
        _onGround = false;
        _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    public void Move(InputAction.CallbackContext _callback){
        if(!isLocalPlayer)return;
        if(_callback.started){
            _horizontal = _callback.ReadValue<Vector2>().x;
            _isMoving = true;
        }
        else if(_callback.canceled){
            _horizontal = 0;
            _isMoving = false;
        }
    }

    private void OnGroundCheck(Collision2D other){
        if(!isLocalPlayer)return;
        var _isground = other.gameObject.GetComponent<Floor>();
        if(_isground){
            _onGround = true;
            OnGroundCollision?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        OnGroundCheck(other);
    }

    private void OnCollisionStay2D(Collision2D other){
        OnGroundCheck(other);
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(!isLocalPlayer)return;
        var _isground = other.gameObject.GetComponent<Floor>();
        if(_isground){
            _onGround = false;
        }
    }

    public void Accelerate(float value){
        if(!isLocalPlayer)return;
        if(_currentSpeed + value <= _maxSpeed)_currentSpeed += value;
        else _currentSpeed = _maxSpeed;
    }

    public void Slow(float value){
        if(!isLocalPlayer)return;
        if(_currentSpeed - value >= _minSpeed)_currentSpeed -= value;
        else _currentSpeed = _minSpeed;
    }
}