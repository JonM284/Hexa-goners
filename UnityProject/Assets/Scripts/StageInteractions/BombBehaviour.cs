using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour {

    private MeshRenderer myRend;
    public Collider explosion_Area;
    public ParticleSystem explosionParticles;
    
    private bool isFalling = true;
    private Rigidbody rb;
    private Vector3 velocity;
    [Range(8.0f, 15.0f)]
    public float speed;
    [Range(3.0f, 5.0f)]
    public float waitTime;
    private float countDownTimer;
    private Vector3 originalScale;
    


    // Use this for initialization
    private void Awake()
    {
        myRend = GetComponent<MeshRenderer>();
    }

    void Start () {
        
        rb = GetComponent<Rigidbody>();
        explosionParticles.Stop();
        countDownTimer = waitTime;
        explosion_Area.enabled = false;
        originalScale = transform.localScale;
	}

    private void OnEnable()
    {
        isFalling = true;
        countDownTimer = waitTime;
        explosion_Area.enabled = false;
        originalScale = transform.localScale;
        myRend.enabled = true;
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
        

        if (isFalling) {
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

        if (gameObject.activeSelf && Manager.TurnOffBombs)
        {
            gameObject.SetActive(false);
        }

    }

    


    IEnumerator WaitToGoOff()
    {

        for (int i = 0; i < 4; i++)
        {
            myRend.materials[0].color = Color.red;
            yield return new WaitForSeconds(0.1f);
            myRend.materials[0].color = Color.black;
            yield return new WaitForSeconds(0.1f);
        }


        for (int i = 0; i < 5; i++)
        {
            myRend.materials[0].color = Color.red;
            yield return new WaitForSeconds(0.07f);
            myRend.materials[0].color = Color.black;
            yield return new WaitForSeconds(0.07f);
        }


        for (int i = 0; i < 6; i++)
        {
            myRend.materials[0].color = Color.red;
            yield return new WaitForSeconds(0.05f);
            myRend.materials[0].color = Color.black;
            yield return new WaitForSeconds(0.05f);
        }


        explosion_Area.enabled = true;
        explosionParticles.Play();
        myRend.enabled = false;
        yield return new WaitForSeconds(0.05f);
        GetComponent<Collider>().enabled = false;
        explosion_Area.enabled = false;
        yield return new WaitForSeconds(1f);
        explosionParticles.Stop();
        gameObject.SetActive(false);
    }

    


}
