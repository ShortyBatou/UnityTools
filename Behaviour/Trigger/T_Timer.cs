using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Active les events après un certain laps de temps ( peut boucler )
*/
public class T_Timer : MonoBehaviour, I_Trigger {
    public bool activated;
    public float time;
    public bool loop;
    public GameObject[] eventList;

    private List<I_Event> events;
    private float timer;
    
    public void SetActivate(bool state)
    {
        activated = state;
        if(activated)
        {
            timer = 0;
        }
    }
    private void Start()
    {
        events = new List<I_Event>();
        foreach (GameObject o in eventList)
        {
            events.AddRange(o.GetComponents<I_Event>());
        }
        timer = 0;
    }

    void Update () {
		if(activated)
        {
            timer += Time.deltaTime;
            if(timer > time)
            {
                timer = 0;
                foreach (I_Event e in events)
                {
                    e.Activate();
                }
            }
        }
	}
}
