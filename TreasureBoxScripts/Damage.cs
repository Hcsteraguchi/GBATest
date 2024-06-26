using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {
    [Header("�U�������Gor�G�U������E����������")] public Collider2D _attackEnemy; //���J�K�v
    [Header("Player�I�u�W�F�N�g���i�[")] public GameObject _playerObj; //player�I�u�W�F�N�g���i�[�A���J�K�v

    //�_���[�W�p�ڐG����
    private void OnTriggerEnter2D(Collider2D col) {
        //EnemyAttack�Ƃ̐ڐG
        if (col.gameObject.tag == "EnemyAttack") {

            //�G�U������擾
            _attackEnemy = col;

            //playerMove_KM�̃_���[�W���̍s���̋N�� 
            _playerObj.GetComponent<PlayerMove_KM>().DamageSet();
        }
        //Enemy�Ƃ̐ڐG
        if (col.gameObject.tag == "Enemy") {

            //�U�������G�擾
            _attackEnemy = col;

            //playerMove_KM�̃_���[�W���̍s���̋N�� 
            _playerObj.GetComponent<PlayerMove_KM>().DamageSet();
        }
    }
    //private void OnTriggerStay2D(Collider2D col) {
    //    //EnemyAttack�Ƃ̐ڐG
    //    if (col.gameObject.tag == "EnemyAttack") {

    //        //�G�U������擾
    //        _attackEnemy = col;

    //        //playerMove_KM�̃_���[�W���̍s���̋N�� 
    //        _playerObj.GetComponent<PlayerMove_KM>().DamageSet();
    //    }
    //    //Enemy�Ƃ̐ڐG
    //    if (col.gameObject.tag == "Enemy") {

    //        //�U�������G�擾
    //        _attackEnemy = col;

    //        //playerMove_KM�̃_���[�W���̍s���̋N�� 
    //        _playerObj.GetComponent<PlayerMove_KM>().DamageSet();
    //    }

    //}
    //private void OnTriggerExit2D(Collider2D col) {
    //    //EnemyAttack�Ƃ̐ڐG
    //    if (col.gameObject.tag == "EnemyAttack") {

    //        //�G�U������擾
    //        _attackEnemy = col;

    //        //playerMove_KM�̃_���[�W���̍s���̋N�� 
    //        _playerObj.GetComponent<PlayerMove_KM>().DamageSet();
    //    }
    //    //Enemy�Ƃ̐ڐG
    //    if (col.gameObject.tag == "Enemy") {

    //        //�U�������G�擾
    //        _attackEnemy = col;

    //        //playerMove_KM�̃_���[�W���̍s���̋N�� 
    //        _playerObj.GetComponent<PlayerMove_KM>().DamageSet();
    //    }

    //}


}
