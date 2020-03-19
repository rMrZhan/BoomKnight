using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//目录
//1.按下炸弹
//2.炸弹爆炸并产生火焰

public class P_boom : MonoBehaviour
{
    private Vector3 vec;

    //2.炸弹爆炸并产生火焰
    IEnumerator destory_boom(string boom_name, int x, int y)
    {
        yield return new WaitForSeconds(global.human[0].human_time_boom);
        //销毁炸弹
        Destroy(GameObject.Find(boom_name));
        //将内存更新
        global.G_map[x][y].boom = false;
        //该人物共计摆放的炸弹数量减少一个
        global.human[0].num_boom_now--;

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
        //print(int_tmp + "长度" + global.human.len);
        for (int_tmp = 0; int_tmp <= global.human[0].len; int_tmp++)
        {
            //print("ok");
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
                    if (int_tmp != global.human[0].len)
                    {
                        //如果这个方向有箱子且不可以被破坏
                        if ((global.G_map[x][y - (int_tmp + 1)].wall == true) && (global.G_map[x][y - (int_tmp + 1)].wall_destory == false))
                        {
                            //下一步会撞墙，生成火焰头而非身体
                            water_copy.water_up_head = Instantiate(global.gameobject_water_up_head);
                            water_copy.water_up_head.transform.SetParent(global.lay_water.gameObject.transform, true);
                            water_copy.water_up_head.name = "water_up_head_" + global.G_map[x][y].boom_num.ToString();
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
                            water_up_body_copy.name = "water_up_body_" + global.G_map[x][y].boom_num.ToString();
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
                        water_destory_copy.name = "water_destory_" + global.G_map[x][y + int_tmp].boom_num.ToString();
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
                    if (int_tmp != global.human[0].len)
                    {
                        if ((global.G_map[x][y + (int_tmp + 1)].wall == true) && (global.G_map[x][y + (int_tmp + 1)].wall_destory == false))
                        {
                            //下一步会撞墙，生成火焰头而非身体
                            GameObject water_down_head_copy = Instantiate(global.gameobject_water_down_head);
                            water_down_head_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                            water_down_head_copy.name = "water_down_head_" + global.G_map[x][y].boom_num.ToString();
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
                            water_down_body_copy.name = "water_down_body_" + global.G_map[x][y].boom_num.ToString();
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
                        water_destory_copy.name = "water_destory_" + global.G_map[x - int_tmp][y].boom_num.ToString();
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
                    if (int_tmp != global.human[0].len)
                    {
                        if ((global.G_map[x - (int_tmp + 1)][y].wall == true) && (global.G_map[x - (int_tmp + 1)][y].wall_destory == false))
                        {
                            //下一步会撞墙，生成火焰头而非身体
                            GameObject water_left_head_copy = Instantiate(global.gameobject_water_left_head);
                            water_left_head_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                            water_left_head_copy.name = "water_left_head_" + global.G_map[x][y].boom_num.ToString();
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
                            water_left_body_copy.name = "water_left_body_" + global.G_map[x][y].boom_num.ToString();
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
                        water_destory_copy.name = "water_destory_" + global.G_map[x + int_tmp][y].boom_num.ToString();
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
                    if (int_tmp != global.human[0].len)
                    {
                        if ((global.G_map[x + (int_tmp + 1)][y].wall == true) && (global.G_map[x + (int_tmp + 1)][y].wall_destory == false))
                        {
                            //下一步会撞墙，生成火焰头而非身体
                            GameObject water_right_head_copy = Instantiate(global.gameobject_water_right_head);
                            water_right_head_copy.transform.SetParent(global.lay_water.gameObject.transform, true);
                            water_right_head_copy.name = "water_right_head_" + global.G_map[x][y].boom_num.ToString();
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
                            water_right_body_copy.name = "water_right_body_" + global.G_map[x][y].boom_num.ToString();
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
                water_destory_copy.name = "water_destory_" + global.G_map[tmp_x][tmp_y].boom_num.ToString();
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
                water_up_head_copy.name = "water_up_head_" + global.G_map[x][y].boom_num.ToString();
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
                water_destory_copy.name = "water_destory_" + global.G_map[tmp_x][tmp_y].boom_num.ToString();
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
                water_down_head_copy.name = "water_down_head_" + global.G_map[x][y].boom_num.ToString();

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
                water_destory_copy.name = "water_destory_" + global.G_map[tmp_x][tmp_y].boom_num.ToString();
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
                water_left_head_copy.name = "water_left_head_" + global.G_map[x][y].boom_num.ToString();
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
                water_destory_copy.name = "water_destory_" + global.G_map[tmp_x][tmp_y].boom_num.ToString();
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
                water_right_head_copy.name = "water_right_head_" + global.G_map[x][y].boom_num.ToString();
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
        water_middle_copy.name = "water_middle_" + global.G_map[x][y].boom_num.ToString();
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
        water_copy.len = global.human[0].len;

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

    //1.按下炸弹
    public void put_boom()
    {
        //游戏并没有结束
        //网络处于连接状态
        if (global.gameover == 0)
        {
            int x = (int)((global.human[0].gameobject_human.transform.localPosition.x) / global.length_x);
            //1,0 -> 1,-1 这个是计算坐标转换，如果要给玩家看的坐标，就不是这个了 要把Y写成正数即可
            int y = (int)((global.human[0].gameobject_human.transform.localPosition.y) / global.length_y) - 1;

            //检查这里是否有炸弹
            if (global.G_map[x][-y].boom == false)
            {
                //检查该人物的总炸弹数量是否达到最大值
                if (global.human[0].num_boom_now < global.human[0].num_boom)
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
                    global.human[0].num_boom_now++;
                    StartCoroutine(destory_boom(boom_copy.name, x, -y));
                }
            }
        }
    }


    //Debug专用函数
    public static void debug(string debug)
    {
        GameObject debug_text = GameObject.Find("Debug");
        debug_text.GetComponent<Text>().text = debug;
    }
}
