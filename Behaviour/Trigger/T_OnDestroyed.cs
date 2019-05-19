using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Si l'objet ,auquel le script est rataché, est détruit alors active les events 
*/
public class T_OnDestroyed : MonoBehaviour, I_Trigger {
    public bool activated;
    public GameObject[] eventList;
    private List<I_Event> events;

    void Start () {
        events = new List<I_Event>();
        foreach (GameObject o in eventList)
        {
            events.AddRange(o.GetComponents<I_Event>());
        }
    }

    private void OnDestroy()
    {
        if(activated)
        {
            foreach (I_Event e in events)
            {
                e.Activate();
            }
        }
        
    }

    public void SetActivate(bool state)
    {
        activated = state;
    }
}
