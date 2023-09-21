using UnityEngine;
using Mirror;
using System.Collections;

[RequireComponent(typeof(PlayerMovement))]
public class Climbing : NetworkBehaviour
{
    [SerializeField]private float _climbSpeed;
    [SerializeField]private float _climbTime;
    [SerializeField]private float _climbCooldown;
    private Rigidbody2D _rigidbody;
    private PlayerMovement _player;
    private float _mainGravity;
    private bool _canClimbing = true;
    private bool _isClimbing = false;
    private Wall _wall;

    private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
        _mainGravity = _rigidbody.gravityScale;
        _player = GetComponent<PlayerMovement>();
        _player.OnGroundCollision += OnGroundCollision;
    }

    private void OnGroundCollision(){
        StopAllCoroutines();
        ReturnGravity();
        StartCoroutine(ClimbCoolDown(0));
    }

    private void ReturnGravity(){
        _isClimbing = false;
        _rigidbody.gravityScale = _mainGravity;
        _canClimbing = false;
    }
    
    private void FixedUpdate() {
        if(!isLocalPlayer)return;
        if(_isClimbing && _rigidbody.velocity.x > 0){
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.x);
        }
        else if(_isClimbing && _rigidbody.velocity.x < 0){
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -_rigidbody.velocity.x);
        }
        else if(_isClimbing && _rigidbody.velocity.y <= 0){
            ReturnGravity();
            StartCoroutine(ClimbCoolDown(_climbCooldown));
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(!isLocalPlayer)return;
        _wall = other.gameObject.GetComponent<Wall>();
        if(_wall && _canClimbing){
            _isClimbing = true;
            StartCoroutine(ClimbTimer());
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(!isLocalPlayer)return;
        if(_wall){
            ReturnGravity();
            StartCoroutine(ClimbCoolDown(_climbCooldown));
        }
    }
    
    private IEnumerator ClimbTimer(){
        _rigidbody.gravityScale = 0;
        yield return new WaitForSeconds(_climbTime);
        ReturnGravity();
        yield break;
    }

    private IEnumerator ClimbCoolDown(float coolDown){
        yield return new WaitForSeconds(coolDown);
        _canClimbing = true;
        yield break;
    }

    private void OnDestroy() {
        _player.OnGroundCollision -= OnGroundCollision;
    }
}
