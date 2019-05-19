using System.Collections;
using System.Collections.Generic;


public interface I_Killable {

    void TakeDamage(float damage);
    void TakeTrueDamage(float damage);
    void Die();
    bool IsInvicible();
    float GetLife();
}
