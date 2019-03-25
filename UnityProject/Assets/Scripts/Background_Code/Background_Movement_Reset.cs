using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Movement_Reset : MonoBehaviour {

    public float distance_From_Origin, speed;
    public Transform origin;
    public bool randomize;
    private Vector3 vel;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.z < origin.position.z - distance_From_Origin)
        {
            Reset_To_Origin();
        }

        vel.z = speed;
        rb.MovePosition(rb.position + vel *Time.deltaTime);
	}

    void Reset_To_Origin()
    {
        if (randomize) {
            transform.position = new Vector3(Random.Range(origin.position.x - 1, origin.position.x + 1), Random.Range(origin.position.y - 5, origin.position.y + 1.5f), origin.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, origin.position.z);
        }
    }
}
