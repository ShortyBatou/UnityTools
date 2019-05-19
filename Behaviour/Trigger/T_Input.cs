using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class T_Input : MonoBehaviour,I_Trigger {
    public bool activated;
    public GameObject[] eventList;
    private List<I_Event> events;
    public void SetActivate(bool state)
    {
        
    }

    // Use this for initialization
    void Start () {
        activated = false;
        events = new List<I_Event>();
        foreach(GameObject e in eventList)
        {
            events.AddRange(e.GetComponents<I_Event>());
        }
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.T))
        {
            foreach(I_Event e in events)
            {
                e.Activate();
            }
        }
	}
}
