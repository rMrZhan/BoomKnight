using UnityEngine;

public class O_coll_food : MonoBehaviour
{
    //各种物品的名称
    public string str_boom = "food_boom";
    public string str_len = "food_len";
    public string str_pow = "food_pow";
    public string str_speed = "food_speed";
    public string str_life = "food_life";

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //必须是人来碰
        if (coll.name.Contains("human"))
        {
            string[] array = coll.name.Split('_');
            int human_num = int.Parse(array[1]);
            print("吃到物品的人为" + human_num);
            global.audio_sound_food.Play();
            //当碰到的东西是炸弹的时候
            if (gameObject.name == str_boom)
            {
                //这个人拥有的炸弹增加
                global.human[human_num].num_boom++;
                //吃掉以后删除这个物品
                GameObject.Destroy(gameObject);
            }
            //当碰到的东西是火焰长度
            else if (gameObject.name == str_len)
            {
                //这个人拥有的炸弹增加
                global.human[human_num].len++;
                //吃掉以后删除这个物品
                GameObject.Destroy(gameObject);
            }
            //当碰到的东西是威力
            else if (gameObject.name == str_pow)
            {
                //这个人拥有的炸弹增加
                global.human[human_num].pow++;
                //吃掉以后删除这个物品
                GameObject.Destroy(gameObject);
            }
            //当碰到的东西是速度
            else if (gameObject.name == str_speed)
            {
                //这个人拥有的炸弹增加
                global.human[human_num].speed = global.human[human_num].speed + 0.5f;
                //吃掉以后删除这个物品
                GameObject.Destroy(gameObject);
            }
            //当碰到的东西是生命
            else if (gameObject.name == str_life)
            {
                //补血 20以下给5点 40以下给10点 60以下给15 80以下给20 其他给30
                if (global.human[human_num].life_now < 20)
                {
                    global.human[human_num].life_now = global.human[human_num].life_now + 5;
                    if (global.human[human_num].life_now > global.human[human_num].life)
                    {
                        global.human[human_num].life_now = global.human[human_num].life;
                    }
                }
                else if (global.human[human_num].life_now < 40)
                {
                    global.human[human_num].life_now = global.human[human_num].life_now + 10;
                    if (global.human[human_num].life_now > global.human[human_num].life)
                    {
                        global.human[human_num].life_now = global.human[human_num].life;
                    }
                }
                else if (global.human[human_num].life_now < 60)
                {
                    global.human[human_num].life_now = global.human[human_num].life_now + 15;
                    if (global.human[human_num].life_now > global.human[human_num].life)
                    {
                        global.human[human_num].life_now = global.human[human_num].life;
                    }
                }
                else if (global.human[human_num].life_now < 80)
                {
                    global.human[human_num].life_now = global.human[human_num].life_now + 20;
                    if (global.human[human_num].life_now > global.human[human_num].life)
                    {
                        global.human[human_num].life_now = global.human[human_num].life;
                    }
                }
                else
                {
                    global.human[human_num].life_now = global.human[human_num].life_now + 30;
                    if (global.human[human_num].life_now > global.human[human_num].life)
                    {
                        global.human[human_num].life_now = global.human[human_num].life;
                    }
                }
                //吃掉以后删除这个物品
                GameObject.Destroy(gameObject);
            }
        }
    }
}
