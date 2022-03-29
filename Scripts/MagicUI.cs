using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicUI : MonoBehaviour
{
     public Text magicText;
    public static int magicCurrent;
    public static nint magicMax;
    private Image magicBar;
    // Start is called before the first frame update
    void Start()
    {
        magicBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        magicBar.fillAmount = (float)magicCurrent/(float)magicMax;
        magicText.text = magicCurrent.ToString() + "/" + magicMax.ToString();
    }
}
