﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public Gun theGun;

    public float waitToBeCollected = .5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Wait for x duration before picking up
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            // PLANNED FEATURE :>
            // bool hasGun = true;
            //foreach(Gun gunToCheck in PlayerController.instance.availableGuns)
            //{
                //if(theGun.weaponName == gunToCheck.weaponName)
                //{
                //   hasGun = true;
                //}
            //}

            //if(!hasGun)
            
            // Clone the weapon from Prefabs to equip the player
                Gun gunClone = Instantiate(theGun);
                gunClone.transform.parent = PlayerController.instance.gunArm;
                gunClone.transform.position = PlayerController.instance.gunArm.position;
                gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                gunClone.transform.localScale = Vector3.one;
            // Swap the new weapon. Make it the current weapon
                PlayerController.instance.availableGuns.Add(gunClone);
                PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
                PlayerController.instance.SwitchGun();
            
            // Destroy the "pickup" object
            Destroy(gameObject);

            AudioManager.instance.PlaySFX(7);
        }
    }
}
