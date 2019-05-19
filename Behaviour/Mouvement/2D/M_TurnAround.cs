using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_TurnAround : MonoBehaviour,I_Mouvement
{
    public bool control;
    public float speed;
    public float distance;
    public string[] tags;

    public GameObject target;
    public float angle;

    public bool Activated()
    {
        return control;
    }

    public void SetControl(bool state)
    {
        control = state;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    private void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if(control)
        {
            if(target== null)
            {

                bool find = false;
                
                Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, distance);
                
                foreach (Collider2D c in cols)
                {
                    if(!find)
                    {
                        foreach (string t in tags)
                        {
                            
                            if (c.CompareTag(t))
                            {
                                target = c.gameObject;
                                find = true;
                                angle = Vector2.Angle(target.transform.position, transform.position);
                                break;
                            }
                        }
                    }
                    
                }
            }
            else
            {
                Vector2 position = target.transform.position + new Vector3(Mathf.Cos(angle) * distance,
                Mathf.Sin(angle) * distance);
                transform.position = position;
                angle += Time.deltaTime * speed;
            }
            
        }
    }
}
