using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public  class PlayerManager : MonoBehaviour
{
    static PlayerManager mPlayerManager;

    public static PlayerManager Instance
    {
        get
        {
            if (mPlayerManager == null)
            {
                GameObject go = new GameObject();
                mPlayerManager = go.AddComponent<PlayerManager>();
                DontDestroyOnLoad(go);
                DontDestroyOnLoad(mPlayerManager);
            }
            return mPlayerManager;
        }
    }

    public bool HaveDash = false; 

    public Vector3 Current_Checkpoint;

    public GameObject character;

    public LevelManager levelManager;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void GetItem(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Dash:
                HaveDash = true; 
                break;
            case ItemType.Checkpoint:
                Update_Checkpoint(character.transform.position);
                break;
            case ItemType.LevelEnd:
                CinemachineShake.Instance.FadeOut();
                Invoke("LoadNextLevel", 5f);
                break;
            default:
                break;
        }
    }

    public void Update_Checkpoint(Vector3 _position)
    {
        Current_Checkpoint = _position;
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelManager.NextSceneName);
    }
}
