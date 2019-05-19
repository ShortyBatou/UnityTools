using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu_Laser : MonoBehaviour, I_Turret
{
    public bool canFire;
    public float damage;
    public float distance;
    public bool stopOnCollision;
    public LayerMask layers;
    public string[] tags;
   
    private Vector2[] hitsPosition;
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (canFire)
        {
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }
        hitsPosition = new Vector2[0];
        
    }
    void Update()
    {
        if(canFire)
        {
            Fire();
            UpdateLineRenderer();
        }
    }
    public void Fire()
    {
        RaycastHit2D[] hits = new RaycastHit2D[0];
        if (stopOnCollision)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, distance, layers);
            if(hit)
            {
                hits = new RaycastHit2D[1];
                hits[0] = hit;
            }
     
        }
        else
        {
            hits = Physics2D.RaycastAll(transform.position, transform.up, distance, layers);
        }

        if(hits.Length > 0)
        {
            hitsPosition = new Vector2[hits.Length];
            for(int i=0;i<hits.Length;i++)
            {
                hitsPosition[i] = hits[i].point;
                foreach (string tag in tags)
                {
                    if (hits[i].collider.CompareTag(tag))
                    {
                        hits[i].collider.GetComponent<I_Killable>().TakeDamage(damage);
                    }
                }

            }
        }
        else
        {
            hitsPosition = new Vector2[1];
            hitsPosition[0] = transform.position + transform.up * distance;
        }

        
       

    }
    public void SetCanFire(bool state)
    {
        this.canFire = state;
        if (canFire)
        {
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    public void SetFireRate(float rate)
    {

    }

    public IEnumerator WaitNextShoot()
    {
        yield return new WaitForSeconds(0);
    }

    public void UpdateLineRenderer()
    {
        if(hitsPosition.Length > 0)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hitsPosition[hitsPosition.Length-1]);
        }
        else
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);
        }
    }
    public void affiche()
    {
        string text = "";
        foreach(Vector2 position in hitsPosition)
        {
            text += position + "  :  ";
        }
        Debug.Log(text);
    }
}
