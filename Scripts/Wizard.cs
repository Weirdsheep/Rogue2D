using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Wizard : MonoBehaviour
{           
    [Header("角色属性")]     
    public int hp = 20;
    public int mp = 10;
    public SkillType skill;

    [Header("控制")]
    public Vector3 movePos =  new Vector3(0.5f, 0.5f, 0f);   //角色位置
    public float movespeed = 2f;     //移动速度
    public bool isAttack = true;
    public int moveDirection;
    private PlayInputAction control;
    public LayerMask whatStopMovement;  //碰撞检测图层
    public static int isMoveOver = 0;
    public GameObject ui;
    public Text score;
    Vector3 moveMent;   //获取键盘输入的水平和垂直的值
    Animator anim;  //动画控制器

    void Awake() 
    {
        control = new PlayInputAction();
        control.GamePlay.Attack.started += ctx => Attack();
        control.GamePlay.Skill.started += ctx => Skill();
        control.GamePlay.Flash.started += ctx => Flash();
        control.GamePlay.Pause.started += ctx => PauseGame();
    }


    // Start is called before the first frame update
    void Start()
    {
        HelthUI.helthMax = hp;
        HelthUI.helthCurrent = hp;
        MagicUI.magicMax = mp;
        MagicUI.magicCurrent = mp;
        anim = GetComponent<Animator>();
        moveDirection = 2;
        ui.SetActive(false);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MoveMent();
        MagicUI.magicCurrent = mp;
        HelthUI.helthCurrent = hp;
    }
    public void PauseGame()
    {
        SceneManager.LoadScene(0);
    }
    //受击
    public void TakeDamage(int damage)
    {
        hp -= damage;
        HelthUI.helthCurrent = hp;
        if(hp < 0)
        {
            hp = 0;
        }
        anim.SetTrigger("damage");
        if(hp <= 0)
        {
            anim.SetBool("death", true);
            Invoke("PlayerDeath", 3f);
        }
    }

    //死亡
    public void PlayerDeath()
    {
        Destroy(gameObject);
        score.text = score.text + CoinUI.coinNumber.ToString();
        ui.SetActive(true);
    }

    //Add more method

    //闪现
    public void Flash()
    {
        
        if(mp >= 1)
        {
            MusicManager.WizardFlash();
            switch(moveDirection)
            {
                case 1:
                if(!Physics2D.OverlapCircle(movePos + new Vector3(-2f, 0f, 0f), 0.2f, whatStopMovement))
                {
                    gameObject.SetActive(false);
                    movePos += new Vector3(-2f, 0f, 0f);
                }
                else
                {
                    return;
                }
                break;

                case 2:
                if(!Physics2D.OverlapCircle(movePos + new Vector3(2f, 0f, 0f), 0.2f, whatStopMovement))
                {
                    gameObject.SetActive(false);
                    movePos += new Vector3(2f, 0f, 0f);
                }
                else
                {
                    return;
                }
                break;

                case 3:
                if(!Physics2D.OverlapCircle(movePos + new Vector3(0f, -2f, 0f), 0.2f, whatStopMovement))
                {
                    gameObject.SetActive(false);
                    movePos += new Vector3(0f, -2f, 0f);
                }
                else
                {
                    return;
                }
                break;

                case 4:
                if(!Physics2D.OverlapCircle(movePos + new Vector3(0f, 2f, 0f), 0.2f, whatStopMovement))
                {
                    gameObject.SetActive(false);
                    movePos += new Vector3(0f, 2f, 0f);
                }
                else
                {
                    return;
                }
                break;
            }
            transform.position = movePos;
            Instantiate(skill.Smoke, transform.position, Quaternion.identity);
            Invoke("ReFlash", 0.5f);
            mp--;
        }
    }

    public void ReFlash()
    {
        gameObject.SetActive(true);
    }

    //技能
    public void Skill()
    {
        if(mp >=1)
        {
            MusicManager.WizardThunder();
            anim.SetTrigger("attack");
            Instantiate(skill.Thunder, transform.position + new Vector3(1f, 1f, 0f), Quaternion.identity);
            Instantiate(skill.Thunder, transform.position + new Vector3(-1f, 1f, 0f), Quaternion.identity);
            Instantiate(skill.Thunder, transform.position + new Vector3(1f, -1f, 0f), Quaternion.identity);
            Instantiate(skill.Thunder, transform.position + new Vector3(-1f, -1f, 0f), Quaternion.identity);
            Instantiate(skill.Thunder, transform.position + new Vector3(3f, 0f, 0f), Quaternion.identity);
            Instantiate(skill.Thunder, transform.position + new Vector3(-3f, 0f, 0f), Quaternion.identity);
            Instantiate(skill.Thunder, transform.position + new Vector3(0f, 3f, 0f), Quaternion.identity);
            Instantiate(skill.Thunder, transform.position + new Vector3(0f, -3f, 0f), Quaternion.identity);
            mp--;
        }
    }

    //攻击
    public void Attack()
    {
        if(isAttack == true)
        {
            anim.SetTrigger("attack");
            Invoke("AttackiDrection", 0.5f);
            isAttack = false;
        }
    }

    void AttackiDrection()
    {
        switch(moveDirection)
            {
                case 1:
                Instantiate(skill.Ice[2], transform.position, skill.Ice[2].transform.rotation);
                break;
                case 2:
                Instantiate(skill.Ice[3], transform.position, skill.Ice[3].transform.rotation);
                break;
                case 3:
                Instantiate(skill.Ice[1], transform.position, skill.Ice[1].transform.rotation);
                break;
                case 4:
                Instantiate(skill.Ice[0], transform.position, skill.Ice[0].transform.rotation);
                break;
            }
        MusicManager.WizardIce();
    }

    public void MoveMent()
    {
        //获取键盘输入
        moveMent.x = Input.GetAxisRaw("Horizontal");
        moveMent.y = Input.GetAxisRaw("Vertical");

        //同一时刻只允许在一个方向上移动
        if (moveMent.x != 0)
        {
            moveMent.y = 0f;
        }

        transform.position = Vector3.MoveTowards(transform.position, movePos, movespeed * Time.deltaTime);

        //判断是否在原地
        if(transform.position == movePos)
        {
            if(moveMent.x != 0)
            {
                transform.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1);
                if(!Physics2D.OverlapCircle(movePos + moveMent, 0.2f, whatStopMovement))
                {
                    movePos += moveMent;
                    isMoveOver ++;
                    isAttack = true;
                    if(transform.position.x > movePos.x)
                    {
                        moveDirection = 1;
                    }
                    else
                    {
                        moveDirection = 2;
                    }
                }
            }                       
            else if(moveMent.y != 0)
            {
                if(!Physics2D.OverlapCircle(movePos + moveMent, 0.2f, whatStopMovement))
                {
                    movePos += moveMent;
                    isMoveOver ++;
                    isAttack = true;
                    if(transform.position.y > movePos.y)
                    {
                        moveDirection = 3;
                    }
                    else
                    {
                        moveDirection = 4;
                    }
                }
            }
            //自身在原地未移动，设置动画为idle
            anim.SetBool("moving", false);
        } 
        else
        {
            //自身未停留在原地，设置动画为moving
            anim.SetBool("moving", true);
        }    
    }

    void OnEnable() 
    {
        control.GamePlay.Enable();
    }
    void OnDisable() 
    {
        control.GamePlay.Disable();
    }

    //显示碰撞检测范围
    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;

    //     Gizmos.DrawWireSphere(transform.position , 0.2f);
    // }
}

[System.Serializable]   //未添加时，系统无法识别下方的类
public class SkillType
{
    public GameObject Thunder,Smoke;
    public GameObject[] Ice;
}
