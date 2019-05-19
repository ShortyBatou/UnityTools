using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Bullet {

    void SetDamage(float damage);
    void OnTriggerEnter2D(Collider2D col);
    
}
