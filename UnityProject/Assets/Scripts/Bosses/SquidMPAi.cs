using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidMPAi : MonoBehaviour {

    public Animator myAnim;
    private int current_Anim_Num, attackNum;
    public bool isArm, isHead;
    private bool isDoingEntrance = true;
    public float minWaitTime, maxWaitTime;
    public int min_Attack_Num, max_Attack_Num;
    private float waitTime;
    public static bool Boss_Can_Attack;
    public Manager myManager;
    public GameObject warningIndicator;
    public Sprite FullSweep, QuarterSweep;
    private bool doingAttack;


	// Use this for initialization
	void Start () {
        myAnim = GetComponent<Animator>();
        if (isArm) {
            warningIndicator.SetActive(false);
        }else if (isHead)
        {
            FullSweep = null;
            QuarterSweep = null;
            warningIndicator = null;
        }
        StartCoroutine(DoEntrance());
        waitTime = Random.Range(minWaitTime, maxWaitTime);
    }
	
	// Update is called once per frame
	void Update () {
        if (!isDoingEntrance)
        {
            DoAnimations();
        }
        if (isArm) {
            if (waitTime >= 0 && !myManager.countDown && !doingAttack)
            {
                waitTime -= Time.deltaTime;
            } else if (waitTime < 0 && !doingAttack)
            {
                StartCoroutine(DoAttack());
            }
        }
	}

    void DoAnimations()
    {
        
        if (isHead && current_Anim_Num != 1)
        {
            current_Anim_Num = 1;
        }


        myAnim.SetInteger("CurrentNum", current_Anim_Num);
    }

    IEnumerator DoEntrance()
    {
        if (isHead) {
            yield return new WaitForSeconds(myAnim.GetCurrentAnimatorClipInfo(current_Anim_Num).Length);
        }else if (isArm)
        {
            yield return new WaitForSeconds(myAnim.GetCurrentAnimatorClipInfo(current_Anim_Num).Length);
        }
        isDoingEntrance = false;
        
        current_Anim_Num = 1;
        
    }

    

    IEnumerator DoAttack()
    {
        doingAttack = true;
        attackNum = Random.Range(min_Attack_Num, max_Attack_Num);
        if (attackNum == 2)
        {
            warningIndicator.SetActive(true);
            warningIndicator.GetComponent<SpriteRenderer>().sprite = QuarterSweep;
            for (int i = 0; i < 5; i++)
            {
                warningIndicator.GetComponent<SpriteRenderer>().enabled = true;
                yield return new WaitForSeconds(0.1f);
                warningIndicator.GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(0.1f);
            }
        }
        if (attackNum == 3)
        {
            warningIndicator.SetActive(true);
            warningIndicator.GetComponent<SpriteRenderer>().sprite = FullSweep;
            for (int i = 0; i < 5; i++)
            {
                warningIndicator.GetComponent<SpriteRenderer>().enabled = true;
                yield return new WaitForSeconds(0.1f);
                warningIndicator.GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(0.1f);
            }
        }
        warningIndicator.SetActive(false);
        current_Anim_Num = attackNum; 
        
        yield return new WaitForSeconds(1.5f);
        doingAttack = false;
        current_Anim_Num = 1;
        waitTime = Random.Range(minWaitTime, maxWaitTime);
        
    }

    

    
}
