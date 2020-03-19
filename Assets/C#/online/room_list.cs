using UnityEngine;
using UnityEngine.UI;

public class room_list : MonoBehaviour
{
    private string[] array_buff;
    private string buff;
    //网络刷新间隔
    private int update = 60;
    public int update_len = 60;
    public Vector3[] vec_room = new Vector3[5];
    private GameObject text_create_room_name;
    private GameObject image_create_room;
    private Text text_text_create_room_name;
    private Vector3 vec;
    private GameObject[] ob_room = new GameObject[5];
    private GameObject[] ob_room_num = new GameObject[5];
    private GameObject[] ob_room_name = new GameObject[5];
    private GameObject[] ob_room_human = new GameObject[5];
    private Text[] text_room_num = new Text[5];
    private Text[] text_room_name = new Text[5];
    private Text[] text_room_human = new Text[5];
    private GameObject gameobject_text_gold;
    private Text text_text_gold;
    private GameObject text_page;
    private Text text_text_page;

    private void Awake()
    {
        //加载所有的元素
        for (int i = 0; i < 5; i++)
        {
            ob_room[i]       = GameObject.Find("room_" + i.ToString());
            ob_room_name[i]  = GameObject.Find("room_" + i.ToString() + "_name");
            ob_room_human[i] = GameObject.Find("room_" + i.ToString() + "_human");
            ob_room_num[i]   = GameObject.Find("room_" + i.ToString() + "_num");
            gameobject_text_gold = GameObject.Find("text_gold");
            text_text_gold = gameobject_text_gold.GetComponent<Text>();
            text_room_num[i]   = ob_room_num[i].GetComponent<Text>();
            text_room_name[i]  = ob_room_name[i].GetComponent<Text>();
            text_room_human[i] = ob_room_human[i].GetComponent<Text>();
        }
        //初始化房间列表
        for (int i = 0; i < 5; i++)
        {
            global.room_list[i] = new global.Room_list();
        }

        text_create_room_name = GameObject.Find("text_room_name");
        text_text_create_room_name = text_create_room_name.GetComponent<Text>();
        image_create_room = GameObject.Find("image_create_room");
        text_page = GameObject.Find("text_page");
        text_text_page = text_page.GetComponent<Text>();
    }
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            //如果房间不存在,移除它的图标
            if (global.room_list[i].is_open == false)
            {
                vec.Set(1000, 1000, 0);
                ob_room[i].transform.localPosition = vec;
            }
            //如果房间存在，那么要把它的图标放在正确的位置上，将其内容属性改变
            else
            {
                print(i + "," + vec_room[i]);
                ob_room[i].transform.localPosition = vec_room[i];

                text_room_num[i].text = global.room_list[i].room_num.ToString();
                text_room_name[i].text = global.room_list[i].room_name;
                text_room_human[i].text = global.room_list[i].room_human_num.ToString() + " / 4";
            }
        }
    }

    //按下加入房间
    public void push_insert_room()
    {
        //判断玩家按下哪个键
        string num_push = "";
        int k = 0;
        for (int i = 0; i < 5; i++)
        {
            num_push = "room_" + i + "_button";
            if (gameObject.name == num_push)
            {
                k = i;
                break;
            }
        }

        //发送报文
        if (global.flag_online)
        {
            //玩家更改属性，发送后标志为进入房间，失败后再还原即可
            global.human[global.my_num].is_room_leader = false;
            global.human[global.my_num].room_num = global.room_list[k].room_num;
            lock (global.locker)
            {
                global.send_buff += "[#9|" + global.room_list[k].room_num + "]";
            }
        }
    }

    public void push_create()
    {
        vec.Set(0, 0, 0);
        image_create_room.transform.localPosition = vec;
    }

    public void close_create()
    {
        vec.Set(1000, 1000, 0);
        image_create_room.transform.localPosition = vec;
        text_text_create_room_name.text = "";
    }
    public void push_down_page()
    {
        global.room_list_page++;
        text_text_page.text = global.room_list_page.ToString();
    }
    public void push_up_page()
    {
        if(global.room_list_page <= 1)
        {
            ;
        }
        else
        {
            global.room_list_page--;
            text_text_page.text = global.room_list_page.ToString();
        }
    }

    public void push_create_room()
    {
        if (global.flag_online)
        {
            if (text_text_create_room_name.text == "")
            {
                global.send_buff += "[#7|" + "和我一起游戏吧!" + "]";
                while(global.flag_buff7)
                {
                    break;
                }
            }
            else
            {
                global.send_buff += "[#7|" + text_text_create_room_name.text + "]";
            }
        }
    }
    void Update()
    {
        //4.创建房间成功
        if (global.flag_buff7)
        {
            global.flag_buff7 = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("in_room");
        }

        //5.加入房间成功
        if (global.flag_buff9)
        {
            global.flag_buff9 = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("in_room");
        }

        text_text_gold.text = global.human[global.my_num].user_gold.ToString();

        //5.每隔1秒刷新房间
        if (update == update_len)
        {
            update = 0;
            //#6|页码
            global.send_buff += "[#6|" + global.room_list_page + "]";

            if (global.flag_online)
            {
                for (int i = 0; i < 5; i++)
                {
                    //如果房间不存在,移除它的图标
                    if (global.room_list[i].is_open == false)
                    {
                        vec.Set(1000, 1000, 0);
                        ob_room[i].transform.localPosition = vec;
                    }
                    //如果房间存在，那么要把它的图标放在正确的位置上，将其内容属性改变
                    else
                    {
                        ob_room[i].transform.localPosition = vec_room[i];
                        text_room_num[i].text = global.room_list[i].room_num.ToString();
                        text_room_name[i].text = global.room_list[i].room_name;
                        text_room_human[i].text = global.room_list[i].room_human_num.ToString() + " / 2";
                    }
                }
            }
        }
        update++;
    }
}
