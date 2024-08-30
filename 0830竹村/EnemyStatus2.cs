using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class EnemyStatus2 : MonoBehaviour
{
    //[SerializeField] GameObject playerObj;
    SpriteRenderer spriteRenderer;
    DamageMain DamageMain;
    [Header("�G��HP������")] [SerializeField]private int _enemyHp = 20;
    private Animator _deadAnim;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _DeadSe;
    public bool _isSurvival = true;
    [SerializeField] private float _deadTime = 2f;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _deadAnim = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�v���C���[�̍U�������������ۂɂ��̕���̍U���͂��Q�Ƃ���HP�����炷
        if (collision.gameObject.tag == "PlayerWeapon")
        {
            DamageMethod();
            DamageMain = collision.gameObject.GetComponent<DamageMain>();
            int damage = DamageMain.Damage;
            _enemyHp = _enemyHp - damage;
            print(_enemyHp);

            //Hp��0����������珈��������f�X�g���C�̓e�X�g
            if (_enemyHp <= 0)
            {
                Dead();              
            }
        }
    }
    private async void Dead()
    {
        _isSurvival = false;
        _deadAnim.Play("Dead",0,0f);
        _audioSource.PlayOneShot(_DeadSe);
        await UniTask.Delay(TimeSpan.FromSeconds(_deadTime));
        this.gameObject.transform.position = new Vector2(0, -20);
        _enemyHp = 20;
        _isSurvival = true;
        gameObject.SetActive(false);
    }
    private async void DamageMethod()
    {

        spriteRenderer.enabled = true;
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        spriteRenderer.enabled = false;
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        spriteRenderer.enabled = true;
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        spriteRenderer.enabled = false;
        await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
        spriteRenderer.enabled = true;
    }
}
