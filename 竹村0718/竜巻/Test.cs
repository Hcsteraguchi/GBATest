using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class Test : MonoBehaviour
{
    [Header("+-*/")][SerializeField] private int skillNum; //�X�L���ԍ�+-*/
    //[SerializeField] private int skillKindNum = 4; //�X�L���̎�ސ� 
    [SerializeField] private Action[] _skill=new Action[5]; //�X�L���̊֐����i�[����z��
   
    private const float _speed = 0.15f;//�����̋z���X�s�[�h
    private const float _speedTyphoon = 0.04f;//�����̈ړ��X�s�[�h
    private const float _typoonTime = 7f;// ������������
    private const float _coolTime = 10f;//����������̃N�[���^�C��
    [Header("�v���C���[�̊O�E�����{��")] [SerializeField] private GameObject _typhoon;
    [Header("�v���C���[�z���E�����̏����n�_")] [SerializeField] private GameObject _typoonStart;
    [Header("�v���C���[�z���E�����̍ŏI�ړ��n�_")] [SerializeField] private GameObject _typhoonTarget;
    [Header("�v���C���[�̊O�E�ʒu�̌Œ�Ɏg�����ߕ\������Ȃ��I�u�W�F�N�g")] [SerializeField]private GameObject _typoonStayPos;
  
    //[SerializeField] private List<GameObject> _enemys;
    //[SerializeField]private List<bool> _enemyTyhoon;
    private bool _isTypoon = default;
  
    public void SkillSet()
    {
        _skill[1] = S1TypoonSet;
    }

    private void Start()
    {
        //�֐��̔z��
        SkillSet();
        //���W�����Ă����Ă����ɑ䕗���o��������
        _typhoon.SetActive(false);
    }

    private void Update()
    {
        //�X�L���ԍ�����֐����Ăяo��
        if (Input.GetKeyDown(KeyCode.G))
        {
            print("Test");
            _skill[skillNum]();
        }

    }
    private void FixedUpdate()
    {
        //�����̈ړ�
        if (_isTypoon)
        {
            _typhoon.transform.position = Vector2.MoveTowards(_typhoon.transform.position, _typoonStayPos.transform.position, _speedTyphoon);
        }
    }
    private async void S1TypoonSet()
    {
        //�񓯊�����
        if(!_isTypoon)
        {
            _typhoon.SetActive(true);
            _typhoon.transform.position = _typoonStart.transform.position;
            _typoonStayPos.transform.position = _typhoonTarget.transform.position;
            print(_typoonStayPos);
            _isTypoon = true;
            //��������
            await UniTask.Delay(TimeSpan.FromSeconds(7f));
            _typhoon.SetActive(false);
            //�������ԏI����̃N�[���^�C��
            await UniTask.Delay(TimeSpan.FromSeconds(7f));
            _isTypoon = false;
        }
            
        
    }
    public void Typoon(GameObject enemy)
    { 
        //�����Ɋ������܂ꂽ�G�𗳊��̒��S�Ɉړ�������
      enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, _typhoon.transform.position, _speed);

    }
   
}
