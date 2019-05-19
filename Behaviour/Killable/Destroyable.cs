using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Destroyable : MonoBehaviour,I_Killable {
    public GameObject objectToDestroy;
    public float life;
    public bool invincible;
    public float invincibiltyTime;
    public GameObject OnDeath;
    public GameObject OnDamageTaken;
    public Image lifeUI;
    public SpriteRenderer sprite;
    private I_Event[] damage_events;
    private I_Event[] death_events;
    private Color baseColor;
    private float maxHealth;
    private bool damageTaken;
    private float timer;
    private float transition;
    private bool dead;
    private void Start()
    {
        dead = false;
        damageTaken = false;
        transition = 0.2f;
        baseColor = new Color(1, 1, 1) - sprite.color;
        maxHealth = life;
        timer = 0;
        if (OnDamageTaken != null)
        {
            damage_events = OnDamageTaken.GetComponents<I_Event>();
        }
        if(OnDeath != null)
        {
            death_events = OnDeath.GetComponents<I_Event>();
        }
    }

    public void Update()
    {
        if(damageTaken)
        {
            timer += Time.deltaTime;
            sprite.color = new Color(1, 1, 1) - baseColor * timer/transition;
            if(timer > transition)
            {
                damageTaken = false;
            }
        }
    }

    public void Die()
    { 
        if(!dead)
        {
            dead = true;
            foreach (I_Event e in death_events)
            {
                e.Activate();
            }
            Destroy(objectToDestroy);
        }
        
    }

    public bool IsInvicible()
    {
        return invincible;
    }

    public void TakeDamage(float damage)
    {
        if(!invincible)
        {
            timer = 0;
            damageTaken = true;
            life -= damage;
            if (damage_events != null)
            {
                foreach (I_Event e in damage_events)
                {
                    e.Activate();
                }
            }
            lifeUI.fillAmount = life / maxHealth;
            if (life <= 0)
            {
                Die();
            }
            if(invincibiltyTime > 0)
            {
                StartCoroutine(WaitInvicible());
                
            }
           
        }
    }
    
    private IEnumerator WaitInvicible()
    {
        invincible = true;
        yield return new WaitForSeconds(invincibiltyTime);
        invincible = false;
    }

    public void TakeTrueDamage(float damage)
    {
        timer = 0;
        damageTaken = true;
        life -= damage;
        if (life <= 0)
        {
            Die();
        }
    }

    public float GetLife()
    {
        return life;
    }
}
