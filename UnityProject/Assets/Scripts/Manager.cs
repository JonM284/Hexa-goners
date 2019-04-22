using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Manager : MonoBehaviour
{

    public static bool startRespawn = true;
    public static bool reset_Objects;
    public bool hasStatic_Interactable_In_Scene;
    private float countDownTimer = 4, originalRespawnTime;
    public static int deadPlayersAmt = 0;
    private bool hasReset = false, mayhemMode = false;
    public bool countDown = true;
    public List<GameObject> players;
    public GameObject ScoreBoard;
    public GameObject Confetti;
    public Text timerText;
    public Image winnerBorderImage;
    public float scoreTimerModifier, scoreBoardOffset;
    public static int player1Score = 0, player2Score = 0, player3Score = 0, player4Score = 0;
    private float currentPlayer1Score = 0, currentPlayer2Score = 0, currentPlayer3Score = 0
        , currentPlayer4Score = 0;
    public Slider player1Slider, player2Slider, player3Slider, player4Slider;
    private bool startAddingScores = false, game_Has_Ended = false, playerHasSpawned = false;
    private bool level_Loaded;
    public int winAmount = 9;
    public Transform[] Map_Spawn_Bounds;
    public List<Transform> player_Fed_Spawns = new List<Transform>(4);
    public float mapBounds_Offset_X, mapBounds_Offset_Z;
    /// <summary>
    /// This is all what I figured out during the makeing of Nigel
    /// </summary>
    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;
    
    private int drawDepth = 1;
    private float alpha = 1.0f;
    private int fadeInDir = -1;
    //

    private int winnerNum;
    private float imageTimer, imageTimerMax = 1.5f;

    public List<int> randomInt;

    public bool destructables_In_Scene;
    public GameObject[] destructable_GO;

    private Vector3 originalPos;

    CursorLockMode lockedMode;

    public GameObject endGameMenu, pauseMenu, first_Select_EndG, first_Select_PauseM;
    public EventSystem system;
    public static bool isPausedMenu = false;
    private float currentTimeScale;
    public static bool turnOff = false;
    public static bool TurnOffBombs = false;

    //this is for bomb dropping
    BombDropper bombDropper;
    List<int> playerNumAllowedToDrop;

    public int[] Reverse_Stat;
    public int[] Attack_Stat;
    public int[] Block_Stat;

    public GameObject[] player_Stat_Panels;
    //this is for the in scene pause menu

    private GameObject storedSelected;

    public static Manager manager_Instance;



    void Awake()
    {
        manager_Instance = this;   
        ScoreBoard.SetActive(false);
        
    }
    // Use this for initialization
    void Start()
    {
        timerText.enabled = false;
        originalPos = transform.position;
        originalRespawnTime = countDownTimer;
        winnerBorderImage.fillAmount = 0;
        winnerBorderImage.enabled = false;
        Confetti.SetActive(false);
        endGameMenu.SetActive(false);
        pauseMenu.SetActive(false);
        system.firstSelectedGameObject = first_Select_PauseM;
        if (!level_Loaded)
        {
            Respawner();
        }
        beginFade(-1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        storedSelected = system.firstSelectedGameObject;
        bombDropper = BombDropper.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (startAddingScores)
        {
            addScores();
        }

        if ((player1Score >= winAmount || player2Score >= winAmount
            || player3Score >= winAmount || player4Score >= winAmount) && !game_Has_Ended)
        {
            
            if (player1Score >= winAmount)
            {
                winnerNum = 0;
            }else if (player2Score >= winAmount)
            {
                winnerNum = 1;
            }
            else if (player3Score >= winAmount)
            {
                winnerNum = 2;
            }
            else if (player4Score >= winAmount)
            {
                winnerNum = 3;
            }
            game_Has_Ended = true;
            
            
        }

        if (game_Has_Ended && imageTimer < imageTimerMax && winnerBorderImage.enabled)
        {
            imageTimer += Time.deltaTime;
            float percentage = imageTimer / imageTimerMax;
            winnerBorderImage.fillAmount = percentage;
        }

        if (game_Has_Ended && playerHasSpawned && players[winnerNum].GetComponent<TempPlayerMovement>().isAlive)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(players[winnerNum].transform.position.x, 
                players[winnerNum].transform.position.y + 10f, players[winnerNum].transform.position.z - 5f), 2f * Time.deltaTime);
        }
        else if (!game_Has_Ended && transform.position != originalPos)
        {
            transform.position = originalPos;
        }

        
        if ((deadPlayersAmt >= 3 && !hasReset) || (game_Has_Ended && !hasReset))
        {
            StartCoroutine(showScoreBoard());
        }

        if (deadPlayersAmt == 2 && !mayhemMode && !game_Has_Ended)
        {
            StartCoroutine(BombSquad());
        }
        

        if (isPausedMenu && !game_Has_Ended)
        {
            currentTimeScale = Time.timeScale;
            Time.timeScale = 0;
            if (system.firstSelectedGameObject != first_Select_PauseM) {
                system.SetSelectedGameObject(first_Select_PauseM, new BaseEventData(system));
            }
            pauseMenu.SetActive(true);
            
        }
        else if (!isPausedMenu && pauseMenu.activeInHierarchy)
        {
            Time.timeScale = currentTimeScale;
            pauseMenu.SetActive(false);
            
        }

        if (isPausedMenu || game_Has_Ended) {
            if (system.currentSelectedGameObject != system.firstSelectedGameObject)
            {
                if (system.currentSelectedGameObject == null)
                {
                    system.SetSelectedGameObject(storedSelected);
                }
                else
                {
                    storedSelected = system.currentSelectedGameObject;
                }
            }
        }
        

    }



    private void LateUpdate()
    {
        
    }

    private void OnLevelWasLoaded(int level)
    {
        level_Loaded = true;
        
        isPausedMenu = false;
        player1Score = 0;
        currentPlayer1Score = 0;
        player2Score = 0;
        currentPlayer2Score = 0;
        player3Score = 0;
        currentPlayer3Score = 0;
        player4Score = 0;
        currentPlayer4Score = 0;
        playerHasSpawned = false;
        imageTimer = 0;
        winnerBorderImage.fillAmount = 0;
        winnerBorderImage.enabled = false;
        Confetti.SetActive(false);
        endGameMenu.SetActive(false);
        deadPlayersAmt = 0;
        system.firstSelectedGameObject = first_Select_PauseM;
        Respawner();
    }

    


    IEnumerator tileStuff()
    {
        turnOff = true;
        yield return new WaitForSeconds(0.05f);
        turnOff = false;
    }


    void CheckIfPlayerWon()
    {
        CheckForLivingPlayers();
    }



    void CheckForLivingPlayers()
    {
        int livingPlayerID;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].GetComponent<TempPlayerMovement>().isAlive == true)
            {
                livingPlayerID = players[i].GetComponent<TempPlayerMovement>().playerNum;
                switch (livingPlayerID)
                {
                    case 4:
                        player4Score++;
                        break;
                    case 3:
                        player3Score++;
                        break;
                    case 2:
                        player2Score++;
                        break;
                    case 1:
                        player1Score++;
                        break;
                    default:

                        break;
                }
            }
        }
    }

    IEnumerator BombSquad()
    {
        mayhemMode = true;
        while (mayhemMode)
        {
            int randomBombAmount = Random.Range(3, 6);
            yield return new WaitForSeconds(3f);
            for (int i = 0; i < randomBombAmount; i++)
            {
                bombDropper.SpawnFromPool("Bomb", new Vector3(Random.Range(Map_Spawn_Bounds[1].transform.position.x
                , Map_Spawn_Bounds[2].transform.position.x), transform.position.y,
                Random.Range(GetComponent<Manager>().Map_Spawn_Bounds[0].transform.position.z
                , GetComponent<Manager>().Map_Spawn_Bounds[3].transform.position.z)), Quaternion.Euler(Vector3.zero));
            }
            
        }

    }

    IEnumerator showScoreBoard()
    {
        mayhemMode = false;
        if (!game_Has_Ended) {
            CheckForLivingPlayers();
        }
        TurnOffBombs = true;
        hasReset = true;
        beginFade(1);
        yield return new WaitForSeconds(fadeSpeed);
        beginFade(-1);
        yield return new WaitForSeconds(fadeSpeed);
        
        ScoreBoard.SetActive(true);
        startAddingScores = true;
        countDown = true;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].activeInHierarchy)
            {
                players[i].GetComponent<TempPlayerMovement>().PausePlayer();

            }
        }
        
        yield return new WaitForSeconds(4.3f);
        TurnOffBombs = false;
        beginFade(1);
        ScoreBoard.SetActive(false);
        startAddingScores = false;
        yield return new WaitForSeconds(fadeSpeed);
        
        if (destructables_In_Scene)
        {
            for (int i = 0; i < destructable_GO.Length; i++)
            {
                destructable_GO[i].GetComponent<DestructableObjectBehaviour>().ResetDestructable();
            }
        }

        if (hasStatic_Interactable_In_Scene)
        {
            reset_Objects = true;
        }
        
        beginFade(-1);
        if (!game_Has_Ended) {
            Respawner();
        }else
        {
            winnerBorderImage.enabled = true;
            ShowWinner();
        }
        yield return new WaitForSeconds(fadeSpeed);
        countDown = true;
        if (game_Has_Ended)
        {
            yield return new WaitForSeconds(3f);
            playerHasSpawned = true;
        }
        if (hasStatic_Interactable_In_Scene)
        {
            reset_Objects = false;
        }

    }

    public void ExitGame()
    {
        Application.Quit();
    }


     public void ResetGame()
    {
        
        player1Score = 0;
        currentPlayer1Score = 0;
        player2Score = 0;
        currentPlayer2Score = 0;
        player3Score = 0;
        currentPlayer3Score = 0;
        player4Score = 0;
        currentPlayer4Score = 0;
        playerHasSpawned = false;
        imageTimer = 0;
        winnerBorderImage.fillAmount = 0;
        winnerBorderImage.enabled = false;
        Confetti.SetActive(false);
        endGameMenu.SetActive(false);
        deadPlayersAmt = 0;
        system.SetSelectedGameObject(first_Select_PauseM, new BaseEventData(system));
        isPausedMenu = false;
        StartCoroutine(resetFromEndGame());
        
    }


    IEnumerator resetFromEndGame()
    {
        beginFade(1);
        yield return new WaitForSeconds(fadeSpeed);
        Respawner();
        beginFade(-1);
        yield return new WaitForSeconds(fadeSpeed);
        if (system.firstSelectedGameObject != first_Select_PauseM)
        {
            system.SetSelectedGameObject(first_Select_PauseM, new BaseEventData(system));
        }

    }

    public void ResumeGame()
    {
        isPausedMenu = false;
    }


    void ShowWinner()
    {
        Confetti.SetActive(true);
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != players[winnerNum])
            {
                players[i].GetComponent<TempPlayerMovement>().isAlive = false;
                players[i].GetComponent<TempPlayerMovement>().myMeshRenderer.SetActive(false); ;
                players[i].GetComponent<TempPlayerMovement>().myAim_Indicator.SetActive(false);
                players[i].GetComponent<TempPlayerMovement>().hat_Holder.SetActive(false);
                players[i].GetComponent<BoxCollider>().enabled = false;
            }else if (players[i] == players[winnerNum])
            {
                players[i].GetComponent<TempPlayerMovement>().StartRespawning(new Vector3(0,players[i].GetComponent<TempPlayerMovement>().startingYPos, 0));
            }

        }
        StartCoroutine(waitForMenu());

    }

    IEnumerator waitForMenu()
    {
        yield return new WaitForSeconds(8f);
        players[winnerNum].GetComponent<TempPlayerMovement>().PausePlayer();
        system.SetSelectedGameObject(first_Select_EndG, new BaseEventData(system));
        
        endGameMenu.SetActive(true);
        pauseMenu.SetActive(false);
        for (int i = 0; i < player_Stat_Panels.Length; i++)
        {
            player_Stat_Panels[i].GetComponent<Stats_Manager>().Do_Stats();
        }

    }

    void Respawner()
    {
        for (int i = 0; i < 4; i++)
        {
            player_Fed_Spawns.Add(Map_Spawn_Bounds[i]);
        }
        // StartCoroutine(tileStuff());
        for (int i = 0; i < players.Count; i++)
        {
            int randomSpawnInt = Random.Range(0, player_Fed_Spawns.Count);
            if (players[i].activeInHierarchy)
            {
                players[i].GetComponent<TempPlayerMovement>().StartRespawning(new Vector3(player_Fed_Spawns[randomSpawnInt].position.x, players[i].GetComponent<TempPlayerMovement>().startingYPos, player_Fed_Spawns[randomSpawnInt].position.z));
                player_Fed_Spawns.RemoveAt(randomSpawnInt);
            }
           
        }

        if (!game_Has_Ended) {
            StartCoroutine(RespawnCountdown());
        }
        if (game_Has_Ended)
        {
            hasReset = false;
            game_Has_Ended = false;
        }
    }

    void addScores()
    {
        
        if (currentPlayer1Score < player1Score - scoreBoardOffset)
        {
            currentPlayer1Score += Time.deltaTime * scoreTimerModifier;
        }else if (currentPlayer1Score > player1Score + scoreBoardOffset) currentPlayer1Score -= Time.deltaTime * scoreTimerModifier;
        
        if (currentPlayer2Score < player2Score - scoreBoardOffset)
        {
            currentPlayer2Score += Time.deltaTime * scoreTimerModifier;
        }else if (currentPlayer2Score > player2Score + scoreBoardOffset) currentPlayer2Score -= Time.deltaTime * scoreTimerModifier;
        
        if (currentPlayer3Score < player3Score - scoreBoardOffset)
        {
            currentPlayer3Score += Time.deltaTime * scoreTimerModifier;
        }else if (currentPlayer3Score > player3Score + scoreBoardOffset) currentPlayer3Score -= Time.deltaTime * scoreTimerModifier;
       
        if (currentPlayer4Score < player4Score - scoreBoardOffset)
        {
            currentPlayer4Score += Time.deltaTime * scoreTimerModifier;
        }else if (currentPlayer4Score > player4Score + scoreBoardOffset) currentPlayer4Score -= Time.deltaTime * scoreTimerModifier;

        player1Slider.value = currentPlayer1Score;
        player2Slider.value = currentPlayer2Score;
        player3Slider.value = currentPlayer3Score;
        player4Slider.value = currentPlayer4Score;

    }


    IEnumerator RespawnCountdown()
    {
        countDown = true;
        
        yield return new WaitForSeconds(4f);
        countDown = false;
        
        deadPlayersAmt = 0;
        startRespawn = false;
        countDownTimer = originalRespawnTime;
        hasReset = false;
        
    }

    void OnGUI()
    {
        alpha += fadeInDir * fadeSpeed * Time.deltaTime;

        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }


    public float beginFade(int direction)
    {
        fadeInDir = direction;
        return (fadeSpeed);
    }

    
}
