using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//目录
//1.怪物血条生成
//2.怪物碰撞地砖或炸弹
//3.怪物碰到火焰
//4.AI走路

public class monster_1 : MonoBehaviour
{

    private string monster_name;
    private int monster_num;

    private int stat = 2;

    private Vector3 vec;
    private List<int> monster_move_list = new List<int>();
    private Animator ani_monster;

    private Vector3 vec_last;
    private int last_x = 0;
    private int last_y = 0;

    GameObject gameobject_monster_life_full_copy;
    GameObject gameobject_monster_life_empty_copy;

    void Start()
    {
        //1.怪物血条生成
        if (gameObject.name.Contains("copy"))
        {
            //取得怪物在内存中的编号,更新怪物名称
            monster_name = gameObject.name;
            string[] array_name = monster_name.Split('_');
            monster_num = int.Parse(array_name[3]);
            monster_name = array_name[0] + "_" + array_name[1] + "_" + array_name[2];
            //取得怪物动画ani
            ani_monster = gameObject.GetComponent<Animator>();
            //血条复制
            gameobject_monster_life_full_copy  = Instantiate(global.gameobject_monster_life_full);
            gameobject_monster_life_empty_copy = Instantiate(global.gameobject_monster_life_empty);
            gameobject_monster_life_empty_copy.transform.SetParent(global.lay_monster.gameObject.transform, true);
            gameobject_monster_life_full_copy.transform.SetParent(global.lay_monster.gameObject.transform, true);
            gameobject_monster_life_empty_copy.name = global.gameobject_monster_life_empty.name + "_copy";
            gameobject_monster_life_full_copy.name  = global.gameobject_monster_life_full.name + "_copy";

        }
    }

    IEnumerator restore_monster()
    {
        yield return new WaitForSeconds(0.5f);
        int rand = UnityEngine.Random.Range(1, 4);
        stat = rand;
    }

    IEnumerator destory_monster()
    {
        yield return new WaitForSeconds(0.5f);
        if (global.G_MONSTER[monster_num].is_alive)
        {
            global.G_MONSTER[monster_num].is_alive = false;
            //删除这个怪物和它的血条
            Destroy(global.G_MONSTER[monster_num].gameobject_monster);
            Destroy(gameobject_monster_life_full_copy);
            Destroy(gameobject_monster_life_empty_copy);
            //杀死怪物的总数加1
            global.human[0].kill_monster++;
            //更新显示怪物剩余量
            global.text_monster_count.text = "X  " + (global.G_MONSTER.Count - global.human[0].kill_monster);
            //更新经验值和金币值
            global.human[0].get_exp = global.human[0].get_exp + global.G_MONSTER[monster_num].exp;
            global.human[0].get_gold = global.human[0].get_gold + global.G_MONSTER[monster_num].gold;
            if (global.human[0].kill_monster == global.G_MONSTER.Count)
            {
                //胜利！
                global.gameover = 1;
            }
        }
    }

    
    private void OnTriggerEnter2D(Collider2D coll)
    {
        //如果是拷贝的怪物
        if (gameObject.name.Contains("copy"))
        {
            //如果是第一次进来，就要用目前的坐标覆盖，因为。。刚来的时候前一个坐标还是0
            if (vec_last.x == 0)
            {
                vec_last = gameObject.transform.localPosition;
            }
            else
            {
                gameObject.transform.localPosition = vec_last;
            }

            //2.怪物碰撞地砖或炸弹
            if ((coll.name.Contains("map_") || (coll.name.Contains("boom_"))))
            {
                int x = (int)gameObject.transform.position.x / global.length_x;
                int y = (int)((-gameObject.transform.position.y) / global.length_y + 1);

                int flag = 0;
                monster_move_list.Clear();
                //判断有哪些点可以走
                if ((global.G_map[x + 1][y].boom == false) && (global.G_map[x + 1][y].wall == false))
                {
                    flag++;
                    stat = 4;
                    monster_move_list.Add(4);
                }
                if ((global.G_map[x - 1][y].boom == false) && (global.G_map[x - 1][y].wall == false))
                {
                    flag++;
                    stat = 3;
                    monster_move_list.Add(3);
                }
                if ((global.G_map[x][y + 1].boom == false) && (global.G_map[x][y + 1].wall == false))
                {
                    flag++;
                    stat = 2;
                    monster_move_list.Add(2);
                }
                if ((global.G_map[x][y - 1].boom == false) && (global.G_map[x][y - 1].wall == false))
                {
                    flag++;
                    stat = 1;
                    monster_move_list.Add(1);
                }

                if (flag == 0)
                {
                    //没有口的时候，就是不动，没有状态变化
                    stat = 0;
                }
                else if (flag == 1)
                {
                    //有一个口，就往那个口走，状态不变
                }
                else if (flag == 2)
                {
                    //print("碰到以后有2条路，stat随机:" + stat);
                    int rand = UnityEngine.Random.Range(0, monster_move_list.Count);
                    stat = monster_move_list[rand];
                }
                else if (flag == 3)
                {
                    //碰到以后有3条路，stat随机一个路;
                    int rand = UnityEngine.Random.Range(0, monster_move_list.Count);
                    stat = monster_move_list[rand];
                }
            }
        }

        if (global.gameover == 0)
        {
            if ((stat != 5) && (stat != 6))
            {
                //3.怪物碰到火焰
                if (coll.name.Contains("water"))
                {
                    //怪物掉血
                    global.G_MONSTER[monster_num].life_now = global.G_MONSTER[monster_num].life_now - global.human[0].pow;
                    if (global.G_MONSTER[monster_num].life_now < 0)
                    {
                        global.G_MONSTER[monster_num].life_now = 0;
                    }

                    if (global.G_MONSTER[monster_num].life_now <= 0)
                    {
                        //判断没有结束
                        string str = monster_name + "_5";
                        stat = 5;
                        ani_monster.Play(str);
                        StartCoroutine(destory_monster());
                    }
                    else
                    {
                        stat = 6;
                        string str = monster_name + "_6";
                        ani_monster.Play(str);
                        StartCoroutine(restore_monster());
                    }
                }

                //怪物碰到玩家
                if (coll.name.Contains("human"))
                {
                    //玩家掉血
                    global.human[global.my_num].life_now = global.human[global.my_num].life_now - global.G_MONSTER[monster_num].pow;
                    string str_ani = "human_" + global.human[global.my_num].user_role_num + "_6";
                    global.human[global.my_num].human_stat = 6;
                    global.human[global.my_num].ani_human.Play(str_ani);
                    global.human[global.my_num].wudi = true;
                    global.audio_sound_hit.Play();
                }
            }
        }
    }

