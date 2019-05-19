using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Active les events en cas de collision avec un objet prossédant un des tags 
*/

public class T_OnCollision2D : MonoBehaviour,I_Trigger {
    public bool activated;
    public GameObject[] eventList;
    public string[] tags;
    private List<I_Event> events;
    public void SetActivate(bool state)
    {
        activated = state;
    }

    void Start () {
        events = new List<I_Event>();
        foreach (GameObject o in eventList)
        {
            events.AddRange(o.GetComponents<I_Event>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(activated)
        {
            Collider2D c = collision.collider;
            foreach(string t in tags)
            {
                if(c.CompareTag(t))
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
