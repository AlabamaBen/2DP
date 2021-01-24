using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{


    public bool HaveDash = false; 

    // Start is called before the first frame update
    void Start()
    {
        
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
            default:
                break;
        }
    }
}
