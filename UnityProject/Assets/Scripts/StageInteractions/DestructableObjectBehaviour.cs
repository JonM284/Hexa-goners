using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObjectBehaviour : MonoBehaviour {

    public GameObject RegularObject, ShatteredObject;
    public int amountOfDestrucables;
    public string DestrucableName;
    private bool isResetting = false;
    public List<GameObject> Destructables;
    public List<Vector3> original_Positions;
    public List<Material> brokenObject_Materials;
    public List<Color> original_Colors;
    public Color transparent_Color;
    private Vector3 original_Transform;
    private AudioSource source;

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(Random.Range(-10, 10), original_Transform.y, Random.Range(-10, 10));
        RegularObject.SetActive(true);
        ShatteredObject.SetActive(false);
        original_Transform = transform.position;
        source = GetComponent<AudioSource>();
        for (int i = 0; i < amountOfDestrucables; i++)
        {
            original_Positions[i] = Destructables[i].transform.localPosition;
            
        }

        for (int x = 0; x < 3; x++)
        {
            Destructables[x].GetComponent<Renderer>().material.SetFloat("_Mode", 3f);
            brokenObject_Materials[x] = Destructables[x].GetComponent<MeshRenderer>().materials[x];
            
            original_Colors[x] = brokenObject_Materials[x].color;
        }
    }

    void LateUpdate()
    {
        if (isResetting)
        {
             brokenObject_Materials[0].color = Color.Lerp(brokenObject_Materials[0].color, transparent_Color, Time.deltaTime * 0.4f);
            brokenObject_Materials[1].color = Color.Lerp(brokenObject_Materials[1].color, transparent_Color, Time.deltaTime * 0.4f);
            brokenObject_Materials[2].color = Color.Lerp(brokenObject_Materials[2].color, transparent_Color, Time.deltaTime * 0.4f);
        }
        Destructables[0].GetComponent<MeshRenderer>().material.color = brokenObject_Materials[0].color;
        Destructables[1].GetComponent<MeshRenderer>().material.color = brokenObject_Materials[1].color;
        Destructables[2].GetComponent<MeshRenderer>().material.color = brokenObject_Materials[2].color;
        Destructables[3].GetComponent<MeshRenderer>().material.color = brokenObject_Materials[2].color;


    }

    public void ResetDestructable()
    {
        
        transform.position = new Vector3(Random.Range(-10, 10), original_Transform.y, Random.Range(-10, 10));
        for (int i = 0; i < Destructables.Count; i++)
        {
            Destructables[i].transform.localPosition = original_Positions[i];
            Destructables[i].transform.rotation = Quaternion.Euler(new Vector3(-90,0,0));
        }
        for (int x = 0; x < 3; x++)
        {
            brokenObject_Materials[x].color = original_Colors[x];
        }
        
        GetComponent<BoxCollider>().enabled = true;
        RegularObject.SetActive(true);
        ShatteredObject.SetActive(false);

        
    }

    IEnumerator waitToTurnOff()
    {
        isResetting = true;
        yield return new WaitForSeconds(6f);
        RegularObject.SetActive(false);
        ShatteredObject.SetActive(false);
        yield return new WaitForSeconds(5f);
        isResetting = false;
        
    }

     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Untagged" && other.gameObject.name != "Floor"
            && other.gameObject.tag != "Environment" && other.gameObject.tag != "Player")
        {
            source.PlayOneShot(source.clip);
            GetComponent<BoxCollider>().enabled = false;
            RegularObject.SetActive(false);
            ShatteredObject.SetActive(true);
            StartCoroutine(waitToTurnOff());
        }
        else if (other.gameObject.tag == "Player" 
            && other.gameObject.GetComponent<TempPlayerMovement>().isDashing)
        {
            source.PlayOneShot(source.clip);
            GetComponent<BoxCollider>().enabled = false;
            RegularObject.SetActive(false);
            ShatteredObject.SetActive(true);
            StartCoroutine(waitToTurnOff());
        }
        else if (other.gameObject.tag == "Environment" && other.gameObject.name == DestrucableName)
        {
            ResetDestructable();
        }
        

        
    }
}
