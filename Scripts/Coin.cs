using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            MusicManager.PickCoin();
            CoinUI.coinNumber += 1;
            Destroy(gameObject);
        }
    }
}
