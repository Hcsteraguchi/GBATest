using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackErea : MonoBehaviour
{
    //[Header("�U�����葶�ݎ���")] [SerializeField] float _attckEreaTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        //Invoke("Destroy", _attckEreaTime);//�U������j��x��
    }
    
    void Destroy()
    {
        //this.gameObject.SetActive(false);
        /*Destroy(gameObject);*///�U������̔j��

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            /*Destroy(col.gameObject);*///�U�������Ώۂւ̊���(��)

        }
    }
}
