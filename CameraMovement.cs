using Mirror;
using UnityEngine;

public class CameraMovement : NetworkBehaviour
{
    private Camera _camera;
    [SerializeField]private Vector3 _offset = new Vector3(0,0,-10);
    private void Start() {
        if(!isLocalPlayer)return;
        _camera = Camera.main;
    }

    private void FixedUpdate() {
        if(!isLocalPlayer)return;
        _camera.transform.position = this.transform.position + _offset;
    }
}
