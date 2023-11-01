using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class GellyMThree : MonoBehaviour
{
    PlayerControls controls;
    Vector2 move;
    int upDownSpeed, LRSpeed;

    public GameObject GJet;
    Vector2 GJetPos, GJetDestPos;
    float nextJetFire = 0f;
    float jetFireRate = 0.05f;
    public GameObject PBull, PBull2L, PBull2R, PBull3L, PBull3R, PBomb, DeathWall, FreezeWall;
    Vector2 PBullPos, PBull2LPos, PBull2RPos, PBull3LPos, PBull3RPos, PBombPos, DeathWallPos, FreezeWallPos;
    float nextBullFire = 0f;
    float bullFireRate = .1f;

    //Cannon FireRates
    float nextCanFire = 0f;
    float canFireRate = .5f;
    float nextCanFire2 = 0f;
    float canFireRate2 = .2f;
    float nextCanFire3 = 0f;
    float canFireRate3 = .1f;

    //Missile fire rates
    float nextMissFire = 0f;
    float missFireRate = 1f;
    float nextMissFire2 = 0f;
    float missFireRate2 = .5f;
    float nextMissFire3 = 0f;
    float missFireRate3 = .2f;

    //Bomb drop rates
    float nextBomb = 0f;
    float bombRate = 1f;
    

    //public bool deathJet;
    public GameObject mrk3Hit, playerLaser, playerLaserFlash, mrk3Shield, LSG, RSG, LSC, RSC, LSM, RSM;
    public static int G1Health;
    public static bool cannonFR1, cannonFR2, cannonFR3;
    public static bool missAct1, missAct2, missAct3;
    public static float laserTime;
    public float curLTime;
    public static bool laserStop;
    public static bool shieldAct, bombAct, FreezeOne, FreezeTwo, shield1, DWaveOn, suppGunStat, suppCanStat, suppMissStat, canOn, missOn, laseSwitch;
    public bool mStat1, mStat2, mStat3, canStat1, canStat2, canStat3;
    public GameObject canChng1, canChng2, canChng3, missChng1, missChng2, missChng3, laserChng, frzChng1, frzChng2, shieldChng1, bombChng;
    public GameObject suppChng1, suppChng2, suppChng3, dWaveChng, DC1, DC2;
    Vector2 DC1Pos, DC2Pos;
    bool slowedR;

    public GameObject HBHit;
    public Animator PLayerUI, PlayerAVI;

    public static float fxVol;
    public AudioSource playerHitSound, playerDieSound, playerHealthUpSound;
    public int curHealth;

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
        slowedR = false;
    }

    void Start()
    {
        upDownSpeed = 35;
        LRSpeed = 25;
        GetComponent<Animator> ().SetBool ("isRolling", false);
        GetComponent<Animator> ().SetBool ("isDead", false);
        GetComponent<Animator> ().SetBool ("isFlashing", false);;
        mrk3Hit.SetActive(false);
        playerLaser.SetActive(false);
        playerLaserFlash.SetActive(false);
        mrk3Shield.SetActive(false);
        G1Health = 500; //Change back to 500
        cannonFR1 = false;
        cannonFR2 = false;
        cannonFR3 = false;
        HBHit.SetActive(false);

        missAct1 = false;
        missAct2 = false;
        missAct3 = false;
        laserTime = 25f;
        laserStop = true;
        shieldAct = false;
        bombAct = false;
        FreezeOne = false;
        FreezeTwo = false;
        suppGunStat = false;
        suppCanStat = false;
        suppMissStat = false;
        missOn = false;
        canOn = false;
        laseSwitch = false;

        canChng1.SetActive(false);
        canChng2.SetActive(false);
        canChng3.SetActive(false);
        missChng1.SetActive(false);
        missChng2.SetActive(false);
        missChng3.SetActive(false);
        laserChng.SetActive(false);
        frzChng1.SetActive(false);
        frzChng2.SetActive(false);
        shieldChng1.SetActive(false);
        bombChng.SetActive(false);
        suppChng1.SetActive(false);
        suppChng2.SetActive(false);
        suppChng3.SetActive(false);
        dWaveChng.SetActive(false);
        transform.position = new Vector3(-24f, -34f, 0f);
    }


    void Update()
    {
        curHealth = G1Health;
        mStat1 = missAct1;
        mStat2 = missAct2;
        mStat3 = missAct3;
        canStat1 = cannonFR1;
        canStat2 = cannonFR2;
        canStat3 = cannonFR3;

        fxVol = PersSettings.SFXVolume;
        playerHitSound.volume = fxVol;
        playerDieSound.volume = fxVol;
        playerHealthUpSound.volume = fxVol;

        Vector2 m = new Vector2(move.x, move.y) * Time.deltaTime;
        transform.Translate(m, Space.World);

        if(G1Health > 0)
        {
            if(laserTime < 0)
            {
                laserStop = true;
            }

            if(laserStop == true && laserTime <= 25f)
            {
                laserTime += 1;
            }

            if(laserTime == 25f)
            {
                laserStop = false;
            }

            curLTime = laserTime;

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
                GetComponent<Animator> ().SetBool ("isRolling", true);
                transform.position = new Vector2(transform.position.x + LRSpeed * Time.deltaTime, transform.position.y);
            }

            else if(move.x < -0.5f || isLeftKeyHeld == true) 
            {
                GetComponent<Animator> ().SetBool ("isRolling", true);
                transform.position = new Vector2(transform.position.x - LRSpeed * Time.deltaTime, transform.position.y);
            }

            else
            {
                GetComponent<Animator> ().SetBool ("isRolling", false);
            }

            if(isFireKeyHeld1 == true)
            {
                if(Time.time > nextBullFire)
                {
                    nextBullFire = Time.time + bullFireRate;
                    GellFire1();
                }
            }

            if(isFireKeyHeld2 == true)
            {
                if(cannonFR1 == true)
                {
                    if(Time.time > nextCanFire)
                    {
                        nextCanFire = Time.time + canFireRate;
                        GellFire2Left();
                        GellFire2Right();
                    }
                }

                else if(cannonFR2 == true)
                {
                    if(Time.time > nextCanFire2)
                    {
                        nextCanFire2 = Time.time + canFireRate2;
                        GellFire2Left();
                        GellFire2Right();
                    }
                }

                else if(cannonFR3 == true)
                {
                    if(Time.time > nextCanFire3)
                    {
                        nextCanFire3 = Time.time + canFireRate3;
                        GellFire2Left();
                        GellFire2Right();
                    }
                }
            }

            if(isFireKeyHeld3 == true)
            {
                if(missAct1 == true)
                {
                    if(Time.time > nextMissFire)
                    {
                        nextMissFire = Time.time + missFireRate;
                        GellFire3Left();
                        GellFire3Right();
                    }
                }

                else if(missAct2 == true)
                {
                    if(Time.time > nextMissFire2)
                    {
                        nextMissFire2 = Time.time + missFireRate2;
                        GellFire3Left();
                        GellFire3Right();
                    }
                }

                else if(missAct3 == true)
                {
                    if(Time.time > nextMissFire3)
                    {
                        nextMissFire3 = Time.time + missFireRate3;
                        GellFire3Left();
                        GellFire3Right();
                    }
                }
            }

            if(isFireKeyHeld4 == true)
            {
                if(laserStop == false)
                {
                    if(laserTime > 0)
                    {
                        playerLaser.SetActive(true);
                        playerLaserFlash.SetActive(true);
                    }

                    else
                    {
                        playerLaser.SetActive(false);
                        playerLaserFlash.SetActive(false);
                    }

                    if(laserTime > -15f)
                    {
                        laserTime -= 2f;
                    }
                }

                else
                {
                    playerLaser.SetActive(false);
                    playerLaserFlash.SetActive(false);
                }
            }

            else
            {
                playerLaser.SetActive(false);
                playerLaserFlash.SetActive(false);
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
            slowedR = false;
            cannonFR1 = false;
            cannonFR2 = false;
            cannonFR3 = false;
            missAct1 = false;
            missAct2 = false;
            missAct3 = false;
            laserStop = true;
            shieldAct = false;
            bombAct = false;
            FreezeOne = false;
            FreezeTwo = false;
            suppGunStat = false;
            suppCanStat = false;
            suppMissStat = false;
            missOn = false;
            canOn = false;
            laseSwitch = false;
            LSG.SetActive(false);
            RSG.SetActive(false);
            LSC.SetActive(false);
            RSC.SetActive(false);
            LSM.SetActive(false);
            RSM.SetActive(false);
            mrk3Shield.SetActive(false);
        }
    }

    void GellJet()
    {
        GJetPos = transform.position;
        GJetPos += new Vector2(0f, -4f);
        Instantiate(GJet, GJetPos, Quaternion.identity);
    }

    //Bullets
    void GellFire1()
    {
        PBullPos = transform.position;
        PBullPos += new Vector2(0f, 7.21f);
        Instantiate(PBull, PBullPos, Quaternion.identity);
    }

    void GellFire2Right()
    {
        PBull2RPos = transform.position;
        PBull2RPos += new Vector2(2.55f, 7.21f);
        Instantiate(PBull2R, PBull2RPos, Quaternion.identity);
    }

    void GellFire2Left()
    {
        PBull2LPos = transform.position;
        PBull2LPos += new Vector2(-2.55f, 7.21f);
        Instantiate(PBull2L, PBull2LPos, Quaternion.identity);
    }

    void GellFire3Right()
    {
        PBull3RPos = transform.position;
        PBull3RPos += new Vector2(2.55f, .5f);
        Instantiate(PBull3R, PBull3RPos, Quaternion.identity);
    }

    void GellFire3Left()
    {
        PBull3LPos = transform.position;
        PBull3LPos += new Vector2(-2.55f, .5f);
        Instantiate(PBull3L, PBull3LPos, Quaternion.identity);
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
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);
                G1Health -= 1;
                HBHit.SetActive(true);
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

        if (col.gameObject.tag == "Enem2") 
	    {
            if(G1Health > 0)
            {
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);
                G1Health -= 10;
                HBHit.SetActive(true);
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

        if (col.gameObject.tag == "Enem3") 
	    {
            if(G1Health > 0)
            {
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);;
                G1Health -= 20;
                HBHit.SetActive(true);
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

        if (col.gameObject.tag == "Enem4") 
	    {
            if(G1Health > 0)
            {
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);;
                G1Health -= 40;
                HBHit.SetActive(true);
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
            playerHealthUpSound.Play();
            GetComponent<Animator> ().SetBool ("isFlashing", true);
            Invoke("offHit", .4f);
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
            playerHealthUpSound.Play();
            GetComponent<Animator> ().SetBool ("isFlashing", true);
            Invoke("offHit", .4f);
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
            playerHealthUpSound.Play();
            GetComponent<Animator> ().SetBool ("isFlashing", true);
            Invoke("offHit", .4f);
            if(G1Health > 399)
            {
                G1Health += 100;
            }

            else
            {
                G1Health = 500;
            }
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "Enem1":
            if(G1Health > 0)
            {
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);
                G1Health -= 1;
                HBHit.SetActive(true);
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
            break;

            case "Enem2":
            if(G1Health > 0)
            {
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);
                G1Health -= 10;
                HBHit.SetActive(true);
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
            break;

            case "Enem3":
            if(G1Health > 0)
            {
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);
                G1Health -= 20;
                HBHit.SetActive(true);
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
            break;

            case "Enem4":
            if(G1Health > 0)
            {
                mrk3Hit.SetActive(true);
                Invoke("offHit", .4f);
                G1Health -= 40;
                HBHit.SetActive(true);
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
            G1Health -= 25001;
            MarkThreeLvlMan.instance.LvlDone();
            break;

            case "MissTrig":
            playerHealthUpSound.Play();
            missOn = true;
            GetComponent<Animator> ().SetBool ("isFlashing", true);
            if(missAct1 == false)
            {
                if(missAct2 == false)
                {
                    missChng1.SetActive(true);
                    missAct1 = true;
                    missAct2 = false;
                    missAct3 = false;
                }

                else
                {
                    missChng3.SetActive(true);
                    missAct1 = false;
                    missAct2 = false;
                    missAct3 = true;
                }
            }

            else if(missAct1 == true)
            {
                if(missAct2 == false)
                {
                    missChng2.SetActive(true);
                    missAct1 = false;
                    missAct2 = true;
                    missAct3 = false;
                }

            }

            Invoke("offHit", .4f);
            break;

            case "CanTrig":
            playerHealthUpSound.Play();
            canOn = true;
            GetComponent<Animator> ().SetBool ("isFlashing", true);
            if(cannonFR1 == false)
            {
                if(cannonFR2 == false)
                {
                    canChng1.SetActive(true);
                    cannonFR1 = true;
                    cannonFR2 = false;
                    cannonFR3 = false;
                }

                else
                {
                    canChng3.SetActive(true);
                    cannonFR1 = false;
                    cannonFR2 = false;
                    cannonFR3 = true;
                }
            }

            else if(cannonFR1 == true)
            {
                if(cannonFR2 == false)
                {
                    canChng2.SetActive(true);
                    cannonFR1 = false;
                    cannonFR2 = true;
                    cannonFR3 = false;
                }
            }

            Invoke("offHit", .4f);
            break;

            case "LaserTrig":
            laseSwitch = true;
            playerHealthUpSound.Play();
            laserChng.SetActive(true);
            laserStop = false;
            Invoke("offHit", .4f);
            break;

            case "ShieldTrig":
            playerHealthUpSound.Play();
            shield1 = true;
            shieldChng1.SetActive(true);
            mrk3Shield.SetActive(true);
            Invoke("closeShield", 10f);
            Invoke("offHit", .4f);
            break;

            case "SupportTrig":
            playerHealthUpSound.Play();
            suppGunStat = true;
            suppChng1.SetActive(true);
            LSG.SetActive(true);
            RSG.SetActive(true);
            LSC.SetActive(false);
            RSC.SetActive(false);
            LSM.SetActive(false);
            RSM.SetActive(false);
            Invoke("offHit", .4f);
            break;

            case "SupportTrigTwo":
            playerHealthUpSound.Play();
            suppCanStat = true;
            suppChng2.SetActive(true);
            LSC.SetActive(true);
            RSC.SetActive(true);
            LSG.SetActive(false);
            RSG.SetActive(false);
            LSM.SetActive(false);
            RSM.SetActive(false);
            Invoke("offHit", .4f);
            break;

            case "SupportTrigThree":
            playerHealthUpSound.Play();
            suppMissStat = true;
            suppChng3.SetActive(true);
            LSM.SetActive(true);
            RSM.SetActive(true);
            LSC.SetActive(false);
            RSC.SetActive(false);
            LSG.SetActive(false);
            RSG.SetActive(false);
            Invoke("offHit", .4f);
            break;

            case "DeathWaveTrig":
            playerHealthUpSound.Play();
            dWaveChng.SetActive(true);
            DWall();
            DWaveOn = true;
            Invoke("DWaveOff", 10f);
            Invoke("offHit", .4f);
            break;

            case "FreezeTrigOne":
            playerHealthUpSound.Play();
            frzChng1.SetActive(true);
            FWall();
            FreezeOne = true;
            Invoke("offFreeze2", 10f);
            Invoke("offHit", .4f);
            break;

            case "FreezeTrigTwo":
            playerHealthUpSound.Play();
            frzChng2.SetActive(true);
            FreezeTwo = true;
            FreezeTwo = true;
            Invoke("offFreeze2", 10f);
            Invoke("offHit", .4f);
            break;

            case "BombTrig":
            playerHealthUpSound.Play();
            bombChng.SetActive(true);
            bombAct = true;
            Invoke("offHit", .4f);
            break;

            case "PlayerSlower":
            if(slowedR == false)
            {
                slowedR = true;
                upDownSpeed = 15;
                LRSpeed = 10;
                missFireRate = 3f;
                missFireRate2 = 1.5f;
                missFireRate3 = .8f;
                canFireRate3 = .8f;
                canFireRate = 1.5f;
                canFireRate2 = 1f;
                bullFireRate = .8f;
                Invoke("ReactRates", 5f);
            }
            break;
        }
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

    void offHit()
    {
        HBHit.SetActive(false);
        PlayerAVI.SetBool("isHit", false);
        mrk3Hit.SetActive(false);
        canChng1.SetActive(false);
        canChng2.SetActive(false);
        canChng3.SetActive(false);
        missChng1.SetActive(false);
        missChng2.SetActive(false);
        missChng3.SetActive(false);
        laserChng.SetActive(false);
        frzChng1.SetActive(false);
        frzChng2.SetActive(false);
        shieldChng1.SetActive(false);
        bombChng.SetActive(false);
        suppChng1.SetActive(false);
        suppChng2.SetActive(false);
        suppChng3.SetActive(false);
        dWaveChng.SetActive(false);
        GetComponent<Animator> ().SetBool ("isFlashing", false);
    }

    void ReactRates()
    {
        slowedR = false;
        upDownSpeed = 35;
        LRSpeed = 25;
        missFireRate = 1f;
        missFireRate2 = .5f;
        missFireRate3 = .2f;
        canFireRate3 = .1f;
        canFireRate = .5f;
        canFireRate2 = .2f;
        bullFireRate = .1f;
    }

    void closeShield()
    {
        mrk3Shield.SetActive(false);
        shield1 = false;
    }

    void deactLase()
    {
        laserStop = true;
    }

    void reactLaser()
    {
        laserStop = false;
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
