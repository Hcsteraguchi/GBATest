using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackErea : MonoBehaviour
{
    [SerializeField] float attckEreaTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("BRK", attckEreaTime);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void BRK()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Destroy(col.gameObject);
        }

    }
}
