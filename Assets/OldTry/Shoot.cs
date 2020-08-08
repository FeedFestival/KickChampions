using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float Force;
    public bool _isShooting;

    public GameObject Ball;
    public GameObject ShootToPlatformSelectionDirection;

    public Transform ShootDirection;
    public Transform ShootTo;

    //
    private Rigidbody _rb;
    private Rigidbody _ballRb;
    private MeshCollider MeshCollider;
    private Vector3 originalEuler;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _ballRb = Ball.GetComponent<Rigidbody>();
        MeshCollider = GetComponent<MeshCollider>();
        originalEuler = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            TryShoot();
        }

        //_rb.velocity = Vector3.zero;
        //_rb.angularVelocity = Vector3.zero;

        //if (_isShooting)
        //    _ballRb.AddRelativeForce(Vector3.left * 0.05f, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision col)
    {
        //if (col.gameObject.name != "Ball")
        //    return;

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    public void RotateX()
    {
        transform.localEulerAngles = new Vector3(
            originalEuler.x + CanvasController.Instance.SliderRotX.value,
            transform.localEulerAngles.y,
            transform.localEulerAngles.z
        );
    }

    public void RotateY()
    {
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x, 
            originalEuler.y + CanvasController.Instance.SliderRotY.value, 
            transform.localEulerAngles.z
            );
    }

    private void TryShoot()
    {
        _isShooting = true;
        ShootToPlatformSelectionDirection.SetActive(false);
        //var target = ShootDirection.TransformPoint(ShootTo.position);

        ShootTo.SetParent(null);
        var target = ShootTo.position;

        Debug.Log(target);

        //target = new Vector3(target.x, 0.5f, target.z);

        var dir = (target - transform.position).normalized * Force;
        _rb.AddForce(dir, ForceMode.Impulse);
        //_rb.AddRelativeForce(dir, ForceMode.Acceleration);
    }
}
