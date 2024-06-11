using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float knockBackPower;   // ノックバックさせる力
    //private PlayerMove_KM2 PL;
    public Collider2D Enemy;
    
    public GameObject targetObj;
    
    // Start is called before the first frame update
    void Start()
    {
        //PL = GameObject.Find("player").GetComponent<PlayerMove_KM2>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (state == STATE.DAMAGED)
        //{
        //    return;
        //}
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyAttack")
        {
            //Debug.Log("エアノックバック");
            Enemy = col;
            targetObj.GetComponent<PlayerMove_KM2>().ShowLog();
            ////変更 ノーマルじゃない（ダメージ中、無敵中）ときはリターン
            //if (state != STATE.NOMAL)
            //{
            //    return;
            //}

            //state = STATE.DAMAGED;
            //StartCoroutine(_hit());

            //playerMove = false;
            //Invoke("AGM", attackDelay);
            ////playerStatus = Status.HIT;//?
            //rb.velocity = Vector2.zero;

            //// 自分の位置と接触してきたオブジェクトの位置とを計算して、距離と方向を出して正規化(速度ベクトルを算出)
            //Vector2 distination = (transform.position - col.transform.position).normalized;

            //rb.AddForce(distination * knockBackPower);


        }
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("エアノックバック");
            Enemy = col;
            targetObj.GetComponent<PlayerMove_KM2>().ShowLog();
        }
    }
    


}
