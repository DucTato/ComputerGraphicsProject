﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChest : MonoBehaviour
{
    public GunPickup[] potentialGuns;

    public SpriteRenderer theSR;
    public Sprite chestOpen;

    public GameObject notification;

    private bool canOpen, isOpen;

    public Transform spawnPoint;

    public float scaleSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(canOpen && !isOpen)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                // Upon pressing E, select a random gun from an array
                int gunSelect = Random.Range(0, potentialGuns.Length);
                Instantiate(potentialGuns[gunSelect], spawnPoint.position, spawnPoint.rotation);
                // change the sprite to the opened chest
                theSR.sprite = chestOpen;
                // Update the state of the chest
                isOpen = true;
                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
        }

        if(isOpen)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * scaleSpeed);
        }
    }

    // Toggle the message in vicinity of the player
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            notification.SetActive(true);

            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            notification.SetActive(false);

            canOpen = false;
        }
    }
}
