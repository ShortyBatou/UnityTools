using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_SetAnimation : MonoBehaviour,I_Event {
    public Animator animator;
    public string animationName;
    public bool state;

    public void Activate()
    {
        animator.SetBool(animationName, state);
    }
}
