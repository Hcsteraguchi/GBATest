using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SubWeapon : MonoBehaviour
{
    //   if (Input.GetKeyDown(KeyCode.E))�̏������R���g���[���[�̃X�L�������L�[�ɒu�������āI�I�I
    /// <summary>
    /// ���킲�ƂɐU��ꂽ�ŗL�ԍ��Ǘ��p��Enum
    /// </summary>
    public enum WeaponSelect
    {
        Sword=0,
        Kunai=1,
        Boomerang=2,
        kamaitachi=3,
    }
    //�N�i�C�p
    //�v���C���[�ʒu�ƃN�i�C�I�u�W�F
    [Header("�v���C���[�I�u�W�F�N�g����Ă�")][SerializeField] GameObject _player = null;
    [SerializeField] GameObject _kunai = null;
 
    Rigidbody2D _kunaiRd;

    //�u�[�������p

    [SerializeField] GameObject _boomerang = null;
    [SerializeField] GameObject _boomerangTraget = null;
    private float _boomerangSpeed = 15f;
    private Vector3 _boomerangPos;


    //���ɓ������ォ�̔���E�u�[���������߂�ʒu�ɂ������̔���
    private  bool _existsBoomerang = default;
    private  bool _canReturn = default;
    //���܂������p

    [SerializeField] GameObject _kamaitachi = null;

    //����

    private Vector2 _playerPos;
    PlayerMove_KM _playerScript;
    public int _damage=0;
    public WeaponSelect _weaponSelect=0;
  
  
    void Start()
    {
       
        _playerPos = _player.transform.position;
        _kunai.transform.position = _playerPos;
        _playerScript = _player.GetComponent<PlayerMove_KM>();
        _weaponSelect = 0;
        /**�v���C���[�̈ʒu���i�[
         * �v���C���[�ʒu�ɃN�i�C���i�[
         * �v���C���[�̃X�N���v�g���i�[
         * ���ݕ���̏�����
         **/

        _boomerang.SetActive(false);
        _kamaitachi.SetActive(false);    
    }
    void Update()
    {
        //�I������Ă��镐����A�b�v�f�[�g�ɌĂяo��
      switch(_weaponSelect)
        {
            case WeaponSelect.Sword:
                break;
            case WeaponSelect.Kunai:
                KunaiWeapon();
                break;
            case WeaponSelect.Boomerang:
                BoomerangWeapon();
                break;
            case WeaponSelect.kamaitachi:
                KamaitachiWeapon();
                break;
        }
        //����̐؂�ւ�
        //�e�X�g�p�̋@�\�Ȃ̂ŃC���x���g���@�\���ł�����u�������܂�
        if (Input.GetButtonDown("Change"))
        {
            if ((int)_weaponSelect == 3)
            {
                print("�����Ă��");
                _weaponSelect = 0;
            }
            else
            {
                print((int)_weaponSelect);
             _weaponSelect++ ;
            }
        }
        //�u�[�������̈ړ������X�C�b�`���ɓ����Ƒ����΂���܂�
        if (_existsBoomerang)
        {
            BoomerangOperation();
        }
        //�ȉ��e�X�g�p
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
                break;
            case 1:
                print("�߂�");
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
        if (Input.GetButtonDown("SubAttack"))
        {
            //��b��ɃX�L������
            StartCoroutine(WeaponCoroutine(2, 0));
        }
    }
    private void BoomerangWeapon()
    {
        if (Input.GetButtonDown("SubAttack") && !_existsBoomerang)
        {
            //�u�[�������̈ʒu�Ȃǂ��Z�b�g���ău�[�������𓊂���
            _playerPos = _player.transform.position;
            _boomerang.SetActive(true);
            _boomerang.transform.position = _playerPos;
            _boomerangPos = _boomerangTraget.transform.position;
            _existsBoomerang = true;
            //1.2�b��Ƀu�[�������̈ړ�������ύX����
            StartCoroutine(WeaponCoroutine(1.2f, 1));
        }
       
    }
    private void BoomerangOperation()
    {
        if (_canReturn)
        {
            //�u�[�������̖߂�O��
            _boomerangPos = _player.transform.position;
            _boomerang.transform.position = Vector2.MoveTowards
                (_boomerang.transform.position, _boomerangPos, _boomerangSpeed * Time.deltaTime);
        }
        else
        {
            //�u�[���������ڂ̋O��
            _boomerang.transform.position = Vector2.MoveTowards
           (_boomerang.transform.position, _boomerangPos, _boomerangSpeed * Time.deltaTime);
        }

    }
     private void KunaiWeapon()
    {
        //�N�i�C����
        if (Input.GetButtonDown("SubAttack"))
        {
            //���ݒn�ɃN�i�C�𐶐����Ďw�莞�Ԍ�ɏ�������
            _playerPos = _player.transform.position;
            float time = 1f;
            GameObject newkunai = Instantiate(_kunai);
            _kunaiRd = newkunai.GetComponent<Rigidbody2D>();
            newkunai.transform.position = _playerPos;
            //�v���C���[�̌����ɂ���Ďˏo����������
            //�E�ɔ�΂�else�͍�
            if (_playerScript._isLeft)
            {
                this._kunaiRd.AddForce(new Vector2(-1500f, 0f));
            }
            else
            {
                this._kunaiRd.AddForce(new Vector2(1500f, 0f));
            }
            Destroy(newkunai, time);
          
        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�v���C���[�ɂԂ������I�u�W�F�N�g���u�[�������Ȃ珈��������
        if (collision.gameObject == _boomerang)
        {
            //���ɖ߂菈���ɓ˓����Ă���̂Ȃ�������
            if (_canReturn)
            {
                print("����");
                _canReturn = false;
                _existsBoomerang = false;
                _boomerangSpeed = 15;
                _boomerang.SetActive(false);
            }
        }
    }
}



