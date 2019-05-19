using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Friction : MonoBehaviour, I_Mouvement
{
    public bool control = true;
    [Range(0,1)]
    public float slowPerTick;
    private Rigidbody2D rb2d;
    public bool Activated()
    {
        return control;
    }

    public void SetControl(bool state)
    {
        control = state;        
    }

    public void SetSpeed(float slowPerTick)
    {
        this.slowPerTick = slowPerTick;
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if(control)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x*slowPerTick, rb2d.velocity.y * slowPerTick);
        }
    }
}
