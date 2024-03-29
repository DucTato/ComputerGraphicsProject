﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public GameObject[] brokenPieces;
    public int maxPieces = 5;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    //public int breakSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Smash() // Breaking box
    {
        Destroy(gameObject);

        AudioManager.instance.PlaySFX(0);

        //show broken pieces
        int piecesToDrop = Random.Range(1, maxPieces);
        // Randomize the number of pieces

        for (int i = 0; i < piecesToDrop; i++)
        {
            int randomPiece = Random.Range(0, brokenPieces.Length);

            Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
        }

        //drop items
        if (shouldDropItem)
        {
            float dropChance = Random.Range(0f, 100f); // Change the percentage of item drops

            if (dropChance < itemDropPercent)
            {
                int randomItem = Random.Range(0, itemsToDrop.Length);

                Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player can dash to break box
        if (other.tag == "Player")
        {
            if (PlayerController.instance.dashCounter > 0)
            {
                Smash();
            }
        }
        // Player can shoot to break box
        if(other.tag == "PlayerBullet")
        {
            Smash();
        }
    }
}
