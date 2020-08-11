using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraPosition
{
    Shooting,
    Broadcast,
    TrackBall
}

public class CameraPositioning : MonoBehaviour
{
    public Vector3 ShootingPos;
    public Vector3 ShootingRot;
    public float ShootingPersp;
    public Vector3 BroadcastPos;
    public Vector3 BroadcastRot;
    public float BroadcastPersp;
    private Camera _camera;
    private bool _lookAt;
    public Vector3 LookAtVector;

    // Start is called before the first frame update
    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    public void MoveCamera(CameraPosition cameraPosition, bool instant = false)
    {
        _lookAt = false;
        switch (cameraPosition)
        {
            case CameraPosition.Broadcast:
                if (instant)
                {
                    transform.position = BroadcastPos;
                    transform.eulerAngles = BroadcastRot;
                    _camera.fieldOfView = BroadcastPersp;
                    _lookAt = true;
                    return;
                }
                break;
            case CameraPosition.Shooting:
                if (instant)
                {
                    transform.position = ShootingPos;
                    transform.eulerAngles = ShootingRot;
                    _camera.fieldOfView = ShootingPersp;
                    return;
                }
                break;
            default:
                break;
        }
    }

    void LateUpdate()
    {
        if (_lookAt == false)
        {
            return;
        }
        transform.LookAt(Game.Instance.Ball.transform);
        var y = transform.eulerAngles.y > 90 ? BroadcastRot.y : transform.eulerAngles.y;
        y = y < 52 ? 52 : y; 

        LookAtVector = new Vector3(BroadcastRot.x, y, BroadcastRot.z);
        transform.eulerAngles = LookAtVector;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(LookAtVector), 0.1f * Time.deltaTime);
    }
}
