using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour {

    public GameObject vid_player_GameObject, myUI;
    private VideoPlayer myVideoPlayer;
    private float vidPlayerClipLength;
    public EventSystem ES;
    private GameObject storedSelected;

    // Use this for initialization
    void Start () {
        myVideoPlayer = vid_player_GameObject.GetComponent<VideoPlayer>();
        vidPlayerClipLength = (float)myVideoPlayer.clip.length;
        vid_player_GameObject.SetActive(false);
        myUI.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        storedSelected = ES.firstSelectedGameObject;
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
        StartCoroutine(waitToStopVideo());
    }


    IEnumerator waitToStopVideo()
    {
        yield return new WaitForSeconds(vidPlayerClipLength);
        vid_player_GameObject.SetActive(false);
        myUI.SetActive(true);
        myVideoPlayer.Stop();
    }

    public void PlayGame()
    {
        int level = Random.Range(0,2);
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
