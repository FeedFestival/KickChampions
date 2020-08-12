using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameShooter : MonoBehaviour
{
    public Rigidbody BootRb;
    public float Force;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            TryShoot();
        }
    }

    public void TryShoot()
    {
        Game.Instance.CameraPositioning.CameraTrack = CameraTrack.Ball;
        var target = BootRb.transform.forward;
        // target = new Vector3(target.x, 0.5f, target.z);

        // Debug.Log(target);

        var dir = (target - transform.position).normalized * Force;
        // Debug.Log(dir);
        BootRb.AddForce(dir, ForceMode.Impulse);
        //_rb.AddRelativeForce(dir, ForceMode.Acceleration);

        ReplayController.Instance.Record();
    }
}
