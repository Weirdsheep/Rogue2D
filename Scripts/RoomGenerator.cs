using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomGenerator : MonoBehaviour
{
    public enum Direction { up, down, left, right };    //设置探测方向
    public Direction direction;     //选择方向来进行“门”的探测

    [Header("房间信息")]
    public GameObject roomPrefab;   //获取房间预制体
    public int roomNumber;  //控制房间生成数量
    private Room endRoom; //存储终点房间
    private Room roomGet;   //获取房间
    //public GameObject bossRoom;

    [Header("位置控制")]
    public Transform generatorPoint;    //获取房间探测器（点）的位置
    public float xOffset;   //每个房间的横向距离
    public float yOffset;   //每个房间的纵向距离
    public LayerMask roomLayer; //获取房间的图层（用于检测碰撞）

    [Header("小地图")]
    private PlayInputAction control;
    // public float delayTime = 1f;     //计时间隔
    // private float restTime = 0f;    //计时器
    public GameObject miniMap;

    [Header("杂项")]
    public int maxStep; //距离初始房间的最远距离
    public List<Room> rooms = new List<Room>(); //存储所有房间
    public List<Room> farRooms = new List<Room>();  //存储最远房间
    public List<Room> lessFarRooms = new List<Room>();  //存储次远房间
    public List<Room> oneWayRooms = new List<Room>();   //存储只有单个门的房间
    public WallType wallType;   //房间类型

    [Header("控制")]  
    private Transform mapHolder;    //所有生成的房间实例放在其名下

     void Awake() 
    {
        control = new PlayInputAction();
        control.GamePlay.MiniMap.started += ctx => MiniMap();
    }

    // Start is called before the first frame update
    void Start()
    { 
        mapHolder = new GameObject("MapHolder").transform;
        //循环生成房间
        for (int i = 0; i < roomNumber; i++)
        {
            roomGet = Instantiate(roomPrefab, generatorPoint.position, Quaternion.identity).GetComponent<Room>();
            rooms.Add(roomGet);
            roomGet.transform.SetParent(mapHolder);

            //改变房间探测器的位置
            ChangePointPos();
        }

        //初始化终点房间
        endRoom = rooms[0];

        //遍历房间
        foreach (var room in rooms)
        {
            // //寻找终点房间
            // if(room.transform.position.sqrMagnitude > endRoom.transform.position.sqrMagnitude)  //判断当前房间与原点的距离
            // {
            //     //更新终点房间
            //     endRoom = room.gameObject;
            // }

            //设置房间数据
            SetupRoom(room,room.transform.position);
        }
        FindEndRoom();
        //BossRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
        // //输入任意键重新加载场景
        // if(Input.anyKeyDown)
        // {
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().name); //加载当前活跃的场景
        // }
    }

    //房间探测器
    public void ChangePointPos()
    {
        //用于判断当前位置是否存已经在房间
        bool IfCreated = false;
        do
        {
            direction = (Direction)Random.Range(0, 4);//随机选取方向

            //根据房间的横向和纵向距离在选取的方向上进行探测
            switch (direction)
            {
                case Direction.up:
                    generatorPoint.position += new Vector3(0, yOffset, 0);
                    break;
                case Direction.down:
                    generatorPoint.position += new Vector3(0, -yOffset, 0);
                    break;
                case Direction.left:
                    generatorPoint.position += new Vector3(-xOffset, 0, 0);
                    break;
                case Direction.right:
                    generatorPoint.position += new Vector3(xOffset, 0, 0);
                    break;
            }
            //判断当前位置是否已经存在房间
            IfCreated = IfPositionCreated();
        } while (IfCreated);

        //通过图层判断是否发生碰撞
        //while(Physics2D.OverlapCircle(generatorPoint.position, 0.2f, roomLayer))
    }

    //判断探测器的位置是否存在房间
    public bool IfPositionCreated()
    {
        foreach (Room G in rooms)
        {
            //判断探测器的位置是否与已有房间位置重合
            if (G.transform.position==generatorPoint.position)
            {
                return true;
            }
        }
        return false;
    }

    //控制房间的门是否显示
    public void SetupRoom(Room newRoom,Vector3 roomPosition)
    {
        //判断当前房间的四周是否存在其他房间（判断该方向上是否应该有门）
        newRoom.roomUp = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffset, 0), 0.2f, roomLayer);
        newRoom.roomDown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), 0.2f, roomLayer);
        newRoom.roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0), 0.2f, roomLayer);
        newRoom.roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0, 0), 0.2f, roomLayer);

        newRoom.UpdateRoom(xOffset,yOffset);//计算门的数量，并记录当前房间与初始房间的曼哈顿距离

        //在每个房间生成对应开口的墙壁
        switch (newRoom.doorNumber)
        {
            case 1:
                if (newRoom.roomUp)
                    Instantiate(wallType.singleUp, roomPosition, Quaternion.identity);
                if (newRoom.roomDown)
                    Instantiate(wallType.singleBottom, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft)
                    Instantiate(wallType.singleLeft, roomPosition, Quaternion.identity);
                if (newRoom.roomRight)
                    Instantiate(wallType.singleRight, roomPosition, Quaternion.identity);
                break;
            case 2:
                if (newRoom.roomLeft && newRoom.roomUp)
                    Instantiate(wallType.doubleLU, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomRight)
                    Instantiate(wallType.doubleLR, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomDown)
                    Instantiate(wallType.doubleLB, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomRight)
                    Instantiate(wallType.doubleUR, roomPosition, Quaternion.identity);
                if (newRoom.roomUp && newRoom.roomDown)
                    Instantiate(wallType.doubleUB, roomPosition, Quaternion.identity);
                if (newRoom.roomRight && newRoom.roomDown)
                    Instantiate(wallType.doubleRB, roomPosition, Quaternion.identity);
                break;
            case 3:
                if (newRoom.roomLeft && newRoom.roomUp && newRoom.roomRight)
                    Instantiate(wallType.tripleLUR, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomRight && newRoom.roomDown)
                    Instantiate(wallType.tripleLRB, roomPosition, Quaternion.identity);
                if (newRoom.roomDown && newRoom.roomUp && newRoom.roomRight)
                    Instantiate(wallType.tripleURB, roomPosition, Quaternion.identity);
                if (newRoom.roomLeft && newRoom.roomUp && newRoom.roomDown)
                    Instantiate(wallType.tripleLUB, roomPosition, Quaternion.identity);
                break;
            case 4:
                if (newRoom.roomLeft && newRoom.roomUp && newRoom.roomRight && newRoom.roomDown)
                    Instantiate(wallType.fourDoors, roomPosition, Quaternion.identity);
                break;
        }
    }

    //寻找距离最远的房间
    public void FindEndRoom()
    {
        //初始化最远距离
        maxStep = 0;

        //求取最远距离
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].stepToStart > maxStep)
                maxStep = rooms[i].stepToStart;
        }

        //获得最远房间和次远房间
        foreach (var room in rooms)
        {
            if (room.stepToStart == maxStep)
                farRooms.Add(room);
            if (room.stepToStart == maxStep - 1)
                lessFarRooms.Add(room);
        }

        for (int i = 0; i < farRooms.Count; i++)
        {
            if (farRooms[i].GetComponent<Room>().doorNumber == 1)
                oneWayRooms.Add(farRooms[i]);//最远房间里的单侧门加入
        }

        for (int i = 0; i < lessFarRooms.Count; i++)
        {
            if (lessFarRooms[i].GetComponent<Room>().doorNumber == 1)
                oneWayRooms.Add(lessFarRooms[i]);//第二远远房间里的单侧门加入
        }

        //存在单个门的房间时，从中随机选择一个房间作为最终房间
        if (oneWayRooms.Count != 0)
        {
            endRoom = oneWayRooms[Random.Range(0, oneWayRooms.Count)];
        }
        else
        {
            //否则，在最远房间内随机选择一个作为最终房间
            endRoom = farRooms[Random.Range(0, farRooms.Count)];
        }
    }

    //生成Boss房
    /*public void BossRoom()
    {
        generatorPoint.position = endRoom.transform.position;
        if(!endRoom.roomDown)
        {
            generatorPoint.position += new Vector3(0, -(yOffset), 0);
        }
        else if(!endRoom.roomUp)
        {
            generatorPoint.position += new Vector3(0, yOffset, 0);
        }
        else if(!endRoom.roomLeft)
        {
            generatorPoint.position += new Vector3(-(xOffset), 0, 0);
        }
        else if (!endRoom.roomRight)
        {
            generatorPoint.position += new Vector3(xOffset, 0, 0);
        }

        endRoom = Instantiate(bossRoom, generatorPoint.position, Quaternion.identity).GetComponent<Room>();
    }
    */

    public void MiniMap()
    {
        if(miniMap.activeSelf)
        {
            miniMap.SetActive(false);
        }
        else
        {
            miniMap.SetActive(true);
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

}

//用于存储15种开口的墙
[System.Serializable]   //未添加时，系统无法识别下方的类
public class WallType
{
    public GameObject singleLeft, singleRight, singleUp, singleBottom,
                      doubleLU, doubleLR, doubleLB, doubleUR, doubleUB, doubleRB,
                      tripleLUR, tripleLUB, tripleURB, tripleLRB,
                      fourDoors;
}
