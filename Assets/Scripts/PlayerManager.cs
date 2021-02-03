using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
            default:
                break;
        }
    }

    public void Update_Checkpoint(Vector3 _position)
    {
        Current_Checkpoint = _position;
    }
}
