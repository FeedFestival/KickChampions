using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseOver
{
    Icosphere,
    Hitpoint,
    ShootArea,
    Nothing
}

public enum HitterState
{
    None,
    HitPointPick,
    PowerDirectionPick
}

public class CameraSensor : MonoBehaviour
{
    public Transform FakeIcosphere;
    public MeshCollider HiddenIcosphere;
    public Collider HitPointCollider;
    public Collider ShootAreaCollider;
    public Transform HiddenHitPoint;
    private int _layermask;
    private Vector3 _cameraPos;
    public MouseOver MouseOver;
    private IEnumerator _pressAndHold;
    private bool _mousePressed = false;
    private bool _holdCheck = false;
    public HitterState HitterState;
    private Transform _kickerT;
    private bool _isDistanceOk;
    private Vector3? _raycastHitPoint;
    private Vector3 _dirHitPointToRaycast;

    // Start is called before the first frame update
    void Awake()
    {
        _layermask = LayerMask.GetMask("3DUI");
        HiddenIcosphere.enabled = false;
    }

    void Update()
    {
        if (Game.Instance.GameState != GameState.LiningUpShot)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            _mousePressed = true;
            _holdCheck = true;
            if (MouseOver == MouseOver.Hitpoint)
            {
                _pressAndHold = PressAndHold();
                Game.Instance.CameraPositioning.MoveCamera(CameraPosition.StartZoomBall);
                StartCoroutine(_pressAndHold);
            }
            else if (MouseOver == MouseOver.ShootArea)
            {
                _pressAndHold = PressAndHold(true);
                StartCoroutine(_pressAndHold);
                // EnterShootPower();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _mousePressed = false;
            // Debug.Log(_holdCheck);
            if (_holdCheck)
            {
                CheckPressAndHold();
                CancelZoom();
                return;
            }

            if (MouseOver == MouseOver.Nothing
                || MouseOver == MouseOver.Hitpoint
                || MouseOver == MouseOver.Icosphere)
            {
                CancelZoom();
            }
            else if (MouseOver == MouseOver.ShootArea)
            {
                Game.Instance.GameShooter.TryShoot();
                CancelZoom(Game.Instance.GameShooter.BroadcastCameraOnShoot ? true : false);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Game.Instance.GameState != GameState.LiningUpShot)
        {
            return;
        }

        RaycastHit hit;
        Ray ray = Game.Instance.CameraPositioning.Camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layermask))
        {
            if (hit.transform.tag == "icosphere")
            {
                MouseOverIcosphere(hit);
            }
            if (hit.transform.tag == "ShootArea")
            {
                MouseOverShootArea(hit);
            }
            else
            {
                Debug.DrawRay(_cameraPos, hit.point - _cameraPos, Color.green);
                MouseOver = MouseOver.Hitpoint;
            }
            Transform objectHit = hit.transform;
        }
        else
        {
            MouseOver = MouseOver.Nothing;
        }
    }

    void LateUpdate()
    {
        if (Game.Instance.GameState != GameState.LiningUpShot)
        {
            return;
        }

        _cameraPos = Game.Instance.CameraPositioning.transform.position;
        HiddenIcosphere.transform.LookAt(_cameraPos);
        FakeIcosphere.transform.LookAt(_cameraPos);
        HitPointCollider.transform.LookAt(_cameraPos);

        if (MouseOver == MouseOver.ShootArea
            && HitterState == HitterState.PowerDirectionPick
            && _kickerT != null)
        {
            var ballHitPointPos = Game.Instance.HitPoint.position;
            if (_raycastHitPoint.HasValue)
            {
                _dirHitPointToRaycast = (_raycastHitPoint.Value - ballHitPointPos).normalized * 1.3f;
                Debug.DrawRay(ballHitPointPos, _dirHitPointToRaycast, Color.green);
            }
        }
    }

    IEnumerator PressAndHold(bool _isOverShoot = false)
    {
        yield return new WaitForSecondsRealtime(_isOverShoot ? 0.33f : 1f);
        _holdCheck = false;

        if (_isOverShoot)
        {
            if (MouseOver == MouseOver.ShootArea)
            {
                EnterShootPower();
            }
            else
            {
                CancelZoom();
            }
        }
        else
        {

            if (_mousePressed)
            {
                if (MouseOver == MouseOver.Hitpoint)
                {
                    EnterZoom();
                }
                else if (MouseOver == MouseOver.Nothing)
                {
                    CancelZoom();
                }
                else
                {
                    EnterShootPower();
                }
            }
            else
            {
                CancelZoom();
            }
        }
        CheckPressAndHold();
    }

    private bool CheckPressAndHold()
    {
        bool _isInProgress = _pressAndHold != null;
        if (_isInProgress)
        {
            StopCoroutine(_pressAndHold);
            _pressAndHold = null;
        }
        return !_isInProgress;
    }

    private void CancelZoom(bool noCameraPan = false)
    {
        HitterState = HitterState.None;
        HitPointCollider.enabled = true;
        ShootAreaCollider.enabled = true;
        HiddenIcosphere.enabled = false;
        if (noCameraPan == false)
        {
            Game.Instance.CameraPositioning.MoveCamera(CameraPosition.Shooting);
        }
    }
    private void EnterZoom()
    {
        HitterState = HitterState.HitPointPick;
        HitPointCollider.enabled = false;
        ShootAreaCollider.enabled = false;
        HiddenIcosphere.enabled = true;
        Game.Instance.CameraPositioning.MoveCamera(CameraPosition.ZoomBall);
    }

    private void EnterShootPower()
    {
        HitterState = HitterState.PowerDirectionPick;
        HitPointCollider.enabled = false;
        ShootAreaCollider.enabled = true;
        HiddenIcosphere.enabled = false;



        Game.Instance.CameraPositioning.MoveCamera(CameraPosition.ShootPower);
    }

    private void MouseOverIcosphere(RaycastHit hit)
    {
        Debug.DrawRay(_cameraPos, hit.point - _cameraPos, Color.red);
        MouseOver = MouseOver.Icosphere;

        if (HitterState == HitterState.HitPointPick)
        {
            if (hit.transform.gameObject.name == "_fakeIcosphere")
            {
                Game.Instance.HitPoint.transform.position = hit.point;
            }
            else
            {
                HiddenHitPoint.transform.position = hit.point;
                Game.Instance.HitPoint.transform.localPosition = HiddenHitPoint.transform.localPosition;
            }
        }
    }

    private void MouseOverShootArea(RaycastHit hit)
    {
        Debug.DrawRay(_cameraPos, hit.point - _cameraPos, Color.yellow);
        MouseOver = MouseOver.ShootArea;

        if (HitterState == HitterState.PowerDirectionPick)
        {
            _raycastHitPoint = hit.point;
            if (_kickerT == null)
            {
                _kickerT = Game.Instance.Kicker.transform;
            }
            var point = _dirHitPointToRaycast + transform.InverseTransformPoint(Game.Instance.HitPoint.position);
            _kickerT.position = new Vector3(point.x - 0.3f, _kickerT.position.y, point.z);
        }
    }
}
