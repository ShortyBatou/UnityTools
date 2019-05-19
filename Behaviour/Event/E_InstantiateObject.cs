using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Instancie un objet et le met dans les objets temporaires ( vider entre chaques niveau ) 
*/
public class E_InstantiateObject : MonoBehaviour, I_Event {
    public GameObject obj;
    
    public void Activate()
    {
        GameObject o = Instantiate(obj, transform.position, transform.rotation);
        o.SetActive(true);
        o.transform.SetParent(GameObject.FindGameObjectWithTag("Temporary").transform);
    }

}
