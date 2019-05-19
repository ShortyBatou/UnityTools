using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_TriggerAnimation : MonoBehaviour,I_Event {

    public Animator animator;
    public string animationName;

    public void Activate()
    {
        animator.SetTrigger(animationName);
    }
}
