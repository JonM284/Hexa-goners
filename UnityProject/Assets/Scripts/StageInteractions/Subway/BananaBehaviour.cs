using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaBehaviour : MonoBehaviour
{

    private bool isFalling = true;
    private Rigidbody rb;
    private Vector3 velocity;
    [Range(8.0f, 15.0f)]
    public float speed;
    [Range(3.0f, 15.0f)]
    public float waitTime;
    private float countDownTimer;
    private Vector3 originalScale;



    void Start()
    {

        rb = GetComponent<Rigidbody>();

        countDownTimer = waitTime;

        originalScale = transform.localScale;
    }

    private void OnEnable()
    {
        isFalling = true;
        countDownTimer = waitTime;

        originalScale = transform.localScale;

    }

    void Update()
    {
        if (isFalling)
        {
            velocity.x = 0;
            velocity.y -= speed;
            velocity.z = 0;
            rb.MovePosition(rb.position + Vector3.ClampMagnitude(velocity, speed) * Time.deltaTime);
        }


        if (isFalling)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 10))
            {
                if (hit.collider != null && (hit.collider.name == "Floor") && hit.distance <= 0.75)
                {
                    Debug.DrawRay(transform.position, new Vector3(0, -1, 0), Color.green);
                    StartCoroutine(WaitToGoOff());
                    isFalling = false;
                }
            }
        }
    }



    IEnumerator WaitToGoOff()
    {

        yield return new WaitForSeconds(waitTime);
        
        gameObject.SetActive(false);
    }
}
