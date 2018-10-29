using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.AI;

public class AlligatorAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private string walkingAnimationName;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Walk()
    {
        animator.SetBool(walkingAnimationName, true);
    }
    public void Idle()
    {
        animator.SetBool(walkingAnimationName, false);
    }

}
