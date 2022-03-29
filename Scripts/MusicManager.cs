using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static AudioSource acoustics;
    public static AudioClip wizardIce;
    public static AudioClip wizardFlash;
    public static AudioClip wizardThunder;
    public static AudioClip doorOpen;
    public static AudioClip doorClose;
    public static AudioClip enemyAttack;
    public static AudioClip enemyTakedamage;
    public static AudioClip pickBot;
    public static AudioClip pickCoin;


    // Start is called before the first frame update
    void Start()
    {
        acoustics = GetComponent<AudioSource>();
        wizardIce = Resources.Load<AudioClip>("wizardIce");
        wizardFlash = Resources.Load<AudioClip>("wizardFlash");
        wizardThunder = Resources.Load<AudioClip>("wizardThunder");
        doorClose = Resources.Load<AudioClip>("doorClose");
        doorOpen = Resources.Load<AudioClip>("doorOpen");
        enemyAttack = Resources.Load<AudioClip>("enemyAttack");
        enemyTakedamage = Resources.Load<AudioClip>("enemyTakedamage");
        pickBot = Resources.Load<AudioClip>("pickBot");
        pickCoin = Resources.Load<AudioClip>("pickCoin");

        
    }

    public static void WizardIce()
    {
        acoustics.PlayOneShot(wizardIce);
    }
    public static void WizardFlash()
    {
        acoustics.PlayOneShot(wizardFlash);
    }
    public static void WizardThunder()
    {
        acoustics.PlayOneShot(wizardThunder);
    }
    public static void DoorOpen()
    {
        acoustics.PlayOneShot(doorOpen);
    }
    public static void DoorClose()
    {
        acoustics.PlayOneShot(doorClose);
    }

    public static void EnemyAttack()
    {
        acoustics.PlayOneShot(enemyAttack);
    }
    public static void EnemyTakeDamage()
    {
        acoustics.PlayOneShot(enemyTakedamage);
    }
    public static void PickBot()
    {
        acoustics.PlayOneShot(pickBot);
    }
    public static void PickCoin()
    {
        acoustics.PlayOneShot(pickCoin);
    }


}
