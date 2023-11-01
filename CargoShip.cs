using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class CargoShip : MonoBehaviour
{
    PlayerControls controls;
    Vector2 move;
    int upDownSpeed, LRSpeed;

    public GameObject GJet;
    Vector2 GJetPos, GJetDestPos;
    float nextJetFire = 0f;
    float jetFireRate =.5f;

    public GameObject PBomb, DeathWall, FreezeWall;
    Vector2 PBombPos, DeathWallPos, FreezeWallPos;

    //Bomb drop rates
    float nextBomb = 0f;
    float bombRate = 1f;

    //public bool deathJet;
    public GameObject mrk3Hit, mrk3Shield, mrk3Shield2, LSG, RSG, LSC, RSC, LSM, RSM;
    public static int G1Health;
    public static bool shieldAct, bombAct, FreezeOne, FreezeTwo, shield1, shield2, DWaveOn, suppGunStat, suppCanStat, suppMissStat;
    public GameObject frzChng1, frzChng2, shieldChng1, shieldChng2, bombChng, suppChng1, suppChng2, suppChng3, dWaveChng, DC1, DC2;
    Vector2 DC1Pos, DC2Pos;
    bool slowedR;

    public GameObject HBHit;
    public Animator PLayerUI, PlayerAVI;

    public static float fxVol;
    public AudioSource playerHitSound, playerDieSound, playerHealthUpSound;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
        LSG.SetActive(false);
        RSG.SetActive(false);
        LSC.SetActive(false);
        RSC.SetActive(false);
        LSM.SetActive(false);
        RSM.SetActive(false);
        gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        shield1 = false;
        shield2 = false;
    }

    void Start()
    {
        upDownSpeed = 35;
        LRSpeed = 20;
        GetComponent<Animator> ().SetBool ("isDead", false);
        GetComponent<Animator> ().SetBool ("isFlashing", false);;
        mrk3Hit.SetActive(false);
        mrk3Shield.SetActive(false);
        mrk3Shield2.SetActive(false);
        G1Health = 500; //OG Health = 500

        shieldAct = false;
        bombAct = false;
        FreezeOne = false;
        FreezeTwo = false;

        frzChng1.SetActive(false);
        frzChng2.SetActive(false);
        shieldChng1.SetActive(false);
        shieldChng2.SetActive(false);
        bombChng.SetActive(false);
        suppChng1.SetActive(false);
        suppChng2.SetActive(false);
        suppChng3.SetActive(false);
        dWaveChng.SetActive(false);
        transform.position = new Vector3(-24f, -34f, 0f);
        slowedR = false;
        DWaveOn = false;
        suppGunStat = false;
        suppCanStat = false;
        suppMissStat = false;
        HBHit.SetActive(false);
    }


    void Update()
    {
        Vector2 m = new Vector2(move.x, move.y) * Time.deltaTime;
        transform.Translate(m, Space.World);
        fxVol = PersSettings.SFXVolume;
        playerHitSound.volume = fxVol;
        playerDieSound.volume = fxVol;
        playerHealthUpSound.volume = fxVol;

        if(G1Health > 0)
        {
            bool isLeftKeyHeld = controls.Gameplay.MoveLeft.ReadValue<float>() > 0.1f;
            bool isRightKeyHeld = controls.Gameplay.MoveRight.ReadValue<float>() > 0.1f;
            bool isUpKeyHeld = controls.Gameplay.MoveUp.ReadValue<float>() > 0.1f;
            bool isDownKeyHeld = controls.Gameplay.MoveDown.ReadValue<float>() > 0.1f;
            bool isFireKeyHeld1 = controls.Gameplay.Shot1.ReadValue<float>() > 0.1f;
            bool isFireKeyHeld2 = controls.Gameplay.Shot2.ReadValue<float>() > 0.1f;
            bool isFireKeyHeld3 = controls.Gameplay.Shot3.ReadValue<float>() > 0.1f;
            bool isFireKeyHeld4 = controls.Gameplay.Shot4.ReadValue<float>() > 0.1f;
            bool isBombKeyHeld = controls.Gameplay.Bomb.ReadValue<float>() > 0.1f;

            if(move.y > 0.5f || isUpKeyHeld == true)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + upDownSpeed * Time.deltaTime);
                if(Time.time > nextJetFire)
                {
                    nextJetFire = Time.time + jetFireRate;
                    GellJet();
                }
            }

            else if(move.y < -0.5f || isDownKeyHeld == true)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - upDownSpeed * Time.deltaTime);
            }

            else
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + 5 * Time.deltaTime);
            }

            if(move.x > 0.5f || isRightKeyHeld == true)
            {
                transform.position = new Vector2(transform.position.x + LRSpeed * Time.deltaTime, transform.position.y);
            }

            else if(move.x < -0.5f || isLeftKeyHeld == true) 
            {
                transform.position = new Vector2(transform.position.x - LRSpeed * Time.deltaTime, transform.position.y);
            }

            if(isBombKeyHeld == true)
            {
                if(bombAct == true)
                {
                    if(Time.time > nextBomb)
                    {
                        nextBomb = Time.time + bombRate;
                        GellBomb();
                    }
                }
            }

            if(G1Health < 25)
            {
                PLayerUI.SetBool("healthLow", true);
            }

            else
            {
                PLayerUI.SetBool("healthLow", false);
            }
        }

        else
        {
            shield1 = false;
            shield2 = false;
            shieldAct = false;
            bombAct = false;
            FreezeOne = false;
            FreezeTwo = false;
            slowedR = false;
            DWaveOn = false;
            suppGunStat = false;
            suppCanStat = false;
            suppMissStat = false;
            mrk3Shield.SetActive(false);
            mrk3Shield2.SetActive(false);
            LSG.SetActive(false);
            RSG.SetActive(false);
            LSC.SetActive(false);
            RSC.SetActive(false);
            LSM.SetActive(false);
            RSM.SetActive(false);
        }
    }

    void GellJet()
    {
        GJetPos = transform.position;
        GJetPos += new Vector2(0f, -17f);
        Instantiate(GJet, GJetPos, Quaternion.identity);
    }

    void GellBomb()
    {
        PBombPos = transform.position;
        PBombPos += new Vector2(0f, 0f);
        Instantiate(PBomb, PBombPos, Quaternion.identity);
    }

    void DWall()
    {
        DeathWallPos = transform.position;
        DeathWallPos += new Vector2(0f, -80f);
        Instantiate(DeathWall, DeathWallPos, Quaternion.identity);
    }

    void FWall()
    {
        FreezeWallPos = transform.position;
        FreezeWallPos += new Vector2(0f, -80f);
        Instantiate(FreezeWall, FreezeWallPos, Quaternion.identity);
    }

    void DeathCircle1()
    {
        DC1Pos = transform.position;
        DC1Pos += new Vector2(0f, 0f);
        Instantiate(DC1, DC1Pos, Quaternion.identity);
    }

    void DeathCircle2()
    {
        DC2Pos = transform.position;
        DC2Pos += new Vector2(0f, 0f);
        Instantiate(DC2, DC2Pos, Quaternion.identity);
    }

    void relife()
    {
        G1Health = 500;
        transform.position = new Vector3(-23.8f, -34.3f, 0f);
    }

    void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "Enem1") 
	    {
            if(G1Health > 0)
            {
                HBHit.SetActive(true);
                PlayerAVI.SetBool("isHit", true);
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);
                G1Health -= 1;
                playerHitSound.Play();
            }
            
            else
            {
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                GetComponent<Animator> ().SetBool ("isDead", true);
                Invoke("DeathCircle1", .1f);
                Invoke("DeathCircle2", .2f);
                playerDieSound.Play();
            }
        }

        if (col.gameObject.tag == "Enem2") 
	    {
            if(G1Health > 0)
            {
                HBHit.SetActive(true);
                PlayerAVI.SetBool("isHit", true);
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);
                G1Health -= 10;
                playerHitSound.Play();
            }
            
            else
            {
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                GetComponent<Animator> ().SetBool ("isDead", true);
                Invoke("DeathCircle1", .1f);
                Invoke("DeathCircle2", .2f);
                playerDieSound.Play();
            }
        }

        if (col.gameObject.tag == "Enem3") 
	    {
            if(G1Health > 0)
            {
                HBHit.SetActive(true);
                PlayerAVI.SetBool("isHit", true);
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);;
                G1Health -= 20;
                playerHitSound.Play();
            }
            
            else
            {
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                GetComponent<Animator> ().SetBool ("isDead", true);
                Invoke("DeathCircle1", .1f);
                Invoke("DeathCircle2", .2f);
                playerDieSound.Play();
            }
        }

        if (col.gameObject.tag == "Enem4") 
	    {
            if(G1Health > 0)
            {
                HBHit.SetActive(true);
                PlayerAVI.SetBool("isHit", true);
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);;
                G1Health -= 40;
                playerHitSound.Play();
            }
            
            else
            {
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                GetComponent<Animator> ().SetBool ("isDead", true);
                Invoke("DeathCircle1", .1f);
                Invoke("DeathCircle2", .2f);
                playerDieSound.Play();
            }
        }

        if (col.gameObject.tag == "Enem5") 
	    {
            if(G1Health > 0)
            {
                HBHit.SetActive(true);
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);;
                G1Health -= 60;
                PlayerAVI.SetBool("isHit", true);
                playerHitSound.Play();
            }
            
            else
            {
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                GetComponent<Animator> ().SetBool ("isDead", true);
                Invoke("DeathCircle1", .1f);
                Invoke("DeathCircle2", .2f);
                playerDieSound.Play();
            }
        }

        if (col.gameObject.tag == "InstaKill") 
	    {
            gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GetComponent<Animator> ().SetBool ("isDead", true);
            Invoke("DeathCircle1", .1f);
            Invoke("DeathCircle2", .2f);
            G1Health -= 5001;
            playerDieSound.Play();
        }

        if (col.gameObject.tag == "HeatlthRest25") 
	    {
            GetComponent<Animator> ().SetBool ("isFlashing", true);
            Invoke("offHit", .4f);
            playerHealthUpSound.Play();
            if(G1Health > 474)
            {
                G1Health += 25;
            }

            else
            {
                G1Health = 500;
            }
        }

        if (col.gameObject.tag == "HeatlthRest50") 
	    {
            GetComponent<Animator> ().SetBool ("isFlashing", true);
            Invoke("offHit", .4f);
            playerHealthUpSound.Play();
            if(G1Health > 449)
            {
                G1Health += 50;
            }

            else
            {
                G1Health = 500;
            }
        }

        if (col.gameObject.tag == "HeatlthRest100") 
	    {
            GetComponent<Animator> ().SetBool ("isFlashing", true);
            Invoke("offHit", .4f);
            playerHealthUpSound.Play();
            if(G1Health > 399)
            {
                G1Health += 100;
            }

            else
            {
                G1Health = 500;
            }
        }

        //WeapTrigs
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "Enem1":
            if(G1Health > 0)
            {
                HBHit.SetActive(true);
                PlayerAVI.SetBool("isHit", true);
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);
                G1Health -= 1;
                playerHitSound.Play();
            }
            
            else
            {
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                GetComponent<Animator> ().SetBool ("isDead", true);
                Invoke("DeathCircle1", .1f);
                Invoke("DeathCircle2", .2f);
                playerDieSound.Play();
            }
            break;

            case "Enem2":
            if(G1Health > 0)
            {
                HBHit.SetActive(true);
                PlayerAVI.SetBool("isHit", true);
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);
                G1Health -= 10;
                playerHitSound.Play();
            }

            else
            {
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                GetComponent<Animator> ().SetBool ("isDead", true);
                Invoke("DeathCircle1", .1f);
                Invoke("DeathCircle2", .2f);
                playerDieSound.Play();
            }
            break;

            case "Enem3":
            if(G1Health > 0)
            {
                HBHit.SetActive(true);
                PlayerAVI.SetBool("isHit", true);
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);
                G1Health -= 20;
                playerHitSound.Play();
            }
            
            else
            {
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                GetComponent<Animator> ().SetBool ("isDead", true);
                Invoke("DeathCircle1", .1f);
                Invoke("DeathCircle2", .2f);
                playerDieSound.Play();
            }
            break;

            case "Enem4":
            if(G1Health > 0)
            {
                HBHit.SetActive(true);
                PlayerAVI.SetBool("isHit", true);
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);
                G1Health -= 40;
                playerHitSound.Play();
            }
            
            else
            {
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                GetComponent<Animator> ().SetBool ("isDead", true);
                Invoke("DeathCircle1", .1f);
                Invoke("DeathCircle2", .2f);
                playerDieSound.Play();
            }
            break;

            case "Enem5":
            if(G1Health > 0)
            {
                HBHit.SetActive(true);
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);
                G1Health -= 60;
                PlayerAVI.SetBool("isHit", true);
                playerHitSound.Play();
            }
            
            else
            {
                gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                GetComponent<Animator> ().SetBool ("isDead", true);
                Invoke("DeathCircle1", .1f);
                Invoke("DeathCircle2", .2f);
            }
            break;

            case "InstaKill":
            gameObject.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            GetComponent<Animator> ().SetBool ("isDead", true);
            Invoke("DeathCircle1", .1f);
            Invoke("DeathCircle2", .2f);
            G1Health -= 5001;
            playerDieSound.Play();
            break;

            case "LvlEnd":
            //ScoreManager.instance.pointBonusAndNxtLvlCarg();
            G1Health -= 5001;
            CargoLvlMan.instance.LvlDone();
            break;

            case "ShieldTrig":
            if(shield1 == false)
            {
                shield1 = true;
                shield2 = false;
                shieldChng1.SetActive(true);
                mrk3Shield.SetActive(true);
                Invoke("closeShield", 10f);
                playerHealthUpSound.Play();
            }

            else if(shield1 == true)
            {
                if(shield2 == false)
                {
                    shieldChng2.SetActive(true);
                    mrk3Shield2.SetActive(true);
                    shield2 = true;
                    shield1 = false;
                    Invoke("closeShield2", 10f);
                    playerHealthUpSound.Play();
                }
                
            }
            Invoke("offHit", .4f);
            break;

            case "SupportTrig":
            suppChng1.SetActive(true);
            LSG.SetActive(true);
            RSG.SetActive(true);
            LSC.SetActive(false);
            RSC.SetActive(false);
            LSM.SetActive(false);
            RSM.SetActive(false);
            Invoke("offHit", .4f);
            suppGunStat = true;
            suppCanStat = false;
            suppMissStat = false;
            playerHealthUpSound.Play();
            break;

            case "SupportTrigTwo":
            suppChng2.SetActive(true);
            LSC.SetActive(true);
            RSC.SetActive(true);
            LSG.SetActive(false);
            RSG.SetActive(false);
            LSM.SetActive(false);
            RSM.SetActive(false);
            Invoke("offHit", .4f);
            suppCanStat = true;
            suppGunStat = false;
            suppMissStat = false;
            playerHealthUpSound.Play();
            break;

            case "SupportTrigThree":
            suppChng3.SetActive(true);
            LSM.SetActive(true);
            RSM.SetActive(true);
            LSC.SetActive(false);
            RSC.SetActive(false);
            LSG.SetActive(false);
            RSG.SetActive(false);
            Invoke("offHit", .4f);
            suppMissStat = true;
            suppCanStat = false;
            suppGunStat = false;
            playerHealthUpSound.Play();
            break;

            case "DeathWaveTrig":
            dWaveChng.SetActive(true);
            DWall();
            DWaveOn = true;
            Invoke("DWaveOff", 60f);
            Invoke("offHit", .4f);
            playerHealthUpSound.Play();
            break;

            case "FreezeTrigOne":
            frzChng1.SetActive(true);
            FWall();
            FreezeOne = true;
            Invoke("offHit", .4f);
            playerHealthUpSound.Play();
            break;

            case "FreezeTrigTwo":
            frzChng2.SetActive(true);
            FreezeTwo = true;
            Invoke("offFreeze2", 10f);
            Invoke("offHit", .4f);
            playerHealthUpSound.Play();
            break;

            case "BombTrig":
            bombChng.SetActive(true);
            bombAct = true;
            Invoke("offHit", .4f);
            playerHealthUpSound.Play();
            break;

            case "PlayerSlower":
            if(slowedR == false)
            {
                slowedR = true;
                upDownSpeed = 5;
                LRSpeed = 5;
                Invoke("ReactRates", 5f);
                playerHitSound.Play();
            }
            break;
        }
    }

    void offHit()
    {
        HBHit.SetActive(false);
        PlayerAVI.SetBool("isHit", false);
        mrk3Hit.SetActive(false);
        frzChng1.SetActive(false);
        frzChng2.SetActive(false);
        shieldChng1.SetActive(false);
        shieldChng2.SetActive(false);
        bombChng.SetActive(false);
        suppChng1.SetActive(false);
        suppChng2.SetActive(false);
        suppChng3.SetActive(false);
        dWaveChng.SetActive(false);
        GetComponent<Animator> ().SetBool ("isFlashing", false);
    }

    void DWaveOff()
    {
        DWaveOn = false;
        FreezeOne = false;
    }

    void offFreeze2()
    {
        FreezeTwo = false;
        FreezeOne = false;
    }

     void ReactRates()
    {
        slowedR = false;
        upDownSpeed = 35;
        LRSpeed = 20;
    }

    void closeShield()
    {
        mrk3Shield.SetActive(false);
        shield1 = false;
    }

    void closeShield2()
    {
        mrk3Shield2.SetActive(false);
        shield2 = false;
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
