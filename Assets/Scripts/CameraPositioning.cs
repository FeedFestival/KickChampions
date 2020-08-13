using UnityEngine;
using UnityEngine.UIElements;

public enum CameraPosition
{
    Shooting,
    StartZoomBall,
    ZoomBall,
    ShootPower,
    Broadcast,
    TrackBall
}

public enum CameraTrack
{
    Ball,
    Replay
}

public class CameraPositioning : MonoBehaviour
{
    // public GameObject BroadcastCameraToggle;
    // public Toggle _broadcastCameraToggle;
    private bool _canBroadcastCamera;

    [Header("Shooting Cam")]
    public Vector3 ShootingPos;
    public Vector3 ShootingRot;
    public float ShootingPersp;
    [Header("Broadcast Cam")]
    public Vector3 BroadcastPos;
    public Vector3 BroadcastRot;
    public float BroadcastPersp;
    [Header("StartZoom Cam")]
    public Vector3 StartZoomPos;
    public Vector3 StartZoomRot;
    [Header("Zoom Ball Cam")]
    public Vector3 ZoomBallPos;
    public Vector3 ZoomBallRot;
    public float ZoomBallPersp;
    [Header("Shoot Power Cam")]
    public Vector3 ShootPowerPos;
    public Vector3 ShootPowerRot;
    public float ShootPowerPersp;
    [Header("Settings")]
    public Camera Camera;
    private bool _lookAt;
    public Vector3 LookAtVector;
    public CameraTrack CameraTrack;
    private int? _moveId;
    private int? _rotId;
    private int? _perspectiveId;
    private const float _moveTime = 1.5f;

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
                MoveToBroadcast(true);

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
                else
                {
                    MoveSmooth(ShootingPos, ShootingRot, ShootingPersp, LeanTweenType.easeOutCirc);
                }
                break;
            case CameraPosition.StartZoomBall:

                MoveSmooth(StartZoomPos, StartZoomRot, ShootingPersp);
                break;
            case CameraPosition.ZoomBall:

                MoveSmooth(ZoomBallPos, ZoomBallRot, ZoomBallPersp);
                break;
            case CameraPosition.ShootPower:

                MoveSmooth(ShootPowerPos, ShootPowerRot, ShootPowerPersp, LeanTweenType.easeOutCirc);
                break;
            default:
                break;
        }
    }

    private void MoveSmooth(Vector3 pos, Vector3 rot, float perspective, LeanTweenType leanTweenType = LeanTweenType.linear)
    {
        if (_moveId.HasValue)
        {
            LeanTween.cancel(_moveId.Value);
            _moveId = null;
        }
        if (_rotId.HasValue)
        {
            LeanTween.cancel(_rotId.Value);
            _rotId = null;
        }

        _moveId = LeanTween.move(gameObject, pos, _moveTime).id;
        LeanTween.descr(_moveId.Value).setEase(leanTweenType);
        _rotId = LeanTween.rotate(gameObject, rot, _moveTime).id;
        LeanTween.descr(_rotId.Value).setEase(leanTweenType);

        var currentFieldOfView = Camera.fieldOfView;
        _perspectiveId = LeanTween.value(Camera.gameObject, currentFieldOfView, perspective, _moveTime).id;
        LeanTween.descr(_perspectiveId.Value).setEase(leanTweenType);
        LeanTween.descr(_perspectiveId.Value).setOnUpdate((float value) =>
        {
            Camera.fieldOfView = value;
        });
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
        if (CameraTrack == CameraTrack.Ball)
        {
            transform.LookAt(Game.Instance.Ball.transform);
        }
        else
        {
            transform.LookAt(Game.Instance.ReplayBall.transform);
        }
        var y = transform.eulerAngles.y > 90 ? BroadcastRot.y : transform.eulerAngles.y;
        y = y < 52 ? 52 : y;

        LookAtVector = new Vector3(BroadcastRot.x, y, BroadcastRot.z);
        transform.eulerAngles = LookAtVector;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(LookAtVector), 0.1f * Time.deltaTime);
    }
}
