using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour {
    private static bool shake;
    private static List<Shaker> shakers;

    private class Shaker
    {
        public float totalTime;
        public float time;
        public float intencity;
        public Shaker(float time, float intencity)
        {
            this.time = time;
            this.intencity = intencity;
            this.totalTime = time;
        }
        public void MinusTime(float minus)
        {
            this.time -= minus;
        }
        public float GetIntencity()
        {
            return intencity * time / totalTime;
        }
        
    }

    


    // Use this for initialization
    void Start () {
        shakers = new List<Shaker>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(shake )
        {

            int length = shakers.Count;
            if (length == 0 )
            {
                shake = false;
                transform.localPosition = new Vector3(0, 0, 0);
            }
            else
            {
                float intencity = 0;
                for(int i=0; i<length;i++)
                {
                    shakers[i].MinusTime(Time.deltaTime);

                    if (shakers[i].time <= 0)
                    {
                        shakers.Remove(shakers[i]);
                        i--;
                        length--;
                    }
                    else
                    {
                        
                        intencity += shakers[i].GetIntencity();
                    }
                    
                }

                Vector3 rand = Random.insideUnitCircle * intencity;
                transform.localPosition = new Vector3(rand.x, rand.y, 0);
            }
        }
	}
    public static void Shake(float time, float intencity)
    {
        shakers.Add(new Shaker(time,intencity));
        shake = true;
    }
}
