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
        GameOver,
        LevelUp
    }

    public GameState currentState;
    public GameState previousState;

    [Header("UI Screens")]
    public GameObject pauseScreen;
    public GameObject resultScreen;
    public GameObject LevelUptScreen;


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
    public TextMeshProUGUI TimeSurvivedText;
    public List<Image> chosenWeaponUI = new List<Image>(6);
    public List<Image> chosenPassiveitemUI = new List<Image>(6);

    [Header("Stopwatch")]
    public float timeLimit;
    float stopWatchtime;
    public TextMeshProUGUI stopWatchDisplayext;


    public bool isGameOver = false;
    public bool chosingUpgrade;

    public GameObject playerObject;
    public GameObject Joystick;

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
                UpdateStopWatch();
                Joystick.SetActive(true);
                break;

            case GameState.Paused:
                CheckForPauseAndResume();
                Joystick.SetActive(false);
                break;
            case GameState.GameOver:

                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0;
                    Joystick.SetActive(false);
                    Debug.Log("Game Is Over");
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                Joystick.SetActive(false);
                if (!chosingUpgrade)
                {
                    chosingUpgrade = true;
                    Time.timeScale = 0f;
                    Debug.Log("Level Up Shpwn");
                    LevelUptScreen.SetActive(true);
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
        LevelUptScreen.SetActive(false);
    }


    public void GameOver()
    {
        TimeSurvivedText.text = stopWatchDisplayext.text;
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

    public void UpdateStopWatch()
    {
        stopWatchtime += Time.deltaTime;
        UpdateStopWatchDisplay();
        if (stopWatchtime >= timeLimit)
        {
            GameOver();
        }
    }

    public void UpdateStopWatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopWatchtime / 60);
        int seconds = Mathf.FloorToInt(stopWatchtime % 60);
        stopWatchDisplayext.text = string.Format("{0:00}:{1:00}",minutes,seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        chosingUpgrade = false;
        Time.timeScale = 1f;
        LevelUptScreen.SetActive(false);
        ChangeState(GameState.GamePlay);
    }
}
