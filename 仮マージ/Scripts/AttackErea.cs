using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackErea : MonoBehaviour
{
    //[Header("UŒ‚”»’è‘¶İŠÔ")] [SerializeField] float _attckEreaTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        //Invoke("Destroy", _attckEreaTime);//UŒ‚”»’è”j‰ó’x‰„
    }
    
    void Destroy()
    {
        //this.gameObject.SetActive(false);
        /*Destroy(gameObject);*///UŒ‚”»’è‚Ì”j‰ó

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            /*Destroy(col.gameObject);*///UŒ‚‚µ‚½‘ÎÛ‚Ö‚ÌŠ±Â(‰¼)

        }
    }
}
