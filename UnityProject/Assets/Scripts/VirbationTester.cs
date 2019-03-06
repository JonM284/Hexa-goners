using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class VirbationTester : MonoBehaviour {

    Vector3 velocity;
    private Vector3 rayDir;
    private Rigidbody rb;
    public int playerID = 0;
    public bool fire;
    public float speed;

    //rewired stuff to remember
    private Player player;
    //this is referring to the Rewired Player in input manager

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        player = ReInput.players.GetPlayer(playerID);
        //ReInput = main class for accessing all input related information
	}

     void Update()
    {
        Movement();
        
    }

    void Movement()
    {
        // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
        // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

        velocity.x = player.GetAxisRaw("Move Horizontal") * 4;
        
        velocity.z = player.GetAxisRaw("Move Vertical") * 4;

        fire = player.GetButtonDown("Fire1");

        if (fire)
        {
            Debug.Log("Has Fired");
        }

        float horizontalInput = player.GetAxisRaw("Move Horizontal");
        float verticalInput = player.GetAxisRaw("Move Vertical");

        Vector3 tempDir = new Vector3(horizontalInput, 0,verticalInput);
        if (tempDir.magnitude > 0.1f)
        {
            rayDir = tempDir.normalized;
        }
        transform.forward = rayDir;

       rb.MovePosition(rb.position + velocity * Time.deltaTime);
        
    }

   
    

     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "VibratePlace")
        {
            player.SetVibration(0, 0.5f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "VibratePlace")
        {
            player.StopVibration();
        }
    }


}
