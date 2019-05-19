using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Active un effet de particule 
*/

public class E_Particle : MonoBehaviour, I_Event
{
    public ParticleSystem particle;
    public void Activate()
    {
        particle.Play();
    }
}
