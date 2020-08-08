using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.WSA.Persistence;

public class CanvasController : MonoBehaviour
{
    private static CanvasController _canvasController;
    public static CanvasController Instance{
        get { return _canvasController; }
    }

    void Awake()
    {
        _canvasController = this;
    }

    public Slider SliderRotX;
    public Slider SliderRotY;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
