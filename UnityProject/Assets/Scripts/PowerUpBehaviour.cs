using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour {

    public GameObject myPowerupModel;
    public int powerUp_ID;
    [Tooltip("If 0, rotate y. if, 1 rotate x. if 2 , rotate z")]
    public int dir;
	
	// Update is called once per frame
	void LateUpdate () {
        if (dir == 0)
        {
            myPowerupModel.transform.Rotate(30 * Time.deltaTime, 0, 0);
        }else if (dir == 1)
        {
            myPowerupModel.transform.Rotate(0, 30 * Time.deltaTime, 0);
        }
        else if (dir == 2)
        {
            myPowerupModel.transform.Rotate(0,0, 30 * Time.deltaTime);
        }
    }

     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
