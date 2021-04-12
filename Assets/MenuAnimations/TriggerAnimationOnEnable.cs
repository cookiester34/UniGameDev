using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimationOnEnable : MonoBehaviour
{
    public Animator animator;

    private void OnEnable()
    {
        animator.SetTrigger("Play");
    }
}
