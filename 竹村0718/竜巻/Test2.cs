using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    //�G�̋��ʏ����Ƀ}�[�W����
    [SerializeField] private bool _istypoon = true;
    public bool _hit = default;
    //�v���C���[
    private GameObject _player;
    Test _test;
    private void Start()
    {
        //�v���C���[��T��
        _player = GameObject.Find("Player");
        _test = _player.GetComponent<Test>();
    }
    private void FixedUpdate()
    {
        //�����̌��ʂ���炤
        if (_istypoon&&_hit)
        {
            _test.Typoon(this.gameObject);
        }
    }
}
