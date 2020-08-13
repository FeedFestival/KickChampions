using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameShooter : MonoBehaviour
{
    public Rigidbody BootRb;
    public float Force;
    public Transform HitPoint;
    public Toggle BroadcastCameraToggle;
    public bool BroadcastCameraOnShoot;
    private IEnumerator _tryShoot;

    void Start()
    {
        BroadcastCameraToggle.isOn = BroadcastCameraOnShoot;
        Game.Instance.CameraPositioning.OnBroadcastCameraToggle(BroadcastCameraOnShoot);
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
        _tryShoot = Shoot();
        StartCoroutine(_tryShoot);

        Game.Instance.GameState = GameState.DuringShooting;
        Game.Instance.CameraPositioning.MoveCamera(CameraPosition.Broadcast, true);
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.2f);

        Game.Instance.CameraPositioning.CameraTrack = CameraTrack.Ball;

        // var target = BootRb.transform.forward;
        var target = BootRb.transform.position - HitPoint.position;
        // target = new Vector3(target.x, 0.5f, target.z);

        // Debug.Log(target);

        var dir = (target - transform.position).normalized * Force;
        // Debug.Log(dir);
        BootRb.AddForce(dir, ForceMode.Impulse);
        //_rb.AddRelativeForce(dir, ForceMode.Acceleration);

        ReplayController.Instance.Record();

        StopCoroutine(_tryShoot);
        _tryShoot = null;
    }
}
