using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Rewired;
using UnityEngine.PostProcessing;

public class MainMenuManager : MonoBehaviour {

    public GameObject vid_player_GameObject, myUI;
    private VideoPlayer myVideoPlayer;
    private float vidPlayerClipLength;
    public float speedMod;
    public EventSystem ES;
    private GameObject storedSelected;
    public AudioClip[] AC;
    private AudioSource source;
    private bool has_Seen_Tutorial, skip_intro, has_Selected_Tut, has_Selected_Play;

    public GameObject[] Buttons;
    public Transform[] original_Scales;

    private Player myPlayer;
    private int player_Num = 1;

    private float wait_timer;

    public PostProcessingProfile currentProfile;

    // Use this for initialization
    void Start () {
        myVideoPlayer = vid_player_GameObject.GetComponent<VideoPlayer>();
        source = GetComponent<AudioSource>();
        source.clip = AC[0];
        vidPlayerClipLength = (float)myVideoPlayer.clip.length;
        vid_player_GameObject.SetActive(false);
        wait_timer = vidPlayerClipLength;
        myUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        storedSelected = ES.firstSelectedGameObject;
        has_Seen_Tutorial = false;
        myPlayer = ReInput.players.GetPlayer(player_Num - 1);
        
    }

    private void Update()
    {
        if (!has_Seen_Tutorial && wait_timer > 0 && !skip_intro && (has_Selected_Tut || has_Selected_Play))
        {
            wait_timer -= Time.deltaTime;
        }

        if ((wait_timer <= 0 && !has_Seen_Tutorial) || skip_intro)
        {
            wait_timer = vidPlayerClipLength;
            has_Seen_Tutorial = true;
            if (has_Selected_Play)
            {
                vid_player_GameObject.SetActive(false);
                myUI.SetActive(true);
                myVideoPlayer.Stop();
                PlayGame();
                has_Selected_Play = false;
            }
            else if (has_Selected_Tut)
            {
                vid_player_GameObject.SetActive(false);
                myUI.SetActive(true);
                myVideoPlayer.Stop();
                has_Seen_Tutorial = true;
                has_Selected_Tut = false;
            }
        }

        if (myPlayer.GetButtonLongPress("Dash"))
        {
            skip_intro = true;
        }

        
    }

    private void OnLevelWasLoaded(int level)
    {
        has_Seen_Tutorial = false;
        skip_intro = false;
        currentProfile.chromaticAberration.enabled = false;
        
    }

    private void LateUpdate()
    {
        if (ES.currentSelectedGameObject != ES.firstSelectedGameObject)
        {
            if (ES.currentSelectedGameObject == null)
            {
                ES.SetSelectedGameObject(storedSelected);
            }
            else
            {
                storedSelected = ES.currentSelectedGameObject;
            }
        }
    }

    public void StartTutorial()
    {
        vid_player_GameObject.SetActive(true);
        myUI.SetActive(false);
        myVideoPlayer.Play();
        has_Selected_Tut = true;
    }

    
    public void Pulse_Current_Button(int button_ID)
    {
        float _pulse_X;
        float _pulse_Y;
        _pulse_X = Mathf.Abs(Mathf.Cos(2 * (Time.time * speedMod)));
        _pulse_Y = Mathf.Cos((Time.time * speedMod) + original_Scales[button_ID].localScale.y);

        Buttons[button_ID].transform.localScale = new Vector3(_pulse_X, _pulse_X, original_Scales[button_ID].localScale.z);
    }

    public void Reset_To_Original_Scale(int button_ID)
    {
        Buttons[button_ID].transform.localScale = original_Scales[button_ID].localScale;
    }

    IEnumerator waitToStopVideo()
    {
        yield return new WaitForSeconds(vidPlayerClipLength);
        vid_player_GameObject.SetActive(false);
        myUI.SetActive(true);
        myVideoPlayer.Stop();
        has_Seen_Tutorial = true;
    }


    IEnumerator play_Tut_from_play()
    {
        vid_player_GameObject.SetActive(true);
        myUI.SetActive(false);
        myVideoPlayer.Play();
        yield return new WaitForSeconds(vidPlayerClipLength);
        vid_player_GameObject.SetActive(false);
        myUI.SetActive(true);
        myVideoPlayer.Stop();
        int level = Random.Range(0, 2);
        switch (level)
        {
            case 1:
                SceneManager.LoadScene("PirateLevelMP");
                break;
            default:
                SceneManager.LoadScene("LabLevelMP");
                break;
        }
    }

    public void Play_Start_Audio()
    {
        source.Stop();
        source.clip = AC[0];
        source.Play();
    }

    public void Play_Settings_Audio()
    {
        source.Stop();
        source.clip = AC[1];
        source.Play();
    }

    public void Play_Exit_Audio()
    {
        source.Stop();
        source.clip = AC[2];
        source.Play();
    }

    public void PlayGame()
    {

        int level = Random.Range(0, 2);
        switch (level)
        {
            case 1:
                SceneManager.LoadScene("PirateLevelMP");
                break;
            default:
                SceneManager.LoadScene("LabLevelMP");
                break;
        }

    }

    IEnumerator leave_Scene()
    {
       
        yield return new WaitForSeconds(2f);
        int level = Random.Range(0, 2);
        switch (level)
        {
            case 1:
                SceneManager.LoadScene("PirateLevelMP");
                break;
            default:
                SceneManager.LoadScene("LabLevelMP");
                break;
        }
    }


    

    public void QuitGame()
    {
        Application.Quit();
    }


}
