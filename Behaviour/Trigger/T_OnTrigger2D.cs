using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Activer les events si un objet possédant un des tags rentre dans le trigger lié au script 
*/
public class T_OnTrigger2D : MonoBehaviour, I_Trigger {
    public bool activated;
    public GameObject[] eventList;
    public string[] tags;
    private List<I_Event> events;
    public void SetActivate(bool state)
    {
        activated = state;
    }

    void Start()
    {
        events = new List<I_Event>();
        foreach (GameObject o in eventList)
        {
            events.AddRange(o.GetComponents<I_Event>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated)
        {
            foreach (string t in tags)
            {
                if (collision.CompareTag(t))
                {
                    foreach (I_Event e in events)
                    {
                        e.Activate();
                    }
                }
            }
        }
    }
}
