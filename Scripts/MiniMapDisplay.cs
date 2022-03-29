using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapDisplay : MonoBehaviour
{
    public GameObject mapDisplay;
    // Start is called before the first frame update
    
    private void OnEnable()
    {
        mapDisplay.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag ("Player"))
            mapDisplay.SetActive(true);
    } 
}
