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

public class map : MonoBehaviour
{
    Vector3 vec;
    private Sprite[] sprite;
    //加载的地图文件
    public int num_level = 1;
    //地图长宽
    public Vector2 size = new Vector2(22, 17);
    //怪物摆放 monster[怪物编号] = 数量    0号是小鸡
    public int[] monster;
    //可以破坏箱子的名称
    public string str_map = "map_2";
    //开始游戏时人物坐标
    public Vector2 vec_start_point = new Vector2(8, 8);
    //可以破坏箱子的编号
    public int num_break_box = 6;
    //一共放置的箱子数量
    public int num_box = 1;
    //开始和结束放箱子的位置
    public Vector2 start_put_box = new Vector2(8, 8);
    public Vector2 end_put_box = new Vector2(13, 10);
    //开始和结束放怪物的位置
    public Vector2 start_put_monster = new Vector2(10, 8);
    public Vector2 end_put_monster = new Vector2(13, 10);
    //不允许放箱子的位置
    public Vector2[] no_box = new Vector2[3];
    //一定要放箱子的位置，注意不要和不允许放置的位置冲突
    public Vector2[] must_box = new Vector2[0];
    //一定要放怪物的位置 x y 怪物种类
    public Vector3[] must_monster;
    //可以使用的炸弹数量
    public int num_boom = 0;
    public int num_pow = 0;
    public int num_len = 0;
    public int num_speed = 0;
    public int num_life = 0;
    //游戏时间
    public int time = 120;
    //游戏地图文件名
    private string str_tmx;

    public class Put_box
    {
        public int x;
        public int y;
        //是否有物品，物品是什么
        public bool put_food = false;
        public GameObject food;
    };

    public class Put_monster
    {
        public int x;
        public int y;
    };
    //用一个数组来统计可以摆放箱子的位置
    private List<Put_box> put_box = new List<Put_box>();
    //用一个数组来统计可以摆放怪物的位置
    private List<Put_monster> put_monster = new List<Put_monster>();
    private GameObject map_clone_lay_1;
    private GameObject map_clone_lay_2;

