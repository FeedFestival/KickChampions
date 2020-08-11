using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private static Game _game;
    public static Game Instance {
        get {
            return _game;
        }
    }
    public Ball Ball;
    public Kicker Kicker;
    public CameraPositioning CameraPositioning;

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
        Ball.Reset();
        Kicker.Reset();
        CameraPositioning.MoveCamera(CameraPosition.Shooting, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Init();
        }
    }
}
