using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    PlayerPrepare,
    LiningUpShot,
    DuringShooting,
    InReplay
}

public class Game : MonoBehaviour
{
    private static Game _game;
    public static Game Instance
    {
        get
        {
            return _game;
        }
    }
    public Ball Ball;
    public Kicker Kicker;
    public Transform HitPoint;
    public CameraPositioning CameraPositioning;
    public GameShooter GameShooter;
    public GameObject ReplayBall;
    

    public GameState GameState;

    void Awake()
    {
        _game = this;
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        LineUpShot();
    }

    public void LineUpShot()
    {
        GameState = GameState.LiningUpShot;
        Ball.Reset();
        Kicker.Reset();
        CameraPositioning.MoveCamera(CameraPosition.Shooting, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            LineUpShot();
        }
    }
}
