using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SubWeapon : MonoBehaviour
{
    //   if (Input.GetKeyDown(KeyCode.E))の条件をコントローラーのスキル発動キーに置き換えて！！！
    /// <summary>
    /// 武器ごとに振られた固有番号管理用のEnum
    /// </summary>
    public enum WeaponSelect
    {
        Sword=0,
        Kunai=1,
        _boomerang=2,
        kamaitachi=3,
    }
    //クナイ用
    //プレイヤー位置とクナイオブジェ
    [Header("プレイヤー入れてね")][SerializeField] GameObject player = null;
    [SerializeField] GameObject kunai = null;
 
    Rigidbody2D kunaiRd;

    //ブーメラン用

    [SerializeField] GameObject _boomerang = null;
    [SerializeField] GameObject __boomerangTraget = null;
    private float _boomerangSpeed = 15f;
    private Vector3 __boomerangPos;


    //既に投げた後かの判定・ブーメランが戻る位置についたかの判定
    private  bool _exists_boomerang = default;
    private  bool _canReturn = default;
    //かまいたち用

    [SerializeField] GameObject kamaitachi = null;
    private bool _iskamaitati = default;

    //共通

    private Vector2 _playerPos;
    PrayerC playerScript;
    public int _damage=0;
    public WeaponSelect _weaponSelect=0;
  
  
    void Start()
    {
       
        _playerPos = player.transform.position;
       // kunai.transform.position = _playerPos;
        playerScript = player.GetComponent<PrayerC>();
        _weaponSelect = 0;
        /**プレイヤーの位置を格納
         * プレイヤー位置にクナイを格納
         * プレイヤーのスクリプトを格納
         * 現在武器の初期化
         **/

        _boomerang.SetActive(false);
        kamaitachi.SetActive(false);    
    }
    void Update()
    {
        //選択されている武器をアップデートに呼び出す
      switch(_weaponSelect)
        {
            case WeaponSelect.Sword:
                break;
            case WeaponSelect.Kunai:
                KunaiWeapon();
                break;
            case WeaponSelect._boomerang:
                _boomerangWeapon();
                break;
            case WeaponSelect.kamaitachi:
                KamaitachiWeapon();
                break;
        }
        //武器の切り替え
        //テスト用の機能なのでインベントリ機能ができたら置き換わります
        if (Input.GetKeyDown(KeyCode.R))
        {
            if ((int)_weaponSelect == 3)
            {
                print("押してるよ");
                _weaponSelect = 0;
            }
            else
            {
                print((int)_weaponSelect);
             _weaponSelect++ ;
            }
        }
        //ブーメランの移動処理スイッチ文に入れると多分ばぐります
        if (_exists_boomerang)
        {
            _boomerangOperation();
        }
        //以下テスト用
    }
   
    IEnumerator WeaponCoroutine(float time,int switchCor)
    {
        yield return new WaitForSeconds(time);
        switch(switchCor)  
        {
            case 0:
                kamaitachi.SetActive(true);
                yield return new WaitForSeconds (2);
                kamaitachi.SetActive (false);
                _iskamaitati = false;
                break;
            case 1:
                print("戻り");
                _boomerangSpeed = _boomerangSpeed + 5;
                _canReturn = true;
                break;
            default: 
                break;
       
                //case 2:
                //    break;
                //case 3:
                //    break;
                //case 4:
                //break;
        }
    }
    private void KamaitachiWeapon()
    {
        if (Input.GetKeyDown(KeyCode.E)&&!_iskamaitati)
        {
            _iskamaitati = true;
            //二秒後にスキル発動      
            StartCoroutine(WeaponCoroutine(2, 0));
        }
    }
    private void _boomerangWeapon()
    {
        if (Input.GetKeyDown(KeyCode.E) && !_exists_boomerang)
        {
            //ブーメランの位置などをセットしてブーメランを投げる
            _playerPos = player.transform.position;
            _boomerang.SetActive(true);
            _boomerang.transform.position = _playerPos;
            __boomerangPos = __boomerangTraget.transform.position;
            _exists_boomerang = true;
            //1.2秒後にブーメランの移動向きを変更する
            StartCoroutine(WeaponCoroutine(1.2f, 1));
        }
       
    }
    private void _boomerangOperation()
    {
        if (_canReturn)
        {
            //ブーメランの戻り軌道
            __boomerangPos = player.transform.position;
            _boomerang.transform.position = Vector2.MoveTowards
                (_boomerang.transform.position, __boomerangPos, _boomerangSpeed * Time.deltaTime);
        }
        else
        {
            //ブーメラン一回目の軌道
            _boomerang.transform.position = Vector2.MoveTowards
           (_boomerang.transform.position, __boomerangPos, _boomerangSpeed * Time.deltaTime);
        }

    }
     private void KunaiWeapon()
    {
        //クナイ生成
        if (Input.GetKeyDown(KeyCode.E))
        {
            //現在地にクナイを生成して指定時間後に消去する
            _playerPos = player.transform.position;
            float time = 1f;
            GameObject newkunai = Instantiate(kunai);
            kunaiRd = newkunai.GetComponent<Rigidbody2D>();
            newkunai.transform.position = _playerPos;
            //プレイヤーの向きによって射出方向を決定
            //右に飛ばすelseは左
            if (playerScript.rightNow)
            {
                this.kunaiRd.AddForce(new Vector2(1500f, 0f));
            }
            else
            {
                this.kunaiRd.AddForce(new Vector2(-1500f, 0f));
            }
            Destroy(newkunai, time);
          
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーにぶつかったオブジェクトがブーメランなら処理をする
        if (collision.gameObject == _boomerang)
        {
            //既に戻り処理に突入しているのなら回収する
            if (_canReturn)
            {
                print("消滅");
                _canReturn = false;
                _exists_boomerang = false;
                _boomerangSpeed = 15;
                _boomerang.SetActive(false);
            }
        }
    }
}



