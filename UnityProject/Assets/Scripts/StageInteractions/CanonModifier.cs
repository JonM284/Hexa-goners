using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonModifier : MonoBehaviour {

    private Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        anim.speed = Random.Range(0.1f, 0.21f);
	}
	
	
}
