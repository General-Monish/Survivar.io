using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
   public enum GameState
    {
        GamePlay,
        Paused,
        GameOver
    }

    public GameState currentState;
    public GameState previousState;

    [Header("UI Screens")]
    public GameObject pauseScreen;
    public GameObject resultScreen;


    [Header("Stats Screen")]
    public TextMeshProUGUI recovery;
    public TextMeshProUGUI moveSpeed;
    public TextMeshProUGUI projectileSpeed;
    public TextMeshProUGUI magnetRange;
    public TextMeshProUGUI might;
    public TextMeshProUGUI health;

    [Header("Result Screen")]
    public TextMeshProUGUI characterName;
    public Image characterImage;
    public TextMeshProUGUI levelReachedText;
    public List<Image> chosenWeaponUI = new List<Image>(6);
    public List<Image> chosenPassiveitemUI = new List<Image>(6);


    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Extra" + this + "Deleted");
            Destroy(gameObject);
        }
        DisableScreen();
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.GamePlay:
                CheckForPauseAndResume();
                break;

            case GameState.Paused:
                CheckForPauseAndResume();
                break;
            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0;
                    Debug.Log("Game Is Over");
                    DisplayResults();
                }
                break;

            default:
                Debug.Log("No Current State Exit ");
                break;
        }
    }

    void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
            Debug.Log("Game Is Paused");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
            Debug.Log("Game Resumed");
        }
    }

    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScreen()
    {
        pauseScreen.SetActive(false);
        resultScreen.SetActive(false);
    }


    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }
    void DisplayResults()
    {
        resultScreen.SetActive(true);
    }

    public void ChosenCharacterUI(CharacterScriptableObjects choosenCharData)
    {
        characterImage.sprite = choosenCharData.Icon;
        characterName.text = choosenCharData.name;
    }

    public void AssignCharUI(int levelReachedData)
    {
        levelReachedText.text = levelReachedData.ToString();
    }

    public void AssignchosenWeaponAndPassiveItemUI(List<Image> chosenWeapondata,List<Image> chosenPassiveItemData)
    {
        if (chosenWeapondata.Count != chosenWeaponUI.Count || chosenPassiveItemData.Count != chosenPassiveitemUI.Count)
        {
            Debug.Log("Chosen Weapon and Data HAve Different Lengths");
            return;
        }

        for(int i = 0; i < chosenWeaponUI.Count - 1; i++)
        {
            if (chosenWeapondata[i].sprite)
            {
                chosenWeaponUI[i].enabled = true;
                chosenWeaponUI[i].sprite = chosenWeapondata[i].sprite;
            }
            else
            {
                chosenWeaponUI[i].enabled = false;
            }
        } 
        
        for(int i = 0; i < chosenPassiveitemUI.Count - 1; i++)
        {
            if (chosenPassiveItemData[i].sprite)
            {
                chosenPassiveitemUI[i].enabled = true;
                chosenPassiveitemUI[i].sprite = chosenPassiveItemData[i].sprite;
            }
            else
            {
                chosenPassiveitemUI[i].enabled = false;
            }
        }
    }
}
