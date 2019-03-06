using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileColliderManager : MonoBehaviour {

    public GameObject firstHolder, secondHolder;
    public GameObject[] tilesSideOne, tilesSideTwo;
    

    private void Awake()
    {
        tilesSideOne = new GameObject[firstHolder.transform.childCount]; 

        for (int i = 0; i < firstHolder.transform.childCount; i++)
        {
            tilesSideOne[i] = firstHolder.transform.GetChild(i).gameObject;
        }

        tilesSideTwo = new GameObject[secondHolder.transform.childCount];

        for (int i = 0; i < secondHolder.transform.childCount; i++)
        {
            tilesSideTwo[i] = secondHolder.transform.GetChild(i).gameObject;
        }
    }

    public void TurnOffColliders()
    {
        foreach (GameObject tile in tilesSideOne)
        {
            tile.GetComponent<CapsuleCollider>().enabled = false;
            tile.GetComponent<MeshRenderer>().enabled = false;
        }

        foreach (GameObject tile in tilesSideTwo)
        {
            tile.GetComponent<CapsuleCollider>().enabled = false;
            tile.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void TurnOnColliders()
    {
        foreach (GameObject tile in tilesSideOne)
        {
            tile.GetComponent<CapsuleCollider>().enabled = true;
        }

        foreach (GameObject tile in tilesSideTwo)
        {
            tile.GetComponent<CapsuleCollider>().enabled = true;
        }
    }
}
