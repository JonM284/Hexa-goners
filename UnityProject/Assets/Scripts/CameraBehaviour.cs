using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraBehaviour : MonoBehaviour {
    //this script was made using the help of brackeys on youtube
    public float slowedTime = 0.05f;
    public float effectDuration = 2.2f;
    public float stregnth = 1;
    private Vector3 originalPosition;
    private bool zoom = false;
    private Vector3 zoomed_In_Position;
    public PostProcessingProfile currentProfile;
     void Start()
    {
        currentProfile.chromaticAberration.enabled = false;
        originalPosition = transform.position;
        zoomed_In_Position = new Vector3(originalPosition.x, originalPosition.y - 5f, originalPosition.z + 5f);
    }

    // Update is called once per frame
    void Update () {
        Time.timeScale += (1f / effectDuration) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        
	}

    public void SlowedDownEffect()
    {
        
        Time.timeScale = slowedTime;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        StartCoroutine(CameraShake());
    }

    public void ChromVoid()
    {
        StartCoroutine(ChromaticStuff());
    }


    IEnumerator ChromaticStuff()
    {
        currentProfile.chromaticAberration.enabled = true;
        yield return new WaitUntil(() => Time.timeScale > 0.7f);
        currentProfile.chromaticAberration.enabled = false;
    }

    IEnumerator CameraShake()
    {
        
        while (Time.timeScale < 0.8f && !Manager.isPausedMenu)
        {
            float Xposition = Random.Range(-1f,1f) * stregnth;
            float Yposition = Random.Range(-1f,1f) * stregnth;
            
            transform.localPosition = new Vector3(Xposition + originalPosition.x, Yposition + originalPosition.y, originalPosition.z);
            yield return null;
        }
        transform.position = originalPosition;
        

    }


}
