using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_ChangeTimeScale : MonoBehaviour,I_Event
{
    public float scale;
    public float duration;

    public void Activate()
    {
        TimeManager.ChangeTimeScale(scale, duration);
    }
}
