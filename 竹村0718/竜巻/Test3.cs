using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test3 : MonoBehaviour
{
    //�����̓����蔻������{�̂ɃA�^�b�`����
    Test2 _test2;
    Rigidbody2D _rd;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            _test2 = collision.GetComponent<Test2>();
            _rd = collision.GetComponent<Rigidbody2D>();
            _test2._hit = true;
            _rd.gravityScale = 0.1f;
            //���������̏d�͂���߂�
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            _test2 = collision.GetComponent<Test2>();
            _test2._hit = false;
            _rd = collision.GetComponent<Rigidbody2D>();
            _rd.gravityScale = 1;
            //��������o����d�͂�߂�
        }
    }
}
