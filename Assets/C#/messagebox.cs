using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

//目录
//1.按下信息框提示Ok
//2.按下重连按钮
//3.退出游戏
//4.断网处理
//5.计算网络延迟
//6.登录成功进入场景2

public class messagebox : MonoBehaviour {

    //临时变量
    Vector3 vec;

    private void Awake()
    {
        //提示窗口
        global.gameobject_messagebox = GameObject.Find("messagebox");
        global.gameobject_text_messagebox = GameObject.Find("text_messagebox");
        global.text_text_messagebox = global.gameobject_text_messagebox.GetComponent<Text>();
        //网络连接窗口
        global.gameobject_messagebox_online = GameObject.Find("messagebox_online");
        global.gameobject_text_messagebox_online = GameObject.Find("text_messagebox_online");
        global.gameobject_button_reline = GameObject.Find("button_reline");
        global.gameobject_button_exit = GameObject.Find("button_exit");
        global.gameobject_delay = GameObject.Find("delay");
        global.text_delay = global.gameobject_delay.GetComponent<Text>();
        global.text_text_messagebox_online = global.gameobject_text_messagebox_online.GetComponent<Text>();
    }

    //1.按下提示Ok
    public void push_ok()
    {
        //6.登录成功，进入场景2
        if (global.flag_login_in == 1)
        {
            global.text_text_messagebox.text = "";
            vec.Set(10000, 10000, 0);
            global.gameobject_messagebox.transform.localPosition = vec;
            global.flag_login_in = 0;
            global.flag_sign_in = 0;
            UnityEngine.SceneManagement.SceneManager.LoadScene("scene2");
        }
        else
        {
            global.text_text_messagebox.text = "";
            vec.Set(10000, 10000, 0);
            global.flag_login_in = 0;
            global.flag_sign_in = 0;
            global.gameobject_messagebox.transform.localPosition = vec;
        }
    }


    //2.按下重连按钮
    public void push_reline()
    {
        global.flag_reline = true;
        global.flag_online = false;
        global.str_online_message = "正在连接服务器...\n";
        //按钮移除
        vec.Set(10000, 10000, 0);
        global.gameobject_button_reline.transform.localPosition = vec;
        vec.Set(10000, 10000, 0);
        global.gameobject_button_exit.transform.localPosition = vec;
        StartCoroutine(reline());
    }

    IEnumerator reline()
    {
        //1秒后重连
        yield return new WaitForSeconds(1.0f);

        global.flag_online_first = true;
        global.flag_reline = false;
        global.flag_online = true;
        global.client.server_socket.Close();
        Thread thread_send = new Thread(new ThreadStart(pthread.send_message));
        Thread thread_recv = new Thread(new ThreadStart(pthread.recv_message));
        thread_send.Start();
        thread_recv.Start();
    }

    //3.退出游戏
    public void push_exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void Update ()
    {
        //6.实时更新messagebox的内容
        if (global.str_online_message != "")
        {
            global.text_text_messagebox_online.text = global.str_online_message;
        }
        if (global.str_message != "")
        {
            global.text_text_messagebox.text = global.str_message;
        }

        //4.断网处理
        if (global.flag_online == false)
        {
            if (global.flag_reline == false)
            {
                vec.Set(0, 0, 0);
                global.str_online_message = "与服务器断开连接\n";
                global.gameobject_messagebox_online.transform.localPosition = vec;
                //按钮返回
                vec.Set(-57, -56, 0);
                global.gameobject_button_reline.transform.localPosition = vec;
                vec.Set(64, -56, 0);
                global.gameobject_button_exit.transform.localPosition = vec;
            }
        }
        else
        {
            if (global.flag_reline == false)
            {
                //网络连接成功
                global.str_online_message = "连接成功\n";
                //移走对话框
                vec.Set(10000, 10000, 0);
                global.gameobject_messagebox_online.transform.localPosition = vec;
            }
        }

        //5.计算网络延迟
        if (global.flag_wait == false)
        {
            if (global.time_wait != 0)
            {
                global.delay = (int)(global.time_wait * 1000);
                print("网络延迟: " + global.delay + " ms");
                global.text_delay.text = "网络延迟: " + global.delay + " ms";
                global.time_wait = 0;
            }
        }
        else
        {
            global.time_wait += Time.deltaTime;
            print("时间:" + global.time_wait);
            if(global.time_wait >= global.time_recv)
            {
                global.flag_close_send_pthread = true;
                global.flag_close_recv_pthread = true;
                global.flag_wait = false;
                global.flag_online = false;
                global.flag_online_first = false;
                print("关闭连接");
                global.client.server_socket.Close();
            }
        }
    }
}
