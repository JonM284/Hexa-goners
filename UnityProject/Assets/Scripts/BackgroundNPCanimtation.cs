using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BackgroundNPCanimtation : MonoBehaviour {

    private Animator anim;
    private int randomAnim_ID;
    private float JumpingTimer;
    public int max_Anim_ID;
    public static bool PlayerDeath = false;

	// Use this for initialization
	void Start () {
        
        anim = GetComponent<Animator>();
        JumpingTimer = anim.GetCurrentAnimatorClipInfo(0).Length;
        StartCoroutine(WaitToSwitch());
	}
	
	// Update is called once per frame
	void LateUpdate () {
        DoAnimations();
	}

    void DoAnimations()
    {
        if (PlayerDeath)
        {
            anim.SetInteger("CurrentInt", 0);
            StartCoroutine(WaitToSwitch());
        }else
        {
            anim.SetInteger("CurrentInt", randomAnim_ID);
        }
    }

    IEnumerator WaitToSwitch()
    {
        yield return new WaitForSeconds(JumpingTimer - 0.3f);
        randomAnim_ID = Random.Range(1,max_Anim_ID);
        PlayerDeath = false;
    }
}
