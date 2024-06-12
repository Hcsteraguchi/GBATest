using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("�U�������Gor�G�U������E����������")] public Collider2D�@_attackEnemy; //���J�K�v
    [Header("Player�I�u�W�F�N�g���i�[")] public GameObject _playerObj; //player�I�u�W�F�N�g���i�[�A���J�K�v
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyAttack")//EnemyAttack�Ƃ̐ڐG
        {
            _attackEnemy = col;//�G�U������擾
            _playerObj.GetComponent<PlayerMove_KM>().ShowLog();//playerMove_KM�̃_���[�W���̍s���̋N��        
        }
        if (col.gameObject.tag == "Enemy")//Enemy�Ƃ̐ڐG
        {    
            _attackEnemy = col;//�U�������G�擾
            _playerObj.GetComponent<PlayerMove_KM>().ShowLog();//playerMove_KM�̃_���[�W���̍s���̋N�� 
        }
    }
    


}
