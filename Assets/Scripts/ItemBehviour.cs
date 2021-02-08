using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Dash,
    Checkpoint,
    LevelEnd
}

public class ItemBehviour : MonoBehaviour
{

    public ItemType itemType;

    public ParticleSystem IdleParticles;
    public ParticleSystem EndParticles;
    public AudioClip EndSound;

    public SpriteRenderer spriteRenderer; 

    AudioSource audioSource;

    PlayerManager playerManager; 

    void Start()
    {
        IdleParticles.Play();
        audioSource = GetComponent<AudioSource>();
        playerManager = PlayerManager.Instance;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerManager.GetItem(itemType);
            EndParticles.Play();
            IdleParticles.Clear();
            IdleParticles.Stop();
            audioSource.clip = EndSound;
            audioSource.Play();
            Destroy(GetComponent<Rigidbody2D>());
            Destroy(GetComponent<Collider2D>());
            spriteRenderer.enabled = false; 
            Invoke("End", 3f);
        }

    }

    void End()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
