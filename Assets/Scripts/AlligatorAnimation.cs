using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AlligatorAnimation : MonoBehaviour {

    private Animator animator;
    [SerializeField]
    private string walkAnimationTrigger;
    [SerializeField]
    private string idleAnimationTrigger;


	// Use this for initialization
	void Awake () {
        animator = GetComponent<Animator>();

    }
    public void Walk()
    {
        animator.SetTrigger(walkAnimationTrigger);
    }
    public void Idle()
    {
        animator.SetTrigger(idleAnimationTrigger);

    }

}
