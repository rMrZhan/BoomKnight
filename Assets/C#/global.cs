using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Threading;

//目录
//全局变量表

public class global : MonoBehaviour
{
    /***公用部分***/
    public static bool kill_game = false;


    //常量-地图宽/高
    public static int length_x = 68;
    public static int length_y = 64;
    //常量-无敌持续时间
    public static float time_wudi = 1.0f;
    public static float time_fade = 0.5f;
    //常量-爆炸火焰存在时间
    public static float time_water = 0.5f;
    //精灵-镜头
    public static GameObject gameobject_maincamera;
    public static Camera camera_maincamera;
    //精灵-人物
    public static Animator[]   ani_human        = new Animator[4];
    //精灵-摇杆
    public static GameObject gameobject_control_joy;
    public static GameObject gameobject_control_bg;
    //精灵-属性字体
    public static GameObject gameobject_text_life;
    public static GameObject gameobject_text_boom;
    public static GameObject gameobject_text_pow;
    public static GameObject gameobject_text_len;
    public static GameObject gameobject_text_speed;
    public static GameObject gameobject_text_time;
    public static GameObject gameobject_monster_count;
    public static Text text_life;
    public static Text text_boom;
    public static Text text_pow;
    public static Text text_len;
    public static Text text_speed;
    public static Text text_monster_count;
    //精灵-成功/失败板/任务板
    public static GameObject gameobject_gameover_fail;
    public static GameObject gameobject_gameover_success;
    public static GameObject gameobject_gameover_tip;
    public static GameObject gameobject_success_exp;
    public static GameObject gameobject_success_gold;
    public static GameObject gameobject_success_monster;
    public static GameObject gameobject_mission;
    public static GameObject gameobject_text_mission;
    public static Text text_gameover_tip;
    public static Text text_success_exp;
    public static Text text_success_gold;
    public static Text text_success_monster;
    public static Text text_mission;
    public static Text text_time;
    //精灵-声音
    public static GameObject  gameobject_sound_lose;
    public static GameObject  gameobject_sound_win;
    public static GameObject  gameobject_music;
    public static GameObject  gameobject_sound_food;
    public static GameObject  gameobject_sound_hit;
    public static AudioSource audio_music;
    public static AudioSource audio_sound_lose;
    public static AudioSource audio_sound_win;
    public static AudioSource audio_sound_food;
    public static AudioSource audio_sound_hit;
    //精灵-火焰元素
    public static GameObject gameobject_boom;
    public static GameObject gameobject_water_destory;
    public static GameObject gameobject_water_middle;
    public static GameObject gameobject_water_up_head;
    public static GameObject gameobject_water_up_body;
    public static GameObject gameobject_water_down_head;
    public static GameObject gameobject_water_down_body;
    public static GameObject gameobject_water_left_head;
    public static GameObject gameobject_water_left_body;
    public static GameObject gameobject_water_right_head;
    public static GameObject gameobject_water_right_body;
    //精灵-物品
    public static GameObject gameobject_food_boom;
    public static GameObject gameobject_food_len;
    public static GameObject gameobject_food_pow;
    public static GameObject gameobject_food_speed;
    public static GameObject gameobject_food_life;
    //精灵-怪物血条
    public static GameObject gameobject_monster_life_full_copy;
    public static GameObject gameobject_monster_life_empty_copy;
    public static GameObject gameobject_monster_life_full;
    public static GameObject gameobject_monster_life_empty;
    //对象-火焰
    public class Water
    {
        //爆炸出这个火焰的炸弹编号
        public int num_boom;
        //爆炸的长度
        public float len;
        //爆炸的伤害
        public int pow;
        //爆炸出这个火焰的炸弹坐标
        public int x;
        public int y;
        //一个火焰包含四个火焰头
        public GameObject water_up_head;
        public GameObject water_down_head;
        public GameObject water_left_head;
        public GameObject water_right_head;
        //一个火焰包含四组火焰身体
        public List<GameObject> water_up_body_list = new List<GameObject>();
        public List<GameObject> water_down_body_list = new List<GameObject>();
        public List<GameObject> water_left_body_list = new List<GameObject>();
        public List<GameObject> water_right_body_list = new List<GameObject>();
        //一个火焰包含一个火焰中心
        public GameObject water_middle;
        //炸到的箱子
        public List<GameObject> box_destory_list = new List<GameObject>();
        //炸到的箱子位置
        public List<Vector2> box_position = new List<Vector2>();
        //炸到箱子触发的动画
        public List<GameObject> water_destory_list = new List<GameObject>();
        //向右方向继续生成火焰的标志，生成是false
        public bool flag_stop_up = false;
        public bool flag_stop_down = false;
        public bool flag_stop_left = false;
        public bool flag_stop_right = false;
        //这个火焰是否已经被销毁
        public bool flag_destory = false;
    }
    public static List<Water> water_list = new List<Water>();
    //精灵-层级
    public static GameObject lay_water;
    public static GameObject lay_mid;
    public static GameObject lay_monster;
    public static GameObject lay_food;
    //变量-屏幕长宽
    public static int width;
    public static int height;
    //变量-缩放比例
    public static float rate_x;
    public static float rate_y;
    //变量-人物死亡提示信息 每行11个字
    public static string[] str_gameover_tip = new string[3];
    //变量-任务信息
    public static string str_mission;
    //对象-人物信息
    public class Human
    {
        public GameObject gameobject_human;
        /**************人物账号属性**************/
        //人物账号
        //人物密码
        //人物昵称
        //人物等级
        //人物当前使用的人物编号,如果是1 就是human_1
        //人物当前金币
        //人物当前经验
        //人物拥有的所有人物列表
        //人物已经开启的关卡
        public string user_id = "";
        public string user_passwd;
        public string user_name;
        public int user_level = 1;
        public int user_role_num = 1;
        public int user_boom_num = 1;
        public int user_gold = 0;
        public int user_exp = 0;
        public int user_stage = 1;
        public bool[] man = new bool[12];
        /**************人物等级基础属性**************/
        //该等级总生命值
        //该等级威力
        //该等级炸弹量
        //该等级移动速度
        //该等级火焰长度
        //该等级升级所需exp
        public int human_life = 1;
        public int human_pow = 1;
        public int human_boom = 1;
        public float human_speed = 3.0f;
        public int human_len = 1;
        public int human_exp = 0;
        public float human_time_boom = 4.0f;
        /**************人物在房间内的属性**************/
        //人物已经开启的关卡
        //人物所在房间号
        //人物是否为房主
        //是否已经准备
        public int room_num = 0;
        public int room_position = 0;
        public bool is_room_leader = false;
        public bool is_ready = false;
        public bool flag_light = false;
        public Vector3 vec_human;
        public Vector3 vec_light;
        public Vector3 vec_image_ready;
        public Vector3 vec_name;
        public GameObject gameobject_light;
        public GameObject gameobject_image_ready;
        public GameObject gameobject_text_name;
        public GameObject gameobject_name;
        public GameObject gameobject_name_sign;
        public Text text_name;
        /**************人物游戏中的临时属性**************/
        //人物object
        //人物ani
        //人物当前状态
        //人物当前位置和方向
        //人物总生命
        //人物当前生命
        //人物已经摆放的炸弹数量
        //人物移动速度
        //人物放炸弹的威力
        //人物火焰长度
        //人物是否处于无敌状态/无敌时间/后仰时间
        //获得的经验/金币
        //杀死的人物/怪物
        public Animator ani_human;
        public int human_stat;
        public float x;
        public float y;
        public int   direction;
        public int   life_now;
        public int   life;
        public int   num_boom_now;
        public int   num_boom;
        public float speed;
        public int   pow;
        public int   len;
        public bool  wudi = false;
        public float wudi_time = 0;
        public float fade_time = 0;
        public int   get_exp = 0;
        public int   get_gold = 0;
        public int   kill_human = 0;
        public int   kill_monster = 0;

