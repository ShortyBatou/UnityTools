using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Si l'axe y+ de l'objet est alligné avec le joueur alors active les events ( et répète régulièrement si l'axe reste aligné avec le joueur)   
*/

public class T_SeePlayer : MonoBehaviour,I_Trigger {
    public bool activated;
    public float activationRate;
    public float distance;
    private bool seePlayer;
    private float timer;
    public GameObject[] eventList;
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
        timer = 0;
	}
	
	void Update () {
		if(activated)
        {
            timer += Time.deltaTime;
            if(timer > activationRate)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up, distance);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.collider.CompareTag("Environment"))
                    {
                        break;
                    }
                    else if (hit.collider.CompareTag("Player"))
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
	}
}
