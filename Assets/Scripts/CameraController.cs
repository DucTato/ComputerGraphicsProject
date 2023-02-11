using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float moveSpeed;

    public Transform target;

    public Camera mainCamera, bigMapCamera;
    // 2 cameras, press M for big map.

    private bool bigMapActive;

    public bool isBossRoom;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(isBossRoom)
        {
            target = PlayerController.instance.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // move the camera to the current room
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }

        // Key pressed == M, shows big Map; otherwise, turn off the big Map
        if(Input.GetKeyDown(KeyCode.M) && !isBossRoom)
        {
            if(!bigMapActive)
            {
                ActivateBigMap();
            } else
            {
                DeactivateBigMap();
            }
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ActivateBigMap()
    {
        if (!LevelManager.instance.isPaused)
        {
            // if the game is during the paused session, cannot open big Map
            bigMapActive = true;

            bigMapCamera.enabled = true;
            mainCamera.enabled = false;

            PlayerController.instance.canMove = false; // player is unable to move :>

            Time.timeScale = 0f; // stops game time

            UIController.instance.mapDisplay.SetActive(false); // hides the minimap on the corner
            UIController.instance.bigMapText.SetActive(true); // Message for the player
        }
    }

    public void DeactivateBigMap()
    {
        if (!LevelManager.instance.isPaused)
        {
            bigMapActive = false;

            bigMapCamera.enabled = false;
            mainCamera.enabled = true;

            PlayerController.instance.canMove = true;

            Time.timeScale = 1f; // game time scale to 1

            UIController.instance.mapDisplay.SetActive(true);
            UIController.instance.bigMapText.SetActive(false);
        }
    }
}
