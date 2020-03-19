using System.Text;
using UnityEngine;

//1.启动发送-接收线程
//2.登录 #1|yhliu(账号)|123456(密码)
//3.根据服务端信息重新建立端口连接

public class pthread : MonoBehaviour
{
    Vector3 vec;

    public static void recv_message()
    {
        while (true)
        {
            int len = 0;
            byte[] buffer_1 = new byte[1024];
            byte[] buffer_2 = new byte[1024];

            len = 0;
            if (global.flag_close_recv_pthread)
            {
                global.flag_close_recv_pthread = false;
                print("退出接收线程");
                break;
            }

            global.client.server_socket.Blocking = true;
            len = global.client.server_socket.Receive(buffer_1);

            if (len == 0)
            {
                continue;
            }

            if (global.flag_online)
            {
                //清空已经发送的报文
                global.send_buff = "";
                global.flag_wait = false;
                print(buffer_1);
                print(buffer_1[0] + "," + buffer_1[len - 1]);
                if ((buffer_1[0] == 91) && (buffer_1[len - 1] == 93))
                {
                    int j = 0;
                    for (int i = 1; i < len - 1; i++)
                    {
                        buffer_2[j] = buffer_1[i];
                        j++;
                    }
                    buffer_2[j] = 0;
                    global.recv_buff = Encoding.UTF8.GetString(buffer_2);
                    print("接收到" + global.recv_buff);
                    global.flag_recv = true;

                    string[] array_buff = new string[1024];
                    array_buff = global.recv_buff.Split('|');
                    //#1|yhliu(账号)|123456(密码)
                    if (array_buff[0] == "#1")
                    {
                        if (array_buff[1] == "0")
                        {
                            global.flag_login_in = 1;
                            global.str_message = array_buff[2];
                            global.human[global.my_num] = new global.Human();
                        }
                        else
                        {
                            global.flag_login_in = -1;
                            global.str_message = array_buff[2];
                            global.flag_online_first = true;
                            global.client.server_socket.Close();
                        }
                    }
                    else if(array_buff[0] == "#0")
                    {
                        if (array_buff[1] == "0")
                        {
                            global.flag_sign_in = 1;
                            global.str_message = array_buff[2];
                            global.flag_online_first = true;
                            global.client.server_socket.Close();
                        }
                        else
                        {
                            global.flag_sign_in = -1;
                            global.str_message = array_buff[2];
                            global.flag_online_first = true;
                            global.client.server_socket.Close();
                            
                        }
                        break;
                    }
                    //#2|石头城|9|2|1240|300|1|9|1|1|3.20|1
                    else if (array_buff[0] == "#2")
                    {
                        update_persional();
                    }
                    //#6|||||||||||||||
                    else if (array_buff[0] == "#6")
                    {
                        update_list();
                    }
                    //#7|1(房间号)
                    else if (array_buff[0] == "#7")
                    {
                        create_room();
                    }
                    else if (array_buff[0] == "#8")
                    {
                        update_room();
                    }
                    else if (array_buff[0] == "#9")
                    {
                        insert_room();
                    }
                    //#11|1(地图号)
                    else if (array_buff[0] == "#11")
                    {
                        game_start();
                    }
                    else if (array_buff[0] == "#E")
                    {
                        error();
                    }
                    //#D|yhliu(玩家姓名)
                    else if (array_buff[0] == "#D")
                    {
                        delete_user(array_buff[1]);
                    }
                    //[#12]玩家退出房间
                    else if (array_buff[0] == "#12")
                    {
                        print("退出房间");
                        exit_room();
                    }
                    //[#KILL]玩家退出房间
                    else if (array_buff[0] == "#KILL")
                    {
                        print("强制退出游戏");
                        global.kill_game = true;
                    }
                }
            }
        }
    }


    public static void exit_room()
    {
        global.flag_buff12 = true;
    }

    public static void delete_user(string delete_id)
    {
        print("开始挪动");

        for (int i = 0; i < 4; i++)
        {
            print(global.human[i].user_id + "==" + delete_id);
            if (global.human[i].user_id == delete_id)
            {
                print("走的人是第" + i + "位");
                for (int j = i; j < 4; j++)
                {
                    //挪出空位要归零
                    if (global.human[j + 1].user_id == "")
                    {
                        print("挪出空位要归零" + (j - 1));
                        //global.human[j] = new global.Human();
                        global.human[j].user_id = "";
                        break;
                    }
                    //其他人向前挪动
                    else
                    {
                        
                        //如果是房主
                        if (i == 0)
                        {
                            print("房主走了");
                            global.human[1].is_ready = false;
                            global.human[1].is_room_leader = true;
                        }

                        print("其他人向前挪动" + j);
                        global.human[j] = global.human[j + 1];
                    }
                }
            }
            
            global.flag_buff8 = true;
        }
    }



    //1.启动发送-本来是线程，但是线程支持性很差
    public static void send_message()
    {
        while (true)
        {
            if (global.flag_online == false)
            {
                print("退出发送线程");
                break;
            }

            if (global.flag_close_send_pthread)
            {
                global.flag_close_send_pthread = false;
                print("退出发送线程");
                break;
            }

            if (global.send_buff == "")
            {
                continue;
            }

            if ((global.flag_online) && (global.client.server_socket.Connected))
            {
                global.client.server_socket.Send(Encoding.UTF8.GetBytes(global.send_buff));
                print("发送:" + global.send_buff);
                global.send_buff = "";
                //计算网络延迟
                global.flag_wait = true;
                global.time_wait = 0;
            }
        }
    }

