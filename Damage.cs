using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{   
    public Collider2D Enemy; 
    public GameObject targetObj; 
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyAttack")
        {           
            Enemy = col;
            targetObj.GetComponent<PlayerMove_KM>().ShowLog();           
        }
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("�G�A�m�b�N�o�b�N");
            Enemy = col;
            targetObj.GetComponent<PlayerMove_KM>().ShowLog();
        }
    }
    


}
