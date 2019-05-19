using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Random : MonoBehaviour, I_Event
{
    [Range(0,1)]
    public float chance;
    public GameObject events;
    public void Activate()
    {
        float coef = Random.Range(0.0f, 1.0f);
       
        if(coef < chance)
        {
            I_Event[] eventList = events.GetComponents<I_Event>();
            
            foreach (I_Event e in eventList)
            {
                e.Activate();
            }
            
        }
       
    }
}