    //换行数量，针对各平台设计
    private int n = 1;
    //为了节省内存，先把Lay_2找出来
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
        str_tmx = "level_" + num_level.ToString() + ".tmx";
        //游戏地图元素位置
        sprite = Resources.LoadAll<Sprite>("map/" + str_map);

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
        for (int i = 7 * n; i < (7 * n + size.y * n); i = i + n)
        {
            string[] num = line[i].Split(',');
            for (int j = 0; j < size.x; j++)
            {
                //开始计算使用素材编号，并复制粘贴素材
                int k = int.Parse(num[j]) - 1;
                GameObject tile_copy = Instantiate(map_clone_lay_1);
                //重新命名，把clone给去掉
                tile_copy.transform.name = sprite[k].name;
                tile_copy.GetComponent<Image>().sprite = sprite[k];
                tile_copy.transform.SetParent(GameObject.Find("lay_1").gameObject.transform, true);
                vec.Set(68 * j, -64 * (i - 7 * n) / n, 0);
                tile_copy.transform.localPosition = vec;
            }
        }
        //2.铺第二层地砖
        for (int i = 11 * n + ((int)size.y * n); i < (11 * n + size.y * n * 2); i = i + n)
        {
            print(line[i]);
            string[] num = line[i].Split(',');
            for (int j = 0; j < size.x; j++)
            {
                //开始计算使用素材编号，并复制粘贴素材
                //跳过没有贴图的部分
                if (int.Parse(num[j]) == 0)
                {
                    continue;
                }
                int k = int.Parse(num[j]) - 1;
                GameObject tile_copy = Instantiate(map_clone_lay_2);

                tile_copy.transform.name = sprite[k].name;
                tile_copy.GetComponent<Image>().sprite = sprite[k];
                tile_copy.transform.SetParent(lay_2.gameObject.transform, true);
                //这里的Y坐标要用i减去初始值，然后再除以平台倍数
                vec.Set(68 * j, -64 * (i - 11 * n - (size.y * n)) / n, 0);
                tile_copy.transform.localPosition = vec;
                //更新全局变量
                global.G_map[j][(i - 11 * n - ((int)size.y * n)) / n].wall = true;
            }
        }
        //3.必须放的箱子
        //执行必须要放置箱子的位置
        for (int i = 0; i < must_box.Length; i++)
        {
            if (global.G_map[(int)must_box[i].x][(int)must_box[i].y].wall == false)
            {
                GameObject gameobject_box_copy = Instantiate(map_clone_lay_2);
                vec.Set(68 * must_box[i].x, -64 * must_box[i].y, 0);
                gameobject_box_copy.transform.SetParent(lay_2.gameObject.transform, true);
                gameobject_box_copy.GetComponent<Image>().sprite = sprite[num_break_box];
                gameobject_box_copy.name = "map_box_" + num_break_box;
                gameobject_box_copy.transform.localPosition = vec;
                //更新全局变量
                global.G_map[(int)must_box[i].x][(int)must_box[i].y].wall = true;
                global.G_map[(int)must_box[i].x][(int)must_box[i].y].wall_destory = true;
                //存储
                global.G_map[(int)must_box[i].x][(int)must_box[i].y].gameobject_destory_wall = gameobject_box_copy;
            }
        }
        //4.必须放的怪物
        for (int i = 0; i < must_monster.Length; i++)
        {

            string str = "monster_" + (must_monster[i].z + 1).ToString();
            int monster_num = (int)must_monster[i].z;
            GameObject gameobject_monster = GameObject.Find(str);
            //克隆
            GameObject gameobject_monster_copy = Instantiate(gameobject_monster);
            gameobject_monster_copy.transform.SetParent(global.lay_monster.gameObject.transform, true);
            //摆放
            vec.Set(68 * (must_monster[i].x + 0.5f), -64 * (must_monster[i].y - 0.5f), 0);
            gameobject_monster_copy.transform.localPosition = vec;
            gameobject_monster_copy.name = str + "_copy_" + global.monster_number;
            //属性
            global.Monster mon = new global.Monster();
            mon.name = global.MONSTER[monster_num].name;
            mon.life_now = global.MONSTER[monster_num].life_full * global.hard;
            mon.life_full = global.MONSTER[monster_num].life_full * global.hard;
            mon.speed = global.MONSTER[monster_num].speed;
            if (global.hard == 2)
            {
                mon.speed = global.MONSTER[monster_num].speed * 1.5f;
            }
            else if (global.hard == 3)
            {
                mon.speed = global.MONSTER[monster_num].speed * 2;
            }
            mon.pow = global.MONSTER[monster_num].pow * global.hard;
            mon.exp = global.MONSTER[monster_num].exp * global.hard;
            mon.gold = global.MONSTER[monster_num].gold * global.hard;
            mon.gameobject_monster = gameobject_monster_copy;
            global.G_MONSTER.Add(mon);
            global.monster_number++;
        }
        //5.随机放的箱子 包含必须放的物品
        for (int i = (int)start_put_box.x; i <= end_put_box.x; i++)
        {
            for (int j = (int)start_put_box.y; j <= end_put_box.y; j++)
            {
                if (global.G_map[i][j].wall == false)
                {
                    //检查是否不允许这样放
                    int flag_no = 0;
                    int size = no_box.Length;
                    for (int a = 0; a < size; a++)
                    {
                        if ((no_box[a].x == i) && (no_box[a].y == j))
                        {
                            flag_no = 1;
                            break;
                        }
                    }

                    if (flag_no != 1)
                    {
                        Put_box tmp = new Put_box();
                        tmp.x = i;
                        tmp.y = j;
                        put_box.Add(tmp);
                    }
                }
            }
        }
        //可以放置的位置不为空
        if (put_box.Count != 0)
        {
            //开始随机铺设箱子
            double put_box_count = put_box.Count;
            double seed_box = (double)(num_box / put_box_count);

            //计算一下随机物品的位置 炸弹
            for (int i = 0; i < num_boom; i++)
            {
                int rand = UnityEngine.Random.Range(0, put_box.Count);
                if (put_box[rand].put_food == false)
                {
                    put_box[rand].put_food = true;
                    put_box[rand].food = global.gameobject_food_boom;
                }
                else
                {
                    //如果不巧，遇到了这个箱子已经有了东西，那我们需要重新随机数
                    i--;
                }
            }
            //计算一下随机物品的位置 威力
            for (int i = 0; i < num_pow; i++)
            {
                int rand = UnityEngine.Random.Range(0, put_box.Count);
                if (put_box[rand].put_food == false)
                {
                    put_box[rand].put_food = true;
                    put_box[rand].food = global.gameobject_food_pow;
                }
                else
                {
                    //如果不巧，遇到了这个箱子已经有了东西，那我们需要重新随机数
                    i--;
                }
            }
            //计算一下随机物品的位置 火焰长度
            for (int i = 0; i < num_len; i++)
            {
                int rand = UnityEngine.Random.Range(0, put_box.Count);
                if (put_box[rand].put_food == false)
                {
                    put_box[rand].put_food = true;
                    put_box[rand].food = global.gameobject_food_len;
                }
                else
                {
                    //如果不巧，遇到了这个箱子已经有了东西，那我们需要重新随机数
                    i--;
                }
            }
            //计算一下随机物品的位置 速度
            for (int i = 0; i < num_speed; i++)
            {
                int rand = UnityEngine.Random.Range(0, put_box.Count);
                if (put_box[rand].put_food == false)
                {
                    put_box[rand].put_food = true;
                    put_box[rand].food = global.gameobject_food_speed;
                }
                else
                {
                    //如果不巧，遇到了这个箱子已经有了东西，那我们需要重新随机数
                    i--;
                }
            }
            //计算一下随机物品的位置 生命
            for (int i = 0; i < num_life; i++)
            {
                int rand = UnityEngine.Random.Range(0, put_box.Count);
                if (put_box[rand].put_food == false)
                {
                    put_box[rand].put_food = true;
                    put_box[rand].food = global.gameobject_food_life;
                }
                else
                {
                    //如果不巧，遇到了这个箱子已经有了东西，那我们需要重新随机数
                    i--;
                }
            }

            //随机放置箱子，如果这个箱子里面有东西，那么就必须放
            for (int i = 0; i < put_box_count; i++)
            {
                if (put_box[i].put_food == true)
                {
                    //先放物品
                    GameObject gameobject_food_copy = Instantiate(put_box[i].food);
                    vec.Set(68 * (put_box[i].x + 0.5f), -64 * (put_box[i].y - 0.5f), 0);
                    gameobject_food_copy.transform.SetParent(GameObject.Find("food").gameObject.transform, true);
                    gameobject_food_copy.name = put_box[i].food.name;
                    gameobject_food_copy.transform.localPosition = vec;
                    //一定要放箱子
                    GameObject gameobject_box_copy = Instantiate(map_clone_lay_2);
                    gameobject_box_copy.GetComponent<Image>().sprite = sprite[num_break_box];
                    gameobject_box_copy.transform.SetParent(lay_2.gameObject.transform, true);
                    vec.Set(68 * put_box[i].x, -64 * put_box[i].y, 0);
                    gameobject_box_copy.name = "map_box_" + num_break_box;
                    gameobject_box_copy.transform.localPosition = vec;
                    //更新全局变量
                    global.G_map[put_box[i].x][put_box[i].y].wall = true;
                    global.G_map[put_box[i].x][put_box[i].y].wall_destory = true;
                    //存储
                    global.G_map[put_box[i].x][put_box[i].y].gameobject_destory_wall = gameobject_box_copy;
                }
                else
                {
                    int rand = UnityEngine.Random.Range(0, 10000);
                    if (rand < seed_box * 10000)
                    {
                        //随机成功，摆放箱子
                        GameObject gameobject_box_copy = Instantiate(map_clone_lay_2);
                        vec.Set(68 * put_box[i].x, -64 * put_box[i].y, 0);
                        gameobject_box_copy.transform.SetParent(lay_2.gameObject.transform, true);
                        gameobject_box_copy.GetComponent<Image>().sprite = sprite[num_break_box];
                        gameobject_box_copy.name = "map_box_" + num_break_box;
                        gameobject_box_copy.transform.localPosition = vec;
                        //更新全局变量
                        global.G_map[put_box[i].x][put_box[i].y].wall = true;
                        global.G_map[put_box[i].x][put_box[i].y].wall_destory = true;
                        //存储
                        global.G_map[put_box[i].x][put_box[i].y].gameobject_destory_wall = gameobject_box_copy;
                    }
                }
            }
        }
        
