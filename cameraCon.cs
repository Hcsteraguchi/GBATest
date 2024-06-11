using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraCon : MonoBehaviour
{
    GameObject player;
   // player movecamera;
    // Start is called before the first frame update
    private void Start()
    {
        this.player = GameObject.Find("player");
      //  movecamera = player.GetComponent<playerMove_KM2>();
    }

    // Update is called once per frame
    void Update() //ÉJÉÅÉâà⁄ìÆèàóù
    {
        //if (movecamera.astro == false)
        //{
        //    Vector3 playerPos = this.player.transform.position;
        //    transform.position = new Vector3(227, -67, -10);
        //}
        //else if (movecamera.boss == false)
        //{
        //    Vector3 playerPos = this.player.transform.position;
        //    transform.position = new Vector3(playerPos.x + 5, -67, -10);
        //}
        //else if (movecamera.fool == false)
        //{
        //    Vector3 playerPos = this.player.transform.position;
        //    transform.position = new Vector3(playerPos.x + 5, playerPos.y, -10);
        //}
        //else
        //{
            Vector3 playerPos = this.player.transform.position;
            transform.position = new Vector3(playerPos.x + 5, 0, -10);
        //}
    }
}
