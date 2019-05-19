using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct S_InfoCam
{
    public Vector2 smoothTime;

    public Vector2 decalage;

    public float size;
    public float smoothTimeSize;

    public S_InfoCam(float s, Vector2 d, Vector2 smoothT, float smoothTS)
    {
        this.size = s;
        this.decalage = d;
        this.smoothTime = smoothT;
        this.smoothTimeSize = smoothTS;
    }

}
