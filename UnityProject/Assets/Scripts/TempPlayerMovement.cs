using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;
using UnityEngine.UI;
using UnityEditor;


public class TempPlayerMovement : MonoBehaviour {
    // all code created in this game was created for the (Currently named) game "H or Hexagon" by Jonathan Mendez before and after 10_12_2018
    private Animator anim;
    private Vector3 velocity;
    private Rigidbody rb;
    public int lives = 3;
    private Vector3 rayDir;
    private float horizontalInput, verticalInput;
    public float speed, InitialSpeed;
    ///
    private bool isAttacking = false, isMeleeing = false, is_Holding_Melee = false, isStunned = false, isPaused = false;
    private bool canAttack = true, canDash = true, canMelee = true, canBlock = true, canRegen = true, canRecharge = false;
    private bool isTouchingGround = true, hasStartedFalling = false, fallOffWorld = false;
    private bool canTakeHit = true;
    public bool isAlive = true, isDashing = false, hasTakenHit = false, isSpawning = true, hasSpawned = false,
        wrongMove = false;
    private bool Is_holding_Charge = false; 
    ///
    private float velModifier = 1, fall_timer = 0.15f, original_Fall_Timer;
    public ParticleSystem bashingParticles, ExplosionParticles, SpawningParticles, StunParticles,
        snoozeParticles, deathParticles, chargingParticles;
    public int playerNum, randomInt;
    public GameObject myShockWave, myAim_Indicator, hat_Holder;
    public Collider block_Col;
    //
    private Player myPlayer;
    public GameObject myMeshRenderer;

    public CameraBehaviour GameCamera;
    //color managers
    public Material[] myPlayerMaterials;
    public Color[] originalMaterialsColor;
    public ParticleSystem[] deathExplosion;
    // reset position variable
    public float startingYPos;
    ///stamina variables
    public float stamina = 100f;
    private float maxStamina_amt;
    public bool hasMaxed_out = false;
    public float stam_Regen_modifier = 10f;
    private float original_Stam_Regen_Mod;
    //
    private Vector3 currentSpawnPos;
    private int attacker_ID;

    private float AOE_Attack_Timer = 4f, originalAOE_Attack_Timer;
    private Vector3 originalShockScale, original_ShockIndicator_Scale;
    public AudioClip[] clips;
    private AudioSource source;

    

