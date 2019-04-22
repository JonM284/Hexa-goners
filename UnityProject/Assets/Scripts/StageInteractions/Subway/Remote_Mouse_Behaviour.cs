using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remote_Mouse_Behaviour : MonoBehaviour {

    private Rigidbody rb;
    private Vector3 vel, dir;
    public float speed, timer, range;
    private bool has_began;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
	}

    void Set_Dir()
    {
       
    }

    void Movement()
    {
        

        rb.MovePosition(rb.position + Vector3.ClampMagnitude(transform.forward, speed) * Time.deltaTime);
    }
}
