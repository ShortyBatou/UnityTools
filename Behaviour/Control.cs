using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour {
    public float speed;
    private Rigidbody2D rb2d;
    
    private void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update () {
        float ho = Input.GetAxis("Horizontal") * speed;
        float ver = Input.GetAxis("Vertical") * speed;
        if (ho == 0  && ver == 0)
        {
            rb2d.velocity = new Vector2(0, 0);
        }
        else
        {
            rb2d.velocity = new Vector2(ho, ver);
        }
    }
}
