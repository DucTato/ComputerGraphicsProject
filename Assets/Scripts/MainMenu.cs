using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string levelToLoad;
    private string[] cheatCode;
    public Image CreditScreen;
    public float waitForAnyKey = 3f;
    public GameObject anyKeyText;
    private bool creditON = false;
    private int index;

    //public GameObject deletePanel;

    //public CharacterSelector[] charactersToDelete;

    // Start is called before the first frame update
    void Start()
    {
        // Set value for the cheat code sequence
        index= 0;
        cheatCode = new string[] { "up", "up", "down", "down", "left", "right", "left", "right", "b", "a"};
        Debug.Log("Game Started");
    }

    // Update is called once per frame
    void Update()
    {
        // Detect the cheat code sequence :D
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(cheatCode[index]))
            {
                index++;
            }
            else
            {
                index = 0; // If the sequence is interrupted by a wrong input, reset the input sequence
            }
            if (index == cheatCode.Length)
            {
                Debug.Log("Cheat Code Activated");
                CharacterTracker.instance.currentHealth = 30;
                CharacterTracker.instance.maxHealth = 30;
                CharacterTracker.instance.currentCoins = 333;
                index= 0; // Reset the input sequence
            }
        }
        
       if (creditON)
        {
            // Wait for x time before showing up
            if (waitForAnyKey > 0)
            {
                waitForAnyKey -= Time.deltaTime;
                if (waitForAnyKey <= 0)
                {
                    anyKeyText.SetActive(true);
                }
            }
            else
            {
                if (Input.anyKeyDown && creditON)
                {
                    CreditScreen.gameObject.SetActive(false);
                    anyKeyText.SetActive(false);
                    creditON = false;
                    waitForAnyKey = 3f;
                }
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }
    public void ShowCredits()
    {
        creditON= true;
        CreditScreen.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // PLANNED FEATURE :D
    //public void DeleteSave()
    //{
    //    deletePanel.SetActive(true);
    //}

    //public void ConfirmDelete()
    //{
    //    deletePanel.SetActive(false);
    //    foreach(CharacterSelector theChar in charactersToDelete)
    //    {
    //        PlayerPrefs.SetInt(theChar.playerToSpawn.name, 0);
    //    }
    //}
    //public void CancelDelete()
    //{
    //    deletePanel.SetActive(false);
    //}
}
