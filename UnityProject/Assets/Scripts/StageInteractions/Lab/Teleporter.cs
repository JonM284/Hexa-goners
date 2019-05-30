using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    public int tel_ID_Num;
    private int move_Int;
    public Transform other_Tel_Pos;
    private bool canReset = true;
    private bool is_On_CoolDown = false;
    public ParticleSystem teleporter, particles, pop_particles;
    private ParticleSystem.EmissionModule module_Tel;
    private float originalEmissionRate;
    public Color redColor, blueColor;
    public float minZ, maxZ;
    private Vector3 vel;
    private Rigidbody rb;
    private Material LightMat;
    private AudioSource m_source;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(NewPos());
        move_Int = 0;
        module_Tel = teleporter.emission;
        originalEmissionRate = module_Tel.rateOverTime.constant;
        LightMat = GetComponent<MeshRenderer>().materials[0];
        LightMat.color = Color.green;
        m_source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Manager.reset_Objects && canReset)
        {
            StartCoroutine(NewPos());
        }
        if (move_Int == 1 && canReset)
        {
            Movement();
        }
    }

    void Movement()
    {
        vel.x = 0;
        vel.y = 0;
        vel.z = Mathf.Cos(Time.time) * (maxZ - 2.5f);

        rb.MovePosition(rb.position + vel * Time.deltaTime);
    }

    IEnumerator CoolDown()
    {
        pop_particles.Play();
        m_source.Play();
        module_Tel.rateOverTime = 0;
        particles.gameObject.SetActive(false);
        is_On_CoolDown = true;
        LightMat.color = Color.red;
        yield return new WaitForSeconds(2.0f);
        module_Tel.rateOverTime = originalEmissionRate;
        particles.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        LightMat.color = Color.green;
        is_On_CoolDown = false;
    }

    IEnumerator NewPos()
    {
        canReset = false;
        /*move_Int = Random.Range(0,2);
        if (move_Int == 0) {
            transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(minZ, maxZ));
        }else if (move_Int == 1)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }*/
        if (tel_ID_Num == 0 || tel_ID_Num == 1) {
            transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(minZ, maxZ));
        }else if (tel_ID_Num == 2 || tel_ID_Num == 3)
        {
            transform.position = new Vector3(Random.Range(minZ, maxZ), transform.position.y, transform.position.z);
        }
        yield return new WaitForSeconds(2f);
        canReset = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player" && !is_On_CoolDown) || (other.gameObject.tag == "ShockWave" && !is_On_CoolDown)) {
            StartCoroutine(CoolDown());
            if (tel_ID_Num == 0) {
                other.gameObject.transform.position = new Vector3(other_Tel_Pos.position.x - 1.5f, other.gameObject.transform.position.y,
                    other_Tel_Pos.position.z);
            }
            else if (tel_ID_Num == 1)
            {
                other.gameObject.transform.position = new Vector3(other_Tel_Pos.position.x + 1.5f, other.gameObject.transform.position.y,
                    other_Tel_Pos.position.z);
            }else if (tel_ID_Num == 2)
            {
                other.gameObject.transform.position = new Vector3(other_Tel_Pos.position.x, other.gameObject.transform.position.y,
                    other_Tel_Pos.position.z + 1.5f);
            }
            else if (tel_ID_Num == 3)
            {
                other.gameObject.transform.position = new Vector3(other_Tel_Pos.position.x, other.gameObject.transform.position.y,
                    other_Tel_Pos.position.z - 1.5f);
            }

        }
    }
}
