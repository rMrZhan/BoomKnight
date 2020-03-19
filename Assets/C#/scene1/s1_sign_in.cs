using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class s1_sign_in : MonoBehaviour
{

    GameObject gameobject_input_id;
    GameObject gameobject_input_passwd_1;
    GameObject gameobject_input_passwd_2;
    GameObject gameobject_input_human_name;
    GameObject gameobject_messagebox;
    GameObject gameobject_text_messagebox;

    GameObject gameobject_waring_2;
    GameObject gameobject_waring_3;
    //GameObject gameobject_waring_4;
    GameObject gameobject_sign_in;

    Text text_messagebox;
    Text text_messagebox_online;

    Text text_waring_2;
    Text text_waring_3;

    InputField input_id;
    InputField input_passwd_1;
    InputField input_passwd_2;
    InputField input_human_name;

    public Sprite[] sprite_image;

    GameObject gameobject_image_human;
    Image image_human;


    Vector3 vec;

    private int flag_sign_success_1 = 0;
    private int flag_sign_success_2 = 0;

    private string buff;
    //private byte[] data = new byte[1024];
    string[] array_buff;

    void Start()
    {
        //取得UI
        gameobject_sign_in = GameObject.Find("image_sign_in");
        //取得填写信息对象
        gameobject_input_id = GameObject.Find("input_id");
        gameobject_input_passwd_1 = GameObject.Find("input_passwd_1");
        gameobject_input_passwd_2 = GameObject.Find("input_passwd_2");
        gameobject_input_human_name = GameObject.Find("input_human_name");



        input_id = gameobject_input_id.GetComponent<InputField>();
        input_passwd_1 = gameobject_input_passwd_1.GetComponent<InputField>();
        input_passwd_2 = gameobject_input_passwd_2.GetComponent<InputField>();
        input_human_name = gameobject_input_human_name.GetComponent<InputField>();

        //提示窗
        gameobject_messagebox = GameObject.Find("messagebox");
        gameobject_text_messagebox = GameObject.Find("text_messagebox");
        text_messagebox = gameobject_text_messagebox.GetComponent<Text>();
        //换人物
        gameobject_image_human = GameObject.Find("image_human");
        image_human = gameobject_image_human.GetComponent<Image>();
        //警告

        gameobject_waring_2 = GameObject.Find("waring_2");
        gameobject_waring_3 = GameObject.Find("waring_3");
        //gameobject_waring_4 = GameObject.Find("waring_4");

        text_waring_2 = gameobject_waring_2.GetComponent<Text>();
        text_waring_3 = gameobject_waring_3.GetComponent<Text>();
    }

    public void push_sign()
    {
        vec.Set(0, 0, 0);
        gameobject_sign_in.transform.position = vec;
    }

    public void sign_exit()
    {
        //注册信息清空
        input_id.text = "";
        input_passwd_1.text = "";
        input_passwd_2.text = "";
        input_human_name.text = "";
        //关闭注册窗口
        vec.Set(1000, 1000, 0);
        gameobject_sign_in.transform.localPosition = vec;
    }

    public void sign_in()
    {
        if (input_id.text == "")
        {
            text_messagebox.text = "请输入账号!";
            vec.Set(0, 0, 0);
            gameobject_messagebox.transform.localPosition = vec;
        }
        else if (input_passwd_1.text == "")
        {
            text_messagebox.text = "请输入密码!";
            vec.Set(0, 0, 0);
            gameobject_messagebox.transform.localPosition = vec;
        }
        else if (input_passwd_2.text == "")
        {
            text_messagebox.text = "请确认密码!";
            vec.Set(0, 0, 0);
            gameobject_messagebox.transform.localPosition = vec;
        }
        else if (input_human_name.text == "")
        {
            text_messagebox.text = "请输入昵称!";
            vec.Set(0, 0, 0);
            gameobject_messagebox.transform.localPosition = vec;
        }
        else if ((flag_sign_success_1 == 0) || (flag_sign_success_2 == 0))
        {
            ;
        }
        else
        {
            if ((global.flag_online_first) && (global.flag_online))
            {
                try
                {
                    global.client.server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    global.client.server_socket.Connect(new IPEndPoint(IPAddress.Parse(global.client.ip), global.client.port));
                    global.flag_online = true;
                    global.flag_online_first = false;
                    global.flag_close_send_pthread = false;
                    global.flag_close_recv_pthread = false;

                    Thread thread_send = new Thread(new ThreadStart(pthread.send_message));
                    Thread thread_recv = new Thread(new ThreadStart(pthread.recv_message));

                    thread_send.Start();
                    thread_recv.Start();

                }
                catch (Exception)
                {
                    global.flag_online = false;
                    global.flag_online_first = false;
                    global.flag_close_send_pthread = true;
                    global.flag_close_recv_pthread = true;
                }
            }
            //发送
            global.send_buff = "[#0|" + input_id.text + "|" + input_passwd_1.text + "|" + input_human_name.text + "|" + (global.s1_choose_human + 1).ToString() +"]";
        }
    }
    public void change_human()
    {
        if (global.s1_choose_human == 0)
        {
            global.s1_choose_human = 1;
            image_human.sprite = sprite_image[1];
        }
        else
        {
            global.s1_choose_human = 0;
            image_human.sprite = sprite_image[0];
        }
    }
    private void Update()
    {
        if(global.flag_sign_in == -1)
        {
            vec.Set(0, 0, 0);
            gameobject_messagebox.transform.localPosition = vec;
        }
        else if(global.flag_sign_in == 1)
        {
            //成功
            vec.Set(0, 0, 0);
            gameobject_messagebox.transform.localPosition = vec;
            //注册信息清空
            input_id.text = "";
            input_passwd_1.text = "";
            input_passwd_2.text = "";
            //关闭注册窗口
            vec.Set(1000, 1000, 0);
            gameobject_sign_in.transform.localPosition = vec;
        }

        if (input_passwd_1.text.Contains("|") || input_passwd_1.text.Contains("#") || input_passwd_1.text.Contains("%") || input_passwd_1.text.Contains("&"))
        {
            text_waring_2.text = "密码不可以包括'|' '#' '%' '&'";
            flag_sign_success_1 = 0;
        }
        else
        {
            text_waring_2.text = "";
            flag_sign_success_1 = 1;
        }


        if (input_passwd_1.text != input_passwd_2.text)
        {
            text_waring_3.text = "密码两次输入不相同";
            flag_sign_success_2 = 0;
        }
        else
        {
            text_waring_3.text = "";
            flag_sign_success_2 = 1;
        }

        if (input_human_name.text.Contains("|") || input_human_name.text.Contains("#") || input_human_name.text.Contains("%") || input_human_name.text.Contains("&"))
        {
            text_waring_2.text = "昵称不可以包括'|' '#' '%' '&'";
            flag_sign_success_1 = 0;
        }
        else
        {
            text_waring_2.text = "";
            flag_sign_success_1 = 1;
        }
    }
}
