using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossDamage : MonoBehaviour
{
    //当たり判定エリア
    public bool attack1;
    public bool attack2;
    public bool attack3;
    GameObject attackErea1;
    GameObject attackErea2; 
    GameObject attackErea3;
    // Start is called before the first frame update
    void Start()
    {
        //当たり判定の所得
      attackErea1 = GameObject.Find("bossAtt1");
      attackErea2 = GameObject.Find("bossAtt2");
      attackErea3 = GameObject.Find("bossAtt3");
    }

    // Update is called once per frame
    void Update()
    {
        //アニメーションによる当たり判定の管理
        if(!attack1)
        { 
            attackErea1.SetActive(false); 
        }
        else
        {
            attackErea1.SetActive(true);
        }
        if (!attack2)
        {
            attackErea2.SetActive(false);
        }
        else
        {
            attackErea2.SetActive(true);
        }
        if (!attack3)
        {
            attackErea3.SetActive(false);
        }
        else
        {
            attackErea3.SetActive(true);
        }
    }
}
