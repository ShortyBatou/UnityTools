using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Détruit l'objet donné ( avec la possibilité de mettre un delai avant déstruction )
*/


public class E_DestroyObject : MonoBehaviour,I_Event {
    public GameObject obj;
    public float destroyTime;
    public void Activate()
    {
        Destroy(obj, destroyTime);
    }
}
