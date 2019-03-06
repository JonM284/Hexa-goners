using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFollower : MonoBehaviour {
    public GameObject myPlayer;
    private Camera cam;
    public Slider mySlider;
    public Image[] SliderImages;
    public Image[] LivesImages;
    public Image wrongButton_Img;
    private Color playersColor;
    private bool isSetActive = false, livesSetActive = true, flashingWhite = false, turnOffLives = false;
	// Use this for initialization
	void Start () {
        cam = Camera.main;
        playersColor = myPlayer.GetComponent<TempPlayerMovement>().originalMaterialsColor[0];
        for (int i = 0; i < SliderImages.Length; i++)
        {
            SliderImages[i].enabled = false;
            SliderImages[i].color = playersColor;
        }
        
        for (int x = 0; x < LivesImages.Length; x++)
        {
            LivesImages[x].enabled = true;
            LivesImages[x].color = playersColor;
        }
        wrongButton_Img.enabled = false;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (myPlayer.GetComponent<TempPlayerMovement>().isAlive) {
            transform.position = cam.WorldToScreenPoint(new Vector3(myPlayer.transform.position.x,
                myPlayer.transform.position.y + 2f, myPlayer.transform.position.z + 1.2f));
            if (myPlayer.GetComponent<TempPlayerMovement>().wrongMove && !wrongButton_Img.enabled)
            {
                wrongButton_Img.enabled = true;
            }else if (!myPlayer.GetComponent<TempPlayerMovement>().wrongMove && wrongButton_Img.enabled)
            {
                wrongButton_Img.enabled = false;
            }

            if (myPlayer.GetComponent<TempPlayerMovement>().hasMaxed_out && !flashingWhite)
            {
                StartCoroutine(flashToWhite());
            }
            if (myPlayer.GetComponent<TempPlayerMovement>().hasTakenHit && livesSetActive)
            {
                for (int x = 0; x < LivesImages.Length; x++)
                {
                    LivesImages[x].enabled = false;
                }
                livesSetActive = false;
            }else if(!myPlayer.GetComponent<TempPlayerMovement>().hasTakenHit && !livesSetActive)
            {
                if (myPlayer.GetComponent<TempPlayerMovement>().lives > 0) {
                    for (int x = 0; x < myPlayer.GetComponent<TempPlayerMovement>().lives - 1; x++)
                    {
                        LivesImages[x].enabled = true;
                    }
                }else
                {
                    for (int x = 0; x < 2; x++)
                    {
                        LivesImages[x].enabled = false;
                    }
                }
                livesSetActive = true;
            }else if (myPlayer.GetComponent<TempPlayerMovement>().hasSpawned && livesSetActive)
            {
                for (int x = 0; x < LivesImages.Length; x++)
                {
                    SliderImages[x].enabled = true;
                    LivesImages[x].enabled = true;
                }
                livesSetActive = false;
            }
            if (myPlayer.GetComponent<TempPlayerMovement>().stamina < 100)
            {
                mySlider.value = myPlayer.GetComponent<TempPlayerMovement>().stamina;
                if (!isSetActive)
                {
                    for (int i = 0; i < SliderImages.Length; i++)
                    {
                        SliderImages[i].enabled = true;
                    }
                    isSetActive = true;
                }
                
            }
            else if (myPlayer.GetComponent<TempPlayerMovement>().stamina >= 100 && isSetActive)
            {
                for (int i = 0; i < SliderImages.Length; i++)
                {
                    SliderImages[i].enabled = false;
                }
                isSetActive = false;
            }
        }else if ((!myPlayer.GetComponent<TempPlayerMovement>().isAlive 
            || myPlayer.GetComponent<TempPlayerMovement>().isSpawning)
             && !turnOffLives)
        {
            for (int i = 0; i < SliderImages.Length; i++)
            {
                SliderImages[i].enabled = false;
                LivesImages[i].enabled = false;   
            }
            if (wrongButton_Img.enabled)
            {
                wrongButton_Img.enabled = false;
            }

            
            turnOffLives = true;
            StartCoroutine(Turn_Off_Lives_Switch());
        }
    }

    IEnumerator Turn_Off_Lives_Switch()
    {
        yield return new WaitUntil(() => myPlayer.GetComponent<TempPlayerMovement>().isAlive);
        turnOffLives = false;
    }

    IEnumerator flashToWhite()
    {
        flashingWhite = true;
        while (myPlayer.GetComponent<TempPlayerMovement>().hasMaxed_out)
        {
            SliderImages[1].color = Color.white;
            yield return new WaitForSeconds(0.08f);
            SliderImages[1].color = playersColor;
            yield return new WaitForSeconds(0.08f);
        }
        flashingWhite = false;
    }
}
