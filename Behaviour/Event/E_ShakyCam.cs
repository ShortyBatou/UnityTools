using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_ShakyCam : MonoBehaviour, I_Event {
    public float intencity;
    public float time;
    public void Activate()
    {
        CameraShaker.Shake(time, intencity);
    }



}
