using System.Collections;
using System.Text;
using UnityEngine;

//目录
//1.键盘/摇杆发送人物状态
//2.根据人物状态改变行为(附带优化后的平滑触发)

public class O_walk : MonoBehaviour
{
    /*
    //人物中心点到上下左右触发碰撞的坐标
    int coll_left = 40;
    int coll_right = 40;
    int coll_up = 30;
    int coll_down = 40;
    */

    //人物中心点到上下左右触发碰撞的坐标
    int coll_left = 32;
    int coll_right = 32;
    int coll_up = 20;
    int coll_down = 36;
    //偏移触发量，滑度
    //左右不可以大于32
    int pianyi_left = 30;
    int pianyi_right = 30;
    int pianyi_up = 16;
    int pianyi_down = 30;
    //偏移触发量，滑度
    //变量-记录上一次的坐标
    private Vector3[] vec_main_camera = new Vector3[4];
    //变量-摇杆的大小直径/背景半径的平方/背景坐标
    private float control_bg_x;
    private float control_bg_length2;
    private Vector3 vec_control;
    //符号-平台
    private int flag_unity;
    //临时变量
    private Vector3 vec;
    private int flag_pushdown = 0;
    private string str_ani;

    //发送
    byte[] send = new byte[6];
    private bool recv_true = true;

    //播放帧数(为了防止报文粘连，进行快进播放)
    private int frame = 1;
    private int frame_now = 0;
    //时间片占用帧数  延迟 = 1秒时间/时间片
    private int delay = 8;
    //接收数据分析
    //private int num_send = 0;
    private int control = 0;

    void Start()
    {
        /******************************初始化*******************************/
        //判断平台
#if UNITY_EDITOR
        flag_unity = 1;
#elif UNITY_ANDROID
        flag_unity = 2;
#endif
        control_bg_x = global.gameobject_control_bg.GetComponent<CircleCollider2D>().bounds.size.x;
        vec_control = global.gameobject_control_bg.transform.localPosition;
        control_bg_length2 = (control_bg_x / 2) * (control_bg_x / 2);
        for (int i = 0; i < 4; i++)
        {
            if (global.human[i].user_id == "")
            {
                vec.Set(10000, 10000, 0);
                global.human[i].gameobject_human.transform.localPosition = vec;
            }
            else
            {
                vec.Set(global.human[i].x, global.human[i].y, 0);
                global.human[i].gameobject_human.transform.localPosition = vec;
            }
        }
    }

    


