using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

//目录
//1.铺第一层地面
//2.铺第二层地砖
//3.必须放的箱子
//4.必须放的怪物
//5.随机放的箱子 包含必须放的物品
//6.随机放的怪物
//7.初始化游戏

public class O_map : MonoBehaviour
{
    Vector3 vec;
    private Sprite[] sprite;

    /*
    //可以使用的炸弹数量
    public int num_boom = 0;
    public int num_pow = 0;
    public int num_len = 0;
    public int num_speed = 0;
    public int num_life = 0;
    */
    //游戏地图文件名
    private string str_tmx;
    /*
    public class Put_box
    {
        public int x;
        public int y;
        //是否有物品，物品是什么
        public bool put_food = false;
        public GameObject food;
    };
    */

    private GameObject map_clone_lay_1;
    private GameObject map_clone_lay_2;

    private int n = 1;

    private GameObject lay_1;
    private GameObject lay_2;

    private void Start()
    {
        //windows平台读取文件换行一次两行，其他平台一行
#if UNITY_EDITOR
        n = 2;
#elif UNITY_ANDROID
    n = 1;
#endif
        //游戏地图文件名
        str_tmx = "online_" + global.online_level.ToString() + ".tmx";
        //游戏地图元素位置
        sprite = Resources.LoadAll<Sprite>("map/" + global.online_map[global.online_level - 1].str_image);

        lay_1 = GameObject.Find("lay_1");
        lay_2 = GameObject.Find("lay_2");

        //加载一下复制体
        map_clone_lay_1 = GameObject.Find("map_clone_lay_1");
        map_clone_lay_2 = GameObject.Find("map_clone_lay_2");

        /***制作地图***/
        //加载地图文件
        string myStr = LoadFile(str_tmx);
        //关于这个特殊的换行符，是每两个一行，所以取值要取 0 2 4 6 ...
        string[] line = myStr.Split(Environment.NewLine.ToCharArray());

        //1.铺第一层地面
        for (int i = 7 * n; i < (7 * n + global.online_map[global.online_level - 1].size_y * n); i = i + n)
        {
            string[] num = line[i].Split(',');
            for (int j = 0; j < global.online_map[global.online_level - 1].size_x; j++)
            {
                //开始计算使用素材编号，并复制粘贴素材
                int k = int.Parse(num[j]) - 1;
                GameObject tile_copy = Instantiate(map_clone_lay_1);
                //重新命名，把clone给去掉
                tile_copy.transform.name = sprite[k].name;
                tile_copy.GetComponent<Image>().sprite = sprite[k];
                tile_copy.transform.SetParent(lay_1.gameObject.transform, true);
                vec.Set(68 * j, -64 * (i - 7 * n) / n, 0);
                tile_copy.transform.localPosition = vec;
            }
        }

        //2.铺第二层地砖
        for (int i = 11 * n + ((int)global.online_map[global.online_level - 1].size_y * n); i < (11 * n + global.online_map[global.online_level - 1].size_y * n * 2); i = i + n)
        {
            string[] num = line[i].Split(',');
            for (int j = 0; j < global.online_map[global.online_level - 1].size_x; j++)
            {
                //开始计算使用素材编号，并复制粘贴素材
                //跳过没有贴图的部分
                if (int.Parse(num[j]) == 0)
                {
                    continue;
                }

                int k = int.Parse(num[j]) - 1;
                //可破坏的箱子
                if (k == global.online_map[global.online_level - 1].num_break_box)
                {
                    GameObject gameobject_box_copy = Instantiate(map_clone_lay_2);
                    vec.Set(68 * j, -64 * (i - 11 * n - (global.online_map[global.online_level - 1].size_y * n)) / n, 0);
                    gameobject_box_copy.transform.SetParent(lay_2.gameObject.transform, true);
                    gameobject_box_copy.GetComponent<Image>().sprite = sprite[k];
                    gameobject_box_copy.name = "map_box_" + k;
                    gameobject_box_copy.transform.localPosition = vec;
                    //更新全局变量
                    global.G_map[j][(i - 11 * n - ((int)global.online_map[global.online_level - 1].size_y * n)) / n].wall = true;
                    global.G_map[j][(i - 11 * n - ((int)global.online_map[global.online_level - 1].size_y * n)) / n].wall_destory = true;
                    //存储
                    global.G_map[j][(i - 11 * n - ((int)global.online_map[global.online_level - 1].size_y * n)) / n].gameobject_destory_wall = gameobject_box_copy;
                }
                else
                {
                    GameObject tile_copy = Instantiate(map_clone_lay_2);

                    tile_copy.transform.name = sprite[k].name;
                    tile_copy.GetComponent<Image>().sprite = sprite[k];
                    tile_copy.transform.SetParent(lay_2.gameObject.transform, true);
                    //这里的Y坐标要用i减去初始值，然后再除以平台倍数
                    vec.Set(68 * j, -64 * (i - 11 * n - (global.online_map[global.online_level - 1].size_y * n)) / n, 0);
                    tile_copy.transform.localPosition = vec;
                    //更新全局变量
                    global.G_map[j][(i - 11 * n - ((int)global.online_map[global.online_level - 1].size_y * n)) / n].wall = true;
                }

            }
        }

        //7.初始化游戏

        //人物移动到初始位置
        for (int i = 0; i < 4; i++)
        {
            global.human[i].x = (global.online_map[global.online_level - 1].human[i].x + 0.5f) * 68;
            global.human[i].y = -(global.online_map[global.online_level - 1].human[i].y - 1.5f) * 64;
            vec.Set(global.human[i].x, global.human[i].y, 0);
            global.human[i].gameobject_human.transform.localPosition = vec;
            if (i == global.my_num)
            {
                global.gameobject_maincamera.transform.localPosition = vec;
            }
        }

        //游戏开始
        global.gameover = 0;
        global.flag_close_send_pthread = true;
    }

    //判断平台读取tile_map文件
    public static string LoadFile(string filePath)
    {
        string url = Application.streamingAssetsPath + "/" + filePath;
#if UNITY_EDITOR
        return File.ReadAllText(url);
#elif UNITY_ANDROID
        WWW www = new WWW(url);
        while (!www.isDone) { }
        return www.text;
#endif
    }

}
