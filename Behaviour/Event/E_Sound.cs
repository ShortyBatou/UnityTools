using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Active un son avec un volume donnée
*/

public class E_Sound : MonoBehaviour, I_Event {
    public AudioSource audioSource;
    public AudioClip sound;
    [Range(0,1)]
    public float volume;

    public void Activate()
    {
        audioSource.PlayOneShot(sound,volume);
    }
}
