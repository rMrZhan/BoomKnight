using UnityEngine;
using UnityEngine.UI;

//目录
//1.选择单人游戏
//2.选择多人游戏
//3.选择我的物品
//4.进入单人游戏
//5.进入多人游戏
//6.进入我的物品
//7.按下取消按钮
//8.镜头移动及对话框显示

public class s2_choose_game : MonoBehaviour
{
    private Vector3 vec;

    void Awake()
    {
        //主摄像机
        global.gameobject_maincamera = GameObject.Find("MainCamera");
        global.camera_maincamera = gameObject.GetComponent<Camera>();
        //对话框
        global.gameobject_window_1 = GameObject.Find("window_1");
        global.gameobject_window_1_text = GameObject.Find("window_1_text_1");
        global.text_window_1_text = global.gameobject_window_1_text.GetComponent<Text>();

    }
    //1.进入单人游戏
    public void choose_single()
    {
        //选择单人游戏最佳位置
        global.position_x = -1.8f;
        global.position_y = 0.5f;
        global.size_personal = 1.5f;
        //计算改变系数
        global.change_x = (global.position_x - global.camera_maincamera.transform.localPosition.x) / global.change_frame;
        global.change_y = (global.position_y - global.camera_maincamera.transform.localPosition.y) / global.change_frame;
        global.change_size = (global.camera_maincamera.orthographicSize - global.size_personal) / global.change_frame;
        //设置选择项
        global.my_choose = 1;
        global.my_game = 1;
        global.move_frame = 0;
    }

    //2.进入多人游戏
    public void choose_online()
    {
        //选择多人游戏最佳位置
        global.position_x = 1.0f;
        global.position_y = 0.2f;
        global.size_personal = 1.5f;
        //计算改变系数
        global.change_x = (global.position_x - global.camera_maincamera.transform.localPosition.x) / global.change_frame;
        global.change_y = (global.position_y - global.camera_maincamera.transform.localPosition.y) / global.change_frame;
        global.change_size = (global.camera_maincamera.orthographicSize - global.size_personal) / global.change_frame;
        //设置选择项
        global.my_choose = 2;
        global.my_game = 2;
        global.move_frame = 0;
    }
    //3.进入我的物品
    public void choose_good()
    {
        //选择单人游戏最佳位置
        //往左 x减小  往下 y减小
        global.position_x = -4.0f;
        global.position_y = -1.0f;
        global.size_personal = 1.5f;
        //计算改变系数
        global.change_x = (global.position_x - global.camera_maincamera.transform.localPosition.x) / global.change_frame;
        global.change_y = (global.position_y - global.camera_maincamera.transform.localPosition.y) / global.change_frame;
        global.change_size = (global.camera_maincamera.orthographicSize - global.size_personal) / global.change_frame;
        //设置选择项
        global.my_choose = 3;
        global.my_game = 3;
        global.move_frame = 0;
    }

