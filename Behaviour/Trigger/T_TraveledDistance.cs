using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Si l'objet parcours une certaine distance alors acitve tous les events 
*/
public class T_TraveledDistance : MonoBehaviour, I_Trigger {
    public bool activated;
    public float distance;
    public GameObject[] eventList;
    private List<I_Event> events;
    private float distanceTraveled;
    private Vector2 lastPosition;

    public void SetActivate(bool state)
    {
        activated = state;
    }

    void Start () {
        events = new List<I_Event>();
        lastPosition = transform.position;
        distanceTraveled = 0;
        foreach (GameObject o in eventList)
        {
            events.AddRange(o.GetComponents<I_Event>());
        }
    }


    void Update () {
		if(activated)
        {
            distanceTraveled += Vector2.Distance(lastPosition, transform.position);
            lastPosition = transform.position;
            if(distanceTraveled > distance)
            {
                foreach (I_Event e in events)
                {
                    e.Activate();
                }
                distanceTraveled -= distance;
            }
        }
	}
}
