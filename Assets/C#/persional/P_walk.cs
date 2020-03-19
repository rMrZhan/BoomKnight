using UnityEngine;

//目录
//1.键盘/摇杆发送人物状态
//2.根据人物状态改变行为(附带优化后的平滑触发)

public class P_walk : MonoBehaviour
{
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
    //变量-记录上一次的坐标
    private Vector3 vec_main_camera;
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
    }


    private void FixedUpdate()
    {

        //2.根据人物状态改变行为
        vec_main_camera = global.gameobject_maincamera.transform.localPosition;
        if (global.human[0].human_stat == 1)
        {
            //判断面前有没有箱子或者炸弹，计算人物偏移后的位置是否有物品
            int x = (int)gameObject.transform.position.x / global.length_x;
            int y = (int)((-gameObject.transform.position.y - coll_up) / global.length_y + 1);
            if (global.G_map[x][y].wall == true)
            {
                //前方有墙壁，不能走了
            }
            else
            {
                //无墙壁先判断是否到墙根
                int y_now = (int)((-gameObject.transform.position.y) / global.length_y + 1);
                if (y_now == y)
                {
                    //没有到墙根，正常行走
                    vec_main_camera.Set(vec_main_camera.x, vec_main_camera.y + global.human[0].speed, vec_main_camera.z);
                    global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                    global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
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
                        int x_left = (int)((gameObject.transform.position.x - pianyi_left) / global.length_x);
                        int x_right = (int)((gameObject.transform.position.x + pianyi_right) / global.length_x);
                        if (global.G_map[x_left][y].wall == true)
                        {
                            //左手边有墙壁，要往右走
                            vec_main_camera.Set(vec_main_camera.x + global.human[0].speed, vec_main_camera.y, vec_main_camera.z);
                            global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                            global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
                        }
                        else if (global.G_map[x_right][y].wall == true)
                        {
                            //右手边有墙壁，要往左走
                            vec_main_camera.Set(vec_main_camera.x - global.human[0].speed, vec_main_camera.y, vec_main_camera.z);
                            global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                            global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
                        }
                        else
                        {
                            //左右都没有墙壁，正常走
                            vec_main_camera.Set(vec_main_camera.x, vec_main_camera.y + global.human[0].speed, vec_main_camera.z);
                            global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                            global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
                        }
                    }
                }
            }
        }
        else if (global.human[0].human_stat == 2)
        {
            //判断面前有没有箱子或者炸弹，计算人物偏移后的位置是否有物品
            int x = (int)gameObject.transform.position.x / global.length_x;
            int y = (int)((-gameObject.transform.position.y + coll_down) / global.length_y + 1);
            if (global.G_map[x][y].wall == true)
            {
                //前方有墙壁，不能走了
            }
            else
            {
                //无墙壁先判断是否到墙根
                int y_now = (int)((-gameObject.transform.position.y) / global.length_y + 1);
                if (y_now == y)
                {
                    //没有到墙根，正常行走
                    vec_main_camera.Set(vec_main_camera.x, vec_main_camera.y - global.human[0].speed, vec_main_camera.z);
                    global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                    global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
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
                        int x_left = (int)((gameObject.transform.position.x - pianyi_left) / global.length_x);
                        int x_right = (int)((gameObject.transform.position.x + pianyi_right) / global.length_x);
                        if (global.G_map[x_left][y].wall == true)
                        {
                            //左手边有墙壁，要往右走
                            vec_main_camera.Set(vec_main_camera.x + global.human[0].speed, vec_main_camera.y, vec_main_camera.z);
                            global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                            global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
                        }
                        else if (global.G_map[x_right][y].wall == true)
                        {
                            //右手边有墙壁，要往左走
                            vec_main_camera.Set(vec_main_camera.x - global.human[0].speed, vec_main_camera.y, vec_main_camera.z);
                            global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                            global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
                        }
                        else
                        {
                            //左右都没有墙壁，正常走
                            vec_main_camera.Set(vec_main_camera.x, vec_main_camera.y - global.human[0].speed, vec_main_camera.z);
                            global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                            global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
                        }
                    }
                }
            }
        }
        else if (global.human[0].human_stat == 3)
        {
            //判断面前有没有箱子或者炸弹，计算人物偏移后的位置是否有物品
            int x = (int)((gameObject.transform.position.x - coll_left) / global.length_x);
            //y向下偏移11个坐标，因为人物中心点不在正中心
            int y = (int)((-gameObject.transform.position.y + 11) / global.length_y + 1);
            if (global.G_map[x][y].wall == true)
            {
                //前方有墙壁，不能走了
            }
            else
            {

                //无墙壁先判断是否到墙根
                int x_now = (int)(gameObject.transform.position.x / global.length_x);
                if (x_now == x)
                {
                    //没有到墙根，正常行走
                    vec_main_camera.Set(vec_main_camera.x - global.human[0].speed, vec_main_camera.y, vec_main_camera.z);
                    global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                    global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
                }
                else
                {
                    if (global.G_map[x][y].boom == true)
                    {
                        ;
                    }
                    else
                    {
                        int y_up = (int)((-gameObject.transform.position.y - pianyi_up) / global.length_y + 1);
                        int y_down = (int)((-gameObject.transform.position.y + pianyi_down) / global.length_y + 1);
                        if (global.G_map[x][y_up].wall == true)
                        {
                            //上边有墙壁，要往下走
                            vec_main_camera.Set(vec_main_camera.x, vec_main_camera.y - global.human[0].speed, vec_main_camera.z);
                            global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                            global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
                        }
                        else if (global.G_map[x][y_down].wall == true)
                        {
                            vec_main_camera.Set(vec_main_camera.x, vec_main_camera.y + global.human[0].speed, vec_main_camera.z);
                            global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                            global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
                        }
                        else
                        {
                            //上下都没有墙壁，正常走
                            vec_main_camera.Set(vec_main_camera.x - global.human[0].speed, vec_main_camera.y, vec_main_camera.z);
                            global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                            global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
                        }
                    }
                }
            }
        }
        else if (global.human[0].human_stat == 4)
        {
            //判断面前有没有箱子或者炸弹，计算人物偏移后的位置是否有物品
            int x = (int)((gameObject.transform.position.x + coll_right) / global.length_x);
            //y向下偏移11个坐标，因为人物中心点不在正中心
            int y = (int)((-gameObject.transform.position.y + 11) / global.length_y + 1);
            if (global.G_map[x][y].wall == true)
            {
                //前方有墙壁，不能走了
            }
            else
            {
                //无墙壁先判断是否到墙根
                int x_now = (int)(gameObject.transform.position.x / global.length_x);
                if (x_now == x)
                {
                    //没有到墙根，正常行走
                    vec_main_camera.Set(vec_main_camera.x + global.human[0].speed, vec_main_camera.y, vec_main_camera.z);
                    global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                    global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
                }
                else
                {
                    if (global.G_map[x][y].boom == true)
                    {
                        ;
                    }
                    else
                    {
                        int y_up = (int)((-gameObject.transform.position.y - pianyi_up) / global.length_y + 1);
                        int y_down = (int)((-gameObject.transform.position.y + pianyi_down) / global.length_y + 1);
                        if (global.G_map[x][y_up].wall == true)
                        {
                            //上边有墙壁，要往下走
                            vec_main_camera.Set(vec_main_camera.x, vec_main_camera.y - global.human[0].speed, vec_main_camera.z);
                            global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                            global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
                        }
                        else if (global.G_map[x][y_down].wall == true)
                        {
                            //下边有墙壁，要往上走
                            vec_main_camera.Set(vec_main_camera.x, vec_main_camera.y + global.human[0].speed, vec_main_camera.z);
                            global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                            global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
                        }
                        else
                        {
                            //上下都没有墙壁，正常走
                            vec_main_camera.Set(vec_main_camera.x + global.human[0].speed, vec_main_camera.y, vec_main_camera.z);
                            global.gameobject_maincamera.transform.localPosition = vec_main_camera;
                            global.human[0].gameobject_human.transform.localPosition = vec_main_camera;
                        }
                    }
                }
            }
        }
        else
        {
            ;
        }
    }

    //1.键盘/摇杆发送人物状态
    void Update()
    {
        if (global.gameover == 0)
        {
            if (global.human[global.my_num].human_stat != 6)
            {
                if (flag_unity == 1)
                {
                    //电脑控制用键盘吧
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        str_ani = "human_" + global.human[0].user_role_num + "_1";
                        global.human[0].ani_human.Play(str_ani);
                        global.human[0].human_stat = 1;
                    }
                    else if (Input.GetKey(KeyCode.DownArrow))
                    {
                        str_ani = "human_" + global.human[0].user_role_num + "_2";
                        global.human[0].ani_human.Play(str_ani);
                        global.human[0].human_stat = 2;
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        str_ani = "human_" + global.human[0].user_role_num + "_3";
                        global.human[0].ani_human.Play(str_ani);
                        global.human[0].human_stat = 3;
                    }
                    else if (Input.GetKey(KeyCode.RightArrow))
                    {
                        str_ani = "human_" + global.human[0].user_role_num + "_4";
                        global.human[0].ani_human.Play(str_ani);
                        global.human[0].human_stat = 4;
                    }
                    else
                    {
                        str_ani = "human_" + global.human[0].user_role_num + "_0";
                        global.human[0].ani_human.Play(str_ani);
                        global.human[0].human_stat = 0;
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
                                    str_ani = "human_" + global.human[0].user_role_num + "_4";
                                    global.human[0].ani_human.Play(str_ani);
                                    global.human[0].human_stat = 4;
                                }
                                else if (tan > 1)
                                {
                                    str_ani = "human_" + global.human[0].user_role_num + "_1";
                                    global.human[0].ani_human.Play(str_ani);
                                    global.human[0].human_stat = 1;
                                }
                                else if (tan < -1)
                                {
                                    str_ani = "human_" + global.human[0].user_role_num + "_2";
                                    global.human[0].ani_human.Play(str_ani);
                                    global.human[0].human_stat = 2;
                                }
                            }
                            else if ((local_x - vec_control.x) < 0)
                            {
                                if ((tan > -1) && (tan < 1))
                                {
                                    str_ani = "human_" + global.human[0].user_role_num + "_3";
                                    global.human[0].ani_human.Play(str_ani);
                                    global.human[0].human_stat = 3;
                                }
                                else if (tan > 1)
                                {
                                    str_ani = "human_" + global.human[0].user_role_num + "_2";
                                    global.human[0].ani_human.Play(str_ani);
                                    global.human[0].human_stat = 2;
                                }
                                else if (tan < -1)
                                {
                                    str_ani = "human_" + global.human[0].user_role_num + "_1";
                                    global.human[0].ani_human.Play(str_ani);
                                    global.human[0].human_stat = 1;
                                }
                            }
                            else if ((local_x - vec_control.x) == 0)
                            {
                                if ((local_y - vec_control.y) > 0)
                                {
                                    str_ani = "human_" + global.human[0].user_role_num + "_1";
                                    global.human[0].ani_human.Play(str_ani);
                                    global.human[0].human_stat = 1;
                                }
                                else if ((local_y - vec_control.y) < 0)
                                {
                                    str_ani = "human_" + global.human[0].user_role_num + "_2";
                                    global.human[0].ani_human.Play(str_ani);
                                    global.human[0].human_stat = 2;
                                }
                                else
                                {
                                    str_ani = "human_" + global.human[0].user_role_num + "_0";
                                    global.human[0].ani_human.Play(str_ani);
                                    global.human[0].human_stat = 0;
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
                                str_ani = "human_" + global.human[0].user_role_num + "_4";
                                global.human[0].ani_human.Play(str_ani);
                                global.human[0].human_stat = 4;
                            }
                            else if (tan > 1)
                            {
                                str_ani = "human_" + global.human[0].user_role_num + "_1";
                                global.human[0].ani_human.Play(str_ani);
                                global.human[0].human_stat = 1;
                            }
                            else if (tan < -1)
                            {
                                str_ani = "human_" + global.human[0].user_role_num + "_2";
                                global.human[0].ani_human.Play(str_ani);
                                global.human[0].human_stat = 2;
                            }
                        }
                        else if ((local_x - vec_control.x) < 0)
                        {
                            if ((tan > -1) && (tan < 1))
                            {
                                str_ani = "human_" + global.human[0].user_role_num + "_3";
                                global.human[0].ani_human.Play(str_ani);
                                global.human[0].human_stat = 3;
                            }
                            else if (tan > 1)
                            {
                                str_ani = "human_" + global.human[0].user_role_num + "_2";
                                global.human[0].ani_human.Play(str_ani);
                                global.human[0].human_stat = 2;
                            }
                            else if (tan < -1)
                            {
                                str_ani = "human_" + global.human[0].user_role_num + "_1";
                                global.human[0].ani_human.Play(str_ani);
                                global.human[0].human_stat = 1;
                            }
                        }
                        else if ((local_x - vec_control.x) == 0)
                        {
                            if ((local_y - vec_control.y) > 0)
                            {
                                str_ani = "human_" + global.human[0].user_role_num + "_1";
                                global.human[0].ani_human.Play(str_ani);
                                global.human[0].human_stat = 1;
                            }
                            else if ((local_y - vec_control.y) < 0)
                            {
                                str_ani = "human_" + global.human[0].user_role_num + "_2";
                                global.human[0].ani_human.Play(str_ani);
                                global.human[0].human_stat = 2;
                            }
                            else
                            {
                                str_ani = "human_" + global.human[0].user_role_num + "_0";
                                global.human[0].ani_human.Play(str_ani);
                                global.human[0].human_stat = 0;
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
                        str_ani = "human_" + global.human[0].user_role_num + "_0";
                        global.human[0].ani_human.Play(str_ani);
                        global.human[0].human_stat = 0;
                    }
                }
            }
        }
    }
}
