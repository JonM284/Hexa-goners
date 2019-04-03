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
    public EventSystem ES;
    private GameObject storedSelected;
    public AudioClip[] AC;
    private AudioSource source;
    private bool has_Seen_Tutorial, skip_intro, has_Selected_Tut, has_Selected_Play;

    

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

        if (!has_Seen_Tutorial)
        {
            vid_player_GameObject.SetActive(true);
            myUI.SetActive(false);
            myVideoPlayer.Play();
            has_Selected_Play = true;
            return;
        }
        else
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
