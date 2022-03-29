using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
public class Room : MonoBehaviour
{
    public GameObject doorUp,doorDown,doorLeft,doorRight;   //控制上下左右四个门
    public bool roomUp,roomDown,roomLeft,roomRight; //标识上下左右的房间是否存在

    [Header("门相关")]
    public Wall outWall;
    public int stepToStart;//距离初始点的网格距离
    public int doorNumber;//当前房间的门的数量/入口数量

    [Header("项目生成")]
    public CreateBox[] cb;
    public GameObject groundPrefab;


    
    // Start is called before the first frame update
    void Start()
    {
        //控制门的显示（该方向上存在其他房间则开启）
        doorDown.SetActive(false);
        doorUp.SetActive(false);
        doorLeft.SetActive(false);
        doorRight.SetActive(false);  
        cb = Instantiate(groundPrefab,transform.position,Quaternion.identity).GetComponentsInChildren<CreateBox>();  
    }

    // Update is called once per frame
    void Update()
    {
        IsOpenDoor();
    }

    public void IsOpenDoor()
    {  
        if(roomDown)
        {
            doorDown.SetActive(cb[0].isOpen);
        }
        if(roomUp)
        {
            doorUp.SetActive(cb[0].isOpen);
        }
        if(roomLeft)
        {
            doorLeft.SetActive(cb[0].isOpen);
        }
        if(roomRight)
        {
            doorRight.SetActive(cb[0].isOpen);
        }
    }

    public void UpdateRoom(float xOffset, float yOffset)
    {
        stepToStart = (int)(Mathf.Abs(transform.position.x / xOffset) + Mathf.Abs(transform.position.y / yOffset));

        if (roomUp)
            doorNumber++;
        if (roomDown)
            doorNumber++;
        if (roomLeft)
            doorNumber++;
        if (roomRight)
            doorNumber++;
        
    }

    //小地图摄像机移动触发条件
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraController.instance.ChangeTarget(transform);
            MiniMapCamera.instance.ChangeTarget(transform);
        }
    }
}

[System.Serializable] 
public class Wall
{
    public GameObject Corner_L,Corner_R,Bottom_L,Bottom_R;
    public GameObject[] Down,Up,Left,Right;
}

