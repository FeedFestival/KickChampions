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

public class CameraSensor : MonoBehaviour
{
    public Transform FakeIcosphere;
    public MeshCollider HiddenIcosphere;
    public Collider HitPointCollider;
    public Collider ShootAreaCollider;
    public Transform HiddenHitPoint;
    public Transform FakeHitPointTransform;
    private int _layermask;
    private Vector3 _cameraPos;
    public MouseOver MouseOver;
    private IEnumerator _pressAndHold;
    private bool _mousePressed = false;
    private bool _holdCheck = false;

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

            Debug.Log(_holdCheck);
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
                Debug.DrawRay(_cameraPos, hit.point - _cameraPos, Color.red);
                MouseOver = MouseOver.Icosphere;
                if (hit.transform.gameObject.name == "_fakeIcosphere")
                {
                    FakeHitPointTransform.transform.position = hit.point;
                }
                else
                {
                    HiddenHitPoint.transform.position = hit.point;
                    FakeHitPointTransform.transform.localPosition = HiddenHitPoint.transform.localPosition;
                }
            }
            if (hit.transform.tag == "ShootArea")
            {
                Debug.DrawRay(_cameraPos, hit.point - _cameraPos, Color.yellow);
                MouseOver = MouseOver.ShootArea;
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

    private void CancelZoom()
    {
        HitPointCollider.enabled = true;
        ShootAreaCollider.enabled = true;
        HiddenIcosphere.enabled = false;
        Game.Instance.CameraPositioning.MoveCamera(CameraPosition.Shooting);
    }
    private void EnterZoom()
    {
        HitPointCollider.enabled = false;
        ShootAreaCollider.enabled = false;
        HiddenIcosphere.enabled = true;
        Game.Instance.CameraPositioning.MoveCamera(CameraPosition.ZoomBall);
    }

    private void EnterShootPower()
    {
        HitPointCollider.enabled = false;
        HiddenIcosphere.enabled = false;
        Game.Instance.CameraPositioning.MoveCamera(CameraPosition.ShootPower);
    }
}
