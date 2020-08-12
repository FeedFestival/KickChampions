using System;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

public class ReplayController : MonoBehaviour
{
    public GameObject ReplayBall;
    private static ReplayController _replayController;
    public static ReplayController Instance
    {
        get
        {
            return _replayController;
        }
    }
    public List<ReplayOption> ReplayOptions;
    private bool _startRecord;
    private int? _playId;
    private int? _playRotId;
    private int _shotIndex;

    private DateTime _startTime;
    private TimeSpan _elapsedTime;
    private DateTime _fullStartTime;
    private TimeSpan _fullElapsedTime;
    private float _percentOffset;

    private int frames;

    void Awake()
    {
        _replayController = this;
    }

    public void Record()
    {
        _startTime = DateTime.Now.AddSeconds(60);
        _fullStartTime = DateTime.Now;
        _startRecord = true;
    }

    public void StopRecording()
    {
        _fullElapsedTime = DateTime.Now - _fullStartTime;
        // Debug.Log(_fullElapsedTime.Seconds);
        Debug.Log(ReplayOptions.Count);
        float fullTime = 0;
        ReplayOptions.ForEach(rp => fullTime += rp.Seconds);
        // Debug.Log(fullTime);

        _percentOffset = Mathf.Floor(UsefullUtils.GetValuePercent(_fullElapsedTime.Seconds, fullTime));

        _startRecord = false;
        Game.Instance.Ball.CanStopRecording = false;
    }

    public void PlayRecording()
    {
        Debug.ClearDeveloperConsole();
        Debug.Log(ReplayOptions.Count);

        if (ReplayOptions == null || ReplayOptions.Count == 0)
        {
            return;
        }
        _startRecord = false;
        _shotIndex = 0;
        ReplayBall.transform.position = ReplayOptions[_shotIndex].BallPos;
        ReplayBall.transform.eulerAngles = ReplayOptions[_shotIndex].BallRot;

        PlayShot();
    }

    private void PlayShot()
    {
        _shotIndex++;
        if (_shotIndex >= ReplayOptions.Count)
        {
            return;
        }

        if (_playId.HasValue)
        {
            // LeanTween.descr(_playId.Value).pause();
            LeanTween.cancel(_playId.Value);
            _playId = null;
        }
        if (_playRotId.HasValue)
        {
            // LeanTween.descr(_playRotId.Value).pause();
            LeanTween.cancel(_playRotId.Value);
            _playRotId = null;
        }

        var time = UsefullUtils.GetPercent(ReplayOptions[_shotIndex].Seconds, _percentOffset);

        _playId = LeanTween.move(ReplayBall, ReplayOptions[_shotIndex].BallPos, time).id;
        LeanTween.descr(_playId.Value).setEase(LeanTweenType.linear);

        _playRotId = LeanTween.rotate(ReplayBall, ReplayOptions[_shotIndex].BallRot, time).id;
        LeanTween.descr(_playRotId.Value).setEase(LeanTweenType.linear);

        LeanTween.descr(_playId.Value)
            .setOnComplete(() =>
            {
                PlayShot();
            });
        // Debug.Log(ReplayOptions[_shotIndex].String());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_startRecord)
        {
            if (ReplayOptions == null)
            {
                ReplayOptions = new List<ReplayOption>();
            }

            if (ReplayOptions.Count > 10)
            {
                Game.Instance.Ball.CanStopRecording = true;
            }

            frames++;
            if (frames == 4)
            {
                frames = 0;
            }
            else
            {
                return;
            }

            _elapsedTime = _startTime - DateTime.Now;
            float seconds = (float)(Math.Truncate(Convert.ToDouble(_elapsedTime.Milliseconds * 0.00001f) * 1000) / 1000);
            if (seconds > 1)
            {
                seconds = 1;
            }

            var posX = (float)Math.Round((Double)Game.Instance.Ball.transform.position.x, 2);
            var posY = (float)Math.Round((Double)Game.Instance.Ball.transform.position.y, 2);
            var posZ = (float)Math.Round((Double)Game.Instance.Ball.transform.position.z, 2);

            var rotX = (float)Math.Round((Double)Game.Instance.Ball.transform.eulerAngles.x, 2);
            var rotY = (float)Math.Round((Double)Game.Instance.Ball.transform.eulerAngles.y, 2);
            var rotZ = (float)Math.Round((Double)Game.Instance.Ball.transform.eulerAngles.z, 2);

            var replayOption = new ReplayOption()
            {
                BallPos = new Vector3(posX, posY, posZ),
                BallRot = new Vector3(rotX, rotY, rotZ),
                Seconds = seconds
            };

            var index = ReplayOptions.FindIndex(rp => rp.IsEqual(replayOption));
            if (index >= 0)
            {
                // ReplayOptions[index].Seconds += replayOption.Seconds;
                _startTime = DateTime.Now.AddSeconds(60);
                // Debug.Log("Exists");
                return;
            }

            // Debug.Log(replayOption.String());
            ReplayOptions.Add(replayOption);

            _startTime = DateTime.Now.AddSeconds(60);
        }
    }
}
