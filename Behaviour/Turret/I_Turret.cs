using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Turret {
    void SetCanFire(bool state);
    void Fire();
    void SetFireRate(float rate);
    IEnumerator WaitNextShoot();
}
