using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_SetMouvement : MonoBehaviour, I_Event
{
    public GameObject[] objects;
    public bool state;
    public void Activate()
    {

        foreach(GameObject o in objects)
        {
            I_Mouvement[] mvts = o.GetComponents<I_Mouvement>();
            foreach(I_Mouvement m in mvts)
            {
                m.SetControl(state);
            }
        }
    }

}
