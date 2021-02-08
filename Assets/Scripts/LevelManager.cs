using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public CinemachineVirtualCamera CVcamera;
    public GameObject CharacterPrefab;
    public Transform CharacterSpawn;
    public string LevelDescription;
    public string NextSceneName;
    public bool IsFirstLevel;

    private PlayerManager playerManager; 

    // Start is called before the first frame update
    void Start()
    {
        CVcamera.Follow = CharacterSpawn;
        playerManager = PlayerManager.Instance;
        playerManager.levelManager = this;
    }

    public void Start_Game()
    {
        GameObject character = Instantiate(CharacterPrefab, CharacterSpawn, true);
        CVcamera.Follow = character.transform;
        playerManager.character = character;
        playerManager.Update_Checkpoint(character.transform.position);
    }

}
