using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIOptions : MonoBehaviour
{
    public List<AIOption> AIShots;

    private IEnumerator _shootAsAI;
    // Start is called before the first frame update
    void Start()
    {
        InitAIShots();
    }

    public void PrepareToShootAsAI()
    {
        var shot = AIShots[0];
        
        Game.Instance.Ball.Reset();
        Game.Instance.Ball.MaxLeftPerc = shot.BallMaxLeftPerc;
        Game.Instance.Ball.TimeInMagnus = shot.TimeInMagnus;
        Game.Instance.Ball.TimeOutMagnus = shot.TimeOutMagnus;

        Game.Instance.Kicker.Reset();
        Game.Instance.Kicker.MoveTo(shot.KickPos, shot.KickRot);

        Game.Instance.GameShooter.Force = shot.GameShooterForce;
        Game.Instance.CameraPositioning.MoveToBroadcast(true);

        _shootAsAI = ShootAsAI();
        StartCoroutine(_shootAsAI);
    }

    IEnumerator ShootAsAI()
    {
        yield return new WaitForSeconds(1f);

        Game.Instance.GameShooter.TryShoot();
        StopCoroutine(_shootAsAI);
        _shootAsAI = null;
    }

    void InitAIShots()
    {
        AIShots = new List<AIOption>{
            new AIOption{
                BallMaxLeftPerc = 4.8f,
                TimeInMagnus = 0.4f,
                TimeOutMagnus = 0.8f,
                KickPos = new Vector3(-0.499f, 0.111f, -11.602f),
                KickRot = new Vector3(-89.98f, 47.79f, 0),
                GameShooterForce = 30
            }
        };
    }
}
