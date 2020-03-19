using UnityEngine;
using System.Text;
using UnityEngine.UI;

//目录
//1.初始化游戏
//2.更新数据
//3.游戏胜利
//4.游戏失败

public class P_start : MonoBehaviour
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
        global.gameobject_maincamera       = GameObject.Find("MainCamera");
        global.gameobject_control_joy      = GameObject.Find("control_joy");
        global.gameobject_control_bg       = GameObject.Find("control");
        //游戏内精灵初始化-火焰
        global.gameobject_boom             = GameObject.Find("boom");
        global.gameobject_water_destory    = GameObject.Find("water_destory");
        global.gameobject_water_middle     = GameObject.Find("water_middle");
        global.gameobject_water_up_head    = GameObject.Find("water_up_head");
        global.gameobject_water_up_body    = GameObject.Find("water_up_body");
        global.gameobject_water_down_head  = GameObject.Find("water_down_head");
        global.gameobject_water_down_body  = GameObject.Find("water_down_body");
        global.gameobject_water_left_head  = GameObject.Find("water_left_head");
        global.gameobject_water_left_body  = GameObject.Find("water_left_body");
        global.gameobject_water_right_head = GameObject.Find("water_right_head");
        global.gameobject_water_right_body = GameObject.Find("water_right_body");
        
        //游戏内精灵初始化-怪物血条
        global.gameobject_monster_life_full  = GameObject.Find("monster_life_full");
        global.gameobject_monster_life_empty = GameObject.Find("monster_life_empty");
        //游戏内精灵初始化-层级
        global.lay_monster                 = GameObject.Find("lay_monster");
        global.lay_water                   = GameObject.Find("lay_water");
        global.lay_mid                     = GameObject.Find("lay_mid");
        //游戏内精灵初始化-物品
        global.gameobject_food_boom        = GameObject.Find("food_boom");
        global.gameobject_food_len         = GameObject.Find("food_len");
        global.gameobject_food_pow         = GameObject.Find("food_pow");
        global.gameobject_food_speed       = GameObject.Find("food_speed");
        global.gameobject_food_life        = GameObject.Find("food_life");
        //游戏内精灵初始化-人物
        //global.human[0]                    = new global.Human();
        //发送更新人物信息
        global.send_buff = "[#2]";
        while(true)
        {
            if(global.flag_buff2)
            {
                global.flag_buff2 = false;
                break;
            }
        }
        global.human[0].gameobject_human   = GameObject.Find("human");
        global.human[0].ani_human          = global.human[0].gameobject_human.GetComponent<Animator>();
        //游戏内精灵初始化-属性字体
        global.gameobject_text_life        = GameObject.Find("text_life");
        global.gameobject_text_boom        = GameObject.Find("text_boom");
        global.gameobject_text_pow         = GameObject.Find("text_pow");
        global.gameobject_text_len         = GameObject.Find("text_len");
        global.gameobject_text_speed       = GameObject.Find("text_speed");
        global.gameobject_monster_count    = GameObject.Find("text_monster");
        global.gameobject_text_time        = GameObject.Find("text_time");
        global.text_life                   = global.gameobject_text_life.GetComponent<Text>();
        global.text_boom                   = global.gameobject_text_boom.GetComponent<Text>();
        global.text_pow                    = global.gameobject_text_pow.GetComponent<Text>();
        global.text_len                    = global.gameobject_text_len.GetComponent<Text>();
        global.text_speed                  = global.gameobject_text_speed.GetComponent<Text>();
        global.text_time                   = global.gameobject_text_time.GetComponent<Text>();
        global.text_monster_count          = global.gameobject_monster_count.GetComponent<Text>();
        //游戏内精灵初始化-成功板/失败板/任务板

        global.gameobject_gameover_fail    = GameObject.Find("gameover_fail");
        global.gameobject_gameover_success = GameObject.Find("gameover_success");
        global.gameobject_success_monster  = GameObject.Find("text_text_success_monster");
        global.gameobject_success_exp      = GameObject.Find("text_text_success_exp");
        global.gameobject_success_gold     = GameObject.Find("text_text_success_gold");
        global.gameobject_gameover_tip     = GameObject.Find("gameover_tip");
        global.gameobject_mission          = GameObject.Find("mission");
        global.gameobject_text_mission     = GameObject.Find("text_mission");
        //游戏内音乐初始化-背景/胜利/失败
        global.gameobject_music            = GameObject.Find("music");
        global.gameobject_sound_lose       = GameObject.Find("sound_lose");
        global.gameobject_sound_win        = GameObject.Find("sound_win");
        global.gameobject_sound_food       = GameObject.Find("sound_food");
        global.gameobject_sound_hit        = GameObject.Find("sound_hit");
        
        global.audio_music                 = global.gameobject_music.GetComponent<AudioSource>();
        global.audio_sound_lose            = global.gameobject_sound_lose.GetComponent<AudioSource>();
        global.audio_sound_win             = global.gameobject_sound_win.GetComponent<AudioSource>();
        global.audio_sound_food            = global.gameobject_sound_food.GetComponent<AudioSource>();
        global.audio_sound_hit = global.gameobject_sound_hit.GetComponent<AudioSource>();
        //游戏内文字初始化-游戏胜利/失败/任务
        global.text_success_monster        = global.gameobject_success_monster.GetComponent<Text>();
        global.text_success_exp            = global.gameobject_success_exp.GetComponent<Text>();
        global.text_success_gold           = global.gameobject_success_gold.GetComponent<Text>();
        global.text_gameover_tip           = global.gameobject_gameover_tip.GetComponent<Text>();
        global.text_mission                = global.gameobject_text_mission.GetComponent<Text>();
        global.str_gameover_tip[0]         = "Tip:打败更难的关卡更容易获\n得晶石哦！";
        global.str_gameover_tip[1]         = "Tip:晶石在我的物品中镶嵌可\n以提升能力哦！";
        global.str_gameover_tip[2]         = "Tip:注意放完炸弹以后要立刻\n往回跑哦！";
        //游戏内属性初始化
        global.human[0].num_boom           = global.human[0].human_boom;
        global.human[0].life_now           = global.human[0].human_life;
        global.human[0].life               = global.human[0].human_life;
        global.human[0].speed              = global.human[0].human_speed;
        global.human[0].pow                = global.human[0].human_pow;
        global.human[0].len                = global.human[0].human_len;
        global.human[0].num_boom_now       = 0;
        global.human[0].get_exp            = 0;
        global.human[0].get_gold           = 0;
        global.human[0].kill_monster       = 0;
        global.human[0].kill_human         = 0;
        print("属性:" + global.human[0].num_boom + global.human[0].life_now);
        //游戏内系统初始化
        global.gameover       = -2;
        global.boom_number    = 0;
        global.monster_number = 0;
        //游戏内怪物初始化
        global.G_MONSTER.Clear();
        global.MONSTER[0] = new global.Monster();
        global.MONSTER[0].name = "monster_1_copy";
        global.MONSTER[0].life_full = 1;
        global.MONSTER[0].life_now = 1;
        global.MONSTER[0].speed = 1.0f;
        global.MONSTER[0].pow = 1;
        global.MONSTER[0].exp = 100;
        global.MONSTER[0].gold = 10;
        global.MONSTER[1] = new global.Monster();
        global.MONSTER[1].name = "monster_2_copy";
        global.MONSTER[1].life_full = 3;
        global.MONSTER[1].life_now = 3;
        global.MONSTER[1].speed = 2.0f;
        global.MONSTER[1].pow = 3;
        global.MONSTER[1].exp = 200;
        global.MONSTER[1].gold = 20;
        //游戏内系统初始化
        global.width  = Screen.width;
        global.height = Screen.height;
        global.rate_x = global.width / 1280;
        global.rate_y = global.height / 720;
        
    }

    private void FixedUpdate()
    {
        //1.人物后仰计时
        if(global.human[global.my_num].human_stat == 6)
        {
            if(global.human[global.my_num].fade_time >= global.time_fade)
            {
                global.human[global.my_num].fade_time = 0;
                global.human[global.my_num].human_stat = 0;
            }
            else
            {
                global.human[global.my_num].fade_time = global.human[global.my_num].fade_time + 0.02f;
            }
        }
        //2.无敌计时
        if (global.human[global.my_num].wudi)
        {
            if (global.human[global.my_num].wudi_time >= global.time_wudi)
            {
                global.human[global.my_num].wudi_time = 0;
                global.human[global.my_num].wudi = false;
                print("无敌结束");
            }
            else
            {
                global.human[global.my_num].wudi_time += global.human[global.my_num].wudi_time + 0.02f;
            }
        }
    }



    private void Update()
    {
        //2.更新数据
        if (global.gameover == 0)
        {
            global.text_boom.text = "X  " + global.human[0].num_boom.ToString();
            global.text_len.text = global.human[0].len.ToString();
            global.text_pow.text = global.human[0].pow.ToString();
            global.text_speed.text = global.human[0].speed.ToString();
            global.text_life.text = global.human[0].life_now.ToString() + " / " + global.human[0].life.ToString();
        }

        //3.时间计算
        if ((global.gameover == 0) && (global.flag_online))
        {
            int sec = (global.time - (int)Time.timeSinceLevelLoad) % 60;
            int min = (global.time - (int)Time.timeSinceLevelLoad) / 60;
            if ((sec == 0) && (min == 0))
            {
                //时间到，游戏失败
                global.human[0].life_now = 0;
            }
            string str_sec, str_min;
            if (sec < 10)
            {
                str_sec = "0" + sec;
            }
            else
            {
                str_sec = sec.ToString();
            }
            if (min < 10)
            {
                str_min = "0" + min;
            }
            else
            {
                str_min = min.ToString();
            }
            //显示出来
            global.text_time.text = str_min + ":" + str_sec;
        }

        //3.游戏胜利
        if ((global.gameover == 1) && (success_flag == 0))
        {
            //胜利板内容
            vec.Set(0, 1000, 0);
            global.text_success_monster.text = "X  " + global.human[0].kill_monster.ToString();
            global.text_success_exp.text = global.human[0].get_exp.ToString();
            global.text_success_gold.text = global.human[0].get_gold.ToString();
            global.gameobject_gameover_success.transform.localPosition = vec;
            global.audio_music.Stop();
            global.audio_sound_win.Play();
            success_flag = 1;
        }
        //落下统计版
        if ((global.gameover == 1) && (success_flag == 1) && (global.gameobject_gameover_success.transform.localPosition.y > 0))
        {
            vec.Set(0, global.gameobject_gameover_success.transform.localPosition.y - 50, 0);
            global.gameobject_gameover_success.transform.localPosition = vec;
        }

        //4.游戏失败
        if ((global.human[0].life_now <= 0) && (success_flag == 0))
        {
            global.human[0].life_now = 0;
            global.text_life.text = global.human[0].life_now + "/" + global.human[0].life;
            string str_ani = "human_" + global.human[0].user_role_num + "_5";
            global.human[0].ani_human.Play(str_ani);
            global.human[0].human_stat = 5;
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
        }

        //落下统计版
        if ((global.gameover == -1) && (success_flag == -1) && (global.gameobject_gameover_fail.transform.localPosition.y > 0))
        {
            vec.Set(0, global.gameobject_gameover_fail.transform.localPosition.y - 50, 0);
            global.gameobject_gameover_fail.transform.localPosition = vec;
        }
    }
}
