using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ShockWaveBehaviour : MonoBehaviour {

    public float speed;
    public int playerNum;
    private bool isTouchingGround = true, hasTouched = false;
    public float fall_timer;
    private float original_Fall_Timer;
    public Color myPlayersColor;
    public AudioSource reversalSound;
    public ParticleSystem blockParticles, sparkParticles;
    private Manager game_Manager;
	// Use this for initialization
	void Start () {
        game_Manager = Manager.manager_Instance;
        GetComponent<Collider>().enabled = false;
        original_Fall_Timer = fall_timer;
        StartCoroutine(turnOnCollider());
        if (reversalSound.isPlaying)
        {
            reversalSound.Stop();
        }
        if (blockParticles.isPlaying)
        {
            blockParticles.Stop();
        }
    }

    void Update()
    {
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

    IEnumerator turnAround()
    {
        game_Manager.GetComponent<Manager>().Reverse_Stat[playerNum - 1]++;
        PlayerPrefs.SetInt("ReverseP" + playerNum, game_Manager.GetComponent<Manager>().Reverse_Stat[playerNum - 1]);
        reversalSound.Play();
        var main = blockParticles.main;
        main.startColor = myPlayersColor;
        blockParticles.Play();
        hasTouched = true;
        speed *= -1;
        yield return new WaitForSeconds(0.1f);
        hasTouched = false;
    }

     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ShockWave")
        {
            var main = sparkParticles.main;
            main.startColor = myPlayersColor;
            speed = 0;
            GetComponent<Collider>().enabled = true;
            sparkParticles.Play();
            Destroy(gameObject, 0.6f);
        }

        if (other.gameObject.tag == "Block")
        {
            var main = sparkParticles.main;
            main.startColor = myPlayersColor;
            speed = 0;
            GetComponent<Collider>().enabled = true;
            sparkParticles.Play();
            Destroy(gameObject, 0.6f);
        }

        if (other.gameObject.tag == "Environment")
        {
            var main = sparkParticles.main;
            main.startColor = myPlayersColor;
            speed = 0;
            GetComponent<Collider>().enabled = true;
            sparkParticles.Play();
            Destroy(gameObject, 0.6f);
        }

        
    }

     void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.tag == "Melee" && other.gameObject.name != "Player" + playerNum + "Melee") 
            && !hasTouched)
        {
            playerNum = other.gameObject.GetComponent<TileBehaviour>().Cp_Attack_Num;
            myPlayersColor = other.gameObject.GetComponent<TileBehaviour>().currentColor;
            StartCoroutine(turnAround());
        }
    }


}
