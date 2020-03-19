using System.Threading;
using UnityEngine;
using UnityEngine.UI;

//目录
//1.初始化游戏
//2.更新数据
//3.游戏胜利
//4.游戏失败

public class O_start : MonoBehaviour
{
    //临时变量
    private int success_flag = 0;
    Vector3 vec;

    //1.初始化游戏
    private void Awake()
    {
        //地面元素初始化
        for (int i = 0; i < 1024; i++)
        {
            global.G_map[i] = new global.G_MAP[1024];
            for (int j = 0; j < 1024; j++)
            {
                global.G_map[i][j] = new global.G_MAP();
            }
        }
        //游戏内精灵初始化-镜头/摇杆
        global.gameobject_maincamera = GameObject.Find("MainCamera");
        global.gameobject_control_joy = GameObject.Find("control_joy");
        global.gameobject_control_bg = GameObject.Find("control");
        //游戏内精灵初始化-火焰
        global.gameobject_boom = GameObject.Find("boom");
        global.gameobject_water_destory = GameObject.Find("water_destory");
        global.gameobject_water_middle = GameObject.Find("water_middle_body");
        global.gameobject_water_up_head = GameObject.Find("water_up_head");
        global.gameobject_water_up_body = GameObject.Find("water_up_body");
        global.gameobject_water_down_head = GameObject.Find("water_down_head");
        global.gameobject_water_down_body = GameObject.Find("water_down_body");
        global.gameobject_water_left_head = GameObject.Find("water_left_head");
        global.gameobject_water_left_body = GameObject.Find("water_left_body");
        global.gameobject_water_right_head = GameObject.Find("water_right_head");
        global.gameobject_water_right_body = GameObject.Find("water_right_body");
        //游戏内精灵初始化-怪物血条
        global.gameobject_monster_life_full = GameObject.Find("monster_life_full");
        global.gameobject_monster_life_empty = GameObject.Find("monster_life_empty");
        //游戏内精灵初始化-层级
        global.lay_monster = GameObject.Find("lay_monster");
        global.lay_water = GameObject.Find("lay_water");
        global.lay_mid = GameObject.Find("lay_mid");
        global.lay_food = GameObject.Find("lay_food");
        //游戏内精灵初始化-物品
        global.gameobject_food_boom = GameObject.Find("food_boom");
        global.gameobject_food_len = GameObject.Find("food_len");
        global.gameobject_food_pow = GameObject.Find("food_pow");
        global.gameobject_food_speed = GameObject.Find("food_speed");
        global.gameobject_food_life = GameObject.Find("food_life");

        //游戏内精灵初始化-人物
        for(int i=0;i<4;i++)
        {
            global.human[i].gameobject_human = GameObject.Find("human_" + i);
            global.human[i].ani_human = global.human[i].gameobject_human.GetComponent<Animator>();

            //游戏内属性初始化
            global.human[i].num_boom = global.human[i].human_boom;
            global.human[i].life_now = global.human[i].human_life;
            global.human[i].life = global.human[i].human_life;
            global.human[i].speed = global.human[i].human_speed;
            global.human[i].pow = global.human[i].human_pow;
            global.human[i].len = global.human[i].human_len;
            global.human[i].num_boom_now = 0;
            global.human[i].get_exp = 0;
            global.human[i].get_gold = 0;
            global.human[i].kill_monster = 0;
            global.human[i].kill_human = 0;

            global.human[i].online_human_stat[0] = 0;
        }

        //游戏内精灵初始化-属性字体
        global.gameobject_text_life = GameObject.Find("text_life");
        global.gameobject_text_boom = GameObject.Find("text_boom");
        global.gameobject_text_pow = GameObject.Find("text_pow");
        global.gameobject_text_len = GameObject.Find("text_len");
        global.gameobject_text_speed = GameObject.Find("text_speed");
        global.text_life = global.gameobject_text_life.GetComponent<Text>();
        global.text_boom = global.gameobject_text_boom.GetComponent<Text>();
        global.text_pow = global.gameobject_text_pow.GetComponent<Text>();
        global.text_len = global.gameobject_text_len.GetComponent<Text>();
        global.text_speed = global.gameobject_text_speed.GetComponent<Text>();
        //游戏内精灵初始化-成功板/失败板/任务板
        global.gameobject_gameover_fail = GameObject.Find("gameover_fail");
        global.gameobject_gameover_success = GameObject.Find("gameover_success");
        global.gameobject_success_monster = GameObject.Find("text_text_success_monster");
        global.gameobject_success_exp = GameObject.Find("text_text_success_exp");
        global.gameobject_success_gold = GameObject.Find("text_text_success_gold");
        global.gameobject_gameover_tip = GameObject.Find("gameover_tip");
        global.gameobject_mission = GameObject.Find("mission");
        global.gameobject_text_mission = GameObject.Find("text_mission");
        //游戏内音乐初始化-背景/胜利/失败
        global.gameobject_music = GameObject.Find("music");
        global.gameobject_sound_lose = GameObject.Find("sound_lose");
        global.gameobject_sound_win = GameObject.Find("sound_win");
        global.gameobject_sound_food = GameObject.Find("sound_food");
        global.gameobject_sound_hit = GameObject.Find("sound_hit");
        global.audio_music = global.gameobject_music.GetComponent<AudioSource>();
        global.audio_sound_lose = global.gameobject_sound_lose.GetComponent<AudioSource>();
        global.audio_sound_win = global.gameobject_sound_win.GetComponent<AudioSource>();
        global.audio_sound_food = global.gameobject_sound_food.GetComponent<AudioSource>();
        global.audio_sound_hit = global.gameobject_sound_hit.GetComponent<AudioSource>();
        //游戏内文字初始化-游戏胜利/失败/任务
        global.text_success_exp = global.gameobject_success_exp.GetComponent<Text>();
        global.text_success_gold = global.gameobject_success_gold.GetComponent<Text>();
        global.text_gameover_tip = global.gameobject_gameover_tip.GetComponent<Text>();
        global.text_mission = global.gameobject_text_mission.GetComponent<Text>();
        global.text_mission.text = "打败地图右边怪物即可获得胜利！\n怪物剩余数量在右上角显示。";
        global.str_gameover_tip[0] = "Tip:打败更难的关卡更容易获\n得晶石哦！";
        global.str_gameover_tip[1] = "Tip:晶石在我的物品中镶嵌可\n以提升能力哦！";
        global.str_gameover_tip[2] = "Tip:注意放完炸弹以后要立刻\n往回跑哦！";
        
        //游戏内系统初始化
        global.gameover = -2;
        global.boom_number = 0;
        global.online_game_start = true;

        global.width = Screen.width;
        global.height = Screen.height;
        global.rate_x = global.width / 1280;
        global.rate_y = global.height / 720;

    }