        //6.随机放的怪物
        for (int i = (int)start_put_monster.x; i <= end_put_monster.x; i++)
        {
            for (int j = (int)start_put_monster.y; j <= end_put_monster.y; j++)
            {
                if (global.G_map[i][j].wall == false)
                {
                    //检查是否不允许这样放
                    int flag_no = 0;
                    int size = no_box.Length;
                    for (int a = 0; a < size; a++)
                    {
                        if ((no_box[a].x == i) && (no_box[a].y == j))
                        {
                            flag_no = 1;
                            break;
                        }
                    }

                    if (flag_no != 1)
                    {
                        Put_monster tmp = new Put_monster();
                        tmp.x = i;
                        tmp.y = j;
                        put_monster.Add(tmp);
                    }
                }
            }
        }

        if (put_monster.Count != 0)
        {
            //定义编号，这个编号就是整个怪物的编号
            for (int i = 0; i < monster.Length; i++)
            {
                //如果这个怪物没有数量，换下一种怪物
                if (monster[i] == 0)
                {
                    continue;
                }
                else
                {
                    string str = "monster_" + (i + 1).ToString();
                    //找到这个怪物的克隆object
                    GameObject gameobject_monster = GameObject.Find(str);
                    for (int j = 0; j < monster[i]; j++)
                    {
                        //如果拿到报文头不是需要的，杀死游戏
                        //计算随机数
                        int rand = UnityEngine.Random.Range(0, put_monster.Count);
                        //克隆
                        GameObject gameobject_monster_copy = Instantiate(gameobject_monster);
                        gameobject_monster_copy.transform.SetParent(global.lay_monster.gameObject.transform, true);
                        //摆放
                        vec.Set(68 * (put_monster[rand].x + 0.5f), -64 * (put_monster[rand].y - 0.5f), 0);
                        gameobject_monster_copy.transform.localPosition = vec;
                        gameobject_monster_copy.name = str + "_copy_" + global.monster_number;
                        //更新属性
                        global.Monster mon = new global.Monster();
                        mon.name = global.MONSTER[i].name;
                        mon.life_now = global.MONSTER[i].life_full * global.hard;
                        mon.life_full = global.MONSTER[i].life_full * global.hard;
                        mon.speed = global.MONSTER[i].speed;
                        if (global.hard == 2)
                        {
                            mon.speed = global.MONSTER[i].speed * 1.5f;
                        }
                        else if (global.hard == 3)
                        {
                            mon.speed = global.MONSTER[i].speed * 2;
                        }
                        mon.pow = global.MONSTER[i].pow * global.hard;
                        mon.exp = global.MONSTER[i].exp * global.hard;
                        mon.gold = global.MONSTER[i].gold * global.hard;
                        mon.gameobject_monster = gameobject_monster_copy;
                        global.G_MONSTER.Add(mon);
                        global.monster_number++;
                    }
                }
            }
        }
        
        //7.初始化游戏
        global.time = time;
        global.text_monster_count.text = "X  " + global.G_MONSTER.Count;
        //镜头移动到初始位置
        global.human[0].x = (vec_start_point.x + 0.5f) * 68;
        global.human[0].y = -(vec_start_point.y - 1.5f) * 64;
        vec.Set(global.human[0].x, global.human[0].y, 0);
        global.gameobject_maincamera.transform.localPosition = vec;
        //人物移动到初始位置
        global.human[0].gameobject_human.transform.localPosition = vec;
        //游戏开始
        global.gameover = 0;
        //任务板
        vec.Set(0, 0, 0);
        global.gameobject_mission.transform.localPosition = vec;
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
