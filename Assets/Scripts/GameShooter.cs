using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameShooter : MonoBehaviour
{
    public Rigidbody BootRb;
    public float Force;
    public Transform HitPoint;

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
        // Time.timeScale = 0.1f;

        Game.Instance.CameraPositioning.CameraTrack = CameraTrack.Ball;

        var target = (HitPoint.position - BootRb.transform.position).normalized;
        // BootRb.transform.eulerAngles = Quaternion.LookRotation(target, BootRb.transform.up).eulerAngles;

        BootRb.AddForce(target * Force, ForceMode.Impulse);

        

    }
}
