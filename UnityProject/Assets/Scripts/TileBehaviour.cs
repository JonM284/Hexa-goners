using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour {

    private Color originalColor;
    private Vector3 myOriginalScale;
    private string originalTag, originalName;
    private bool isBeingUsed = false;
    private Material myMaterial;
    private MeshRenderer myRenderer;
    public int Cp_Attack_Num;
    public Color currentColor;
    private bool isMarked = false;
    


	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<MeshRenderer>();
        myMaterial = GetComponent<MeshRenderer>().material;
        currentColor = myMaterial.color;
        originalColor = myMaterial.color;
        myOriginalScale = transform.localScale;
        originalTag = "Untagged";
        originalName = "GridPiece";
        gameObject.tag = originalTag;
        gameObject.name = originalName;
        myRenderer.enabled = false;
        
	}

    

    IEnumerator Earthquake()
    {
        StopCoroutine(WaitToQuake());
        isMarked = false;
        
        myRenderer.enabled = true;
        transform.localScale = new Vector3(myOriginalScale.x, myOriginalScale.y, myOriginalScale.z + 3);
        yield return new WaitForSeconds(.3f);
        transform.localScale = myOriginalScale;
        myMaterial.color = originalColor;
        currentColor = originalColor;
        myRenderer.enabled = false;
        isBeingUsed = false;
        gameObject.tag = originalTag;
        gameObject.name = originalName;
        Cp_Attack_Num = 0;
    }

    IEnumerator WaitToQuake()
    {
        yield return new WaitForSeconds(0.75f);
        gameObject.tag = "Attack";
        gameObject.name = "Player" + Cp_Attack_Num + "Attack";
        StartCoroutine(Earthquake());
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ShockWave" && !isBeingUsed)
        {
            Cp_Attack_Num = other.gameObject.GetComponent<ShockWaveBehaviour>().playerNum;
            gameObject.tag = "Attack";
            gameObject.name = "Player" + Cp_Attack_Num + "Attack";
            myMaterial.color = other.gameObject.GetComponent<ShockWaveBehaviour>().myPlayersColor;
            currentColor = myMaterial.color;
            isBeingUsed = true;
            StartCoroutine(Earthquake()); 
        }
        if (other.gameObject.tag == "Player" && !isBeingUsed)
        {
            myRenderer.enabled = true;
            myMaterial.color = other.gameObject.GetComponent<TempPlayerMovement>().originalMaterialsColor[0]; 
        }
        if (other.gameObject.tag == "Enemy")
        {
            
            myMaterial.color = Color.black;
            gameObject.tag = "Enemy";
            gameObject.name = "EnemyAttack";
            StartCoroutine(Earthquake());
        }
        if (other.gameObject.tag == "Marker")
        {
            isMarked = true;
            myRenderer.enabled = true;
            myMaterial.color = other.gameObject.GetComponentInParent<TempPlayerMovement>().originalMaterialsColor[0];
            Cp_Attack_Num = other.gameObject.GetComponentInParent<TempPlayerMovement>().playerNum;
            isBeingUsed = true;
            StartCoroutine(WaitToQuake());
        }

        if (other.gameObject.tag == "Block" && !isMarked)
        {
            myMaterial.color = other.gameObject.GetComponentInParent<TempPlayerMovement>().originalMaterialsColor[0];   
            Cp_Attack_Num = other.gameObject.GetComponentInParent<TempPlayerMovement>().playerNum;
            gameObject.tag = "Block";
            gameObject.name = "Player" + Cp_Attack_Num + "Block";
            StartCoroutine(Earthquake());
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<TempPlayerMovement>().isAlive
            &&!isBeingUsed && !isMarked)
        {
            myRenderer.enabled = true;
            myMaterial.color = other.gameObject.GetComponent<TempPlayerMovement>().originalMaterialsColor[0];
            
        }

        if (other.gameObject.tag == "Player" && other.GetComponent<TempPlayerMovement>().isDashing
            && !isBeingUsed)
        {
            Cp_Attack_Num = other.gameObject.GetComponent<TempPlayerMovement>().playerNum;
            gameObject.tag = "Melee";
            gameObject.name = "Player" + Cp_Attack_Num + "Melee";
            myMaterial.color = other.gameObject.GetComponent<TempPlayerMovement>().originalMaterialsColor[0];
            currentColor = myMaterial.color;
            isBeingUsed = true;
            StartCoroutine(Earthquake()); 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isBeingUsed && !isMarked)
        {
            myRenderer.enabled = false;
            myMaterial.color = originalColor;
        }
        
    }
}
