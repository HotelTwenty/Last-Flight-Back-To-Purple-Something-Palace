using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class TrailerSceneTwoManager : MonoBehaviour
{
    PlayerControls controls;
    public bool yesOn, noOn;
    public AudioSource buttSel;
    public static float SFVol;
    public GameObject UIFlash, Asambeni, GoodLuckPilot, yesButt, noButt, MainQuest;

    public GameObject FasterFlash, HigherFlash, FasterAndHigherFlash;
    // Start is called before the first frame update

    void Awake()
    {
        //PlayerPrefs.DeleteAll();
        controls = new PlayerControls();
        controls.MenuNav.MoveLeft.performed += ctx => moveButtLeft();
        controls.MenuNav.MoveRight.performed += ctx => moveButRight();
        
        controls.MenuNav.SelectBut.performed += ctx => selectMiss();
    }

    void Start()
    {
        yesOn = true;
        UIFlash.SetActive(false);
        Asambeni.SetActive(false);
        GoodLuckPilot.SetActive(false);
        FasterFlash.SetActive(false);
        HigherFlash.SetActive(false);
        FasterAndHigherFlash.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SFVol = PersSettings.SFXVolume;
        buttSel.volume = SFVol;
    }

    public void moveButRight()
    {
        if(yesOn == true)
        {
            yesOn = false;
            noOn = true;
        }
    }

    public void moveButtLeft()
    {
        if(noOn == true)
        {
            yesOn = true;
            noOn = false;
        }
    }

     public void selectMiss()
    {
        buttSel.Play();
        if(yesOn == true)
        {
            Invoke("actFlash",  .2f);
            Invoke("deactFlash", .7f);
            Invoke("toNxtScene", 1f);
        }

        else
        {
            Invoke("actFlash",  .2f);
            Invoke("deactFlash", .4f);
            Invoke("actAsambeni", .4f);
            Invoke("deactInitialScreen", .5f);
            Invoke("actGoodLuckPilot", .9f);
            Invoke("deactAsambeni", 1f);
            Invoke("actHigherFlash", 7f);
            Invoke("deactHigherFlash", 10f);
            Invoke("ActFasterFlash", 10f);
            Invoke("deactFasterFlash", 15f);
            Invoke("actFasterAndHigherFlash", 15f);
            Invoke("toMotto", 20f);
        }
    }

    public void toNxtScene()
    {
        SceneManager.LoadScene("TrailerSceneOne");
    }

    void toMotto()
    {
        SceneManager.LoadScene("PostMrkFour");
    }

    public void actFlash()
    {
        UIFlash.SetActive(true);
    }

    public void deactFlash()
    {
        UIFlash.SetActive(false);
    }

    public void ActFasterFlash()
    {
        FasterFlash.SetActive(true);
    }

    public void deactFasterFlash()
    {
        FasterFlash.SetActive(false);
    }

    public void actHigherFlash()
    {
        HigherFlash.SetActive(true);
    }

    public void deactHigherFlash()
    {
        HigherFlash.SetActive(false);
    }

    public void actFasterAndHigherFlash()
    {
        FasterAndHigherFlash.SetActive(true);
    }

    public void deactFasterAndHigherFlash()
    {
        FasterAndHigherFlash.SetActive(false);
    }

    void actAsambeni()
    {
        Asambeni.SetActive(true);
    }

    void deactAsambeni()
    {
        Asambeni.SetActive(false);
    }

    void actGoodLuckPilot()
    {
        GoodLuckPilot.SetActive(true);
    }

    void deactGoodLuckPilot()
    {
        GoodLuckPilot.SetActive(false);
    }

    void deactInitialScreen()
    {
        yesButt.SetActive(false);
        noButt.SetActive(false);
        MainQuest.SetActive(false);
    }

    void OnEnable()
    {
        controls.MenuNav.Enable();
    }

    void OnDisable()
    {
        controls.MenuNav.Disable();
    }
}
