using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FC_GameManager : MonoBehaviour
{
    [Header("Game Conditions")]
    public static string GameMode = "Elimination"; // Survival, Elimination.
    public static int GameDifficulty = 1; // 0~7
    [Space]
    [Header("UI Handlers")]
    public UI_IconBar hpUI;
    public UI_ScoreIndicator scoreIndicator;
    public FC_EndGameEvent gameEndEvent;
    public TextMeshProUGUI gameEndNoticeUI;
    public UI_Description condiReminder;
    public UI_Timer gameTimer;

    public int playerHP = 10;
    public int playerEnergy = 5;
    public int destroyCount = 0;
    public int winningCount = 10;
    public float timeLimit = 60;
    public static int scoreFor100;

    public static bool IsGameActive = true;
    public static FC_GameManager GameManager;

    // Start is called before the first frame update
    void Awake()
    {
        GameManager = this;
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerInfo();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FC_GameManager.ReloadScene();
        }
    }
    private void FixedUpdate()
    {
        CheckGameConditions();
    }


    void InitGame()
    {
        //TODO::
        // FlakCannon: string gameMode = "Elimination" , "Survival"
        //             int difficulty = 3 //0~7
        //             bool IsBossFight = false// if I have time, I can working on it.

        // FlakCannon Player Status:
        //             CannonFireSpeed = 4; // 1 ~ 8
        //             CannonMagazingNumber = 20; // 10 ~ 60
        //             ShieldSustain = 100; // 50 ~ 400
        //             CannonHP = 7; // 1 ~ 15
        //             EnergyGain = 10; // 1 ~ 100

        //EventData.GetData().difficulty = 7;   // For testing
        //EventData.GetData().gameMode = "Survival";

        FC_GameManager.IsGameActive = true;
        playerEnergy = 5;
        destroyCount = 0;

        if (EventData.GetData() is object)
        {
            GameMode = EventData.GetData().gameMode;
            int difficulty = EventData.GetData().difficulty;

            if (difficulty == 0)
            {
                playerHP = 19;
                if (GameMode == "Survival")
                {
                    timeLimit = 30;
                }
                else if (GameMode == "Elimination")
                {
                    timeLimit = 120;
                    winningCount = 15;
                }
            }
            else
            {
                if (GameMode == "Survival")
                {
                    playerHP = Mathf.RoundToInt((7 - difficulty) / 2) + 1;
                    timeLimit = 30 + 30 * ((float)difficulty / 7);
                }
                else if (GameMode == "Elimination")
                {
                    playerHP = (7 - difficulty) * 2 + 1;
                    timeLimit = 80 - 20f * ((float)difficulty / 7f);
                    winningCount = (int)(20 + 20f * ((float)difficulty / 7f));
                }
            }

            if (GameMode == "Survival")
            {
                scoreFor100 = 4000;
                scoreFor100 += (int)(timeLimit / 2f) * 50;
                scoreFor100 += difficulty * 100;
            }
            else if (GameMode == "Elimination")
            {
                scoreFor100 = winningCount * 100;
                scoreFor100 += (int)(timeLimit / 2f) * 50;
                scoreFor100 += difficulty * 100;
            }
            else
            {
                scoreFor100 = 9999;
            }
            Debug.Log(scoreFor100);
        }

        if (gameTimer)
        {
            gameTimer.RunCountDown(timeLimit);
        }
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }





    void UpdatePlayerInfo()
    {
        if (hpUI != null)
        {
            hpUI.ChangeBarValue(playerHP);
        }


        // WinningCondition reminder
        if (condiReminder)
        {
            if (GameMode == "Elimination")
            {
                condiReminder.Clear();
                condiReminder.AddWords("Defeat ");
                condiReminder.AddWords(((winningCount - destroyCount) >= 0 ? (winningCount - destroyCount) : 0).ToString(), ((winningCount - destroyCount) <= 0 ? Color.green : Color.red));
                condiReminder.AddWords(" before run out the time.");
                condiReminder.UpdateSentence();
            }
            else if (GameMode == "Survival")
            {
                condiReminder.Clear();
                condiReminder.AddWords("Please ");
                condiReminder.AddWords("SURVIVAL", Color.red);
                condiReminder.AddWords(" in the remaining time.");
                condiReminder.UpdateSentence();
            }
        }
    }


    void CheckGameConditions()
    {
        if (IsGameActive)
        {
            if (GameMode == "Elimination")
            {
                if (destroyCount >= winningCount)
                {
                    WinningEvent();
                    FC_ScoreTaker.AddScore("Time Bonus", Mathf.RoundToInt(gameTimer.time * 100));
                }
                if (gameTimer.IsTime())
                {
                    FC_ScoreTaker.AddScore("Mission Failed", -2000);
                    LosingEvent();
                }
            }
            else if (GameMode == "Survival")
            {
                if (gameTimer.IsTime())
                {
                    WinningEvent();
                    FC_ScoreTaker.AddScore("Survival Reward", 3000);
                }
            }

            //All cases
            if (playerHP <= 0)
            {
                LosingEvent();
            }
        }
    }
    void WinningEvent()
    {
        // TODO:: deal the events when player won the game
        if (gameEndEvent)
        {
            FC_EndGameEvent.EnableEndGameEvent();
            FC_EndGameEvent.SetTitle("YOU WIN!", Color.green);
            FC_GameManager.IsGameActive = false;
            FC_EnemyProjecter.DestroyAllEnemy();
        }
    }
    void LosingEvent()
    {
        // TODO:: deal the events when player lost the game
        if (gameEndEvent)
        {
            UI_ScreenEffect.StopScreenBump();
            FC_EndGameEvent.EnableEndGameEvent();
            FC_EndGameEvent.SetTitle("YOU WIN!", Color.green);
            FC_GameManager.IsGameActive = false;
            FC_EnemyProjecter.DestroyAllEnemy();
        }
    }


    public static void ChangePlayerHP(int add)
    {
        GameManager.playerHP += add;
    }
    public static void PlayerTakeDamage(int damage)
    {
        GameManager.playerHP -= damage;
        UI_ScreenEffect.ScreenGlassFlash(Color.red, 1f, 0.2f);
        UI_ScreenEffect.ScreenUIBump(0.3f, 0.05f, 10f);
        FC_ScoreTaker.AddScore("Lost HP", -100);
    }
    public static void CountDestroy()
    {
        GameManager.destroyCount++;
    }
    public static void ResetFlakCannonGameSettings()
    {
        Time.timeScale = 1;
        Cursor.visible = true;
    }
}
