using System;
using System.Collections.Generic;
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

    void Awake()
    {
        _replayController = this;
    }

    public void Record()
    {
        _startRecord = true;
    }

    public void StopRecording()
    {
        _startRecord = false;
        Game.Instance.Ball.CanStopRecording = false;
    }

    public void PlayRecording()
    {
        Debug.ClearDeveloperConsole();

        if (ReplayOptions == null || ReplayOptions.Count == 0)
        {
            return;
        }
        _startRecord = false;
        _shotIndex = 0;
        _startTime = DateTime.Now.AddSeconds(60);
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
            LeanTween.descr(_playId.Value).pause();
            LeanTween.cancel(_playId.Value);
            _playId = null;
        }
        if (_playRotId.HasValue)
        {
            LeanTween.descr(_playRotId.Value).pause();
            LeanTween.cancel(_playRotId.Value);
            _playRotId = null;
        }

        _playId = LeanTween.move(ReplayBall, ReplayOptions[_shotIndex].BallPos, ReplayOptions[_shotIndex].Seconds).id;
        LeanTween.descr(_playId.Value).setEase(LeanTweenType.linear);

        _playRotId = LeanTween.rotate(ReplayBall, ReplayOptions[_shotIndex].BallRot, ReplayOptions[_shotIndex].Seconds).id;
        LeanTween.descr(_playRotId.Value).setEase(LeanTweenType.linear);

        LeanTween.descr(_playId.Value)
            .setOnComplete(() =>
            {
                PlayShot();
            });
        Debug.Log(ReplayOptions[_shotIndex].String());
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

            if (ReplayOptions.Count > 10) {
                Game.Instance.Ball.CanStopRecording = true;
            }

            _elapsedTime = _startTime - DateTime.Now;
            // if (_elapsedTime.Minutes >= 0 && _elapsedTime.Milliseconds >= 0)
            // {
            //     string displayTime = String.Format("{0:60}:{1:00}", _elapsedTime.Minutes, _elapsedTime.Milliseconds);
            //     Debug.Log(displayTime);
            // }

            float seconds = _elapsedTime.Milliseconds * 0.00001f;
            if (seconds > 1)
            {
                seconds = 1;
            }

            var replayOption = new ReplayOption()
            {
                BallPos = Game.Instance.Ball.transform.position,
                BallRot = Game.Instance.Ball.transform.eulerAngles,
                Seconds = seconds
            };
            Debug.Log(replayOption.String());
            ReplayOptions.Add(replayOption);

            _startTime = DateTime.Now.AddSeconds(60);
        }
    }
}
