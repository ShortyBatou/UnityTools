using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Mouvement {
    void SetControl(bool state);
    void SetSpeed(float speed);
    bool Activated();
}