    private void Update()
    {
        if (global.gameover == 0)
        {
            if ((global.flag_online) && (global.client.server_socket.Connected))
            {
                if (flag_unity == 1)
                {
                    //电脑控制用键盘吧
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        global.human[global.my_num].human_stat = 1;
                    }
                    else if (Input.GetKey(KeyCode.DownArrow))
                    {
                        global.human[global.my_num].human_stat = 2;
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        global.human[global.my_num].human_stat = 3;
                    }
                    else if (Input.GetKey(KeyCode.RightArrow))
                    {
                        global.human[global.my_num].human_stat = 4;
                    }
                    else
                    {
                        global.human[global.my_num].human_stat = 0;
                    }
                }
                //手机按键触发
                if ((flag_unity == 2) && (Input.touchCount == 1))
                {
                    /***************************初始化触屏******************************/
                    Vector3 vec_touch = Input.GetTouch(0).position;
                    float x = vec_touch.x;
                    float y = vec_touch.y;
                    //把玩家点击的坐标转换成实际Local的坐标
                    float local_x = (x - 640 * global.rate_x) / global.rate_x;
                    float local_y = (y - 360 * global.rate_y) / global.rate_y;
                    /***************************手指刚按下******************************/
                    if (Input.touches[0].phase == TouchPhase.Began)
                    {
                        /***************************改变摇杆的坐标**************************/
                        //判断摇杆是否在范围内
                        //用 (按键坐标-摇杆坐标) 的平方
                        float length2_tmp = (local_x - vec_control.x) * (local_x - vec_control.x) + (local_y - vec_control.y) * (local_y - vec_control.y);
                        //取半径的平方预其比较
                        if (length2_tmp < control_bg_length2)
                        {
                            //改变位置
                            vec.Set(local_x, local_y, 0);
                            global.gameobject_control_joy.transform.localPosition = vec;
                            /**********************通过tan角度判断位置***************************/
                            //计算tan角度
                            float tan = (local_y - vec_control.y) / (local_x - vec_control.x);
                            if ((local_x - vec_control.x) > 0)
                            {

                                if ((tan > -1) && (tan < 1))
                                {
                                    global.human[global.my_num].human_stat = 4;
                                }
                                else if (tan > 1)
                                {
                                    global.human[global.my_num].human_stat = 1;
                                }
                                else if (tan < -1)
                                {
                                    global.human[global.my_num].human_stat = 2;
                                }
                            }
                            else if ((local_x - vec_control.x) < 0)
                            {
                                if ((tan > -1) && (tan < 1))
                                {
                                    global.human[global.my_num].human_stat = 3;
                                }
                                else if (tan > 1)
                                {
                                    global.human[global.my_num].human_stat = 2;
                                }
                                else if (tan < -1)
                                {
                                    global.human[global.my_num].human_stat = 1;
                                }
                            }
                            else if ((local_x - vec_control.x) == 0)
                            {
                                if ((local_y - vec_control.y) > 0)
                                {
                                    global.human[global.my_num].human_stat = 1;
                                }
                                else if ((local_y - vec_control.y) < 0)
                                {
                                    global.human[global.my_num].human_stat = 2;
                                }
                                else
                                {
                                    global.human[global.my_num].human_stat = 0;
                                }
                            }
                            //通知手指滑动可以触发
                            flag_pushdown = 1;
                        }
                    }
                    /***************************手指滑动**********************************/
                    if ((flag_pushdown == 1) && (Input.touches[0].phase == TouchPhase.Moved))
                    {
                        /***************************改变摇杆的坐标**************************/
                        float length2_tmp = (local_x - vec_control.x) * (local_x - vec_control.x) + (local_y - vec_control.y) * (local_y - vec_control.y);
                        //改变位置，滑动的时候，手可能会滑动到外面去，所以要通过外面做摇杆的映射
                        //这样处理的好处是摇杆不会出边界，而随着手动
                        //计算tan角度
                        float tan = (local_y - vec_control.y) / (local_x - vec_control.x);
                        if (length2_tmp < control_bg_length2)
                        {
                            vec.Set(local_x, local_y, 0);
                            global.gameobject_control_joy.transform.localPosition = vec;
                        }
                        /**********************通过tan角度判断位置***************************/
                        if ((local_x - vec_control.x) > 0)
                        {
                            if ((tan > -1) && (tan < 1))
                            {
                                global.human[global.my_num].human_stat = 4;
                            }
                            else if (tan > 1)
                            {
                                global.human[global.my_num].human_stat = 1;
                            }
                            else if (tan < -1)
                            {
                                global.human[global.my_num].human_stat = 2;
                            }
                        }
                        else if ((local_x - vec_control.x) < 0)
                        {
                            if ((tan > -1) && (tan < 1))
                            {
                                global.human[global.my_num].human_stat = 3;
                            }
                            else if (tan > 1)
                            {
                                global.human[global.my_num].human_stat = 2;
                            }
                            else if (tan < -1)
                            {
                                global.human[global.my_num].human_stat = 1;
                            }
                        }
                        else if ((local_x - vec_control.x) == 0)
                        {
                            if ((local_y - vec_control.y) > 0)
                            {
                                global.human[global.my_num].human_stat = 1;
                            }
                            else if ((local_y - vec_control.y) < 0)
                            {
                                global.human[global.my_num].human_stat = 2;
                            }
                            else
                            {
                                global.human[global.my_num].human_stat = 0;
                            }
                        }
                    }
                    /************************按键结束********************************/
                    if (Input.touches[0].phase == TouchPhase.Ended)
                    {
                        //要把摇杆放回去
                        global.gameobject_control_joy.transform.localPosition = vec_control;
                        flag_pushdown = 0;
                        //人物动画停止
                        global.human[global.my_num].human_stat = 0;
                    }
                }

                if (control == delay)
                {
                    control = 0;

                    //2.发送

                    global.human[global.my_num].online_human_stat[0] = global.human[global.my_num].human_stat;
                    send[0] = (byte)'[';
                    send[1] = (byte)'#';
                    send[2] = (byte)'G';
                    send[3] = (byte)'|';
                    send[4] = (byte)global.send_ascii[global.ascii_button][global.human[global.my_num].online_human_stat[0]];
                    send[5] = (byte)']';
                    //发送完了就置为0
                    global.ascii_button = 0;

                    global.client.server_socket.Send(send);

                    //3.同步接收
                    byte[] buffer_1 = new byte[1024];
                    //byte[] buffer_2 = new byte[1024];
                    global.client.server_socket.Blocking = false;
                    frame = 0;
                    frame_now = 0;
                    try
                    {
                        int len = global.client.server_socket.Receive(buffer_1);
                        if(len > 1022)
                        {
                            Application.Quit();
                        }

                        if ((buffer_1[0] == 91) && (buffer_1[len - 1] == 93))
                        {
                            //[#I|1100][#I|1100]
                            global.recv_buff = Encoding.UTF8.GetString(buffer_1);
                            string[] array = global.recv_buff.Split(']');
                            for (int i = 0; i < array.Length; i++)
                            {
                                string[] tmp_array = array[i].Split('|');
                                if (tmp_array[0] == "[#I")
                                {
                                    frame++;
                                    char[] chr_buff = tmp_array[1].ToCharArray();
                                    for (int y = 0; y < 4; y++)
                                    {
                                        if ((chr_buff[y] - 1) / 5 == 1)
                                        {
                                            put_boom(y);
                                        }
                                        global.human[y].online_human_stat[frame - 1] = (chr_buff[y] - 1) % 5;
                                        //print("玩家" + y + "的第" + frame + "帧为" + global.human[y].online_human_stat[frame - 1]);
                                    }
                                }

                                //[#P|1|1|0|0|2|38|15|23|14]
                                else if (tmp_array[0] == "[#P")
                                {
                                    frame++;
                                    for (int y = 0; y < 4; y++)
                                    {
                                        //print(int.Parse(tmp_array[1 + y]) - 1);
                                        if ((int.Parse(tmp_array[1 + y]) - 1) / 5 == 1)
                                        {
                                            put_boom(y);
                                        }
                                        global.human[y].online_human_stat[frame - 1] = (int.Parse(tmp_array[1 + y]) - 1) % 5;
                                    }
                                    //print("要摆放物品的位置为:" + array[5] + "," + array[6] + "," + array[7] + "," + array[8] + "," + array[9]);


                                    int food_x = int.Parse(tmp_array[5]) % 8 + 10;
                                    int food_y = int.Parse(tmp_array[5]) / 8 + 10;
                                    //print(food_x + "," + food_y);
                                    if (global.G_map[food_x][food_y].boom == false)
                                    {
                                        if (global.G_map[food_x][food_y].wall)
                                        {
                                            if (global.G_map[food_x][food_y].wall_destory)
                                            {
                                                vec.Set(food_x * 68 + 34, -food_y * 64 + 32, 0);
                                                GameObject gameobject_boom_copy = Instantiate(global.gameobject_food_boom);
                                                gameobject_boom_copy.transform.SetParent(global.lay_food.gameObject.transform, true);
                                                gameobject_boom_copy.name = global.gameobject_food_boom.name;
                                                gameobject_boom_copy.transform.localPosition = vec;
                                                //print("炸弹放在" + vec);
                                            }
                                        }
                                        else
                                        {
                                            vec.Set(food_x * 68 + 34, -food_y * 64 + 32, 0);
                                            GameObject gameobject_boom_copy = Instantiate(global.gameobject_food_boom);
                                            gameobject_boom_copy.transform.SetParent(global.lay_food.gameObject.transform, true);
                                            gameobject_boom_copy.name = global.gameobject_food_boom.name;
                                            gameobject_boom_copy.transform.localPosition = vec;
                                            //print("炸弹放在" + vec);
                                        }
                                    }

                                    food_x = int.Parse(tmp_array[6]) % 8 + 10;
                                    food_y = int.Parse(tmp_array[6]) / 8 + 10;
                                    //print(food_x + "," + food_y);
                                    if (global.G_map[food_x][food_y].boom == false)
                                    {
                                        if (global.G_map[food_x][food_y].wall)
                                        {
                                            if (global.G_map[food_x][food_y].wall_destory)
                                            {
                                                vec.Set(food_x * 68 + 34, -food_y * 64 + 32, 0);
                                                GameObject gameobject_pow_copy = Instantiate(global.gameobject_food_pow);
                                                gameobject_pow_copy.transform.SetParent(global.lay_food.gameObject.transform, true);
                                                gameobject_pow_copy.name = global.gameobject_food_pow.name;
                                                gameobject_pow_copy.transform.localPosition = vec;
                                                //print("威力放在" + vec);
                                            }
                                        }
                                        else
                                        {
                                            vec.Set(food_x * 68 + 34, -food_y * 64 + 32, 0);
                                            GameObject gameobject_pow_copy = Instantiate(global.gameobject_food_pow);
                                            gameobject_pow_copy.transform.SetParent(global.lay_food.gameObject.transform, true);
                                            gameobject_pow_copy.name = global.gameobject_food_pow.name;
                                            gameobject_pow_copy.transform.localPosition = vec;
                                            //print("威力放在" + vec);
                                        }
                                    }

                                    food_x = int.Parse(tmp_array[7]) % 8 + 10;
                                    food_y = int.Parse(tmp_array[7]) / 8 + 10;
                                    //print(food_x + "," + food_y);
                                    if (global.G_map[food_x][food_y].boom == false)
                                    {
                                        if (global.G_map[food_x][food_y].wall)
                                        {
                                            if (global.G_map[food_x][food_y].wall_destory)
                                            {
                                                vec.Set(food_x * 68 + 34, -food_y * 64 + 32, 0);
                                                GameObject gameobject_len_copy = Instantiate(global.gameobject_food_len);
                                                gameobject_len_copy.transform.SetParent(global.lay_food.gameObject.transform, true);
                                                gameobject_len_copy.name = global.gameobject_food_len.name;
                                                gameobject_len_copy.transform.localPosition = vec;
                                                //print("长度放在" + vec);
                                            }
                                        }
                                        else
                                        {
                                            vec.Set(food_x * 68 + 34, -food_y * 64 + 32, 0);
                                            GameObject gameobject_len_copy = Instantiate(global.gameobject_food_len);
                                            gameobject_len_copy.transform.SetParent(global.lay_food.gameObject.transform, true);
                                            gameobject_len_copy.name = global.gameobject_food_len.name;
                                            gameobject_len_copy.transform.localPosition = vec;
                                            //print("长度放在" + vec);
                                        }
                                    }

                                    food_x = int.Parse(tmp_array[8]) % 8 + 10;
                                    food_y = int.Parse(tmp_array[8]) / 8 + 10;
                                    //print(food_x + "," + food_y);
                                    if (global.G_map[food_x][food_y].boom == false)
                                    {
                                        if (global.G_map[food_x][food_y].wall)
                                        {
                                            if (global.G_map[food_x][food_y].wall_destory)
                                            {
                                                vec.Set(food_x * 68 + 34, -food_y * 64 + 32, 0);
                                                GameObject gameobject_speed_copy = Instantiate(global.gameobject_food_speed);
                                                gameobject_speed_copy.transform.SetParent(global.lay_food.gameObject.transform, true);
                                                gameobject_speed_copy.name = global.gameobject_food_speed.name;
                                                gameobject_speed_copy.transform.localPosition = vec;
                                                //print("速度放在" + vec);
                                            }
                                        }
                                        else
                                        {
                                            vec.Set(food_x * 68 + 34, -food_y * 64 + 32, 0);
                                            GameObject gameobject_speed_copy = Instantiate(global.gameobject_food_speed);
                                            gameobject_speed_copy.transform.SetParent(global.lay_food.gameObject.transform, true);
                                            gameobject_speed_copy.name = global.gameobject_food_speed.name;
                                            gameobject_speed_copy.transform.localPosition = vec;
                                            //print("速度放在" + vec);
                                        }

                                    }
                                    /*
                                    //最后一个坐标摆放不好的东西,比如陷阱
                                    vec.Set(int.Parse(array[9]) / 8, int.Parse(array[9]) % 8, 0);
                                    print("炸弹放在" + vec);
                                    */
                                }
                                else if (tmp_array[0] == "[#D")
                                {
                                    for (int j = 0; j < 4; j++)
                                    {
                                        print(global.human[j].user_id + "==" + tmp_array[1]);
                                        if (global.human[j].user_id == tmp_array[1])
                                        {
                                            print("走的人是第" + j + "位");
                                            global.human[j].life_now = 0;
                                        }
                                    }
                                }
                                //[#KILL]玩家退出房间
                                else if (tmp_array[0] == "#KILL")
                                {
                                    print("强制退出游戏");
                                    Application.Quit();
                                }
                            }
                        }
                        recv_true = true;
                    }
                    catch (System.Exception)
                    {
                        recv_true = false;
                    }
                    
                }

                if (recv_true)
                {
                    //4.播放所有人动画
                    if(frame > 1)
                    {
                        print("现在播放帧数为" + frame);
                    }

                    for (int f = 0; f < frame; f++)
                    {
                        //print(frame_now);
                        for (int i = 0; i < 4; i++)
                        {
                            //播放动画
                            if (global.human[i].online_human_stat[frame_now/ delay] != -1)
                            {
                                str_ani = "human_" + global.human[i].user_role_num + "_" + global.human[i].online_human_stat[frame_now / delay];
                                global.human[i].ani_human.Play(str_ani);
                            }

                            vec_main_camera[i] = global.human[i].gameobject_human.transform.localPosition;

                            if (global.human[i].online_human_stat[frame_now / delay] == 1)
                            {
                                //判断面前有没有箱子或者炸弹，计算人物偏移后的位置是否有物品
                                int x = (int)global.human[i].gameobject_human.transform.position.x / global.length_x;
                                int y = (int)((-global.human[i].gameobject_human.transform.position.y - coll_up) / global.length_y + 1);
                                if (global.G_map[x][y].wall == true)
                                {
                                    //前方有墙壁，不能走了
                                }
                                else
                                {
                                    //无墙壁先判断是否到墙根
                                    int y_now = (int)((-global.human[i].gameobject_human.transform.position.y) / global.length_y + 1);
                                    if (y_now == y)
                                    {
                                        //没有到墙根，正常行走
                                        vec_main_camera[i].Set(vec_main_camera[i].x, vec_main_camera[i].y + global.human[i].speed, vec_main_camera[i].z);
                                        global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                    }
                                    else
                                    {
                                        if (global.G_map[x][y].boom == true)
                                        {
                                            ;
                                        }
                                        else
                                        {
                                            //到墙根了，判断两边前有没有墙壁
                                            int x_left = (int)((global.human[i].gameobject_human.transform.position.x - pianyi_left) / global.length_x);
                                            int x_right = (int)((global.human[i].gameobject_human.transform.position.x + pianyi_right) / global.length_x);
                                            if (global.G_map[x_left][y].wall == true)
                                            {
                                                //左手边有墙壁，要往右走
                                                vec_main_camera[i].Set(vec_main_camera[i].x + global.human[i].speed, vec_main_camera[i].y, vec_main_camera[i].z);
                                                global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                            }
                                            else if (global.G_map[x_right][y].wall == true)
                                            {
                                                //右手边有墙壁，要往左走
                                                vec_main_camera[i].Set(vec_main_camera[i].x - global.human[i].speed, vec_main_camera[i].y, vec_main_camera[i].z);
                                                global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                            }
                                            else
                                            {
                                                //没有到墙根，正常行走
                                                vec_main_camera[i].Set(vec_main_camera[i].x, vec_main_camera[i].y + global.human[i].speed, vec_main_camera[i].z);
                                                global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                            }
                                        }
                                    }
                                }
                            }
                            else if (global.human[i].online_human_stat[frame_now / delay] == 2)
                            {
                                //判断面前有没有箱子或者炸弹，计算人物偏移后的位置是否有物品
                                int x = (int)global.human[i].gameobject_human.transform.position.x / global.length_x;
                                int y = (int)((-global.human[i].gameobject_human.transform.position.y + coll_down) / global.length_y + 1);
                                if (global.G_map[x][y].wall == true)
                                {
                                    //前方有墙壁，不能走了
                                }
                                else
                                {
                                    //无墙壁先判断是否到墙根
                                    int y_now = (int)((-global.human[i].gameobject_human.transform.position.y) / global.length_y + 1);
                                    if (y_now == y)
                                    {
                                        //没有到墙根，正常行走
                                        vec_main_camera[i].Set(vec_main_camera[i].x, vec_main_camera[i].y - global.human[i].speed, vec_main_camera[i].z);
                                        global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                    }
                                    else
                                    {
                                        if (global.G_map[x][y].boom == true)
                                        {
                                            ;
                                        }
                                        else
                                        {
                                            //到墙根了，判断两边前有没有墙壁
                                            int x_left = (int)((global.human[i].gameobject_human.transform.position.x - pianyi_left) / global.length_x);
                                            int x_right = (int)((global.human[i].gameobject_human.transform.position.x + pianyi_right) / global.length_x);
                                            if (global.G_map[x_left][y].wall == true)
                                            {
                                                //左手边有墙壁，要往右走
                                                vec_main_camera[i].Set(vec_main_camera[i].x + global.human[i].speed, vec_main_camera[i].y, vec_main_camera[i].z);
                                                global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                            }
                                            else if (global.G_map[x_right][y].wall == true)
                                            {
                                                //左手边有墙壁，要往右走
                                                vec_main_camera[i].Set(vec_main_camera[i].x - global.human[i].speed, vec_main_camera[i].y, vec_main_camera[i].z);
                                                global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                            }
                                            else
                                            {
                                                //左右都没有墙壁，正常走
                                                vec_main_camera[i].Set(vec_main_camera[i].x, vec_main_camera[i].y - global.human[i].speed, vec_main_camera[i].z);
                                                global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                            }
                                        }
                                    }
                                }
                            }
                            else if (global.human[i].online_human_stat[frame_now / delay] == 3)
                            {
                                //判断面前有没有箱子或者炸弹，计算人物偏移后的位置是否有物品
                                int x = (int)((global.human[i].gameobject_human.transform.position.x - coll_left) / global.length_x);
                                //y向下偏移11个坐标，因为人物中心点不在正中心
                                int y = (int)((-global.human[i].gameobject_human.transform.position.y + 11) / global.length_y + 1);
                                if (global.G_map[x][y].wall == true)
                                {
                                    //前方有墙壁，不能走了
                                }
                                else
                                {

                                    //无墙壁先判断是否到墙根
                                    int x_now = (int)(global.human[i].gameobject_human.transform.position.x / global.length_x);
                                    if (x_now == x)
                                    {
                                        //没有到墙根，正常行走
                                        vec_main_camera[i].Set(vec_main_camera[i].x - global.human[i].speed, vec_main_camera[i].y, vec_main_camera[i].z);
                                        global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                    }
                                    else
                                    {
                                        if (global.G_map[x][y].boom == true)
                                        {
                                            ;
                                        }
                                        else
                                        {
                                            int y_up = (int)((-global.human[i].gameobject_human.transform.position.y - pianyi_up) / global.length_y + 1);
                                            int y_down = (int)((-global.human[i].gameobject_human.transform.position.y + pianyi_down) / global.length_y + 1);
                                            if (global.G_map[x][y_up].wall == true)
                                            {
                                                //上边有墙壁，要往下走
                                                vec_main_camera[i].Set(vec_main_camera[i].x, vec_main_camera[i].y - global.human[i].speed, vec_main_camera[i].z);
                                                global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                            }
                                            else if (global.G_map[x][y_down].wall == true)
                                            {
                                                vec_main_camera[i].Set(vec_main_camera[i].x, vec_main_camera[i].y + global.human[i].speed, vec_main_camera[i].z);
                                                global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                            }
                                            else
                                            {
                                                //上下都没有墙壁，正常走
                                                vec_main_camera[i].Set(vec_main_camera[i].x - global.human[i].speed, vec_main_camera[i].y, vec_main_camera[i].z);
                                                global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                            }
                                        }
                                    }
                                }
                            }
                            else if (global.human[i].online_human_stat[frame_now / delay] == 4)
                            {
                                //判断面前有没有箱子或者炸弹，计算人物偏移后的位置是否有物品
                                int x = (int)((global.human[i].gameobject_human.transform.position.x + coll_right) / global.length_x);
                                //y向下偏移11个坐标，因为人物中心点不在正中心
                                int y = (int)((-global.human[i].gameobject_human.transform.position.y + 11) / global.length_y + 1);
                                if (global.G_map[x][y].wall == true)
                                {
                                    //前方有墙壁，不能走了
                                }
                                else
                                {
                                    //无墙壁先判断是否到墙根
                                    int x_now = (int)(global.human[i].gameobject_human.transform.position.x / global.length_x);
                                    if (x_now == x)
                                    {
                                        //没有到墙根，正常行走
                                        vec_main_camera[i].Set(vec_main_camera[i].x + global.human[i].speed, vec_main_camera[i].y, vec_main_camera[i].z);
                                        global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                    }
                                    else
                                    {
                                        if (global.G_map[x][y].boom == true)
                                        {
                                            ;
                                        }
                                        else
                                        {
                                            int y_up = (int)((-global.human[i].gameobject_human.transform.position.y - pianyi_up) / global.length_y + 1);
                                            int y_down = (int)((-global.human[i].gameobject_human.transform.position.y + pianyi_down) / global.length_y + 1);
                                            if (global.G_map[x][y_up].wall == true)
                                            {
                                                //上边有墙壁，要往下走
                                                vec_main_camera[i].Set(vec_main_camera[i].x, vec_main_camera[i].y - global.human[i].speed, vec_main_camera[i].z);
                                                global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                            }
                                            else if (global.G_map[x][y_down].wall == true)
                                            {
                                                //下边有墙壁，要往上走
                                                vec_main_camera[i].Set(vec_main_camera[i].x, vec_main_camera[i].y + global.human[i].speed, vec_main_camera[i].z);
                                                global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                            }
                                            else
                                            {
                                                //上下都没有墙壁，正常走
                                                vec_main_camera[i].Set(vec_main_camera[i].x + global.human[i].speed, vec_main_camera[i].y, vec_main_camera[i].z);
                                                global.human[i].gameobject_human.transform.localPosition = vec_main_camera[i];
                                            }
                                        }
                                    }
                                }
                            }
                            if (i == global.my_num)
                            {
                                global.gameobject_maincamera.transform.localPosition = vec_main_camera[i];
                            }
                        }
                        frame_now++;
                    }
                }
                control++;
                print(control);
            }
        }
    }





    //炸弹

    public void push_put_boom()
    {
        global.ascii_button = 1;
    }

    //2.炸弹爆炸并产生火焰
    IEnumerator destory_boom(int num,string boom_name, int x, int y)
    {
        yield return new WaitForSeconds(global.human[num].human_time_boom);
        //销毁炸弹
        Destroy(GameObject.Find(boom_name));
        //将内存更新
        global.G_map[x][y].boom = false;
        //该人物共计摆放的炸弹数量减少一个
        global.human[num].num_boom_now--;

        /******************************生成火焰动画**************************/
        //实例化一个火焰对象，将克隆的内容存储进去
        global.Water water_copy = new global.Water();
        //初始化
        water_copy.flag_stop_up = false;
        water_copy.flag_stop_left = false;
        water_copy.flag_stop_right = false;
        water_copy.flag_stop_down = false;

        //生成火焰身体
        int int_tmp = 0;
        for (int_tmp = 0; int_tmp <= global.human[num].len; int_tmp++)
        {
            //上方向
            if (water_copy.flag_stop_up == false)
            {
                if (global.G_map[x][y - int_tmp].wall == true)
                {
                    //这个点有箱子，不能再继续生成火焰身体
                    water_copy.flag_stop_up = true;

                    //检查箱子是否能摧毁，如果能，就摧毁箱子
                    if (global.G_map[x][y - int_tmp].wall_destory == true)
                    {
                        water_copy.box_destory_list.Add(global.G_map[x][y - int_tmp].gameobject_destory_wall);

                        GameObject water_destory_copy = Instantiate(global.gameobject_water_destory);
                        water_destory_copy.transform.SetParent(global.lay_mid.gameObject.transform, true);
                        water_destory_copy.name = "water_destory_" + global.G_map[x][y - int_tmp].boom_num.ToString();
                        //设置位置
                        vec.Set(global.length_x * (x + 0.5f), -global.length_y * ((y - int_tmp) - 0.5f), 0);
                        water_destory_copy.transform.localPosition = vec;
                        //存储
                        water_copy.water_destory_list.Add(water_destory_copy);
                        Vector2 vec_2 = new Vector2();
                        vec_2.Set(x, y - int_tmp);
                        water_copy.box_position.Add(vec_2);
                    }
                }
                else
                {
                    if (int_tmp != global.human[num].len)
                    {
                        //如果这个方向有箱子且不可以被破坏
                        if ((global.G_map[x][y - (int_tmp + 1)].wall == true) && (global.G_map[x][y - (int_tmp + 1)].wall_destory == false))
                        {
                            //下一步会撞墙，生成火焰头而非身体
                            water_copy.water_up_head = Instantiate(global.gameobject_water_up_head);
                            water_copy.water_up_head.transform.SetParent(global.lay_water.gameObject.transform, true);
                            water_copy.water_up_head.name = "water_up_head_" + num.ToString();
                            //这个点没有火焰头，生成火焰头
                            vec.Set(global.length_x * (x + 0.5f), -global.length_y * (y - int_tmp - 0.5f), 0);
                            water_copy.water_up_head.transform.localPosition = vec;
                            //停止继续生成
                            water_copy.flag_stop_up = true;
                        }
                        else
                        {
                            //克隆一个火焰身体
                            GameObject water_up_body_copy = Instantiate(global.gameobject_water_up_body);
                            water_up_body_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                            water_up_body_copy.name = "water_up_body_" + num.ToString();
                            //这个点没有箱子，生成火焰身体
                            vec.Set(global.length_x * (x + 0.5f), -global.length_y * (y - int_tmp - 0.5f), 0);
                            water_up_body_copy.transform.localPosition = vec;
                            //存储
                            water_copy.water_up_body_list.Add(water_up_body_copy);
                        }
                    }
                }
            }

            //下方向
            if (water_copy.flag_stop_down == false)
            {
                if (global.G_map[x][y + int_tmp].wall == true)
                {
                    //这个点有箱子，不能再继续生成火焰身体
                    water_copy.flag_stop_down = true;

                    //检查箱子是否能摧毁，如果能，就摧毁箱子
                    if (global.G_map[x][y + int_tmp].wall_destory == true)
                    {
                        water_copy.box_destory_list.Add(global.G_map[x][y + int_tmp].gameobject_destory_wall);

                        GameObject water_destory_copy = Instantiate(global.gameobject_water_destory);
                        water_destory_copy.transform.SetParent(global.lay_mid.gameObject.transform, true);
                        water_destory_copy.name = "water_destory_" + num.ToString();
                        //设置位置
                        vec.Set(global.length_x * (x + 0.5f), -global.length_y * ((y + int_tmp) - 0.5f), 0);
                        water_destory_copy.transform.localPosition = vec;
                        //存储
                        water_copy.water_destory_list.Add(water_destory_copy);
                        Vector2 vec_2 = new Vector2();
                        vec_2.Set(x, y + int_tmp);
                        water_copy.box_position.Add(vec_2);
                    }
                }
                else
                {
                    if (int_tmp != global.human[num].len)
                    {
                        if ((global.G_map[x][y + (int_tmp + 1)].wall == true) && (global.G_map[x][y + (int_tmp + 1)].wall_destory == false))
                        {
                            //下一步会撞墙，生成火焰头而非身体
                            GameObject water_down_head_copy = Instantiate(global.gameobject_water_down_head);
                            water_down_head_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                            water_down_head_copy.name = "water_down_head_" + num.ToString();
                            //这个点没有火焰头，生成火焰头
                            vec.Set(global.length_x * (x + 0.5f), -global.length_y * (y + int_tmp - 0.5f), 0);
                            water_down_head_copy.transform.localPosition = vec;
                            //存储
                            water_copy.water_down_head = water_down_head_copy;
                            //停止继续生成
                            water_copy.flag_stop_down = true;

                        }
                        else
                        {
                            //克隆一个火焰身体
                            GameObject water_down_body_copy = Instantiate(global.gameobject_water_down_body);
                            water_down_body_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                            water_down_body_copy.name = "water_down_body_" + num.ToString();
                            //这个点没有箱子，生成火焰身体
                            vec.Set(global.length_x * (x + 0.5f), -global.length_y * (y + int_tmp - 0.5f), 0);
                            water_down_body_copy.transform.localPosition = vec;
                            //存储
                            water_copy.water_down_body_list.Add(water_down_body_copy);
                        }
                    }
                }
            }

            //左方向
            if (water_copy.flag_stop_left == false)
            {
                if (global.G_map[x - int_tmp][y].wall == true)
                {
                    //这个点有箱子，不能再继续生成火焰身体
                    water_copy.flag_stop_left = true;

                    //检查箱子是否能摧毁，如果能，就摧毁箱子
                    if (global.G_map[x - int_tmp][y].wall_destory == true)
                    {
                        water_copy.box_destory_list.Add(global.G_map[x - int_tmp][y].gameobject_destory_wall);

                        GameObject water_destory_copy = Instantiate(global.gameobject_water_destory);
                        water_destory_copy.transform.SetParent(global.lay_mid.gameObject.transform, true);
                        water_destory_copy.name = "water_destory_" + num.ToString();
                        //设置位置
                        vec.Set(global.length_x * (x - int_tmp + 0.5f), -global.length_y * (y - 0.5f), 0);
                        water_destory_copy.transform.localPosition = vec;
                        //存储
                        water_copy.water_destory_list.Add(water_destory_copy);
                        Vector2 vec_2 = new Vector2();
                        vec_2.Set(x - int_tmp, y);
                        water_copy.box_position.Add(vec_2);
                    }
                }
                else
                {
                    if (int_tmp != global.human[num].len)
                    {
                        if ((global.G_map[x - (int_tmp + 1)][y].wall == true) && (global.G_map[x - (int_tmp + 1)][y].wall_destory == false))
                        {
                            //下一步会撞墙，生成火焰头而非身体
                            GameObject water_left_head_copy = Instantiate(global.gameobject_water_left_head);
                            water_left_head_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                            water_left_head_copy.name = "water_left_head_" + num.ToString();
                            //这个点没有火焰头，生成火焰头
                            vec.Set(global.length_x * (x - int_tmp + 0.5f), -global.length_y * (y - 0.5f), 0);
                            water_left_head_copy.transform.localPosition = vec;
                            //存储
                            water_copy.water_left_head = water_left_head_copy;
                            //停止继续生成
                            water_copy.flag_stop_left = true;
                        }
                        else
                        {
                            //克隆一个火焰身体
                            GameObject water_left_body_copy = Instantiate(global.gameobject_water_left_body);
                            water_left_body_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                            water_left_body_copy.name = "water_left_body_" + num.ToString();
                            //这个点没有箱子，生成火焰身体
                            vec.Set(global.length_x * (x - int_tmp + 0.5f), -global.length_y * (y - 0.5f), 0);
                            water_left_body_copy.transform.localPosition = vec;
                            //存储
                            water_copy.water_left_body_list.Add(water_left_body_copy);

                        }
                    }
                }
            }

            //右方向
            if (water_copy.flag_stop_right == false)
            {
                if (global.G_map[x + int_tmp][y].wall == true)
                {
                    //这个点有箱子，不能再继续生成火焰身体
                    water_copy.flag_stop_right = true;
                    //检查箱子是否能摧毁，如果能，就摧毁箱子
                    if (global.G_map[x + int_tmp][y].wall_destory == true)
                    {

                        water_copy.box_destory_list.Add(global.G_map[x + int_tmp][y].gameobject_destory_wall);

                        GameObject water_destory_copy = Instantiate(global.gameobject_water_destory);
                        water_destory_copy.transform.SetParent(global.lay_mid.gameObject.transform, true);
                        water_destory_copy.name = "water_destory_" + num.ToString();
                        //设置位置
                        vec.Set(global.length_x * (x + int_tmp + 0.5f), -global.length_y * (y - 0.5f), 0);
                        water_destory_copy.transform.localPosition = vec;
                        //存储
                        water_copy.water_destory_list.Add(water_destory_copy);
                        Vector2 vec_2 = new Vector2();
                        vec_2.Set(x + int_tmp, y);
                        water_copy.box_position.Add(vec_2);
                    }
                }
                else
                {
                    if (int_tmp != global.human[num].len)
                    {
                        if ((global.G_map[x + (int_tmp + 1)][y].wall == true) && (global.G_map[x + (int_tmp + 1)][y].wall_destory == false))
                        {
                            //下一步会撞墙，生成火焰头而非身体
                            GameObject water_right_head_copy = Instantiate(global.gameobject_water_right_head);
                            water_right_head_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                            water_right_head_copy.name = "water_right_head_" + num.ToString();
                            //这个点没有火焰头，生成火焰头
                            vec.Set(global.length_x * (x + int_tmp + 0.5f), -global.length_y * (y - 0.5f), 0);
                            water_right_head_copy.transform.localPosition = vec;
                            //存储
                            water_copy.water_right_head = water_right_head_copy;
                            //停止继续生成
                            water_copy.flag_stop_right = true;
                        }
                        else
                        {
                            //克隆一个火焰身体
                            GameObject water_right_body_copy = Instantiate(global.gameobject_water_right_body);
                            water_right_body_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                            water_right_body_copy.name = "water_right_body_" + num.ToString();
                            //这个点没有箱子，生成火焰身体
                            vec.Set(global.length_x * (x + int_tmp + 0.5f), -global.length_y * (y - 0.5f), 0);
                            water_right_body_copy.transform.localPosition = vec;
                            //存储
                            water_copy.water_right_body_list.Add(water_right_body_copy);
                        }
                    }
                }
            }
        }

        //上
        if (water_copy.flag_stop_up == false)
        {
            int tmp_x = x;
            int tmp_y = y - (int_tmp - 1);
            //检查箱子是否能摧毁，如果能，就摧毁箱子
            if (global.G_map[tmp_x][tmp_y].wall_destory == true)
            {

                water_copy.box_destory_list.Add(global.G_map[tmp_x][tmp_y].gameobject_destory_wall);

                GameObject water_destory_copy = Instantiate(global.gameobject_water_destory);
                water_destory_copy.transform.SetParent(global.lay_mid.gameObject.transform, true);
                water_destory_copy.name = "water_destory_" + num.ToString();
                //设置位置
                vec.Set(global.length_x * (tmp_x + 0.5f), -global.length_y * (tmp_y - 0.5f), 0);
                water_destory_copy.transform.localPosition = vec;
                //存储
                water_copy.water_destory_list.Add(water_destory_copy);
                Vector2 vec_2 = new Vector2();
                vec_2.Set(tmp_x, tmp_y);
                water_copy.box_position.Add(vec_2);
            }
            else
            {
                GameObject water_up_head_copy = Instantiate(global.gameobject_water_up_head);
                water_up_head_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                water_up_head_copy.name = "water_up_head_" + num.ToString();
                //这个点没有火焰头，生成火焰头
                vec.Set(global.length_x * (tmp_x + 0.5f), -global.length_y * (tmp_y - 0.5f), 0);
                water_up_head_copy.transform.localPosition = vec;
                //存储
                water_copy.water_up_head = water_up_head_copy;
            }
        }
        //下
        if (water_copy.flag_stop_down == false)
        {
            int tmp_x = x;
            int tmp_y = y + (int_tmp - 1);
            //检查箱子是否能摧毁，如果能，就摧毁箱子
            if (global.G_map[tmp_x][tmp_y].wall_destory == true)
            {
                water_copy.box_destory_list.Add(global.G_map[tmp_x][tmp_y].gameobject_destory_wall);

                GameObject water_destory_copy = Instantiate(global.gameobject_water_destory);
                water_destory_copy.transform.SetParent(global.lay_mid.gameObject.transform, true);
                water_destory_copy.name = "water_destory_" + num.ToString();
                //设置位置
                vec.Set(global.length_x * (tmp_x + 0.5f), -global.length_y * (tmp_y - 0.5f), 0);
                water_destory_copy.transform.localPosition = vec;
                //存储
                water_copy.water_destory_list.Add(water_destory_copy);
                Vector2 vec_2 = new Vector2();
                vec_2.Set(tmp_x, tmp_y);
                water_copy.box_position.Add(vec_2);
            }
            else
            {
                GameObject water_down_head_copy = Instantiate(global.gameobject_water_down_head);
                water_down_head_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                water_down_head_copy.name = "water_down_head_" + num.ToString();

                vec.Set(global.length_x * (tmp_x + 0.5f), -global.length_y * (tmp_y - 0.5f), 0);
                water_down_head_copy.transform.localPosition = vec;
                //存储
                water_copy.water_down_head = water_down_head_copy;
            }
        }

        //左
        if (water_copy.flag_stop_left == false)
        {
            int tmp_x = x - (int_tmp - 1);
            int tmp_y = y;
            //检查箱子是否能摧毁，如果能，就摧毁箱子
            if (global.G_map[tmp_x][tmp_y].wall_destory == true)
            {
                water_copy.box_destory_list.Add(global.G_map[tmp_x][tmp_y].gameobject_destory_wall);

                GameObject water_destory_copy = Instantiate(global.gameobject_water_destory);
                water_destory_copy.transform.SetParent(global.lay_mid.gameObject.transform, true);
                water_destory_copy.name = "water_destory_" + num.ToString();
                //设置位置
                vec.Set(global.length_x * (tmp_x + 0.5f), -global.length_y * (tmp_y - 0.5f), 0);
                water_destory_copy.transform.localPosition = vec;
                //存储
                water_copy.water_destory_list.Add(water_destory_copy);
                Vector2 vec_2 = new Vector2();
                vec_2.Set(tmp_x, tmp_y);
                water_copy.box_position.Add(vec_2);
            }
            else
            {
                GameObject water_left_head_copy = Instantiate(global.gameobject_water_left_head);
                water_left_head_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                water_left_head_copy.name = "water_left_head_" + num.ToString();
                //这个点没有火焰头，生成火焰头
                vec.Set(global.length_x * (x - (int_tmp - 1) + 0.5f), -global.length_y * (y - 0.5f), 0);
                water_left_head_copy.transform.localPosition = vec;
                //存储
                water_copy.water_left_head = water_left_head_copy;
            }
        }

        //右
        if (water_copy.flag_stop_right == false)
        {

            int tmp_x = x + (int_tmp - 1);
            int tmp_y = y;
            //检查箱子是否能摧毁，如果能，就摧毁箱子
            if (global.G_map[tmp_x][tmp_y].wall_destory == true)
            {
                water_copy.box_destory_list.Add(global.G_map[tmp_x][tmp_y].gameobject_destory_wall);

                GameObject water_destory_copy = Instantiate(global.gameobject_water_destory);
                water_destory_copy.transform.SetParent(global.lay_mid.gameObject.transform, true);
                water_destory_copy.name = "water_destory_" + num.ToString();
                //设置位置
                vec.Set(global.length_x * (tmp_x + 0.5f), -global.length_y * (tmp_y - 0.5f), 0);
                water_destory_copy.transform.localPosition = vec;
                //存储
                water_copy.water_destory_list.Add(water_destory_copy);
                Vector2 vec_2 = new Vector2();
                vec_2.Set(tmp_x, tmp_y);
                water_copy.box_position.Add(vec_2);
            }
            else
            {
                GameObject water_right_head_copy = Instantiate(global.gameobject_water_right_head);
                water_right_head_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                water_right_head_copy.name = "water_right_head_" + num.ToString();
                //这个点没有火焰头，生成火焰头
                vec.Set(global.length_x * (x + (int_tmp - 1) + 0.5f), -global.length_y * (y - 0.5f), 0);
                water_right_head_copy.transform.localPosition = vec;
                //存储s
                water_copy.water_right_head = water_right_head_copy;
            }
        }
        /******************************插入火焰中心点************************/
        GameObject water_middle_copy = Instantiate(global.gameobject_water_middle);
        water_middle_copy.transform.SetParent(global.lay_mid.gameObject.transform, true);
        water_middle_copy.name = "water_middle_body_" + num.ToString();
        vec.Set(global.length_x * (x + 0.5f), -global.length_y * (y - 0.5f), 0);
        water_middle_copy.transform.localPosition = vec;

        //存储
        water_copy.water_middle = water_middle_copy;
        /******************************存储火焰动画**************************/
        //存储炸弹编号
        water_copy.num_boom = global.G_map[x][y].boom_num;
        //存储炸弹坐标
        water_copy.x = x;
        water_copy.y = y;
        //威力读取
        water_copy.len = global.human[num].len;

        /*****************************销毁协程*********************************/
        StartCoroutine(destory_water(water_copy));
    }

    //协程，0.5秒以后销毁
    IEnumerator destory_water(global.Water water)
    {
        yield return new WaitForSeconds(global.time_water);
        //销毁事件
        GameObject.Destroy(water.water_up_head);
        GameObject.Destroy(water.water_down_head);
        GameObject.Destroy(water.water_left_head);
        GameObject.Destroy(water.water_right_head);

        for (int i = 0; i < water.water_up_body_list.Count; i++)
        {
            GameObject.Destroy(water.water_up_body_list[i]);
        }
        for (int i = 0; i < water.water_down_body_list.Count; i++)
        {
            GameObject.Destroy(water.water_down_body_list[i]);
        }
        for (int i = 0; i < water.water_left_body_list.Count; i++)
        {
            GameObject.Destroy(water.water_left_body_list[i]);
        }
        for (int i = 0; i < water.water_right_body_list.Count; i++)
        {
            GameObject.Destroy(water.water_right_body_list[i]);
        }
        //摧毁可以破坏的箱子
        for (int i = 0; i < water.box_destory_list.Count; i++)
        {
            //print("摧毁" + (int)water.box_position[i].x + "," + (int)water.box_position[i].y);
            GameObject.Destroy(water.box_destory_list[i]);
            //更新地图
            global.G_map[(int)water.box_position[i].x][(int)water.box_position[i].y].wall = false;
            global.G_map[(int)water.box_position[i].x][(int)water.box_position[i].y].wall_destory = false;
        }
        //摧毁动画
        for (int i = 0; i < water.water_destory_list.Count; i++)
        {
            GameObject.Destroy(water.water_destory_list[i]);
        }

        GameObject.Destroy(water.water_middle);

        //销毁对象
        water = null;

    }

    //1.按下炸弹(放炸弹的人物编号)
    public void put_boom(int num)
    {
        //游戏并没有结束
        //网络处于连接状态
        if (global.gameover == 0)
        {
            int x = (int)((global.human[num].gameobject_human.transform.localPosition.x) / global.length_x);
            //1,0 -> 1,-1 这个是计算坐标转换，如果要给玩家看的坐标，就不是这个了 要把Y写成正数即可
            int y = (int)((global.human[num].gameobject_human.transform.localPosition.y) / global.length_y) - 1;

            //检查这里是否有炸弹
            if (global.G_map[x][-y].boom == false)
            {
                //检查该人物的总炸弹数量是否达到最大值
                if (global.human[num].num_boom_now < global.human[num].num_boom)
                {
                    //克隆并放置炸弹
                    GameObject boom_copy = Instantiate(global.gameobject_boom);
                    //改位置
                    boom_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                    //改名
                    boom_copy.name = "boom_" + global.boom_number.ToString();
                    //放置
                    vec.Set(global.length_x * x, global.length_y * y, 0);
                    boom_copy.transform.localPosition = vec;
                    //存储到全局变量
                    global.G_map[x][-y].boom = true;
                    //炸弹编号数字增加
                    global.G_map[x][-y].boom_num = global.boom_number;
                    global.boom_number++;
                    //该人物共计摆放的炸弹数量增加
                    global.human[num].num_boom_now++;
                    StartCoroutine(destory_boom(num,boom_copy.name, x, -y));
                }
            }
        }
    }

    //1.键盘/摇杆发送人物状态
    void FixedUpdate()
    {
    }
}
