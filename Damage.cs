using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public float knockBackPower;   // �m�b�N�o�b�N�������
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
            //Debug.Log("�G�A�m�b�N�o�b�N");
            Enemy = col;
            targetObj.GetComponent<PlayerMove_KM2>().ShowLog();
            ////�ύX �m�[�}������Ȃ��i�_���[�W���A���G���j�Ƃ��̓��^�[��
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

            //// �����̈ʒu�ƐڐG���Ă����I�u�W�F�N�g�̈ʒu�Ƃ��v�Z���āA�����ƕ������o���Đ��K��(���x�x�N�g�����Z�o)
            //Vector2 distination = (transform.position - col.transform.position).normalized;

            //rb.AddForce(distination * knockBackPower);


        }
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("�G�A�m�b�N�o�b�N");
            Enemy = col;
            targetObj.GetComponent<PlayerMove_KM2>().ShowLog();
        }
    }
    


}
