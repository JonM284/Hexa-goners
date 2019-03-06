using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSceneManager : MonoBehaviour {

    public int current_Level_ID;

	public void GoToNextLevel()
    {
        switch (current_Level_ID)
        {
            case 2:
                SceneManager.LoadScene("PirateLevelMP");
                break;
            case 1:
                SceneManager.LoadScene("LabLevelMP");
                break;

            default:
                Debug.Log("No scene to currently go to");
                break;
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

   
}