    void Awake()
    {
        InitialSpeed = speed;
        bashingParticles.Stop();
        ExplosionParticles.Stop();
        SpawningParticles.Stop();
        StunParticles.Stop();
        chargingParticles.Stop();
        SpawningParticles.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            myPlayerMaterials[i] = myMeshRenderer.GetComponent<SkinnedMeshRenderer>().materials[i];
            originalMaterialsColor[i] = myMeshRenderer.GetComponent<SkinnedMeshRenderer>().materials[i].color;
        }
        myPlayer = ReInput.players.GetPlayer(playerNum-1);
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        CheckController(myPlayer);

    }
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        GameCamera = Camera.main.GetComponent<CameraBehaviour>();
        source = GetComponent<AudioSource>();
        startingYPos = transform.position.y;
        maxStamina_amt = stamina;
        original_Stam_Regen_Mod = stam_Regen_modifier;
        myAim_Indicator.GetComponent<SpriteRenderer>().color = originalMaterialsColor[0];
        var main = StunParticles.main;
        main.startColor = originalMaterialsColor[0];
        var otherMain = snoozeParticles.main;
        otherMain.startColor = originalMaterialsColor[0];
        var otherOtherMain = chargingParticles.main;
        otherOtherMain.startColor = originalMaterialsColor[0];
        var lastMain = deathParticles.main;
        lastMain.startColor = originalMaterialsColor[0];       
        myAim_Indicator.SetActive(false);
        hat_Holder.SetActive(false);
        block_Col.enabled = false;
       for (int i = 0; i < deathExplosion.Length; i++)
        {
            var currentMain = deathExplosion[i].main;
            currentMain.startColor = originalMaterialsColor[0];
        }
        deathExplosion[0].gameObject.SetActive(false);
        originalAOE_Attack_Timer = AOE_Attack_Timer;
        original_Fall_Timer = fall_timer;
        

    }

    void FixedUpdate()
    {
        //only call movement when the inputs are something other than zero
        if (horizontalInput != 0 || verticalInput != 0 && !hasStartedFalling && isAlive)
        {
            Movement();
        }
    }

    // Update is called once per frame
    void Update () {
        horizontalInput = myPlayer.GetAxisRaw("Horizontal");
        verticalInput = myPlayer.GetAxisRaw("Vertical");


        if (myPlayer.GetButtonDown("Pause") && !Manager.isPausedMenu)
        {
            Manager.isPausedMenu = true;
        }else if (myPlayer.GetButtonDown("Pause") && Manager.isPausedMenu)
        {
            Manager.isPausedMenu = false;
        }
        /////////////THIS IS FOR BASIC ATTACKS
        if (!Manager.isPausedMenu) {
            if (myPlayer.GetButtonDown("Attack") && !isAttacking && canAttack && stamina >= 25f)
            {
                GameObject newWave = Instantiate(myShockWave, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z),
                    this.transform.rotation) as GameObject;
                newWave.GetComponent<ShockWaveBehaviour>().playerNum = this.playerNum;
                newWave.GetComponent<ShockWaveBehaviour>().myPlayersColor = this.originalMaterialsColor[0];
                StopCoroutine(Dash());
                isDashing = false;
                stamina -= 25f;
                StartCoroutine(Attack());
            } else if (myPlayer.GetButtonDown("Attack") && (!canAttack || stamina < 25f))
            {
                StartCoroutine(resetWrongImg());
            }

            //place new attack/ movement mechanic here
            if (myPlayer.GetButtonDown("Melee") && isAlive && stamina < maxStamina_amt && !hasMaxed_out && canRecharge && !isPaused && !isStunned)
            {
                StartCoroutine(Melee());
            }

            /////////////THIS IS FOR DASHING
            if (myPlayer.GetButtonDown("Dash") && !isDashing && !isAttacking && canDash && stamina >= 20f)
            {
                StopCoroutine(Attack());
                isAttacking = false;
                stamina -= 20f;
                StartCoroutine(Dash());
            } else if (myPlayer.GetButtonDown("Dash") && (!canDash || stamina < 15f))
            {
                StartCoroutine(resetWrongImg());
            }
            /////////////THIS IS FOR BLOCKING
            if (myPlayer.GetButtonDown("Block") && canBlock)
            {
                StartCoroutine(Block());
            } else if (myPlayer.GetButtonDown("Block") && !canBlock)
            {
                StartCoroutine(resetWrongImg());
            }
            
        }
        //////////// check to see if touching ground
        RaycastHit hit;
        if (isTouchingGround && !isSpawning) {
            if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 1))
            {
                if (hit.collider != null && (hit.collider.name == "Floor" || hit.collider.name == "Environment" 
                    || hit.collider.tag == "Floor"))
                {
                    isTouchingGround = true;
                    Debug.DrawRay(transform.position, new Vector3(0,-1,0), Color.green);
                   
                    if (fall_timer < original_Fall_Timer)
                    {
                        fall_timer = original_Fall_Timer;
                    }
                }
            }else
            {
                fall_timer -= Time.deltaTime;
                if (fall_timer < 0)
                {
                    isTouchingGround = false;
                }
            }
        }

        


        if (!isTouchingGround && fall_timer <= 0 && !fallOffWorld)
        {
            StartCoroutine(beginFall());
            speed = 0;
            velModifier = 1;
            canAttack = false;
            canMelee = false;
            canDash = false;
            canRecharge = false;
        }


        if (hasStartedFalling)
        {
            velocity.y = -8;
            rb.MovePosition(rb.position + velocity * Time.deltaTime);
        }

        if (stamina < maxStamina_amt && canRegen)
        {
            /*if (horizontalInput == 0 && verticalInput == 0 && !hasMaxed_out && !isAttacking && !isDashing && !Is_holding_Charge)
            {
                stam_Regen_modifier = original_Stam_Regen_Mod + (original_Stam_Regen_Mod / 2);

            } else if (horizontalInput != 0 && verticalInput != 0 && !hasMaxed_out && !isAttacking && !isDashing && !Is_holding_Charge)
            {
                stam_Regen_modifier = original_Stam_Regen_Mod;

            } else if (!hasMaxed_out && !isAttacking && !isDashing && Is_holding_Charge) {
                stam_Regen_modifier = original_Stam_Regen_Mod * 3f;

            }*/

            if (!hasMaxed_out && !isAttacking && !isDashing && canRecharge)
            {
                stam_Regen_modifier = original_Stam_Regen_Mod * 2.6f;
            }
            else if (hasMaxed_out)
            {
                stam_Regen_modifier = original_Stam_Regen_Mod * 2.2f;
                myPlayerMaterials[1].color = Color.yellow;
            }
            if (stamina <= 10f && !hasMaxed_out)
            {
                hasMaxed_out = true;
                StartCoroutine(staminaModRegulator());
            }

            stamina += Time.deltaTime * stam_Regen_modifier;
            
        }else if (stamina >= maxStamina_amt)
        {
            stamina = maxStamina_amt;
        }

        if (!isAlive && !isSpawning)
        {
            PausePlayer();
        }
        

        DoAnimations();
	}


    void Movement()
    {
        velocity.x = horizontalInput * speed;
        velocity.z = verticalInput * speed;
        velocity.y = 0;

        Vector3 tempDir = new Vector3(horizontalInput, 0, verticalInput);
        if (tempDir.magnitude > 0.1f)
        {
            rayDir = tempDir.normalized;
        }
        if (!isAttacking && !isStunned && isAlive && isTouchingGround && !Is_holding_Charge) {
            //transform.forward = rayDir;
            /*float step = 7.4f * Time.deltaTime;
            Vector3 TempForw = Vector3.RotateTowards(transform.forward, rayDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(TempForw);*/

            
            transform.forward = Vector3.Slerp(transform.forward, rayDir, Time.deltaTime * 8f);
        }

        velModifier = isDashing ? 3.3f : 1f;
        //what this says: velModifier = whether or not player is dashing.
        //If player is dashing velModifier = 3.3 else velModifier = 1
        //this is the same as if(true){}else{}
        
        rb.MovePosition(rb.position + Vector3.ClampMagnitude(velocity, speed) * velModifier * Time.deltaTime);
    }

    public void StartRespawning(Vector3 newSpawnLocation)
    {
        StartCoroutine(Respawn(newSpawnLocation));
    }

    public void PausePlayer()
    {
        speed = 0;
        canAttack = false;
        canDash = false;
        canMelee = false;
        canBlock = false;
        isPaused = true;
        canRecharge = false;
    }

    IEnumerator resetWrongImg()
    {
        wrongMove = true;
        yield return new WaitForSeconds(1.2f);
        wrongMove = false;
    }

    IEnumerator staminaModRegulator()
    {
        canAttack = false;
        canDash = false;
        yield return new WaitForSeconds(2.5f);
        canAttack = true;
        canDash = true;
        myPlayerMaterials[1].color = originalMaterialsColor[1];
        hasMaxed_out = false;
    }


    IEnumerator Respawn(Vector3 newSpawnLocationR)
    {
        deathExplosion[0].gameObject.SetActive(false);
        isSpawning = true;
        isPaused = false;
        SpawningParticles.Play();
        SpawningParticles.gameObject.SetActive(true);
        myMeshRenderer.SetActive(false);
        hat_Holder.SetActive(false);
        myAim_Indicator.SetActive(false);
        this.GetComponent<BoxCollider>().enabled = false;
        canAttack = false;
        canDash = false;
        canMelee = false;
        canBlock = false;
        canRecharge = false;

        RandomLocation(newSpawnLocationR);
        speed = InitialSpeed / 2;
        yield return new WaitForSeconds(3.5f);
        myPlayer.SetVibration(0, 0.6f, 0.75f);
        myAim_Indicator.SetActive(true);
        hat_Holder.SetActive(true);
        GameCamera.SlowedDownEffect();
        SpawningParticles.Stop();
        SpawningParticles.gameObject.SetActive(false);
        ExplosionParticles.Play();
        stamina = maxStamina_amt - 15f;
        myMeshRenderer.SetActive(true);
        this.GetComponent<BoxCollider>().enabled = true;
        canAttack = true;
        canMelee = true;
        canDash = true;
        
        isAlive = true;
        canBlock = true;
        isSpawning = false;
        hasSpawned = true;
        fallOffWorld = false;
        fall_timer = original_Fall_Timer;
        speed = InitialSpeed;
        lives = 3;
        yield return new WaitForSeconds(.5f);
        hasSpawned = false;
        canRecharge = true;
        yield return new WaitForSeconds(3.0f);
        ExplosionParticles.Stop();
    }

    public void RandomLocation(Vector3 newestSpawnLocation)
    {
        
        transform.position = newestSpawnLocation;
        isTouchingGround = true;
        hasStartedFalling = false;
    }

    IEnumerator beginFall()
    {
        fallOffWorld = true;
        hasStartedFalling = true;
        lives = 0;
        BackgroundNPCanimtation.PlayerDeath = true;
        myAim_Indicator.SetActive(false);
        hat_Holder.SetActive(false);
        canMelee = false;
        canAttack = false;
        canDash = false;
        canRecharge = false;
        this.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(0.5f);
        ExplosionParticles.Play();
        deathParticles.Play();
        deathExplosion[0].gameObject.SetActive(true);
        for (int i = 0; i < deathExplosion.Length; i++)
        {
            deathExplosion[i].Play();
        }
        source.clip = clips[0];
        source.PlayOneShot(clips[0]);
        Manager.deadPlayersAmt++;
        isAlive = false;
        myPlayer.SetVibration(0, 0.6f, 0.75f);
        GameCamera.SlowedDownEffect();
        myMeshRenderer.SetActive(false);
        speed = 0;
        hasStartedFalling = false;
        switch (playerNum)
        {
            case 4:
                if (Manager.player4Score > 0) {
                    Manager.player4Score--;
                }else
                {
                    break;
                }
                break;
            case 3:
                if (Manager.player3Score > 0)
                {
                    Manager.player3Score--;
                }
                else
                {
                    break;
                }
                break;
            case 2:
                if (Manager.player2Score > 0)
                {
                    Manager.player2Score--;
                }
                else
                {
                    break;
                }
                break;
            case 1:
                if (Manager.player1Score > 0)
                {
                    Manager.player1Score--;
                }
                else
                {
                    break;
                }
                break;
            default:

                break;
        }
        yield return new WaitForSeconds(3.5f);
        ExplosionParticles.Stop();
        deathParticles.Stop();

    }
    
    IEnumerator Block()
    {
        speed = 0;
        block_Col.enabled = true;
        canBlock = false;
        canTakeHit = false;
        isMeleeing = true;
        canRecharge = false;
        yield return new WaitForSeconds(0.35f);
        canTakeHit = true;
        isMeleeing = false;  
        speed = InitialSpeed;
        canRecharge = true;
        block_Col.enabled = false;
        yield return new WaitForSeconds(2.7f);
        StunParticles.Play();
        canBlock = true;
        yield return new WaitForSeconds(0.5f);
        StunParticles.Stop();
    }

     IEnumerator Melee()
    {
        Is_holding_Charge = true;
        chargingParticles.Play();
        canDash = false;
        canAttack = false;
        canBlock = false;
        speed = 0;
        yield return new WaitUntil(() => myPlayer.GetButtonUp("Melee") || isStunned || isPaused || !isAlive || stamina > maxStamina_amt);
        if (!isStunned && !isPaused && isAlive) {
            Is_holding_Charge = false;
            chargingParticles.Stop();
            canDash = true;
            canAttack = true;
            canBlock = true;
            speed = InitialSpeed;
        }else
        {
            Is_holding_Charge = false;
            chargingParticles.Stop();
            canDash = false;
            canAttack = false;
            canBlock = false;
            speed = 0;
        }

    }

    IEnumerator Dash()
    {
        myPlayer.SetVibration(0, 0.15f, 0.2f);
        source.clip = clips[4];
        source.PlayOneShot(clips[4]);
        isDashing = true;
        canTakeHit = false;
        canRegen = false;
        canRecharge = false;
        yield return new WaitForSeconds(0.35f);
        isDashing = false;
        isAttacking = false;
        yield return new WaitForSeconds(0.15f);
        canRegen = true;
        canRecharge = true;
        canTakeHit = true;
    }

    IEnumerator Attack()
    {
        myPlayer.SetVibration(0, 0.3f, 0.25f);
        isAttacking = true;
        canMelee = false;
        canAttack = false;
        canRecharge = false;
        source.clip = clips[1];
        source.PlayOneShot(clips[1]);
        bashingParticles.Play();
        speed = 0;
        myPlayerMaterials[1].color = Color.red;
        yield return new WaitForSeconds(.2f);
        bashingParticles.Stop();
        if (!isPaused && !isStunned) {
            speed = InitialSpeed;
            
        }
        isAttacking = false;
        yield return new WaitForSeconds(0.3f);
        myPlayerMaterials[1].color = originalMaterialsColor[1];
        if (!isPaused && !isStunned && !hasMaxed_out) {
            isDashing = false;
            canAttack = true;
            canMelee = true;
            canRecharge = true;
            canDash = true;
        }
    }

    IEnumerator StunPlayer()
    {
      

        speed = 0;
        source.clip = clips[2];
        source.PlayOneShot(clips[2]);
        isStunned = true;
        canTakeHit = false;
        canMelee = false;
        canAttack = false;
        canBlock = false;
        canDash = false;
        canRecharge = false;
        if (isAlive) {
            snoozeParticles.Play();
        }
        yield return new WaitForSeconds(1.5f);
        speed = InitialSpeed;
        isStunned = false;
        isAttacking = false;
        canMelee = true;
        canAttack = true;
        canTakeHit = true;
        canBlock = true;
        canDash = true;
        canRecharge = true;
        snoozeParticles.Stop();
    }

    
    ////////////////////////////////// Take Hit
    IEnumerator TakeHit(Color enemyAttackerColor)
    {
        hasTakenHit = true;
        myPlayer.SetVibration(0,0.6f,0.75f);
        GameCamera.SlowedDownEffect();
        GameCamera.ChromVoid();
        source.clip = clips[0];
        source.PlayOneShot(clips[0]);
        
        
        canMelee = false;
        lives--;

        ExplosionParticles.Play();
        if (lives <= 0)
        {
            GameCamera.SlowedDownEffect();
            myMeshRenderer.SetActive(false);
            var deathMain = deathParticles.main;
            deathMain.startColor = enemyAttackerColor;
            deathParticles.Play();
            deathExplosion[0].gameObject.SetActive(true);
            for (int i = 0; i < deathExplosion.Length; i++)
            {
                deathExplosion[i].Play();
            }
            speed = 0;
            canMelee = false;
            canAttack = false;
            canDash = false;
            isAlive = false;
            canRecharge = false;
            BackgroundNPCanimtation.PlayerDeath = true;
            myAim_Indicator.SetActive(false);
            hat_Holder.SetActive(false);
            this.GetComponent<BoxCollider>().enabled = false;
                switch (attacker_ID)
                {
                    case 4:
                        Manager.player4Score++;
                        break;
                    case 3:
                        Manager.player3Score++;
                        break;
                    case 2:
                        Manager.player2Score++;
                        break;
                    case 1:
                        Manager.player1Score++;
                        break;
                    default:

                        break;
                }
            yield return new WaitForSeconds(0.8f);
            Manager.deadPlayersAmt++;
        } else if (lives >= 1) {
            for (int i = 0; i < 12; i++)
            {
                myPlayerMaterials[0].color = Color.white;
                myPlayerMaterials[1].color = Color.white;
                myPlayerMaterials[2].color = Color.white;
                yield return new WaitForSeconds(0.08f);
                myPlayerMaterials[0].color = originalMaterialsColor[0];
                myPlayerMaterials[1].color = originalMaterialsColor[1];
                myPlayerMaterials[2].color = originalMaterialsColor[2];
                yield return new WaitForSeconds(0.08f);
            }
        }
        yield return new WaitForSeconds(0.1f);
        hasTakenHit = false;
        
        canMelee = true;
        canRecharge = true;
        yield return new WaitForSeconds(3.5f);
        ExplosionParticles.Stop();
        StunParticles.Stop();
        deathParticles.Stop();
        
    }

    ////////////////////////////    Animations
    void DoAnimations()
    {
        if (horizontalInput == 0 && verticalInput == 0 && !isAttacking 
            && !isDashing && !isMeleeing && isTouchingGround && !isStunned)
        {
            anim.SetInteger("RunNum", 2);
        }
        if (isAttacking && !isDashing && !isMeleeing && isTouchingGround && !isStunned)
        {
            anim.SetInteger("RunNum", 3);
        }
        if ((horizontalInput != 0 || verticalInput != 0) && !isAttacking 
            && !isDashing && !isMeleeing && isTouchingGround && !isStunned)
        {
            anim.SetInteger("RunNum", 0);
        }
        if (isDashing && !isAttacking && !isMeleeing && isTouchingGround && !isStunned)
        {
            anim.SetInteger("RunNum", 1);
        }
        if (isMeleeing && !isAttacking && !isDashing && isTouchingGround && !isStunned)
        {
            anim.SetInteger("RunNum", 4);
        }

        if (!isTouchingGround)
        {
            anim.SetInteger("RunNum", 6);
        }

       

        if (isStunned)
        {
            anim.SetInteger("RunNum", 7);
        }


    }

    void OnControllerConnected(ControllerStatusChangedEventArgs arg)
    {
        CheckController(myPlayer);
    }

     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PowerUp")
        {
            int ID = other.gameObject.GetComponent<PowerUpBehaviour>().powerUp_ID;
            switch (ID)
            {
                default:
                    stamina = 100;
                    hasMaxed_out = false;
                    myPlayerMaterials[1].color = originalMaterialsColor[1];
                    break;
            }
        }

        if (other.gameObject.tag == "Block" && other.gameObject.name != "Player"+playerNum+"Block")
        {
            StartCoroutine(StunPlayer());
            attacker_ID = other.GetComponent<TileBehaviour>().Cp_Attack_Num;
            StartCoroutine(TakeHit(other.GetComponent<TileBehaviour>().currentColor));
        }

        
    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Attack" && other.gameObject.name != "Player" + playerNum + "Attack" && !hasTakenHit && canTakeHit)
        {
            attacker_ID = other.GetComponent<TileBehaviour>().Cp_Attack_Num;
            StartCoroutine(TakeHit(other.GetComponent<TileBehaviour>().currentColor));
        }else if (other.gameObject.tag == "Block" && other.gameObject.name != "Player" + playerNum + "Block" && !hasTakenHit && canTakeHit && !isStunned)
        {
            StartCoroutine(StunPlayer());
            attacker_ID = other.GetComponent<TileBehaviour>().Cp_Attack_Num;
            StartCoroutine(TakeHit(other.GetComponent<TileBehaviour>().currentColor));
        }

        if (other.gameObject.tag == "Enemy" && !hasTakenHit && canTakeHit && !isStunned)
        {
            attacker_ID = 0;
            StartCoroutine(TakeHit(Color.gray));
        }

    }

    //////////////
    //this code was made using the help of Rewired's forums found online
    void CheckController(Player player)
    {
        foreach (Joystick joyStick in player.controllers.Joysticks)
        {
            var ds4 = joyStick.GetExtension<DualShock4Extension>();
            if (ds4 == null) continue;//skip this if not DualShock4

            ds4.SetLightColor(originalMaterialsColor[0]);

        }
    }
} 
