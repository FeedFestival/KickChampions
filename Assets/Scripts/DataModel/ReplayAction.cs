using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayOption
{
    public Vector3 BallPos;
    public Vector3 BallRot;
    public float Seconds;

    public string String()
    {
        return @"
s: " + this.Seconds + @"
x: " + this.BallPos.x + ", y: " + this.BallPos.y + ", z: " + this.BallPos.z + @"
Rot {
    x: " + this.BallRot.x + ", y: " + this.BallRot.y + ", z: " + this.BallRot.z + @"
}";
    }
}
