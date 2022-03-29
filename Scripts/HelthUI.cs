using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelthUI : MonoBehaviour
{
    public Text helthText;
    public static int helthCurrent;
    public static nint helthMax;
    private Image helthBar;

    // Start is called before the first frame update
    void Start()
    {
        helthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        helthBar.fillAmount = (float)helthCurrent/(float)helthMax;
        helthText.text = helthCurrent.ToString() + "/" + helthMax.ToString();
    }
}
