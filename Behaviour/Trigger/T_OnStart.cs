using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Activer les events lors de la création de l'objet auquel le script est rataché 
*/
public class T_OnStart : MonoBehaviour,I_Trigger {

    public bool activated;
    public GameObject[] eventList;
    private List<I_Event> events;
    public void SetActivate(bool state)
    {
        activated = state;
    }

    void Start()
    {
        events = new List<I_Event>();
        if (activated)
        {
            foreach (GameObject o in eventList)
            {
                events.AddRange(o.GetComponents<I_Event>());
            }
            foreach (I_Event e in events)
            {
                e.Activate();
            }
        }

    }

}
