﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float moveSpeed;
    private float normalSpeed;


    [Header("Chase Player")]
    public bool shouldChasePlayer;
    public float rangeToChasePlayer;
    private Vector3 moveDirection;

    [Header("Run Away")]
    public bool shouldRunAway;
    public float runawayRange;


    [Header("Wandering")]
    public bool shouldWander;
    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;

    [Header("Patrolling")]
    public bool shouldPatrol;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;

    

    [Header("Shooting")]
    public bool shouldShoot;

    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;
    public float timeBetweenBurst;
    public int BurstSize;

    public float shootRange;

    [Header("Variables")]
    public SpriteRenderer theBody;
    public Animator anim;

    public int health = 150;

    public GameObject[] deathSplatters;
    public GameObject hitEffect;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    // Start is called before the first frame update
    void Start()
    {
        normalSpeed = moveSpeed;
        if(shouldWander)
        {
            // Randomly pauses during wandering
            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the enemies are visible by any camera, and the player is in the Scene
        // Prevent the enemies from going out of the room 
        if (theBody.isVisible && PlayerController.instance.gameObject.activeInHierarchy)
        {
            moveDirection = Vector3.zero;

            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer && shouldChasePlayer)
            {
                // Facing the enemies towards the player
                if (PlayerController.instance.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = Vector3.one;
                }
                // Chase the player
                moveDirection = PlayerController.instance.transform.position - transform.position;
            } 
            else
            {
                if(shouldWander)
                {
                    if(wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        //move the enemy
                        moveDirection = wanderDirection;

                        if(wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
                        }
                    }

                    if(pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;

                        if(pauseCounter <= 0)
                        {
                            wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);

                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }
                }

                if(shouldPatrol)
                {
                    moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;

                    if(Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < .2f)
                    {
                        // The enemies cycle through the patrol points (which are set in the room)
                        currentPatrolPoint++;
                        if(currentPatrolPoint >= patrolPoints.Length)
                        {
                            currentPatrolPoint = 0;
                        }
                    }
                }
            }
            // If the player is in the Run Away range, RUUUUN!
            if(shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= runawayRange)
            {
                moveSpeed = 5;
                // move in the opposite direction of the player
                moveDirection = transform.position - PlayerController.instance.transform.position;
                // Facing the enemies away from the player (while running away from them :D)
                if (PlayerController.instance.transform.position.x <= transform.position.x)
                {
                    transform.localScale = Vector3.one;
                }
                else
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
            // If the player gets out of Run Away range, chase them! :D
            if (shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) > (runawayRange + 0.2f))
            {
                moveSpeed = normalSpeed;   
                if (PlayerController.instance.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = Vector3.one;
                }
                moveDirection = PlayerController.instance.transform.position - transform.position;
            }

            moveDirection.Normalize();

            theRB.velocity = moveDirection * moveSpeed;


            if (shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange)
            {
                // Set the firing frequency
                fireCounter -= Time.deltaTime;

                if (fireCounter <= 0)
                {
                    fireCounter = timeBetweenBurst;
                    StartCoroutine(BrstFire(BurstSize));
                }
            }


        }
        else
        {
            theRB.velocity = Vector2.zero; // enemies always stay in the room
        }
        // Animator State machine :>
        if (moveDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }
    public void DamageEnemy(int damage)
    {
        health -= damage;

        AudioManager.instance.PlaySFX(2);

        Instantiate(hitEffect, transform.position, transform.rotation);

        if(health <= 0)
        {
            Destroy(gameObject);

            AudioManager.instance.PlaySFX(1);
            // When an enemy is killed, spawn a randomized blood puddle

            int selectedSplatter = Random.Range(0, deathSplatters.Length);

            int rotation = Random.Range(0, 4); // random blood puddle 

            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation * 90f));

            //drop items
            if (shouldDropItem)
            {
                float dropChance = Random.Range(0f, 100f);

                if (dropChance < itemDropPercent)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);

                    Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }
        }
    }
    private IEnumerator BrstFire(int BurstSize)
    {
        // Burst Fire Feature (BFF!) Enemies fire in Bursts, to add variety
        for (int i = 0; i < BurstSize; i++)
        {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
            AudioManager.instance.PlaySFX(13);
            yield return new WaitForSeconds(60 / fireRate);
        }
    }
}