    //1.失败按钮退出游戏
    public void push_fail()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("room_list");
    }

    //2.成功按钮退出游戏
    public void push_success()
    {
        //向服务端发送并更新信息
        //发送更新人物信息
        global.send_buff = "[#5|0|" + global.human[global.my_num].get_exp + "|" + global.human[global.my_num].get_gold + "]";
        UnityEngine.SceneManagement.SceneManager.LoadScene("room_list");
    }


    private void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            //1.人物后仰计时
            if (global.human[i].human_stat == 6)
            {
                if (global.human[i].fade_time >= global.time_fade)
                {
                    global.human[i].fade_time = 0;
                    global.human[i].human_stat = 0;
                }
                else
                {
                    global.human[i].fade_time = global.human[i].fade_time + 0.02f;
                }
            }
            //2.无敌计时
            if (global.human[i].user_id != "")
            {
                if (global.human[i].wudi)
                {
                    if (global.human[i].wudi_time >= global.time_wudi)
                    {
                        global.human[i].wudi_time = 0;
                        global.human[i].wudi = false;
                        print("无敌结束");
                    }
                    else
                    {
                        global.human[i].wudi_time += global.time_wudi / 60;
                    }
                }
            }
        }
    }


    private void Update()
    {
        //其他人死亡
        if (global.gameover == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if(global.human[i].life_now <= 0)
                {
                    string str_ani = "human_" + global.human[i].user_role_num + "_5";
                    global.human[i].ani_human.Play(str_ani);
                    if (i != global.my_num)
                    {
                        global.human[i].life_now = 0;
                        global.human[i].user_id = "";
                        global.human[global.my_num].get_exp += 100;
                        global.human[global.my_num].get_gold += 100;
                    }
                }
            }
        }
        
        //检查屋内有多少人
        if (global.gameover == 0)
        {
            int flag = 0;
            for (int i = 0; i < 4; i++)
            {
                if (global.human[i].user_id == "")
                {
                    continue;
                }
                else
                {
                    flag++;
                }
            }
            if ((flag == 1)&&(global.human[global.my_num].life_now > 0))
            {
                global.gameover = 1;
            }
        }


        //2.更新数据
        if (global.gameover == 0)
        {
            global.text_boom.text = "X  " + global.human[global.my_num].num_boom.ToString();
            global.text_len.text = global.human[global.my_num].len.ToString();
            global.text_pow.text = global.human[global.my_num].pow.ToString();
            global.text_speed.text = global.human[global.my_num].speed.ToString();
            global.text_life.text = global.human[global.my_num].life_now.ToString() + " / " + global.human[global.my_num].life.ToString();
        }

        


        //3.游戏胜利
        if ((global.gameover == 1) && (success_flag == 0))
        {
            //胜利板内容
            vec.Set(0, 1000, 0);
            global.text_success_exp.text = global.human[global.my_num].get_exp.ToString();
            global.text_success_gold.text = global.human[global.my_num].get_gold.ToString();
            global.gameobject_gameover_success.transform.localPosition = vec;
            global.audio_music.Stop();
            global.audio_sound_win.Play();
            success_flag = 1;

            //初始化网络
            global.flag_online = true;
            global.flag_online_first = false;
            global.flag_close_send_pthread = false;
            global.flag_close_recv_pthread = false;

            Thread thread_send = new Thread(new ThreadStart(pthread.send_message));
            Thread thread_recv = new Thread(new ThreadStart(pthread.recv_message));

            thread_send.Start();
            thread_recv.Start();

            global.send_buff = "[#D]";

            //初始化人物所有属性
            global.my_num = 0;
            for (int i = 0; i < 4; i++)
            {
                global.human[i] = new global.Human();
            }
        }
        //落下统计版
        if ((global.gameover == 1) && (success_flag == 1) && (global.gameobject_gameover_success.transform.localPosition.y > 0))
        {
            vec.Set(0, global.gameobject_gameover_success.transform.localPosition.y - 50, 0);
            global.gameobject_gameover_success.transform.localPosition = vec;
        }




        //4.游戏失败
        if ((global.human[global.my_num].life_now <= 0)&&(success_flag == 0))
        {
            global.human[global.my_num].life_now = 0;
            global.text_life.text = global.human[global.my_num].life_now + "/" + global.human[0].life;
            string str_ani = "human_" + global.human[global.my_num].user_role_num + "_5";
            global.human[global.my_num].ani_human.Play(str_ani);
            global.human[global.my_num].human_stat = 5;
            global.gameover = -1;
            //初始化板的位置
            vec.Set(0, 1000, 0);
            global.gameobject_gameover_fail.transform.localPosition = vec;
            global.audio_music.Stop();
            global.audio_sound_lose.Play();
            //初始化板上内容
            int rand = Random.Range(0, global.str_gameover_tip.Length);
            global.text_gameover_tip.text = global.str_gameover_tip[rand];
            success_flag = -1;
            //初始化网络
            global.flag_online = true;
            global.flag_online_first = false;
            global.flag_close_send_pthread = false;
            global.flag_close_recv_pthread = false;

            Thread thread_send = new Thread(new ThreadStart(pthread.send_message));
            Thread thread_recv = new Thread(new ThreadStart(pthread.recv_message));

            thread_send.Start();
            thread_recv.Start();

            global.send_buff = "[#D]";

            //初始化人物所有属性
            global.my_num = 0;
            for(int i=0;i<4;i++)
            {
                global.human[i] = new global.Human();
            }
        }

        //落下统计版
        if ((global.gameover == -1) && (success_flag == -1) && (global.gameobject_gameover_fail.transform.localPosition.y > 0))
        {
            vec.Set(0, global.gameobject_gameover_fail.transform.localPosition.y - 50, 0);
            global.gameobject_gameover_fail.transform.localPosition = vec;
        }
    }
}
