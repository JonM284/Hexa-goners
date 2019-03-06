using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicShotBehaviour : MonoBehaviour {

    public float speed;
    public int playerNum;
    private bool isTouchingGround = true, hasTouched = false;
    public float fall_timer;
    private float original_Fall_Timer;
    public Color myPlayersColor;

    // Use this for initialization
    void Start () {
        GetComponent<Collider>().enabled = false;
        original_Fall_Timer = fall_timer;
        StartCoroutine(turnOnCollider());
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        if (isTouchingGround)
        {
            if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 1.5f))
            {
                if (hit.collider != null && (hit.collider.name == "Floor" || hit.collider.name == "Environment"))
                {
                    isTouchingGround = true;
                    Debug.DrawRay(transform.position, new Vector3(0, -1, 0), Color.green);

                    if (fall_timer < original_Fall_Timer)
                    {
                        fall_timer = original_Fall_Timer;
                    }
                }
            }
            else
            {
                fall_timer -= Time.deltaTime;
                if (fall_timer < 0)
                {
                    Destroy(gameObject);
                }
            }
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        }
    }

    IEnumerator turnOnCollider()
    {
        yield return new WaitForSeconds(0.075f);
        GetComponent<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Environment")
        {
            speed = 0;
            GetComponent<Collider>().enabled = true;
            Destroy(gameObject, 0.6f);
        }
    }
}
