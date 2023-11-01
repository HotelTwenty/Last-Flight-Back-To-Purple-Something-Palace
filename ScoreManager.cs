using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static int CargoHScore = 0;
    public static int ReconHScore = 0;
    public static int Mrk3HScore = 0;
    public static int Mrk4HScore = 0;
    public static CargoLvlMan instatnce;
    public static int CSHealth, RSHealth, Mrk3Health, Mrk4Health;
    public static int CrgScore, RcnScore, M3Score, M4Score;
    public static int RetCSScore, RetRSScore, RetMrk3Score, RetMrk4SScore;
    public int Cargo, Recon, Mark3, Mark4;
    public static ScoreManager instance;

    void Awake()
    {
        loadScores();
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        loadScores();
        Cargo = CargoHScore;
        Recon = ReconHScore;
        Mark3 = Mrk3HScore;
        Mark4 = Mrk4HScore;
        
        RetCSScore = CargoLvlMan.score;
        RetRSScore = ReconLvlMan.RecScore;
        RetMrk3Score = MarkThreeLvlMan.M3Score;
        RetMrk4SScore = MarkFourLvlMan.M4Score;

        CSHealth = CargoShip.G1Health;
        RSHealth = ReconShip.G1Health;
        Mrk3Health = GellyMThree.G1Health;
        Mrk4Health = GellyMFour.G1Health;

        if(CSHealth <= 0)
        {
            if(CSHealth > -4000)
            {
                if(RetCSScore > CargoHScore)
                {
                    saveCargoHighScore();
                }
            }

            else
            {
                pointBonusAndNxtLvlCarg();
            }
        }
        
        if(RSHealth <= 0)
        {
            if(RSHealth > -10000)
            {
                if(RetRSScore > ReconHScore)
                {
                    saveReconHighScore();
                }
            }

            else
            {
                pointBonusAndNxtLvlRec();
            }
        }
        
        if(Mrk3Health <= 0)
        {
            if(Mrk3Health > -20000)
            {
                if(RetMrk3Score > Mrk3HScore)
                {
                    saveMrk3HighScore();
                }
            }
            
            else
            {
                pointBonusAndNxtLvlMrk3();
            }
        }
        
        if(Mrk4Health <= 0)
        {
            if(Mrk4Health > -30000)
            {
                if(RetMrk4SScore > Mrk4HScore)
                {
                    saveMrk4HighScore();
                }
            }

            else
            {
                pointBonusAndNxtLvlMrk4();
            }
        }
    }

    public void saveCargoHighScore()
    {
        PlayerPrefs.SetInt("HScore", RetCSScore);
    }

    public void saveReconHighScore()
    {
        PlayerPrefs.SetInt("RHScore", RetRSScore);
    }

    public void saveMrk3HighScore()
    {
        PlayerPrefs.SetInt("M3CurScore", RetMrk3Score);
    }

    public void saveMrk4HighScore()
    {
        PlayerPrefs.SetInt("M4CurScore", RetMrk4SScore);
    }

    public void loadScores()
    {
        CargoHScore = PlayerPrefs.GetInt("HScore");
        ReconHScore = PlayerPrefs.GetInt("RHScore");
        Mrk3HScore = PlayerPrefs.GetInt("M3CurScore");
        Mrk4HScore = PlayerPrefs.GetInt("M4CurScore");
    }

    public void cScore()
    {
        PlayerPrefs.SetInt("HScore", 0);
        PlayerPrefs.SetInt("RHScore", 0);
        PlayerPrefs.SetInt("M3CurScore", 0);
        PlayerPrefs.SetInt("M4CurScore", 0);
    }

    public void pointBonusAndNxtLvlCarg()
    {
        RetCSScore += 1500;
        Invoke("saveCargoHighScore", .05f);
    }

    public void pointBonusAndNxtLvlRec()
    {
        RetRSScore += 9000;
        Invoke("saveReconHighScore", .05f);
    }

    public void pointBonusAndNxtLvlMrk3()
    {
        RetMrk3Score += 10000;
        Invoke("saveMrk3HighScore", .05f);
    }

    public void pointBonusAndNxtLvlMrk4()
    {
        RetMrk4SScore += 1000000;
        Invoke("saveMrk4HighScore", .05f);
    }
}
