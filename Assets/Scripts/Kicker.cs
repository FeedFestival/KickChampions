using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kicker : MonoBehaviour
{
    private Rigidbody _rb;
    public Vector3 OriginalPos;
    public Vector3 OriginalRot;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision col)
    {
        col.gameObject.GetComponent<Ball>().Curve();
        Game.Instance.CameraPositioning.MoveCamera(CameraPosition.Broadcast, true);
    }

    public void Reset()
    {
        transform.position = OriginalPos;
        transform.eulerAngles = OriginalRot;

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
}
