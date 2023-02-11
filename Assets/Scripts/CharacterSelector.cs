using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    private bool canSelect;

    public GameObject message;

    public PlayerController playerToSpawn;

    public bool shouldUnlock;

    // Start is called before the first frame update
    void Start()
    {
        if (shouldUnlock)
        {
            if (PlayerPrefs.HasKey(playerToSpawn.name))
            {
                if (PlayerPrefs.GetInt(playerToSpawn.name) == 1)
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(canSelect)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Vector3 playerPos = PlayerController.instance.transform.position;

                Destroy(PlayerController.instance.gameObject);
                // Destroy the current Player Object
                PlayerController newPlayer = Instantiate(playerToSpawn, playerPos, playerToSpawn.transform.rotation);
                PlayerController.instance = newPlayer;

                gameObject.SetActive(false);
                // Camera focuses on the new Player Object
                CameraController.instance.target = newPlayer.transform;
                CharacterSelectManager.instance.activePlayer = newPlayer;
                // Set the previous chosen character to be able to be selected again
                CharacterSelectManager.instance.activeCharSelect.gameObject.SetActive(true);
                CharacterSelectManager.instance.activeCharSelect = this;
                
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canSelect = true;
            message.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canSelect = false;
            message.SetActive(false);
        }
    }
}
