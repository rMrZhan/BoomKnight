using UnityEngine;

public class P_coll_water : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.name.Contains("water"))
        {
            if (global.gameover == 0)
            {
                if (global.human[0].wudi == false)
                {
                    global.human[0].wudi = true;
                    global.human[0].life_now -= global.human[0].pow;
                    global.text_life.text = global.human[0].life_now + "/" + global.human[0].life;
                    string str_ani = "human_" + global.human[global.my_num].user_role_num + "_6";
                    global.human[global.my_num].human_stat = 6;
                    global.human[global.my_num].ani_human.Play(str_ani);
                    global.audio_sound_hit.Play();
                }
            }
        }
    }
}
