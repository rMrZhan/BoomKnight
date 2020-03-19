using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

//目录
//1.账号密码不为空
//2.开始网络连接
//3.发送登陆报文



public class s1_login_in : MonoBehaviour {

    //输入ID框与密码框
    private GameObject gameobject_text_id;
    private GameObject gameobject_passwd;
    private GameObject gameobject_text_password;
    private Text text_text_id;
    private InputField text_text_password;
    private GameObject gameobject_text_ip;
    private Text text_text_ip;
    private GameObject gameobject_text_port;
    private GameObject gameobject_ipandport;
    private Text text_text_port;
    //临时变量
    Vector3 vec;

    private void Awake()
    {
        global.client.ip = "106.54.86.62";
        global.client.port = 9555;
        //ID输入窗口
        gameobject_text_id = GameObject.Find("text_id");
        text_text_id = gameobject_text_id.GetComponent<Text>();
        //密码输入窗口
        gameobject_text_password = GameObject.Find("PassWord");
        text_text_password = gameobject_text_password.GetComponent<InputField>();
        //IP输入窗口
        gameobject_text_ip = GameObject.Find("text_ip");
        text_text_ip = gameobject_text_ip.GetComponent<Text>();
        //PORT输入窗口
        gameobject_text_port = GameObject.Find("text_port");
        text_text_port = gameobject_text_port.GetComponent<Text>();
        //ip and port
        gameobject_ipandport = GameObject.Find("ipandport");
    }
    
    public void push_change_ip()
    {
        vec.Set(0, 0, 0);
        gameobject_ipandport.transform.localPosition = vec;
    }


    public void push_ipport()
    {
        
        if (text_text_ip.text != "")
        {
            global.client.ip = text_text_ip.text;
        }
        else
        {
            global.client.ip = "106.54.86.62";
        }
        if (text_text_port.text != "")
        {
            global.client.port = int.Parse(text_text_port.text);
        }
        else
        {
            global.client.port = 9555;
        }
        vec.Set(10000, 10000, 0);
        gameobject_ipandport.transform.localPosition = vec;
    }
    

    public void exit_game()
    {
        Application.Quit();
    }


    public void login_in()
    {
        //1.账号密码不为空
        if (text_text_id.text == "")
        {
            global.str_message = "请输入账号!";
            vec.Set(0, 0, 0);
            global.gameobject_messagebox.transform.localPosition = vec;
        }
        else if (text_text_password.text == "")
        {
            global.str_message = "请输入密码!";
            vec.Set(0, 0, 0);
            global.gameobject_messagebox.transform.localPosition = vec;
        }
        else
        {

            if ((global.flag_online_first) && (global.flag_online))
            {
                try
                {
                    global.client.server_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    global.client.server_socket.Connect(new IPEndPoint(IPAddress.Parse(global.client.ip), global.client.port));
                    print(global.client.ip);
                    print(global.client.port);
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



            //3.发送登录报文
            global.send_buff = "[#1|" + text_text_id.text + "|" + text_text_password.text + "]";

            while(true)
            {
                if(global.flag_online)
                {
                    if(global.flag_login_in == 1)
                    {
                        vec.Set(0, 0, 0);
                        global.gameobject_messagebox.transform.localPosition = vec;
                        break;
                    }
                    else if (global.flag_login_in == -1)
                    {
                        vec.Set(0, 0, 0);
                        global.gameobject_messagebox.transform.localPosition = vec;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}
