using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBot : MonoBehaviour
{
    public GameObject prefab;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Death()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            MusicManager.PickBot();
            if(player.GetComponent<Wizard>().hp < 20)
            {
                 player.GetComponent<Wizard>().hp += 1;
            }
           
            Death();
        }
    }
}
