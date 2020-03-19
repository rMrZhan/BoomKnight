using UnityEngine;
using System.Collections;

public class all_update : MonoBehaviour {

    public void exit_game()
    {
        Application.Quit();
    }


    void Update ()
    {
        if (global.kill_game)
        {
#if UNITY_EDITOR
            global.flag_close_recv_pthread = true;
            global.flag_close_send_pthread = true;
            UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_ANDROID
        Application.Quit();
#endif
        }
    }
}
