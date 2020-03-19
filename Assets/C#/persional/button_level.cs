using UnityEngine;

public class button_level : MonoBehaviour
{
    public int MAX_level = 5;

    private Vector3 vec;
    private GameObject choose_hard;
    private GameObject[] gameobject_lock_level = new GameObject[50];
    public int level = 1;
    
    private void Start()
    {
        choose_hard = GameObject.Find("choose_hard");
    }

    public void select_level()
    {
        vec.Set(0, 0, 0);
        choose_hard.transform.localPosition = vec;
        global.level = level;
    }

    public void select_hard_1()
    {
        vec.Set(10000, 10000, 0);
        choose_hard.transform.localPosition = vec;
        global.hard = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("level_" + global.level);
    }

    public void select_hard_2()
    {
        vec.Set(10000, 10000, 0);
        choose_hard.transform.localPosition = vec;
        global.hard = 2;
        UnityEngine.SceneManagement.SceneManager.LoadScene("level_" + global.level);
    }

    public void select_hard_3()
    {
        vec.Set(10000, 10000, 0);
        choose_hard.transform.localPosition = vec;
        global.hard = 3;
        UnityEngine.SceneManagement.SceneManager.LoadScene("level_" + global.level);
    }

    private void Update()
    {
        for (int i = 0; i < MAX_level; i++)
        {
            gameobject_lock_level[i] = GameObject.Find("lock_" + i);
            if (global.human[global.my_num].user_stage > i)
            {
                vec.Set(10000, 10000, 0);
                gameobject_lock_level[i].transform.localPosition = vec;
            }
        }
    }
}