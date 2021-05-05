using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UI_Manager : MonoBehaviour
{

    public GameObject StartCanvas;
    public GameObject DashCanvas;

    private bool GameStarted;
    private bool DashShown;

    public LevelManager levelManager; 
    private PlayerManager playerManager;

    public Text LevelDescriptor;
    public Text TitleText;
    public Text EscapeText;


    // Start is called before the first frame update
    void Start()
    {
        GameStarted = false;
        playerManager = PlayerManager.Instance;
        DashShown = playerManager.HaveDash; 
        DashCanvas.SetActive(false);

        StartCanvas.SetActive(true);
        TitleText.text = levelManager.IsFirstLevel ? "HEDERA" : "";
        LevelDescriptor.text = levelManager.LevelDescription;
}

// Update is called once per frame
void Update()
    {
        if(Input.GetButtonDown("Jump") && !GameStarted)
        {
            levelManager.Start_Game();
            StartCanvas.SetActive(false);
            GameStarted = true;
        }
        if (Input.GetButtonDown("Escape"))
        {
            Application.Quit();
        }
        if (playerManager)
        {
            if(playerManager.HaveDash && !DashShown)
            {
                UnlockDash();
            }
        }
        if(playerManager.GameEnd)
        {
            EscapeText.text = "PRESS ESCAPE TO EXIT";
        }
    }
    void UnlockDash()
    {
        DashCanvas.SetActive(true);
        Invoke("Hide_DashCanvas", 10f);
        DashShown = true;
    }

    void Hide_DashCanvas()
    {
        DashCanvas.SetActive(false);
    }
}
