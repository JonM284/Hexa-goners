using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AnimationTester : MonoBehaviour {

    private Animator Anim;
    public int MaxAnimatorNum, minAnimatorNum;
    private int currentAnimatorNum;
    public bool hasEnteranceAnim;
    private bool doingAnimation;
	// Use this for initialization
	void Start () {
        Anim = GetComponent<Animator>();
        currentAnimatorNum = 0;
        StartCoroutine(waitForEnterance());
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentAnimatorNum++;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentAnimatorNum--;
        }
        if (currentAnimatorNum < 0 && !hasEnteranceAnim)
        {
            currentAnimatorNum = MaxAnimatorNum;
        }else if (currentAnimatorNum <= 0 && hasEnteranceAnim)
        {
            currentAnimatorNum = MaxAnimatorNum;
        }
        if(currentAnimatorNum < 1 && hasEnteranceAnim){
            currentAnimatorNum = MaxAnimatorNum;
        }
        if (currentAnimatorNum > MaxAnimatorNum && !hasEnteranceAnim)
        {
                currentAnimatorNum = 0;   
        }else if (currentAnimatorNum > MaxAnimatorNum && hasEnteranceAnim)
        {
            currentAnimatorNum = 1;
        }

        Anim.SetInteger("RunNum", currentAnimatorNum);
	}

    IEnumerator waitForEnterance()
    {
        doingAnimation = true;
        yield return new WaitForSeconds(Anim.GetCurrentAnimatorClipInfo(currentAnimatorNum).Length);
        currentAnimatorNum++;
        doingAnimation = false;
    }

    
}