    //4.AI走路
    void Update()
    {
        /*当怪物走到三岔路和四叉路的时候，就要做一个随机路口转向的判断*/
        if (gameObject.name.Contains("copy"))
        {
            if (global.gameover == 0)
            {
                if (global.G_MONSTER[monster_num].is_alive)
                {

                    //血条放置
                    vec.Set(gameObject.transform.localPosition.x - 34, gameObject.transform.localPosition.y + 32, 0);
                    gameobject_monster_life_empty_copy.transform.localPosition = vec;
                    gameobject_monster_life_full_copy.transform.localPosition = vec;
                    vec.Set(global.G_MONSTER[monster_num].life_now / (float)global.G_MONSTER[monster_num].life_full, 1, 1);
                    gameobject_monster_life_full_copy.transform.localScale = vec;
                    if ((stat != 5) && (stat != 6))
                    {
                        //计算中点偏差0.45 - 0.55之间，认为怪物走到了中点
                        float x_f = gameObject.transform.position.x / global.length_x;
                        float y_f = ((-gameObject.transform.position.y) / global.length_y + 1);

                        float tmp_x = x_f % 1.0f;
                        float tmp_y = y_f % 1.0f;

                        if ((tmp_x < 0.55) && (tmp_x > 0.45) && (tmp_y < 0.55) && (tmp_y > 0.45))
                        {
                            int x = (int)x_f;
                            int y = (int)y_f;
                            //这次的中心点不是上一次所在的中心点
                            if ((x != last_x) || (y != last_y))
                            {
                                //print("和上一个点不同，更新到上一个点:" + x + "y" + y);
                                last_x = x;
                                last_y = y;
                                int flag = 0;
                                monster_move_list.Clear();
                                if ((global.G_map[x + 1][y].boom == false) && (global.G_map[x + 1][y].wall == false))
                                {
                                    flag++;
                                    monster_move_list.Add(4);
                                }
                                if ((global.G_map[x - 1][y].boom == false) && (global.G_map[x - 1][y].wall == false))
                                {
                                    flag++;
                                    monster_move_list.Add(3);
                                }
                                if ((global.G_map[x][y + 1].boom == false) && (global.G_map[x][y + 1].wall == false))
                                {
                                    flag++;
                                    monster_move_list.Add(2);
                                }
                                if ((global.G_map[x][y - 1].boom == false) && (global.G_map[x][y - 1].wall == false))
                                {
                                    flag++;
                                    monster_move_list.Add(1);
                                }
                                if (flag == 3)
                                {
                                    //随机一条路
                                    int rand = UnityEngine.Random.Range(0, monster_move_list.Count);
                                    stat = monster_move_list[rand];
                                }
                                else if (flag == 4)
                                {
                                    //随机一条路
                                    int rand = UnityEngine.Random.Range(0, monster_move_list.Count);
                                    stat = monster_move_list[rand];
                                }
                            }
                        }

                        //怪物行走
                        if (stat == 1)
                        {
                            //print("上走");
                            vec_last = gameObject.transform.localPosition;
                            string str_ani = monster_name + "_1";
                            ani_monster.Play(str_ani);
                            vec.Set(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + global.G_MONSTER[monster_num].speed, 0);
                            gameObject.transform.localPosition = vec;
                        }
                        else if (stat == 2)
                        {
                            //print("下走");
                            vec_last = gameObject.transform.localPosition;
                            string str_ani = monster_name + "_2";
                            ani_monster.Play(str_ani);
                            vec.Set(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - global.G_MONSTER[monster_num].speed, 0);
                            gameObject.transform.localPosition = vec;
                        }
                        else if (stat == 3)
                        {
                            //print("左走");
                            vec_last = gameObject.transform.localPosition;
                            string str_ani = monster_name + "_3";
                            ani_monster.Play(str_ani);
                            vec.Set(gameObject.transform.localPosition.x - global.G_MONSTER[monster_num].speed, gameObject.transform.localPosition.y, 0);
                            gameObject.transform.localPosition = vec;
                        }
                        else if (stat == 4)
                        {
                            //print("右走");
                            vec_last = gameObject.transform.localPosition;
                            string str_ani = monster_name + "_4";
                            ani_monster.Play(str_ani);
                            vec.Set(gameObject.transform.localPosition.x + global.G_MONSTER[monster_num].speed, gameObject.transform.localPosition.y, 0);
                            gameObject.transform.localPosition = vec;
                        }
                        else
                        {
                            //print("战立");
                            string str_ani = monster_name + "_2";
                            ani_monster.Play(str_ani);
                        }
                    }
                }
            }
        }
    }
}
