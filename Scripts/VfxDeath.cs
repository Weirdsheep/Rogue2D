using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxDeath : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("death", 1f);
    }
    void death()
    {
        Destroy(gameObject);
    }
}
