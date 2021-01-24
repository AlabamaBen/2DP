using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{

    public GameObject StartCanvas;
    public GameObject DashCanvas;

    public CinemachineVirtualCamera CVcamera; 

    private bool GameStarted;
    private bool DashShown;



    public GameObject CharacterPrefab;
    public Transform CharacterSpawn;

    private PlayerManager playerManager; 
    // Start is called before the first frame update
    void Start()
    {
        GameStarted = false;
        DashShown = false; 
        DashCanvas.SetActive(false);
        CVcamera.Follow = CharacterSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump") && !GameStarted)
        {
            Start_Game();
        }
        if(playerManager)
        {
            if(playerManager.HaveDash && !DashShown)
            {
                UnlockDash();
            }
        }
    }

    void Start_Game()
    {
        StartCanvas.SetActive(false);
        GameStarted = true;
        GameObject character = Instantiate(CharacterPrefab, CharacterSpawn, true);
        CVcamera.Follow = character.transform;
        playerManager = character.GetComponent<PlayerManager>();
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
