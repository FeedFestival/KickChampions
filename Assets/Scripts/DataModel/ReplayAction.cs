using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayOption
{
    public Vector3 BallPos;
    public Vector3 BallRot;
    public float Seconds;

    public string String(bool flat = false)
    {
        // if (flat) {
        return "s:{" + this.Seconds + "}p:{" + BallPosToString() + "}r:{ " + BallRotToString() + "}";
        // }
        //         return @"
        // s:{" + this.Seconds + @"}
        // p:{" + BallPosToString() + @"}
        // r:{" + BallRotToString() + "}";
    }

    public string BallPosToString()
    {
        return "x: " + this.BallPos.x + ", y: " + this.BallPos.y + ", z: " + this.BallPos.z;
    }

    public string BallRotToString()
    {
        return "x: " + this.BallRot.x + ", y: " + this.BallRot.y + ", z: " + this.BallRot.z;
    }

    public bool IsEqual(ReplayOption otherRo)
    {
        if (
            BallPosToString() == otherRo.BallPosToString()
            && BallRotToString() == otherRo.BallRotToString()
        )
        {
            return true;
        }
        return false;
    }
}
