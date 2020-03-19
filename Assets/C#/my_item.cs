using UnityEngine;
using UnityEngine.UI;

public class my_item : MonoBehaviour
{

    //移动后的最大Y坐标
    public float image_1_end_x = -140;
    public float image_3_end_x = -140;
    public float button_1_end_x = 265;
    public float button_2_end_x = 265;
    public float button_3_end_x = 265;
    public float button_4_end_x = 265;
    //重要的选择
    public static int s5_choose = 0;
    //移动窗口的速度
    public float speed = 5;
    public float light_speed = 2;
    //临时变量
    private Vector3 vec;
    private GameObject gameobject_image_1;
    private GameObject gameobject_image_3;
    private GameObject gameobject_button_1;
    private GameObject gameobject_button_2;
    private GameObject gameobject_button_3;
    private GameObject gameobject_button_4;
    private GameObject gameobject_light_1;

    private GameObject text_text_level;
    private GameObject text_text_exp;
    private GameObject text_text_name;
    private GameObject text_text_life;
    private GameObject text_text_boom;
    private GameObject text_text_pow;
    private GameObject text_text_speed;
    private GameObject text_text_len;
    private GameObject text_text_gold;


    private GameObject text_button_1;
    private Text text_text_button_1;

    private Text text_text_text_level;
    private Text text_text_text_exp;
    private Text text_text_text_name;
    private Text text_text_text_life;
    private Text text_text_text_boom;
    private Text text_text_text_pow;
    private Text text_text_text_speed;
    private Text text_text_text_len;
    private Text text_text_text_gold;
    void Start()
    {
        global.my_num = 0;
        gameobject_image_1 = GameObject.Find("image_1");
        gameobject_image_3 = GameObject.Find("image_3");
        gameobject_button_1 = GameObject.Find("button_1");
        gameobject_button_2 = GameObject.Find("button_2");
        gameobject_button_3 = GameObject.Find("button_3");
        gameobject_button_4 = GameObject.Find("button_4");
        gameobject_light_1 = GameObject.Find("light_1");

        //等级获取
        text_text_level = GameObject.Find("text_text_level");
        //经验获取
        text_text_exp = GameObject.Find("text_text_exp");
        //昵称获取
        text_text_name = GameObject.Find("text_name");
        //生命获取
        text_text_life = GameObject.Find("text_text_life");
        //炸弹数量获取
        text_text_boom = GameObject.Find("text_text_boom");
        //伤害获取
        text_text_pow = GameObject.Find("text_text_pow");
        //火焰长度获取
        text_text_len = GameObject.Find("text_text_len");
        //移动速度获取
        text_text_speed = GameObject.Find("text_text_speed");
        //金币获取
        text_text_gold = GameObject.Find("text_text_gold");

        text_button_1 = GameObject.Find("text_button_1");
        text_text_button_1 = text_button_1.GetComponent<Text>();

        text_text_text_level = text_text_level.GetComponent<Text>();
        text_text_text_exp = text_text_exp.GetComponent<Text>();
        text_text_text_name = text_text_name.GetComponent<Text>();
        text_text_text_life = text_text_life.GetComponent<Text>();
        text_text_text_boom = text_text_boom.GetComponent<Text>();
        text_text_text_pow = text_text_pow.GetComponent<Text>();
        text_text_text_len = text_text_len.GetComponent<Text>();
        text_text_text_speed = text_text_speed.GetComponent<Text>();
        text_text_text_gold = text_text_gold.GetComponent<Text>();
    }


    public void push()
    {
        if (s5_choose != 1)
        {
            s5_choose = 1;
            text_text_button_1.text = "我的信息";
        }
        else
        {
            s5_choose = 0;
            text_text_button_1.text = "我的人物";
        }
    }

    void Update()
    {
        //当什么都不选的时候，显示属性界面
        if (s5_choose == 0)
        {
            //场景5并拢视觉效果
            if (gameobject_image_3.transform.localPosition.x > -700)
            {
                vec.Set(gameobject_image_3.transform.localPosition.x - speed, gameobject_image_3.transform.localPosition.y, 0);
                gameobject_image_3.transform.localPosition = vec;
            }
            else
            {
                if (gameobject_image_1.transform.localPosition.x < image_1_end_x)
                {
                    vec.Set(gameobject_image_1.transform.localPosition.x + speed, gameobject_image_1.transform.localPosition.y, 0);
                    gameobject_image_1.transform.localPosition = vec;
                }
                if (gameobject_button_1.transform.localPosition.x > button_1_end_x)
                {
                    vec.Set(gameobject_button_1.transform.localPosition.x - speed, gameobject_button_1.transform.localPosition.y, 0);
                    gameobject_button_1.transform.localPosition = vec;
                }
                if (gameobject_button_2.transform.localPosition.x > button_2_end_x)
                {
                    vec.Set(gameobject_button_2.transform.localPosition.x - speed, gameobject_button_2.transform.localPosition.y, 0);
                    gameobject_button_2.transform.localPosition = vec;
                }
                if (gameobject_button_3.transform.localPosition.x > button_3_end_x)
                {
                    vec.Set(gameobject_button_3.transform.localPosition.x - speed, gameobject_button_3.transform.localPosition.y, 0);
                    gameobject_button_3.transform.localPosition = vec;
                }
                if (gameobject_button_4.transform.localPosition.x > button_4_end_x)
                {
                    vec.Set(gameobject_button_4.transform.localPosition.x - speed, gameobject_button_4.transform.localPosition.y, 0);
                    gameobject_button_4.transform.localPosition = vec;
                }
            }
        }
        //当选择我的人物的时候，就要把属性界面挪回去
        if (s5_choose == 1)
        {
            if (gameobject_image_1.transform.localPosition.x > -614)
            {
                vec.Set(gameobject_image_1.transform.localPosition.x - speed, gameobject_image_1.transform.localPosition.y, 0);
                gameobject_image_1.transform.localPosition = vec;
            }
            else
            {
                if (gameobject_image_3.transform.localPosition.x < image_3_end_x)
                {
                    vec.Set(gameobject_image_3.transform.localPosition.x + speed, gameobject_image_3.transform.localPosition.y, 0);
                    gameobject_image_3.transform.localPosition = vec;
                }
            }
        }

        //实时更新属性
        //等级
        string str_level = "Lv " + global.human[global.my_num].user_level;
        text_text_text_level.text = str_level;
        //经验
        string str_exp = global.human[global.my_num].user_exp.ToString();
        text_text_text_exp.text = str_exp;
        //昵称
        string str_name = global.human[global.my_num].user_name;
        text_text_text_name.text = str_name;
        //生命
        string str_life = global.human[global.my_num].human_life + "/" + global.human[global.my_num].human_life;
        text_text_text_life.text = str_life;
        //炸弹数量
        string str_num_boom = "X " + global.human[global.my_num].human_boom;
        text_text_text_boom.text = str_num_boom;
        //伤害
        string str_pow = global.human[global.my_num].human_pow.ToString();
        text_text_text_pow.text = str_pow;
        //火焰长度
        string str_len = global.human[global.my_num].human_len.ToString();
        text_text_text_len.text = str_len;
        //移动速度
        string str_speed = global.human[global.my_num].human_speed.ToString();
        text_text_text_speed.text = str_speed;
        //金币
        string str_gold = global.human[global.my_num].user_gold.ToString();
        text_text_text_gold.text = str_gold;



        //场景5光亮旋转效果
        vec.Set(0, 0, light_speed);
        gameobject_light_1.transform.Rotate(vec);
    }
}