        //网络存储
        public int[] online_human_stat = new int[1000];
        public Vector3 vec_old;
    }
    public static Human[] human = new Human[4];

    /***网络***/
    //精灵-网络连接窗口
    public static GameObject gameobject_messagebox_online;
    public static GameObject gameobject_text_messagebox_online;
    public static GameObject gameobject_button_reline;
    public static GameObject gameobject_button_exit;
    public static GameObject gameobject_delay;
    public static Text text_text_messagebox_online;
    public static Text text_delay;

    //常量-延迟多少ms认为断连
    public static int MAX_MS = 500;
    //常量-多少次没有连接成功认为掉线
    public static int MAX_LINE = 5;

    
    //符号-网络连接
    public static bool flag_online = true;
    //符号-首次连接
    public static bool flag_online_first = true;
    //符号-重新连接
    public static bool flag_reline = false;
    //符号-等待接收标志(计算延迟)
    public static bool flag_wait = false;
    //符号-关闭线程
    public static bool flag_close_send_pthread = false;
    public static bool flag_close_recv_pthread = false;
    //符号-成功收发标志
    public static bool flag_recv = false;
    //变量-等待接收时间
    public static float time_wait = 0;
    //变量-接收等待时间(秒，可以小数)
    public static float time_recv = 1.0f;
    //变量-要发送的网络报文
    public static string send_buff = "";
    public static string recv_buff = "";
    //变量-重复发送报文的次数
    public static int send_buff_time = 0;
    //变量-网络延迟
    public static int delay = 0;
    //符号-报文标志
    public static bool flag_online_game = false;
    public static bool flag_buff1 = false;
    public static bool flag_buff2 = false;
    public static bool flag_buff3 = false;
    public static bool flag_buff4 = false;
    public static bool flag_buff5 = false;
    public static bool flag_buff6 = false;
    public static bool flag_buff7 = false;
    public static bool flag_buff8 = false;
    public static bool flag_buff9 = false;
    public static bool flag_buff10 = false;
    public static bool flag_buff11 = false;
    public static bool flag_buff12 = false;

