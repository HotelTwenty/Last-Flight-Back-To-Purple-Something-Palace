using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class TrailerSceneOneMan : MonoBehaviour
{
    PlayerControls controls;
    public bool yesOn, noOn;
    public AudioSource buttSel;
    public static float SFVol;
    public GameObject UIFlash;
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
        }
    }

    public void toNxtScene()
    {
        SceneManager.LoadScene("TrailerSceneTwo");
    }

    public void actFlash()
    {
        UIFlash.SetActive(true);
    }

    public void deactFlash()
    {
        UIFlash.SetActive(false);
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
