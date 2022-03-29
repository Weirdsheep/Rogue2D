using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateBox : MonoBehaviour
{
    private Tilemap tileMap;    //取得自身tilemap组件
    private List<Vector3> tilePosList = new List<Vector3>();    //存放当前tilemap中所有方块的坐标
    public int boxMax;  //障碍物总数
    public int enemyMax; //敌人总数量
    public int currentEnemy = 0;    //当前敌人数量
    bool isEnemyMove = false;   //激活房间内的敌人
    public Wizard player;
    public List<GameObject> itemPrefabList = new List<GameObject>();   //存放障碍物预制体
    public List<GameObject> itemList = new List<GameObject>();  //存放已生成的障碍物
    public List<GameObject> enemyPrefabList = new List<GameObject>();   //存放敌人预制体
    public List<GameObject> enemyList = new List<GameObject>();   //存放已生成的敌人
    public bool isOpen{get;set;}
    




    // Start is called before the first frame update]
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Wizard>();
        isOpen = false;
        tileMap = GetComponent<Tilemap>();
        boxMax  = Random.Range(10,20);
        enemyMax = Random.Range(1,3);
        GetPos();
        CreateItem();
        CreateEnemy();
    }

    void Update()
    {
        if(isOpen)
        {
            EnemyIsLive();
        }
        
        if(isEnemyMove)
        {
            if(Wizard.isMoveOver == 2)
            {
                Invoke("EnemyMove",0.5f);
                Wizard.isMoveOver = 0;
            }
            else if(Wizard.isMoveOver > 2)
            {
                Wizard.isMoveOver = 0;
            }
        }
    }

    public void EnemyMove()
    {
        foreach(var enemy in enemyList)
        {
            if(enemy != null)
            {
                enemy.GetComponent<Enemy>().Move();
            }    
        }    
    }

    void EnemyIsLive()
    {
        foreach(var enemy in enemyList)
        {
            if(enemy == null)
            {
                currentEnemy++;
            }
        }
        if(enemyMax - currentEnemy <= 0)
        {
            isOpen = false;
            MusicManager.DoorOpen();
        }
        else
        {
            currentEnemy = 0;
        }
    }

    public void GetPos()
    {
        tileMap = GetComponent<Tilemap>();
        
        Vector3Int tmOrg = tileMap.origin;  //tilemap原点
        Vector3Int tmSize = tileMap.size;   //tilemap远端点

        for(int x = tmOrg.x; x < tmSize.x; x++)
        {
            for (int y = tmOrg.y; y < tmSize.y; y++)
            {
                if (tileMap.GetTile(new Vector3Int (x, y, 0)) != null)  //判断当前位置是否存在方块
                {
                    tilePosList.Add(tileMap.GetCellCenterWorld(new Vector3Int(x, y, 0)));   //取得当前方块的坐标
                }
            }
        }
    }

    public void CreateItem()
    {
        while(boxMax > 0)
        {
            //随机选择方块位置
            Vector3 boxPos = tilePosList[Random.Range(0,tilePosList.Count)];  
            //随机选择障碍物
            GameObject Box = itemPrefabList[Random.Range(0,itemPrefabList.Count)];

            GameObject item =  Instantiate(Box, boxPos, Quaternion.identity);    //生成障碍物
            item.transform.SetParent(this.transform);
            itemList.Add(item);

            tilePosList.Remove(boxPos);

            boxMax--;
        }
    }
    public void CreateEnemy()
    {
            for(int i = 1; i <= enemyMax; i++)
            {
                //随机选择方块位置
                Vector3 enemyPos = tilePosList[Random.Range(0,tilePosList.Count)];  
                //随机选择障碍物
                GameObject enemyPrefab = enemyPrefabList[Random.Range(0,enemyPrefabList.Count)];

                GameObject enemy =  Instantiate(enemyPrefab, enemyPos, Quaternion.identity);    //生成障碍物
                enemy.transform.SetParent(this.transform);
                enemyList.Add(enemy);

                tilePosList.Remove(enemyPos);
            }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isEnemyMove = true;  
            if(enemyMax - currentEnemy > 0)
            {
                isOpen = true;
                MusicManager.DoorClose();
            }    
        }
    }
    void OnTriggerStay2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            if(enemyMax - currentEnemy > 0)
            {
                isOpen = true;
            }   
        }
    }


    void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            isEnemyMove = false;
        }
    }
}
