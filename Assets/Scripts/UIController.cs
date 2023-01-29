using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider healthSlider;
    public Text healthText, coinText;

    public GameObject deathScreen;

    public Image fadeScreen;
    public float fadeSpeed;
    public GameObject levelText;
    private bool fadeToBlack, fadeOutBlack;

    public string newGameScene, mainMenuScene;

    public GameObject pauseMenu, mapDisplay, bigMapText;

    public Image currentGun;
    public Text gunText;

    public Slider bossHealthBar;
    public float levelTextTime;
     


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        fadeOutBlack = true;
        fadeToBlack = false;

        currentGun.sprite = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].gunUI;
        gunText.text = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].weaponName;
    }

    // Update is called once per frame
    void Update()
    {
        if(fadeOutBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if(fadeScreen.color.a == 0f)
            {
                fadeOutBlack = false;
                fadeScreen.gameObject.SetActive(false);

            }
        }

        if(fadeToBlack)
        {
            fadeScreen.gameObject.SetActive(true);
            levelText.gameObject.SetActive(false);

            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                fadeToBlack = false;
                
            }
        }

        //levelTextTime-= Time.deltaTime;
        //if (levelTextTime < 0)
        //{
        //    UIController.instance.levelText.SetActive(false);
        //}
       
    }

    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    public void NewGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(newGameScene);

        Destroy(PlayerController.instance.gameObject);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(mainMenuScene);

        Destroy(PlayerController.instance.gameObject);
    }

    public void Resume()
    {
        LevelManager.instance.PauseUnpause();
    }
}
