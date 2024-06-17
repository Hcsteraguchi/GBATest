using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrayerC : MonoBehaviour
{
    
    Rigidbody2D rigid2D;
    //最速スピード
    [SerializeField]public float _LimitSpeed=100f;
 　 //移動スピード
    private float move = 25f;

    public bool rightNow = true;
   

    //着地判定
     bool isGround = false;
    [SerializeField] private LayerMask groundLayer;

    public float jump = 450f;
    private float F = 0.25f;
    //[SerializeField] bool PlayMain = true;

    [SerializeField]
    private AudioClip jumpSound;
    private AudioSource audioSource;
    private bool sound = true;



    //ここから追加！！！！！！
    [SerializeField] private GameObject _player;
    private SubWeapon _subWeapon;


    void Start()
    {
        this.rigid2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        // ここから追加！！！！！！！！！
        _subWeapon = _player.GetComponent<SubWeapon>();
    }
    private void Update()
    {
        // サブ武器を呼び出す
        _subWeapon.SubWeaponUpdate();
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        //現在の速度を記録する
        float speedX = Mathf.Abs(this.rigid2D.velocity.x);
        //移動する
        if (speedX < this._LimitSpeed)
        {
            if (Input.GetKey(KeyCode.D))
            {
                this.rigid2D.AddForce(new Vector2(move, 0f));
                if(rightNow==false)
                {
                    transform.Rotate(new Vector3(0, 180, 0));
                    rightNow = true;
                }
                
            }
            if (Input.GetKey(KeyCode.A))
            {
                this.rigid2D.AddForce(new Vector2(-move, 0f));
                if(rightNow==true)
                { 
                    transform.Rotate(new Vector3(0, 180, 0)); 
                    rightNow = false;
                }
            }

        }


        //レイを発射（因数１；レイの始点、因数２；レイの向き、因数３；レイの距離、因数４；レイヤーマスク）
        RaycastHit2D hit = Physics2D.CircleCast(transform.position,0.43f, Vector2.down, F, groundLayer);
        //レイを可視化
       // Gizmos.DrawSphere(transform.position,0.8f, Vector2.down * F, Color.red);

        //着地判定処理
        if (hit)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }

        //スペースキーが押されたときジャンプする
        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            this.rigid2D.AddForce(new Vector2(0f, jump));
            if(sound)
            {
                audioSource.PlayOneShot(jumpSound);
                sound = false;
                Invoke("SoundJump", 0.1f);
            }
           
            
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.rigid2D.AddForce(new Vector2(0, -jump / 2));
        }

    }
    private void SoundJump()
    {
        sound = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Move")
        {
            transform.parent = collision.gameObject.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.parent = null;
    }

}
