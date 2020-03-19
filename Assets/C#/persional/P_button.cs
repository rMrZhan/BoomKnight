using UnityEngine;

//目录
//1.失败按钮退出游戏
//2.成功按钮退出游戏
//3.按下任务确认按钮

public class P_button : MonoBehaviour
{
    private Vector3 vec;

    //1.失败按钮退出游戏
    public void push_fail()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("choose_persional_game");
    }

    //2.成功按钮退出游戏
    public void push_success()
    {
        //向服务端发送并更新信息
        //发送更新人物信息
        global.send_buff = "[#5|" + global.level + "|" + global.human[0].get_exp + "|" + global.human[0].get_gold + "]";
        while (true)
        {
            if(global.flag_buff2 == true)
            {
                break;
            }
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("choose_persional_game");
    }

    //3.按下任务确认按钮
    public void push_mission()
    {
        vec.Set(10000, 10000, 0);
        global.gameobject_mission.transform.localPosition = vec;
    }
}
