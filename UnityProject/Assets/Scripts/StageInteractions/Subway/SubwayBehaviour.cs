using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SubwayBehaviour : MonoBehaviour {

    private Animator anim;
    public GameObject[] warningIndicator;

    private int animationNum, currentAnimationNum;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        currentAnimationNum = animationNum;
        warningIndicator[0].GetComponent<SpriteRenderer>().enabled = false;
        warningIndicator[1].GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(WaitForNextAnimation()); 
	}

   



    void pickNewAnimation()
    {
        animationNum = Random.Range(0,2);
        if (animationNum == currentAnimationNum)
        {
            pickNewAnimation();
            Debug.Log("Re-Picking");
        }else if(animationNum != currentAnimationNum)
        {
            currentAnimationNum = animationNum;
            Debug.Log("Chose new number");
            StartCoroutine(WaitForNextAnimation());
        }
    }

    IEnumerator WaitForNextAnimation()
    {
        if (currentAnimationNum == 1)
        {
            for (int i = 0; i < 5; i++)
            {
                warningIndicator[0].GetComponent<SpriteRenderer>().enabled = true;
                warningIndicator[1].GetComponent<SpriteRenderer>().enabled = true;
                yield return new WaitForSeconds(0.1f);
                warningIndicator[0].GetComponent<SpriteRenderer>().enabled = false;
                warningIndicator[1].GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(0.1f);
            }
        }
        anim.SetInteger("ConditionNum", animationNum);
        Debug.Log("started wait");
        
        yield return new WaitForSeconds(15f);
        pickNewAnimation();
    }
}
