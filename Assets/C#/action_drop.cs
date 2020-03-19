using UnityEngine;
using System.Collections;

//目录
//1.动作，上下左右落下，在全局变量中更改

public class action_drop : MonoBehaviour
{
    public float over_y = 366f;
    public float over_x = 366f;
    public string sign = "right";
    public float speed = 10f;
    private int flag = 0;
    private Vector3 vec;
    //1.动作，上下左右落下，在全局变量中更改
    void Update()
    {
        if (sign == "down")
        {
            if (gameObject.transform.localPosition.y > over_y)
            {
                flag = -1;
            }
            else
            {
                flag = 0;
            }
            if (flag == -1)
            {
                vec.Set(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - speed, 1);
                gameObject.transform.localPosition = vec;
            }
        }
        if (sign == "up")
        {
            if (gameObject.transform.localPosition.y < over_y)
            {
                flag = -1;
            }
            else
            {
                flag = 0;
            }
            if (flag == -1)
            {
                vec.Set(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + speed, 1);
                gameObject.transform.localPosition = vec;
            }
        }
        if (sign == "left")
        {
            if (gameObject.transform.localPosition.x > over_x)
            {
                flag = -1;
            }
            else
            {
                flag = 0;
            }
            if (flag == -1)
            {
                vec.Set(gameObject.transform.localPosition.x - speed, gameObject.transform.localPosition.y, 1);
                gameObject.transform.localPosition = vec;
            }
        }
        if (sign == "right")
        {
            if (gameObject.transform.localPosition.x < over_x)
            {
                flag = -1;
            }
            else
            {
                flag = 0;
            }
            if (flag == -1)
            {
                vec.Set(gameObject.transform.localPosition.x + speed, gameObject.transform.localPosition.y, 1);
                gameObject.transform.localPosition = vec;
            }
        }
    }
}
