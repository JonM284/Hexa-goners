using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


public class Pirate_Animation_starter : MonoBehaviour {

    private float wait_Time;
    private Animator anim;
    private RuntimeAnimatorController r_A_Controller;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        anim.SetInteger("animNum", 0);
        r_A_Controller = anim.runtimeAnimatorController;
        wait_Time = r_A_Controller.animationClips[1].length/ anim.speed;
        Debug.Log(wait_Time);
	}
	
	public void Set_Animation()
    {
        StartCoroutine(animation_go_off());
    }

    IEnumerator animation_go_off()
    {
        anim.SetInteger("animNum", 1);
        yield return new WaitForSeconds(wait_Time);
        anim.SetInteger("animNum", 0);
    }
}
