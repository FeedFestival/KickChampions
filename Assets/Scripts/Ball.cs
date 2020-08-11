using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utils;

public class Ball : MonoBehaviour
{
    public bool UseMagnusEffect;
    public float LeftAmmount;
    public float MaxLeftPerc = 10;
    public float TimeInMagnus = 0.5f;
    public float TimeOutMagnus = 1f;
    private float _maxLeftAmmount = 0;
    private float _minLeftAmmount = 0;
    public bool _isCurving;
    private bool _isTorqueing;
    private int? _curveId;
    private Rigidbody _rb;

    public Vector3 OriginalPos;
    public bool CanStopRecording;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Reset()
    {
        transform.position = OriginalPos;
        // transform.eulerAngles = _ballRot;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    public void Curve()
    {
        _isCurving = true;
        _maxLeftAmmount = UsefullUtils.GetPercent(Mathf.Abs(_rb.velocity.x), MaxLeftPerc);
        _curveId = LeanTween.value(LeftAmmount, _maxLeftAmmount, TimeInMagnus).id;
        LeanTween.descr(_curveId.Value)
            .setOnUpdate(ChangeLeftAmmount);
        LeanTween.descr(_curveId.Value).setEase(LeanTweenType.easeInOutBack);
        LeanTween.descr(_curveId.Value)
            .setOnComplete(() =>
            {
                _curveId = null;
                _isTorqueing = true;
                _curveId = LeanTween.value(LeftAmmount, _minLeftAmmount, TimeOutMagnus).id;
                LeanTween.descr(_curveId.Value)
                    .setOnUpdate(ChangeLeftAmmount);
                LeanTween.descr(_curveId.Value)
                    .setOnComplete(() =>
                    {
                        _curveId = null;
                        _isCurving = false;
                        _isTorqueing = false;
                    });
            });
    }

    void OnCollisionEnter(Collision col)
    {
        // Debug.Log(_rb.velocity);
    }

    private void ChangeLeftAmmount(float value)
    {
        LeftAmmount = value;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (UseMagnusEffect && _isCurving)
        {
            // Debug.Log(_rb.velocity);
            _rb.velocity = new Vector3(
                _rb.velocity.x - LeftAmmount,
                _rb.velocity.y,
                _rb.velocity.z
                );
            if (_isTorqueing)
            {
                _rb.AddRelativeTorque(new Vector3(-(LeftAmmount * 100), 0, 0), ForceMode.VelocityChange);
            }
            // Debug.Log(_rb.angularVelocity);
        }
    }

    void LateUpdate()
    {
        if (CanStopRecording)
        {
            if (_rb.velocity.magnitude < 2)
            {
                ReplayController.Instance.StopRecording();
            }
            else
            {
                // Debug.Log(_rb.velocity.magnitude);
            }
        }
    }
}
