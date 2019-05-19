using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D_OnTrajectory : MonoBehaviour, I_Damage
{
    public bool activated = true;
    public float damage;
    public float lenght;
    public LayerMask layer;
    public string[] tags;
    public float sample_speed;
    public float sample_time;
    public LineRenderer lineRenderer;
    private List<Vector2> trajectory;
    private float timer;
    private float totalDistance;

    private void Start()
    {
        timer = sample_speed - Time.deltaTime*2;
        trajectory = new List<Vector2>();
        
    }

    private void Update()
    {
        UpdateTrajectory();
        DisplayTrajectory();
        CheckTrajectory();

    }
    private void UpdateTrajectory()
    {
        timer += Time.deltaTime;
        if (timer > sample_speed)
        {
            UpdateLineRenderer();
            trajectory.Add(new Vector2(transform.position.x, transform.position.y));
            if (trajectory.Count >= 2)
            {
                totalDistance += Vector2.Distance(trajectory[trajectory.Count - 1], trajectory[trajectory.Count - 2]);
            }
            timer = 0;
        }
        
        if (trajectory.Count == 1)
        {
            float speed = sample_time * Time.deltaTime;
            totalDistance -= speed;
            
            trajectory[0] = Vector2.MoveTowards(trajectory[0], transform.position,speed);
            
            if (trajectory[0] == new Vector2(transform.position.x, transform.position.y) && timer > 0)
            {
                trajectory.RemoveAt(0);
                UpdateLineRenderer();
            }
        }
        else if (trajectory.Count >= 2)
        {
            float speed = sample_time * Time.deltaTime;
            totalDistance -= speed;
            trajectory[0] = Vector2.MoveTowards(trajectory[0], trajectory[1], speed);


            if (trajectory[0] == trajectory[1])
            {
                trajectory.RemoveAt(0);
                UpdateLineRenderer();
            }


        }
    }
    private void DisplayTrajectory()
    {
        if(trajectory.Count >=1 )
        {
            lineRenderer.positionCount = trajectory.Count + 1;

            lineRenderer.SetPosition(trajectory.Count, transform.position);
            lineRenderer.SetPosition(0, trajectory[0]);
            for (int i = 0; i < trajectory.Count; i++)
            {
                lineRenderer.SetPosition(i, trajectory[i]);
            }
        }
        
    }
    private void UpdateLineRenderer()
    {
        for (int i = 0; i < trajectory.Count; i++)
        {
            lineRenderer.SetPosition(i, trajectory[i]);
        }
    }

    private void CheckTrajectory()
    {
        for(int i = 0; i < trajectory.Count; i++)
        {
            
            Vector2 origin = trajectory[i];
            Vector2 direction;
            if (i+1< trajectory.Count)
            {
                direction = trajectory[i + 1] - trajectory[i];
            }
            else
            {
                direction = new Vector2(transform.position.x, transform.position.y) - trajectory[i];
            }
            RaycastHit2D[] hits = Physics2D.CircleCastAll(origin, lenght, direction, direction.magnitude, layer);
            List<I_Killable> targets = new List<I_Killable>();
            foreach(RaycastHit2D hit in hits)
            {
                foreach(string tag in tags)
                {
                    if (hit.collider.CompareTag(tag))
                    {
                        I_Killable target = hit.collider.GetComponent<I_Killable>();
                        if(!targets.Contains(target))
                        {
                            targets.Add(hit.collider.GetComponent<I_Killable>());
                        }
                        
                    }
                }
            }
            foreach(I_Killable target in targets)
            {
                target.TakeDamage(damage);
            }
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        trajectory.Add(new Vector2(transform.position.x, transform.position.y));
        if (trajectory.Count >= 2)
        {
            totalDistance += Vector2.Distance(trajectory[trajectory.Count - 1], trajectory[trajectory.Count - 2]);
        }
    }
    
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetActive(bool state)
    {
        activated = state;
    }
}
