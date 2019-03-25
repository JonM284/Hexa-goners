using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerAi : MonoBehaviour {

    private Animator anim;
    private Vector3 velocity;
    private Rigidbody rb;
    public int lives = 3;
    private Vector3 rayDir;
    private float horizontalInput, verticalInput;
    public float speed, InitialSpeed;

    //

    private bool isAttacking = false, isMeleeing = false, is_Holding_Melee = false, isStunned = false, isPaused = false;
    private bool canAttack = true, canDash = true, canMelee = true, canBlock = true, canRegen = true, canRecharge = false;
    private bool isTouchingGround = true, hasStartedFalling = false, fallOffWorld = false;
    private bool canTakeHit = true;
    public bool isAlive = true, isDashing = false, hasTakenHit = false, isSpawning = true, hasSpawned = false,
    wrongMove = false;

    private float velModifier = 1, fall_timer = 0.15f, original_Fall_Timer;
    public ParticleSystem bashingParticles, ExplosionParticles, SpawningParticles, StunParticles,
        snoozeParticles, deathParticles, chargingParticles;
    public int playerNum, randomInt;
    public GameObject myShockWave, myAim_Indicator, hat_Holder;
    public Collider block_Col;

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

    public AudioClip[] clips;
    private AudioSource source;
    //hat stuff here
    public GameObject myHat;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        InitialSpeed = speed;
        bashingParticles.Stop();
        ExplosionParticles.Stop();
        SpawningParticles.Stop();
        StunParticles.Stop();
        chargingParticles.Stop();
        SpawningParticles.gameObject.SetActive(false);
        snoozeParticles.gameObject.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            myPlayerMaterials[i] = myMeshRenderer.GetComponent<SkinnedMeshRenderer>().materials[i];
            originalMaterialsColor[i] = myMeshRenderer.GetComponent<SkinnedMeshRenderer>().materials[i].color;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
