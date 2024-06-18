using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SubWeapon : MonoBehaviour
{
    //   if (Input.GetKeyDown(KeyCode.E))の条件をコントローラーのスキル発動キーに置き換えて！！！
   
   
    //クナイ用
    //プレイヤー位置とクナイオブジェ
    [Header("プレイヤーを入れる")][SerializeField] GameObject _player = null;
    [Header("クナイを入れる")][SerializeField] GameObject _kunai = null;
 
    Rigidbody2D _kunaiRd;

    //ブーメラン用

    [Header("ブーメランを入れる")] [SerializeField] GameObject _boomerang = null;
    [Header("ブーメランの到達地点を入れる")] [SerializeField] GameObject _boomerangTraget = null;
    private float _boomerangSpeed = 15f;
    private Vector3 _boomerangPos;


    //既に投げた後かの判定・ブーメランが戻る位置についたかの判定
    private  bool _exists_boomerang = default;
    private  bool _canReturn = default;
    //かまいたち用

    [Header("かまいたちを入れる")] [SerializeField] GameObject _kamaitachi = null;
    private bool _iskamaitati = default;

    //共通

    private Vector2 _playerPos;
    PrayerC _playerScript;
    [Header("現在のサブ武器インベントリ番号")] [SerializeField] private int _indexCnt = 1;
    public bool _isOpenBox = default;

    //インベントリが入っているオブジェクトを格納
    [Header("インベントリスクリプトが入ったObjを入れる")] [SerializeField] Inventory _weaponInventory;
  
    void Start()
    {
        _playerPos = _player.transform.position;
        _playerScript = _player.GetComponent<PrayerC>();
        /**プレイヤーの位置を格納
         * プレイヤー位置にクナイを格納
         * プレイヤーのスクリプトを格納
         * 現在武器の初期化
         **/

        //ブーメランとかまいたちを非表示にする
        _boomerang.SetActive(false);
        _kamaitachi.SetActive(false);
       
    }
    public void SubWeaponUpdate()
    {
        //選択されている武器をアップデートに呼び出す
        switch (_weaponInventory._inventory[_indexCnt])
        {
            //空っぽの時
            case Inventory.WeaponSelect.nasi:
                break;
            //クナイの時
            case Inventory.WeaponSelect.Kunai:
                KunaiWeapon();
                break;
            //ブーメランの時
            case Inventory.WeaponSelect._boomerang:
                _boomerangWeapon();
                break;
            //かまいたちの時
            case Inventory.WeaponSelect.kamaitachi:
                KamaitachiWeapon();
                break;
        }
        //もし宝箱開封中でなければ武器を切り替えれるようにする
        if (!_isOpenBox)
        {
            NotOpenBox();
        }

        //ブーメランの移動処理スイッチ文に入れると多分ばぐります
        if (_exists_boomerang)
        {
            _boomerangOperation();
        }
    }
   
    IEnumerator WeaponCoroutine(float time,int switchCor)
    {
        yield return new WaitForSeconds(time);
        switch(switchCor)  
        {
            case 0:
                _kamaitachi.SetActive(true);
                yield return new WaitForSeconds (2);
                _kamaitachi.SetActive (false);
                _iskamaitati = false;
                break;
            //かまいたちを表示させてさらに指定秒後に終了する
            case 1:
                _boomerangSpeed = _boomerangSpeed + 5;
                _canReturn = true;
                break;
            //ブーメランが返ってくるときブーメラン速度を上げる
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
    private void NotOpenBox()
    {
        /*RとLでインベントリを右左に選択する
         * もし現在選択しているインベントリ番号が最後尾あるいは先頭の時
         * 値をリセットする
         */
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_indexCnt == _weaponInventory._maxIndex)
            {
                _indexCnt = 0;
                print("現在のインベントリ番号" + _indexCnt);
            }
            else
            {
                _indexCnt = _indexCnt + 1;
                print("現在のインベントリ番号" + _indexCnt);
            }
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            if (_indexCnt == 0)
            {
                _indexCnt = _weaponInventory._maxIndex;
                print("現在のインベントリ番号" + _indexCnt);
            }
            else
            {
                _indexCnt = _indexCnt - 1;
                print("現在のインベントリ番号" + _indexCnt);
            }
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
            _playerPos = _player.transform.position;
            _boomerang.SetActive(true);
            _boomerang.transform.position = _playerPos;
            _boomerangPos = _boomerangTraget.transform.position;
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
            _boomerangPos = _player.transform.position;
            _boomerang.transform.position = Vector2.MoveTowards
                (_boomerang.transform.position, _boomerangPos, _boomerangSpeed * Time.deltaTime);
        }
        else
        {
            //ブーメラン一回目の軌道
            _boomerang.transform.position = Vector2.MoveTowards
           (_boomerang.transform.position, _boomerangPos, _boomerangSpeed * Time.deltaTime);
        }

    }
     private void KunaiWeapon()
    {
        //クナイ生成
        if (Input.GetKeyDown(KeyCode.E))
        {
            //現在地にクナイを生成して指定時間後に消去する
            _playerPos = _player.transform.position;
            float time = 1f;
            GameObject newkunai = Instantiate(_kunai);
            _kunaiRd = newkunai.GetComponent<Rigidbody2D>();
            newkunai.transform.position = _playerPos;
            //プレイヤーの向きによって射出方向を決定
            //右に飛ばすelseは左
            if (_playerScript.rightNow)
            {
                newkunai.transform.Rotate(new Vector3(0, 0, -90));            
                this._kunaiRd.AddForce(new Vector2(1500f, 0f));
                Destroy(newkunai, time);
            }
            else
            {
                newkunai.transform.Rotate(new Vector3(0, -180, -90));
                this._kunaiRd.AddForce(new Vector2(-1500f, 0f));
                Destroy(newkunai, time);
            }
          
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



