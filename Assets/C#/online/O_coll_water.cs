using UnityEngine;

public class O_coll_water : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.name.Contains("water"))
        {
            if (global.gameover == 0)
            {
                string[] array_human = gameObject.name.Split('_');
                string[] array_water = coll.name.Split('_');
                print(array_water[3] + "的火焰碰撞到" + array_human[1]);
                
                int human_num = int.Parse(array_human[1]);
                int water_num = int.Parse(array_water[3]);
                if (global.human[human_num].wudi == false)
                {
                    global.human[human_num].wudi = true;
                    global.human[human_num].life_now -= global.human[water_num].pow;
                    global.human[human_num].human_stat = 6;
                    string str_ani = "human_" + global.human[global.my_num].user_role_num + "_6";
                    global.human[human_num].ani_human.Play(str_ani);
                    global.audio_sound_hit.Play();
                }
                
            }
        }
    }
}