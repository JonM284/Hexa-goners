using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackBehaviour : MonoBehaviour {

    
    
    public BombDropper bananaDropper;
    public Transform spawnPos, cannonPos;
    private Vector3 dir;

    private void Start()
    {
        dir = cannonPos.position - spawnPos.position;
    }


    public void SetupGameObject()
    {
        bananaDropper.SpawnFromPool("banana", spawnPos.position
          , Quaternion.Euler(transform.forward));
    }
	

   
	
}
