using UnityEngine;

//目录
//1.进入新的场景

public class return_scene : MonoBehaviour {

    public string scene = "level_1";
    //1.进入新的场景
    public void change_scene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
