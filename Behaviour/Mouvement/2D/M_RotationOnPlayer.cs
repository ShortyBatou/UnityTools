using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Fait touner l'objet pour que son axe y+ soit vers le joueur
*/

public class M_RotationOnPlayer : MonoBehaviour, I_Mouvement {

    public bool control;
    public float rotationSpeed;
    private GameObject player;
    private Vector2 relaivePosition;
    private float angleRotat;

    public void SetControl(bool state)
    {
        control = state;
    }
    public void SetSpeed(float speed)
    {
        rotationSpeed = speed;
    }
    public bool Activated()
    {
        return control;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void Update () {
        if(control)
        {
            relaivePosition = player.transform.position - transform.position;
            angleRotat = Mathf.Atan2(relaivePosition.y, relaivePosition.x) * Mathf.Rad2Deg - 90;
            Quaternion targetRotation = Quaternion.AngleAxis(angleRotat, Vector3.forward);
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        
    }
}
