using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public static int coinNumber = 0;
    public Text coin;
    // Start is called before the first frame update
    void Update() 
    {
        coin.text = coinNumber.ToString();
    }
}
