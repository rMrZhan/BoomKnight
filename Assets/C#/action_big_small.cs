using UnityEngine;

//目录
//1.变大变小的动画，挂在谁身上谁就动

public class action_big_small : MonoBehaviour
{

    Vector3 vec;
    int flag = 0;

    void Update()
    {
        if (gameObject.transform.localScale.x > 1.3)
        {
            flag = -1;
        }
        else if (gameObject.transform.localScale.x <= 1)
        {
            flag = 1;
        }

        if (flag == 1)
        {
            vec.Set(gameObject.transform.localScale.x + 0.01f, gameObject.transform.localScale.y + 0.01f, 1);
            gameObject.transform.localScale = vec;
        }
        else if (flag == -1)
        {
            vec.Set(gameObject.transform.localScale.x - 0.01f, gameObject.transform.localScale.y - 0.01f, 1);
            gameObject.transform.localScale = vec;
        }
    }
}
