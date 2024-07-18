using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class SubWeapon : MonoBehaviour
{
    //   if (Input.GetKeyDown(KeyCode.E))の条件をコントローラーのスキル発動キーに置き換えて！！！


    //クナイ用
    //プレイヤー位置とクナイオブジェ
    //[Header("プレイヤーを入れる")][SerializeField] GameObject _player = null;
    [Header("クナイを入れる")] [SerializeField] GameObject _kunai = null;

    Rigidbody2D _kunaiRd;

    //ブーメラン用

    [Header("ブーメランを入れる")] [SerializeField] GameObject _boomerang = null;
    [Header("ブーメランの到達地点を入れる")] [SerializeField] GameObject _boomerangTraget = null;
    private float _boomerangSpeed = 15f;
    private Vector3 _boomerangPos;


    //既に投げた後かの判定・ブーメランが戻る位置についたかの判定
    private bool _exists_boomerang = default;
    private bool _canReturn = default;
    //かまいたち用

    [Header("かまいたちを入れる")] [SerializeField] GameObject _kamaitachi = null;
    private bool _iskamaitati = default;

    //追従魔法
    [Header("火の玉を入れる")] [SerializeField] GameObject _fire = default;
    [Header("火の玉を入れる")] [SerializeField] GameObject _bigFire = default;
    [Header("カメラ範囲")] [SerializeField] GameObject _fireTregetSize;
    public bool _isfire = default;
    private bool _isfireSet = default;
    private float _fireSpeed = 8.5f;
    public Vector2 _fireTreget = default;

    //共通

    private Vector2 _playerPos;
    PlayerMove_KM _playerScript;
    [Header("現在のサブ武器インベントリ番号")] [SerializeField] private int _indexCnt = 1;

    //インベントリが入っているオブジェクトを格納
    [Header("インベントリスクリプトが入ったObjを入れる")] [SerializeField] Inventory _weaponInventory;

    Animator _animator;

    // インベントリ画像
    [Header("対応させる画像")] [SerializeField] private Image _nowInventory = default;

    [SerializeField] private InventoryChange _inventoryChange = default;

    //MP
    public float MP = 100;
    [SerializeField] private int _kunaiMP = 5;
    [SerializeField] private int _booMP = 15;
    [SerializeField] private int _kamaitachiMP = 70;
    [Header("対応するSliderUIをアタッチ")]
    [SerializeField] private SliderMP _sliderMP;
    [Header("対応スライダー")] [SerializeField]
    private GameObject _sliderObject = default;

    //RT,LTの入力値
    private float _subPower;
    [Header("RT,LTの入力")] [SerializeField] private float _subWeaponInput = default;
    private bool _canSelect = default;
    void Start()
    {
        _playerPos = gameObject.transform.position;
        _playerScript = gameObject.GetComponent<PlayerMove_KM>();
        /**プレイヤーの位置を格納
         * プレイヤー位置にクナイを格納
         * プレイヤーのスクリプトを格納
         * 現在武器の初期化
         **/

        //ブーメランとかまいたち・炎を非表示にする
        _boomerang.SetActive(false);
        _kamaitachi.SetActive(false);
        _fire.SetActive(false);
        _bigFire.SetActive(false);
        _fireTregetSize.SetActive(false);
        _animator = GetComponent<Animator>();

        _inventoryChange = _nowInventory.GetComponent<InventoryChange>();
        _sliderMP = _sliderObject.GetComponent<SliderMP>();
        _canSelect = true;
    }
    public void SubWeaponUpdate()
    {
        _subPower = Input.GetAxisRaw("SubAttack");
        //選択されている武器をアップデートに呼び出す
        switch (_weaponInventory._inventoryList[_weaponInventory._indexCnt])
        {
            //空っぽの時
            case Inventory.WeaponSelect.Nasi:
                break;
            //クナイの時
            case Inventory.WeaponSelect.Kunai:
                KunaiWeapon();
                break;
            //ブーメランの時
            case Inventory.WeaponSelect.Boomerang:
                _boomerangWeapon();
                break;
            //かまいたちの時
            case Inventory.WeaponSelect.Kamaitachi:
                KamaitachiWeapon();
                break;
            //爆炎の時
            case Inventory.WeaponSelect.Fire:
                if (Input.GetButtonDown("SubAttack") && !_isfire)
                {
                    _fireTregetSize.SetActive(true);
                }
                if (Input.GetButtonUp("SubAttack") && !_isfire)
                {
                    _fireTregetSize.SetActive(false);
                }
                break;
        }

        //ブーメランの移動処理スイッチ文に入れると多分ばぐります
        if (_exists_boomerang)
        {
            _boomerangOperation();
        }
        //爆炎の処理
        if (_isfire)
        {
            _fireTregetSize.SetActive(false);
            Fire(_fireTreget);
            print(_fireTreget);
        }
    }

    IEnumerator WeaponCoroutine(float time, int switchCor)
    {
        yield return new WaitForSeconds(time);
        switch (switchCor)
        {
            case 0:
                //かまいたちを表示させてさらに指定秒後に終了する
                _kamaitachi.SetActive(true);
                yield return new WaitForSeconds(2);
                _kamaitachi.SetActive(false);
                _iskamaitati = false;
                _canSelect = true;
                break;
         
            case 1:
                //ブーメランが返ってくるときブーメラン速度を上げる
                _boomerangSpeed = _boomerangSpeed + 5;
                _canReturn = true;
                break;
            
            case 2:
                _canSelect = true;
                break;
            case 3:
                //爆炎コルーチン
                _bigFire.SetActive(true);
                yield return new WaitForSeconds(1);
                _bigFire.SetActive(false);
                _fire.SetActive(false);
                _isfire = false;
                _isfireSet = false;

                break;
            default:
                break;
                //case 3:
                //    break;
                //case 4:
                //break;
        }
    }
    //private void NotOpenBox()
    //{
    //    /*RとLでインベントリを右左に選択する
    //     * もし現在選択しているインベントリ番号が最後尾あるいは先頭の時
    //     * 値をリセットする
    //     */
    //    if (Input.GetButtonDown("RightChange")/*Input.GetKeyDown(KeyCode.R)*/)
    //    {
    //        if (_indexCnt == _weaponInventory._maxIndex)
    //        {
    //            _indexCnt = 0;
    //            _inventoryChange._inventoryNum = 0;//配列番号をずらして画像を変更させる
    //            _inventoryChange.InventoryState();
    //            print("現在のインベントリ番号" + _indexCnt);
    //        }
    //        else
    //        {
    //            _inventoryChange._inventoryNum++; //配列番号をずらして画像を変更させる
    //            _inventoryChange.InventoryState();
    //            _indexCnt = _indexCnt + 1;
    //            print("現在のインベントリ番号" + _indexCnt);
    //        }
    //    }
    //    if (Input.GetButtonDown("LeftChange")/*Input.GetKeyDown(KeyCode.L)*/)
    //    {
    //        if (_indexCnt == 0)
    //        {
    //            _inventoryChange._inventoryNum = _weaponInventory._maxIndex; //配列番号をずらして画像を変更させる
    //            _inventoryChange.InventoryState();
    //            _indexCnt = _weaponInventory._maxIndex;
    //            print("現在のインベントリ番号" + _indexCnt);
    //        }
    //        else
    //        {
    //            _inventoryChange._inventoryNum--; //配列番号をずらして画像を変更させる
    //            _inventoryChange.InventoryState();
    //            _indexCnt = _indexCnt - 1;
    //            print("現在のインベントリ番号" + _indexCnt);
    //        }
    //    }
    //}
    private void KamaitachiWeapon()
    {
        if ((_subPower > _subWeaponInput || _subPower < -_subWeaponInput) &&
            _canSelect && !_iskamaitati && MP >= _kamaitachiMP)
        {
            _canSelect = false;
            MP -= _kamaitachiMP;
            _sliderMP.MPSlider();
            _animator.Play("playerThrow");
            _iskamaitati = true;
            //二秒後にスキル発動      
            StartCoroutine(WeaponCoroutine(2, 0));
        }
    }
    private void _boomerangWeapon()
    {
        if ((_subPower > _subWeaponInput || _subPower < -_subWeaponInput) &&
            _canSelect && !_exists_boomerang && MP >= _booMP)
        {
            _canSelect = false;
            MP -= _booMP;
            _sliderMP.MPSlider();
            _animator.Play("playerThrow");
            //ブーメランの位置などをセットしてブーメランを投げる
            _playerPos = gameObject.transform.position;
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
            _boomerangPos = gameObject.transform.position;
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
        if ((_subPower > _subWeaponInput || _subPower < -_subWeaponInput) && 
            _canSelect && MP >= _kunaiMP)
        {
            _canSelect = false;
            MP -= _kunaiMP;
            _sliderMP.MPSlider();
            _animator.Play("playerThrow");
            //現在地にクナイを生成して指定時間後に消去する
            _playerPos = gameObject.transform.position;
            float time = 1f;
            GameObject newkunai = Instantiate(_kunai);
            _kunaiRd = newkunai.GetComponent<Rigidbody2D>();
            newkunai.transform.position = _playerPos;
            //プレイヤーの向きによって射出方向を決定
            //右に飛ばすelseは左
            if (_playerScript._isLeft)
            {
                newkunai.transform.Rotate(new Vector3(0, -180, -90));
                this._kunaiRd.AddForce(new Vector2(-1500f, 0f));
                Destroy(newkunai, time);
            }
            else
            {

                newkunai.transform.Rotate(new Vector3(0, 0, -90));
                this._kunaiRd.AddForce(new Vector2(1500f, 0f));
                Destroy(newkunai, time);
            }
            StartCoroutine(WeaponCoroutine(0.1f, 2));
        }
    }
    private void Fire(Vector2 transform)
    {
        if (!_isfireSet)
        {
            _fire.SetActive(true);
            _playerPos = gameObject.transform.position;
            _fire.transform.position = _playerPos;
            _isfireSet = true;
            StartCoroutine(WeaponCoroutine(1f, 3));
        }
        _fire.transform.position = Vector2.MoveTowards
                (_fire.transform.position, transform, _fireSpeed * Time.deltaTime);

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
                _canSelect = true;
            }
        }
    }
}



