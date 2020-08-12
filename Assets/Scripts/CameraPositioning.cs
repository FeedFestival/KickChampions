using UnityEngine;
using UnityEngine.UIElements;

public enum CameraPosition
{
    Shooting,
    Broadcast,
    TrackBall
}

public class CameraPositioning : MonoBehaviour
{
    // public GameObject BroadcastCameraToggle;
    // public Toggle _broadcastCameraToggle;
    private bool _canBroadcastCamera;

    public Vector3 ShootingPos;
    public Vector3 ShootingRot;
    public float ShootingPersp;
    public Vector3 BroadcastPos;
    public Vector3 BroadcastRot;
    public float BroadcastPersp;
    public Camera Camera;
    private bool _lookAt;
    public Vector3 LookAtVector;

    // Start is called before the first frame update
    void Awake()
    {
        Camera = GetComponent<Camera>();
    }

    void Start()
    {
        // _broadcastCameraToggle = BroadcastCameraToggle.GetComponent<Toggle>();
        // _broadcastCameraToggle.value = true;
    }

    public void OnBroadcastCameraToggle()
    {
        _canBroadcastCamera = !_canBroadcastCamera;
    }

    public void MoveCamera(CameraPosition cameraPosition, bool instant = false)
    {
        switch (cameraPosition)
        {
            case CameraPosition.Broadcast:
                if (_canBroadcastCamera == false)
                {
                    return;
                }
                MoveToBroadcast();

                break;
            case CameraPosition.Shooting:
                _lookAt = false;
                if (instant)
                {
                    transform.position = ShootingPos;
                    transform.eulerAngles = ShootingRot;
                    Camera.fieldOfView = ShootingPersp;
                    return;
                }
                break;
            default:
                break;
        }
    }

    public void MoveToBroadcast(bool instant = false)
    {
        if (instant)
        {
            transform.position = BroadcastPos;
            transform.eulerAngles = BroadcastRot;
            Camera.fieldOfView = BroadcastPersp;
            _lookAt = true;
            return;
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
