using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameShooter : MonoBehaviour
{
    public bool DebugVisually;
    private Rigidbody _kickerRb;
    public float Force;
    public Transform HitPoint;
    public Toggle BroadcastCameraToggle;
    public bool BroadcastCameraOnShoot;
    private IEnumerator _tryShoot;

    private Vector3 _kickerShootPos;
    private Vector3 _shootingDirection;
    private bool _paused;

    void Start()
    {
        BroadcastCameraToggle.isOn = BroadcastCameraOnShoot;
        Game.Instance.CameraPositioning.OnBroadcastCameraToggle(BroadcastCameraOnShoot);
        _kickerRb = Game.Instance.Kicker.GetComponent<Rigidbody>();
        Game.Instance.Kicker.BootCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            if (_paused) {
                _paused = false;
                Time.timeScale = 1;
                return;
            }
            TryShoot();
        }

        if (DebugVisually)
        {
            Debug.DrawRay(_kickerShootPos, _shootingDirection, Color.blue);
        }
    }

    public void TryShoot()
    {
        Game.Instance.Kicker.BootCollider.enabled = true;

        _tryShoot = Shoot();
        StartCoroutine(_tryShoot);

        Game.Instance.GameState = GameState.DuringShooting;
        Game.Instance.CameraPositioning.MoveCamera(CameraPosition.Broadcast, true);
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.2f);

        Game.Instance.CameraPositioning.CameraTrack = CameraTrack.Ball;

        _kickerShootPos = _kickerRb.transform.position + new Vector3(0.3f, 0, 0);
        _shootingDirection = HitPoint.position - _kickerShootPos;

        _shootingDirection = _shootingDirection.normalized * Force;
        _kickerRb.AddForce(_shootingDirection, ForceMode.Impulse);
        //_rb.AddRelativeForce(dir, ForceMode.Acceleration);

        ReplayController.Instance.Record();

        StopCoroutine(_tryShoot);
        _tryShoot = null;

        if (DebugVisually)
        {
            _paused = true;
            Time.timeScale = 0;
        }
    }
}