    //对象-线程锁
    public static readonly object locker = new object();
    //对象-网络套接字
    public class client_socket
    {
        public string ip = "120.79.90.16";
        public int port = 9997;
        public Socket server_socket;
    }
    public static client_socket client = new client_socket();
    //对象-房间列表
    public class Room_list
    {
        //是否存在
        public bool is_open = false;
        //房间编号
        public int room_num;
        public string room_name;
        public int room_human_num;
    }
    public static Room_list[] room_list = new Room_list[5];

    //对象-网络游戏地图属性
    public class para_map
    {
        public int size_x = 0;
        public int size_y = 0;
        //使用的地图图片名称
        public string str_image = "map_2";
        //开始人物坐标
        public Vector2[] human = new Vector2[4];
        //可以破坏箱子的编号
        public int num_break_box = 0;
        //可以破坏箱子的位置
        public Vector2[] must_box;
    }
    public static para_map[] online_map = new para_map[50];
    //变量-页数
    public static int room_list_page = 1;
    /***提示窗***/

    //精灵-提示窗、提示内容
    public static GameObject gameobject_messagebox;
    public static GameObject gameobject_text_messagebox;
    public static Text text_text_messagebox;
    //变量-提示窗口内容
    public static string str_message = "";
    public static string str_online_message = "";
    /***场景1***/

    //符号-登录状态0未登录1成功-1失败
    public static int flag_login_in = 0;
    public static int flag_sign_in = 0;
    public static int s1_choose_human = 0;
    /***场景2***/

    //精灵-场景2特有对话框
    public static GameObject gameobject_window_1;
    public static GameObject gameobject_window_1_text;
    public static Text text_window_1_text;

    //常量-单人窗口最佳位置
    public static float window_1_x = -300;
    public static float winodw_1_y = 83;
    //常量-多人窗口最佳位置
    public static float window_2_x = 170;
    public static float winodw_2_y = 30;
    //常量-我的物品最佳位置
    public static float window_3_x = -690;
    public static float winodw_3_y = -180;
    //常量-移动一次一共需要帧数
    public static float change_frame = 10;

    //变量-移动坐标/大小
    public static float position_x;
    public static float position_y;
    public static float size_personal;
    public static float change_x = 0;
    public static float change_y = 0;
    public static float change_size = 0;
    //变量-记下移动的帧数
    public static int move_frame = 0;

    //符号-开始的镜头选择 0-不选择 1-单人游戏 2-多人游戏 3-我的物品
    public static int my_choose = 0;
    //符号-游戏的选择 0-不选择 1-单人游戏 2-多人游戏 3-我的物品
    public static int my_game = 0;

    /***网络游戏***/
    public static int my_num = 0;
    public static int online_level = 1;
    public static bool online_game_start = false;
    /***单人游戏***/
    //对象-地砖和地砖属性
    public class G_MAP
    {
        //是否存在炸弹 
        public bool boom = false;
        //炸弹的编号
        public int boom_num;
        //是否存在墙体
        public bool wall = false;
        //墙体可否被破坏
        public bool wall_destory = false;
        //可破坏墙体的object
        public GameObject gameobject_destory_wall;
    };
    public static G_MAP[][] G_map = new G_MAP[1024][];
    //对象-怪物数据库/怪物总量
    public class Monster
    {
        //活着
        public bool is_alive = true;
        //名字
        public string name;
        //当前生命
        public int life_now;
        //总生命
        public int life_full;
        //速度
        public float speed;
        //伤害
        public int pow;
        //经验
        public int exp;
        //金币
        public int gold;
        //怪物的object
        public GameObject gameobject_monster;
    }
    public static Monster[] MONSTER = new Monster[10];
    public static List<Monster> G_MONSTER = new List<Monster>();
    //变量-游戏结束状态 -2未开始 -1游戏失败 0游戏开始 1游戏胜利
    public static int gameover = -2;
    //变量-当前关卡
    public static int level = 1;
    //变量-游戏难度
    public static int hard = 1;
    //变量-游戏时间
    public static int time = 0;
    //变量-游戏中炸弹的编号
    public static int boom_number = 0;
    //变量-游戏中怪物的编号
    public static int monster_number = 0;

    //附录-发送编码表
    public static int[][] send_ascii = new int[2][];
    public static int ascii_button = 0;
    public static int ascii_direction = 0;

    private void Awake()
    {
        //初始化
        for (int i = 0; i < 4; i++)
        {
            human[i] = new Human();
        }
        my_num = 0;

        int z = 1;
        //0 1按键码 0 1 2 3 4 停止+4个方向
        for (int button = 0; button < 2; button++)
        {
            send_ascii[button] = new int[5];
            for (int direction = 0; direction < 5; direction++)
            {
                send_ascii[button][direction] = z;
                z++;
            }
        }

        //扩展网络游戏地图
        //#地图1
        online_map[0] = new para_map();
        online_map[0].size_x = 27;
        online_map[0].size_y = 31;
        online_map[0].str_image = "map_2";
        online_map[0].num_break_box = 2;
        online_map[0].human[0].x = 10;
        online_map[0].human[0].y = 11;
        online_map[0].human[1].x = 17;
        online_map[0].human[1].y = 18;

    }
}

