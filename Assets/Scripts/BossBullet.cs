using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    public bool isHoming;

    // Start is called before the first frame update
    void Start()
    {
        // if the bullets are not homing bullets, follow this rule set
        if (isHoming == false)
        {
            direction = transform.right;
            // bullets only travel in 1 direction (initial point)
        }
        else
        {
            Destroy(gameObject, 3); 
            // Destroy the ghost bullets after 3 seconds
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If the bullets are homing bullets, follow this rule set
        if (isHoming == true) 
        {
            direction = PlayerController.instance.transform.position - transform.position; // follow the player
            direction.Normalize();
        }
        transform.position += direction * speed * Time.deltaTime;

        if(!BossController.instance.gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.DamagePlayer();
            // if the bullets hit the Player, call this function
        }

        Destroy(gameObject);
        AudioManager.instance.PlaySFX(4);
        // impact sound effect
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        // If the bullets are not seen by the camera, destroy
    }
}
