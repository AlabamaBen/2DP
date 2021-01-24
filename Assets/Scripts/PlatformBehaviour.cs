using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{

    Vector3 start_pos;
    public Transform target01_pos;
    public Transform target02_pos;
    public float Speed = 3f;


    Transform current_target;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        start_pos = transform.position;
        current_target = target01_pos;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = -(transform.position - current_target.position).normalized * Speed; //Vector3.Lerp(transform.position, current_target.position, 0.01f);
        if ((transform.position - current_target.position).magnitude < 0.1f )
        {
            current_target = current_target== target01_pos ? target02_pos : target01_pos;
        }

    }
}
