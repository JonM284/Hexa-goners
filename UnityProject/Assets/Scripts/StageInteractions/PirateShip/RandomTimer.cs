using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTimer : MonoBehaviour {

    private Animator anim;
    private float timeToWait;
    private bool isInAnimation = false;
    public GameObject myPirate;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        anim.SetInteger("AnimNum", 0);
        isInAnimation = true;
        timeToWait = 0.5f;
        StartCoroutine(waitToShoot());
	}

    public void Set_Off_Pirate()
    {
        myPirate.GetComponent<Pirate_Animation_starter>().Set_Animation();
    }

    IEnumerator waitToShoot()
    {
        while (isInAnimation) {
            
            yield return new WaitForSeconds(timeToWait);
            anim.SetInteger("AnimNum", 1);
            yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(1).Length);
            anim.SetInteger("AnimNum", 0);
            yield return new WaitForSeconds(3f);
            timeToWait = Random.Range(0.5f, 4.0f);
        }
    }
}