    //#E
    public static void error()
    {
        string[] array_buff = new string[1024];
        array_buff = global.recv_buff.Split('|');

        global.str_message = array_buff[1];
        print("错误:" + global.str_message);
    }

    //#11
    public static void game_start()
    {
        string[] array_buff = new string[1024];
        array_buff = global.recv_buff.Split('|');

        if (array_buff[1] == "-1")
        {
            print(array_buff[2]);
        }
        else
        {
            global.online_level = int.Parse(array_buff[1]);
            global.flag_buff11 = true;
            //关闭接收线程，代表下一步就要关闭接收操作
            global.flag_close_recv_pthread = true;
        }
    }



    //#9
    public static void insert_room()
    {
        string[] array_buff = new string[1024];
        array_buff = global.recv_buff.Split('|');

        if (array_buff[1] == "-1")
        {
            print(array_buff[2]);
        }
        else
        {
            //更新人物位置，所以要交换信息
            if (int.Parse(array_buff[1]) != global.my_num)
            {
                global.human[int.Parse(array_buff[1])] = global.human[global.my_num];
                global.human[global.my_num] = new global.Human();
                global.my_num = int.Parse(array_buff[1]);
            }
            global.flag_buff9 = true;
        }
    }


    //#8
    public static void update_room()
    {
        string[] array_buff = new string[1024];
        array_buff = global.recv_buff.Split('|');

        if (array_buff[1] == "-1")
        {
            print(array_buff[2]);
        }
        else
        {
            int j = 1;
            for (int k = 0; k < 4; k++)
            {
                if (array_buff[j] != "")
                {
                    print(j + ":" + array_buff[j]);
                    global.human[k].user_id = array_buff[j]; j++;
                    global.human[k].user_name = array_buff[j]; j++;
                    global.human[k].user_level = int.Parse(array_buff[j]); j++;
                    global.human[k].user_role_num = int.Parse(array_buff[j]); j++;
                    if (array_buff[j] == "0")
                    {
                        global.human[k].is_room_leader = false;
                    }
                    else
                    {
                        global.human[k].is_room_leader = true;
                    }
                    j++;
                    if (array_buff[j] == "0")
                    {
                        global.human[k].is_ready = false;
                    }
                    else
                    {
                        global.human[k].is_ready = true;
                    }
                    j++;
                    //等级属性获取
                    global.human[k].human_life = int.Parse(array_buff[j]); j++;
                    global.human[k].human_pow = int.Parse(array_buff[j]); j++;
                    global.human[k].human_boom = int.Parse(array_buff[j]); j++;
                    global.human[k].human_speed = float.Parse(array_buff[j]); j++;
                    global.human[k].human_len = int.Parse(array_buff[j]); j++;
                }
            }

            global.flag_buff8 = true;
        }
    }

    //#7
    public static void create_room()
    {
        
        string[] array_buff = new string[1024];
        array_buff = global.recv_buff.Split('|');

        if (array_buff[1] != "-1")
        {
            //更新人物位置，所以要交换信息
            if (0 != global.my_num)
            {
                global.human[global.my_num] = global.human[global.my_num];
                global.human[global.my_num] = new global.Human();
                global.my_num = 0;
            }
            global.human[global.my_num].room_num = int.Parse(array_buff[1]);
            global.flag_buff7 = true;
        }
        else
        {
            print("创建房间失败！");
        }
    }

    public static void update_list()
    {
        string[] array_buff = new string[1024];
        array_buff = global.recv_buff.Split('|');
        int j = 1;
        for (int k = 0; k < 5; k++)
        {
            if (array_buff[j] == "")
            {
                global.room_list[k].is_open = false;
                j = j + 3;
            }
            else
            {
                global.room_list[k].is_open = true;
                global.room_list[k].room_num = int.Parse(array_buff[j]);j++;
                global.room_list[k].room_name = array_buff[j];j++;
                global.room_list[k].room_human_num = int.Parse(array_buff[j]);j++;
            }
        }
        global.flag_buff6 = true;
        print("更新房间");
    }
        

    public static void update_persional()
    {
        string[] array_buff = new string[1024];
        array_buff = global.recv_buff.Split('|');

        global.my_num = 0;

        int j = 1;
        //人物属性更新 update_persional|人物姓名|6|2|420|1100|6|1|1|3.20|1| 人物姓名|人物等级|人物金币|人物经验|已过关卡|等级生命|等级伤害|等级炸弹量|等级速度|等级火焰长度|
        //人物基本属性获取
        global.human[global.my_num].user_name = array_buff[j]; j++;
        global.human[global.my_num].user_level = int.Parse(array_buff[j]); j++;
        global.human[global.my_num].user_role_num = int.Parse(array_buff[j]); j++;
        global.human[global.my_num].user_gold = int.Parse(array_buff[j]); j++;
        global.human[global.my_num].user_exp = int.Parse(array_buff[j]); j++;
        global.human[global.my_num].user_stage = int.Parse(array_buff[j]); j++;
        //等级属性获取
        global.human[global.my_num].human_life = int.Parse(array_buff[j]); j++;
        global.human[global.my_num].human_pow = int.Parse(array_buff[j]); j++;
        global.human[global.my_num].human_boom = int.Parse(array_buff[j]); j++;
        global.human[global.my_num].human_speed = float.Parse(array_buff[j]); j++;
        global.human[global.my_num].human_len = int.Parse(array_buff[j]); j++;
        //标志位更新
        global.flag_buff2 = true;
        print("更新属性");
    }

}
