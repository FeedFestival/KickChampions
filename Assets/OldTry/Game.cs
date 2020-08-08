using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject Ball;
    public GameObject Sniker;
    //
    private Vector3 _ballPos;
    private Vector3 _snikerPos;

    private Vector3 _ballRot;
    private Vector3 _snikerRot;

    private Shoot _shoot;

    // Use this for initialization
    void Start ()
    {
        _ballPos = Ball.transform.position;
        _ballRot = Ball.transform.eulerAngles;

        _snikerPos = Sniker.transform.position;
        _snikerRot = Sniker.transform.eulerAngles;

        _shoot = Sniker.GetComponent<Shoot>();

        Init();
    }

    void Init()
    {
        Ball.transform.position = _ballPos;
        Ball.transform.eulerAngles = _ballRot;

        var rb = Ball.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Sniker.transform.position = _snikerPos;
        Sniker.transform.eulerAngles = _snikerRot;

        rb = Sniker.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        _shoot._isShooting = false;

        CanvasController.Instance.SliderRotX.value = 0;
        CanvasController.Instance.SliderRotY.value = 0;
    }

    // Update is called once per frame
	void Update () {
	    if (Input.GetKeyUp(KeyCode.A))
	    {
	        Init();
        }
	}
}
