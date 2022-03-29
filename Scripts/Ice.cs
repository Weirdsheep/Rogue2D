using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    public float speed;
    public int damage = 1;
    public float destroyDistance;
    public Animator anim;

    private Rigidbody2D rb2d;
    private Vector3 startPos;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = transform.right * speed;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if((transform.position - startPos).sqrMagnitude > destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    void death()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Enemy"))
        {
            anim.SetBool("ishit", true);
            rb2d.velocity = transform.right * 0.5f;
            other.GetComponent<Enemy>().TakeDamage(damage);
            Invoke("death", 1f);
        }
    }
}
