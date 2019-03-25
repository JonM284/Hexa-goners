using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab_Saw_Behaviour : MonoBehaviour {

    private Animator anim;
    private int animation_Num = 0;
    private float attack_Pause_Timer_Max;
    private bool is_Attacking = false;
    private int current_Animation_Num;

    [Header("Wait time")]
    [Tooltip("Timer for animations to wait till next attack")]
    public float attack_Pause_Timer;
    [Header("Attack Anim number")]
    [Tooltip("Maximum number of attack animations")]
    public int attack_Animations_Max;
    [Header("Modifier Num")]
    [Tooltip("This is the amount that will be subtracted from the maximum time, it will be the minimum amount of time to wait")]
    public float pause_Modifier_Num;

    private Manager myManager;


    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        myManager = Manager.manager_Instance;
        attack_Pause_Timer_Max = attack_Pause_Timer;
        current_Animation_Num = animation_Num;
	}
	
	// Update is called once per frame
	void Update () {
        if (attack_Pause_Timer >= 0 && !myManager.countDown && !is_Attacking)
        {
            attack_Pause_Timer -= Time.deltaTime;
        }
        if(attack_Pause_Timer < 0 && !is_Attacking)
        {
            StartCoroutine(waitToReset());
        }
        anim.SetInteger("Anim_Num", animation_Num);
	}

    IEnumerator waitToReset()
    {
        Debug.Log("doing attack animation");
        is_Attacking = true;
        while (current_Animation_Num == animation_Num)
        {
            current_Animation_Num = Random.Range(1, attack_Animations_Max);
            if (current_Animation_Num == animation_Num)
            {
                Debug.Log("New number is the same :( trying again");
            }else if (current_Animation_Num != animation_Num)
            {
                Debug.Log("New number is different :) will now leave while loop");
            }
        }
        animation_Num = current_Animation_Num;
        Debug.Log("Now outside of while loop");
        yield return new WaitForSeconds(6f);
        attack_Pause_Timer = Random.Range(attack_Pause_Timer_Max - pause_Modifier_Num, attack_Pause_Timer_Max);
        is_Attacking = false;
   
        Debug.Log("new wait timer is: " + attack_Pause_Timer);
    }
}
