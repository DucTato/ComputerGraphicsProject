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
        if (isHoming == false)
        {
            direction = transform.right;
        }
        else
        {
            Destroy(gameObject, 3); // Destroy the ghost bullets after 3 seconds
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isHoming == true) 
        {
            direction = PlayerController.instance.transform.position - transform.position;
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
        }

        Destroy(gameObject);
        AudioManager.instance.PlaySFX(4);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
