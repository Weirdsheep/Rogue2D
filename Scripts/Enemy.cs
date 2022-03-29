using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("基本属性")]
    public int hp = 4;
    public int damage = 4;
    public int attackiDrection = 1;
    public GameObject damagePoint;
    public List<GameObject> item = new List<GameObject>();
    private Rigidbody2D rb;
    public LayerMask layer;  //碰撞检测图层
    int x,y;    //移动偏移量
    Vector3 movePos;
    Animator anim;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        movePos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePos, 3f * Time.deltaTime);
    }

    public void Attack()
    {
       // if(x != 0)
       // {
            transform.localScale = new Vector3 (attackiDrection, 1, 1);
        //}
        
        anim.SetTrigger("attack");
        MusicManager.EnemyAttack();
        player.GetComponent<Wizard>().TakeDamage(damage);
    }

    public void Move()
    {
        Vector2 offset = player.transform.position - transform.position;
        if(offset.magnitude < 1.1)
        {
            Invoke("Attack",0.5f);
        }
        else
        {
            if(Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
            {
                if(offset.x > 0)
                {
                    x = 1;
                    attackiDrection = 1;
                }
                else
                {
                    x = -1;
                    attackiDrection = -1;
                }
                y = 0;
            }
            else
            {
                if(offset.y > 0)
                {
                    y = 1;
                }
                else
                {
                    y = -1;
                }
                x = 0;
            }
            if(!Physics2D.OverlapCircle(transform.position + new Vector3(x, y ,0f), 0.2f, layer))
        {
            movePos += new Vector3(x, y, 0);
        }
        }
        
            
    }
    
    public void Death()
    {
        Instantiate(item[Random.Range(0,item.Count)], transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        anim.SetTrigger("damage");
        MusicManager.EnemyTakeDamage();
        GameObject dp = Instantiate(damagePoint, transform.position ,Quaternion.identity) as GameObject;
        dp.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString();
        if(hp <= 0)
        {
            anim.SetBool("death",true);
            Invoke("Death",1f);
        }
    }
}
