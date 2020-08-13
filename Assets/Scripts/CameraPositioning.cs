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
    public CameraPosition CameraPosition;

    // Start is called before the first frame update
    void Awake()
    {
        Camera = GetComponent<Camera>();
    }

    void Start()
    {
        // _broadcastCameraToggle = BroadcastCameraToggle.GetComponent<Toggle>();
        // _canBroadcastCamera = _broadcastCameraToggle.value = BroadcastCameraOnShoot;
    }

    public void OnBroadcastCameraToggle(bool? broadCastCamera = null)
    {
        if (broadCastCamera.HasValue)
        {
            _canBroadcastCamera = broadCastCamera.Value;
            return;
        }
        _canBroadcastCamera = !_canBroadcastCamera;
    }

    public void MoveCamera(CameraPosition cameraPosition, bool instant = false)
    {
        // Debug.Log(cameraPosition);
        if (cameraPosition == CameraPosition
            || (cameraPosition == CameraPosition.Broadcast && _canBroadcastCamera == false))
        {
            return;
        }
        switch (cameraPosition)
        {
            case CameraPosition.Broadcast:
                MoveToBroadcast(true);

                break;
            case CameraPosition.Shooting:
                _lookAt = false;
                if (instant)
                {
                    Move(ShootingPos, ShootingRot, ShootingPersp);
                }
                else
                {
                    MoveSmooth(ShootingPos, ShootingRot, ShootingPersp, LeanTweenType.easeOutCirc);
                }
                break;
            case CameraPosition.StartZoomBall:
                _lookAt = false;
                MoveSmooth(StartZoomPos, StartZoomRot, ShootingPersp);
                break;
            case CameraPosition.ZoomBall:
                _lookAt = false;
                MoveSmooth(ZoomBallPos, ZoomBallRot, ZoomBallPersp);
                break;
            case CameraPosition.ShootPower:
                _lookAt = false;
                MoveSmooth(ShootPowerPos, ShootPowerRot, ShootPowerPersp, LeanTweenType.easeOutCirc);
                break;
            default:
                break;
        }
        CameraPosition = cameraPosition;
    }

    private void Move(Vector3 pos, Vector3 rot, float perspective)
    {
        CancelIfRunning();
        transform.position = pos;
        transform.eulerAngles = rot;
        Camera.fieldOfView = perspective;
    }

    private void MoveSmooth(Vector3 pos, Vector3 rot, float perspective, LeanTweenType leanTweenType = LeanTweenType.linear)
    {
        CancelIfRunning();

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
            Move(BroadcastPos, BroadcastRot, BroadcastPersp);
            _lookAt = true;
            return;
        }
    }

    private void CancelIfRunning()
    {
        // Debug.Log(CameraPosition);
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
        if (_perspectiveId.HasValue)
        {
            LeanTween.cancel(_perspectiveId.Value);
            _perspectiveId = null;
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
