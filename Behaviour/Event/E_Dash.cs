using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Si activer stop tout les scripts de déplacement donnée en paramètre et avance tout droit jusqu'à
 * atteindre la distance souhaité ou si collision avec un collider définie par son tag
*/
public class E_Dash : MonoBehaviour, I_Event {

    public bool dashing;
    public float speed;
    public float cooldown;
    public GameObject ship;
    public float distance;
    public string[] tags;
    private float traveledDistance;
    private Vector2 lastPosition;
    private List<I_Mouvement> actifMouvements;
    private Rigidbody2D rb2d;
    private bool canDash;
    
    public void Activate()
    {

        if(canDash)
        {
            dashing = true;
            canDash = false;
            traveledDistance = 0;
            I_Mouvement[] mouvements = ship.GetComponentsInChildren<I_Mouvement>();
            foreach (I_Mouvement m in mouvements)
            {
                if (m.Activated())
                {
                    m.SetControl(false);
                    actifMouvements.Add(m);
                }
            }
            
        }
        
    }

    void Start () {
        canDash = true;
        rb2d = ship.GetComponent<Rigidbody2D>();
        actifMouvements = new List<I_Mouvement>();
        dashing = false;
        traveledDistance = 0;
        lastPosition = transform.position;
       
    }

    void Update()
    {
        if (dashing)
        {
            rb2d.velocity = transform.up.normalized * speed;
            traveledDistance += Vector2.Distance(ship.transform.position, lastPosition);
            lastPosition = ship.transform.position;
            if (traveledDistance > distance)
            {
                dashing = false;
                foreach (I_Mouvement m in actifMouvements)
                {
                    m.SetControl(true);
                }
                actifMouvements.Clear();
                rb2d.velocity = Vector2.zero;
                StartCoroutine(WaitCooldown());
            }
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
    }
    private IEnumerator WaitCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        canDash = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D col = collision.collider;
        foreach (string t in tags)
        {
            if (col.CompareTag(t))
            {
                dashing = false;
                traveledDistance = 0;
                rb2d.velocity = Vector2.zero;
                foreach (I_Mouvement m in actifMouvements)
                {
                    m.SetControl(true);
                }
                StartCoroutine(WaitCooldown());
                actifMouvements.Clear();
            }
                
        }
        
    }
}