    public void push_ok()
    {
        //4.进入单人游戏
        if (global.my_game == 1)
        {
            //发送更新人物信息
            global.send_buff = "[#2]";
            while (true)
            {
                if (global.flag_buff2)
                {
                    global.flag_buff2 = false;
                    break;
                }
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("choose_persional_game");
        }
        //5.进入多人游戏
        else if (global.my_game == 2)
        {
            //发送更新人物信息
            global.send_buff = "[#2]";
            while (true)
            {
                if (global.flag_buff2)
                {
                    global.flag_buff2 = false;
                    break;
                }
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("room_list");
        }
        //6.进入我的物品
        else if (global.my_game == 3)
        {
            //发送更新人物信息
            global.send_buff = "[#2]";
            print("进入场景my_item");
            while (true)
            {
                if (global.flag_buff2)
                {
                    global.flag_buff2 = false;
                    break;
                }
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("my_item");
        }
    }
    
    //7.按下取消按钮
    public void push_exit()
    {
        //返回到原来大小
        global.position_x = 0;
        global.position_y = 0;
        global.size_personal = 3.2f;
        //计算改变系数
        global.change_x = (global.position_x - global.camera_maincamera.transform.localPosition.x) / global.change_frame;
        global.change_y = (global.position_y - global.camera_maincamera.transform.localPosition.y) / global.change_frame;
        global.change_size = (global.camera_maincamera.orthographicSize - global.size_personal) / global.change_frame;
        //设置选择项
        global.my_choose = 1;
        global.my_game = 0;
        global.move_frame = 0;
        //移除对话框
        vec.Set(1000, 1000, 0);
        global.gameobject_window_1.transform.localPosition = vec;
    }


    void Update()
    {
        //8.镜头移动及对话框显示

        //当移动帧数达到预定帧数，就不会再动
        if (global.move_frame >= global.change_frame)
        {
            if (global.my_choose == 1)
            {
                global.my_choose = 0;
                //显示一下对话框
                if (global.my_game != 0)
                {
                    vec.Set(global.window_1_x, global.winodw_1_y, 0);
                    global.text_window_1_text.text = "即将进入单人模式\n是否确定?";
                    global.gameobject_window_1.transform.localPosition = vec;
                }
            }
            else if (global.my_choose == 2)
            {
                global.my_choose = 0;
                //显示一下对话框
                if (global.my_game != 0)
                {
                    vec.Set(global.window_2_x, global.winodw_2_y, 0);
                    global.text_window_1_text.text = "即将进入多人模式\n是否确定?";
                    global.gameobject_window_1.transform.localPosition = vec;
                }
            }
            else if (global.my_choose == 3)
            {
                global.my_choose = 0;
                //显示一下对话框
                if (global.my_game != 0)
                {
                    vec.Set(global.window_3_x, global.winodw_3_y, 0);
                    global.text_window_1_text.text = "即将进入我的物品\n是否确定?";
                    global.gameobject_window_1.transform.localPosition = vec;
                }
            }
        }
        //当选择单人模式时，镜头移动
        else if (global.my_choose == 1)
        {
            //改变镜头的位置和焦距
            vec.Set(global.camera_maincamera.transform.localPosition.x + global.change_x, global.camera_maincamera.transform.localPosition.y + global.change_y, global.camera_maincamera.transform.localPosition.z);
            global.camera_maincamera.orthographicSize = global.camera_maincamera.orthographicSize - global.change_size;
            global.camera_maincamera.transform.localPosition = vec;
            //帧数+1
            global.move_frame++;
        }
        //当选择多人模式时，镜头移动
        else if (global.my_choose == 2)
        {
            //改变镜头的位置和焦距
            vec.Set(global.camera_maincamera.transform.localPosition.x + global.change_x, global.camera_maincamera.transform.localPosition.y + global.change_y, global.camera_maincamera.transform.localPosition.z);
            global.camera_maincamera.orthographicSize = global.camera_maincamera.orthographicSize - global.change_size;
            global.camera_maincamera.transform.localPosition = vec;
            //帧数+1
            global.move_frame++;
        }
        //当选择我的物品时，镜头移动
        else if (global.my_choose == 3)
        {
            //改变镜头的位置和焦距
            vec.Set(global.camera_maincamera.transform.localPosition.x + global.change_x, global.camera_maincamera.transform.localPosition.y + global.change_y, global.camera_maincamera.transform.localPosition.z);
            global.camera_maincamera.orthographicSize = global.camera_maincamera.orthographicSize - global.change_size;
            global.camera_maincamera.transform.localPosition = vec;
            //帧数+1
            global.move_frame++;
        }
        else if (global.my_choose == -1)
        {
            //改变镜头的位置和焦距
            vec.Set(global.camera_maincamera.transform.localPosition.x - global.change_x, global.camera_maincamera.transform.localPosition.y - global.change_y, global.camera_maincamera.transform.localPosition.z);
            global.camera_maincamera.orthographicSize = global.camera_maincamera.orthographicSize - global.change_size;
            global.camera_maincamera.transform.localPosition = vec;
            //帧数+1
            global.move_frame++;
        }
    }
}
