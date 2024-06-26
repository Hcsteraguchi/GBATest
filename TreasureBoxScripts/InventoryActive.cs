using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryActive : MonoBehaviour
{
    [Header("�\���ꏊ")] [SerializeField] private Vector3 _displayPos = default;
    [Header("��������")][SerializeField] private float _activeTime = default;
    public float _deleteTime = default;
    private GameObject _playerObject = default;
    // Start is called before the first frame update
    void Start()
    {
        _playerObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // �v���C���[�̈ʒu���W���X�V��������
        Vector3 playerPos = _playerObject.transform.localPosition;
        // �C���x���g�������ւ����u�ԂɌv���J�n
        _deleteTime += Time.deltaTime;
        // �v���C���[�̓���ɔz�u
        gameObject.transform.position = playerPos + _displayPos;
        if(_activeTime < _deleteTime) // ���Ԍo�ߌ�C���x���g����false�ɂ���
        {
            this.gameObject.SetActive(false);
        }
    }
  
}
