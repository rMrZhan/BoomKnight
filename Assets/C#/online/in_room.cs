using UnityEngine;
using UnityEngine.UI;

public class in_room : MonoBehaviour
{
    public Vector3[] vec_human_position = new Vector3[4];

    private Vector3 vec;
    private Text text_text_button_ready;
    private GameObject text_button_ready;
    private GameObject ob_image_leader;

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            //把属性都赋值
            global.human[i].gameobject_light = GameObject.Find("light_" + i.ToString());
            global.human[i].gameobject_human = GameObject.Find("human_" + i.ToString());
            global.human[i].gameobject_name = GameObject.Find("name_" + i.ToString());
            global.human[i].gameobject_image_ready = GameObject.Find("image_ready_" + i.ToString());
            global.human[i].gameobject_text_name = GameObject.Find("text_name_" + i.ToString());
            global.human[i].gameobject_name_sign = GameObject.Find("name_sign_" + i.ToString());

            global.human[i].ani_human = global.human[i].gameobject_human.GetComponent<Animator>();
            global.human[i].text_name = global.human[i].gameobject_text_name.GetComponent<Text>();
        }

        text_button_ready = GameObject.Find("text_button_ready");
        text_text_button_ready = text_button_ready.GetComponent<Text>();

        ob_image_leader = GameObject.Find("image_leader");

        lock (global.locker)
        {
            global.send_buff = "[#8|" + global.human[global.my_num].room_num + "]";
        }
    }

    //3.按下准备按钮
    public void push_down()
    {
        if (global.flag_online)
        {
            print(global.my_num);
            if (global.human[global.my_num].is_room_leader)
            {
                lock (global.locker)
                {
                    global.send_buff = "[#11|" + global.human[global.my_num].room_num + "|" + global.online_level + "]";
                }
            }
            else
            {
                lock (global.locker)
                {
                    global.send_buff = "[#10]";
                }
            }
        }
    }

    public void push_back()
    {
        if (global.flag_online)
        {
            lock (global.locker)
            {
                global.send_buff = "[#12]";
            }
        }
    }

    private void Update()
    {
        if(global.flag_buff12)
        {
            global.flag_buff12 = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("room_list");
        }

        if(global.flag_buff11)
        {
            global.flag_buff11 = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("online_game");
        }

        //4.准备按钮变换更新
        for (int i = 0; i < 4; i++)
        {
            //如果不存在这个人，就跳过
            if (global.human[i].user_id == "")
            {
                continue;
            }
            //如果是房主，将准备按钮修改为开始游戏
            if (global.human[i].is_room_leader == true)
            {
                if (i == global.my_num)
                {
                    text_text_button_ready.text = "开始";
                }
                continue;
            }
            //如果存在，就把准备按钮拉回来或者放上去
            if (global.human[i].is_ready == false)
            {
                vec.Set(1000, 1000, 0);
                if (i == global.my_num)
                {
                    text_text_button_ready.text = "准备";
                }
                global.human[i].gameobject_image_ready.transform.localPosition = vec;
            }
            else
            {
                if (i == global.my_num)
                {
                    text_text_button_ready.text = "取消准备";
                }
                vec.Set(global.human[i].vec_human.x, global.human[i].vec_human.y + 95, 0);
                global.human[i].gameobject_image_ready.transform.localPosition = vec;
            }
        }

        //4.光旋转动画
        for (int i = 0; i < 4; i++)
        {
            if (global.human[i].flag_light)
            {
                vec.Set(0, 0, 1);
                global.human[i].gameobject_light.transform.Rotate(vec);
            }
        }

        //5.更新房间内人物信息
        if (global.flag_buff8)
        {
            /*
            for (int i = 0; i < 4; i++)
            {
                //把属性都赋值
                global.human[i].gameobject_light = GameObject.Find("light_" + i.ToString());
                global.human[i].gameobject_human = GameObject.Find("human_" + i.ToString());
                global.human[i].gameobject_name = GameObject.Find("name_" + i.ToString());
                global.human[i].gameobject_image_ready = GameObject.Find("image_ready_" + i.ToString());
                global.human[i].gameobject_text_name = GameObject.Find("text_name_" + i.ToString());
                global.human[i].gameobject_name_sign = GameObject.Find("name_sign_" + i.ToString());

                global.human[i].ani_human = global.human[i].gameobject_human.GetComponent<Animator>();
                global.human[i].text_name = global.human[i].gameobject_text_name.GetComponent<Text>();
            }
            */
            //既然有人加入或退出，就要对客户端做出变化
            print("进来了");
            for (int i = 0; i < 4; i++)
            {
                //如果id是空，就是不存在
                if (global.human[i].user_id == "")
                {
                    print(i + "不在");
                    //隐藏人物元素
                    vec.Set(1000, 1000, 0);
                    global.human[i].gameobject_human.transform.localPosition = vec;
                    global.human[i].gameobject_image_ready.transform.localPosition = vec;
                    global.human[i].gameobject_name.transform.localPosition = vec;
                    //停止旋转动画
                    global.human[i].flag_light = false;
                    continue;
                }
                else
                {
                    global.human[i].flag_light = true;
                }
                //人物
                global.human[i].vec_human = vec_human_position[i];
                global.human[i].gameobject_human.transform.localPosition = vec_human_position[i];
                //名字
                vec.Set(global.human[i].vec_human.x, global.human[i].vec_human.y + 60, 0);
                global.human[i].gameobject_name.transform.localPosition = vec;

                //房主
                if (global.human[i].is_room_leader == true)
                {
                    vec.Set(global.human[i].vec_human.x, global.human[i].vec_human.y + 95, 0);
                    ob_image_leader.transform.localPosition = vec;
                    vec.Set(1000, 1000, 0);
                    global.human[i].gameobject_image_ready.transform.localPosition = vec;
                }
                else
                {
                    if (global.human[i].is_ready)
                    {
                        vec.Set(global.human[i].vec_human.x, global.human[i].vec_human.y + 100, 0);
                        global.human[i].gameobject_image_ready.transform.localPosition = vec;
                    }
                    else
                    {
                        vec.Set(1000, 1000, 0);
                        global.human[i].gameobject_image_ready.transform.localPosition = vec;
                    }
                }
                //旋转
                global.human[i].flag_light = true;
                //位置
                global.human[i].vec_human = global.human[i].gameobject_human.transform.localPosition;
                //昵称
                global.human[i].text_name.text = global.human[i].user_name;
                //动画
                string str_ani = "human_" + global.human[i].user_role_num.ToString() + "_0";
                global.human[i].ani_human.Play(str_ani);

                global.flag_buff8 = false;
            }
        }
    }
}