using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{


    public bool HaveDash = false; 

    public Vector3 Current_Checkpoint; 

    // Start is called before the first frame update
    void Start()
    {
        Update_Checkpoint(this.transform.position); 
    }

    // Update is called once per frame
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
                Update_Checkpoint(this.transform.position);
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
