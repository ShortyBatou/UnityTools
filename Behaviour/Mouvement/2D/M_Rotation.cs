using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Fait tourner un objet sur son axe z
*/

public class M_Rotation : MonoBehaviour,I_Mouvement {

    public GameObject ship;
    public float speed;
    public bool control;
    private float angle;

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

	void Update () {
		if(control)
        {
            ship.transform.rotation = Quaternion.Euler(0, 0, angle * speed);
            angle += Time.deltaTime;
        }
	}
}
