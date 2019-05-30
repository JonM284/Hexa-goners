using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats_Manager : MonoBehaviour {

    public GameObject myPlayer;
    public List<Image> UI_Images;
    public int[] player_Stats;
    public Text[] player_Stats_Text;
    private Manager gameManager;
    public int assigned_Player_Num;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            UI_Images.Add(transform.GetChild(i).GetComponent<Image>());
        }
        
    }

    // Use this for initialization
    void Start () {
        gameManager = Manager.manager_Instance;
        
	}

    public void Do_Stats()
    {
        Read_Player_Stats();
        Change_Pannel_Color();
    }

    void Read_Player_Stats()
    {
        /*player_Stats[0] = gameManager.GetComponent<Manager>().Attack_Stat[myPlayer.GetComponent<TempPlayerMovement>().playerNum - 1];
        player_Stats[1] = gameManager.GetComponent<Manager>().Reverse_Stat[myPlayer.GetComponent<TempPlayerMovement>().playerNum - 1];
        player_Stats[2] = gameManager.GetComponent<Manager>().Block_Stat[myPlayer.GetComponent<TempPlayerMovement>().playerNum - 1];
        */

        player_Stats[0] = PlayerPrefs.GetInt("AttackP"+assigned_Player_Num);
        player_Stats[1] = PlayerPrefs.GetInt("ReverseP" + assigned_Player_Num);
        player_Stats[2] = PlayerPrefs.GetInt("BlockP" + assigned_Player_Num);

        for (int i = 0; i < 3; i++)
        {
            player_Stats_Text[i].text = "" + player_Stats[i];
        }
    }

    void Change_Pannel_Color()
    {
        GetComponent<Image>().color = myPlayer.GetComponent<TempPlayerMovement>().originalMaterialsColor[0];
        for (int i = 0; i < UI_Images.Count; i++)
        {
            UI_Images[i].GetComponent<Image>().color = myPlayer.GetComponent<TempPlayerMovement>().originalMaterialsColor[0];
        }
    }
}
