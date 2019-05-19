using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, I_Killable {
    public GameObject ship;
    public float life;
    public bool invincible;
    public float invincibiltyTime;
    public Image lifeImage;
    public SpriteRenderer sprite;
    private float maxHealth;
    private bool damageTaken;
    private float timer;
    private float transition;
    private Color baseColor;
    public void Die()
    {
        Destroy(ship);
    }

    public bool IsInvicible()
    {
        return invincible;
    }

    public void TakeDamage(float damage)
    {
        if (!invincible)
        {
            timer = 0;
            damageTaken = true;
            life -= damage;
            lifeImage.fillAmount = life / maxHealth;
            if (life <= 0)
            {
                Die();
            }
            if (invincibiltyTime > 0)
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
        life += damage;
        if (life <= 0)
        {
            Die();
        }
    }

    private void Start()
    {
        timer = 0;
        transition = 0.1f;
        baseColor = new Color(1, 1, 1) - sprite.color;

        maxHealth = life;
    }
    private void Update()
    {
        if (damageTaken)
        {
            timer += Time.deltaTime;
            sprite.color = new Color(1, 1, 1,1) - baseColor * timer / transition;

            if (timer > transition)
            {
                damageTaken = false;
            }
        }
    }
    public float GetLife()
    {
        return life;
    }
}
