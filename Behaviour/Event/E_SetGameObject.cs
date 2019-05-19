using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_SetGameObject : MonoBehaviour, I_Event
{
    public GameObject[] objects;
    public bool state;
    public void Activate()
    {
        foreach (GameObject o in objects)
        {
            o.SetActive(state);
        }
    }

    
}
