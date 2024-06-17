using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossMove : MonoBehaviour
{
    Transform playerTr; // �v���C���[��Transform
    [SerializeField] float speed ; // �G�̓����X�s�[�h
    [SerializeField] float erea ; //�ړ��J�n����
    [SerializeField] float attack ; //�U������
    [SerializeField] private bool coolTime;//�U���Ԋu
    [SerializeField] private int moveSelect;//�����_���s��
    [SerializeField] private int moveCount;//�t�F�C���g�U����������
    [SerializeField] private int powerMove = 0;//�t�F�C���g�U���ׂ̈̃J�E���g
    //�����_���s���̍ő�A�ŏ��l
    private int min = 1;
    private int max = 4;
    //�J�E���g�̃��Z�b�g
    private int zero = 0;
    //�����A�傫���̒����X�N���v�g
    [SerializeField] private bool isBack;
    [SerializeField] Vector3 frontMove;
    [SerializeField] Vector3 backMove;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        // �v���C���[��Transform���擾�i�v���C���[�̃^�O��Player�ɐݒ�j
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (coolTime)//�A�j���[�V������bossStand�ȊO�̎��Ɏ��s�����X�N���v�g
        {
            //�v���C���[��attack���߂��ɂ���ƒǔ�����
            if (Vector2.Distance(transform.position, playerTr.position) < attack)
            {
                moveSelect = Random.Range(min, max); 
                if (powerMove == moveCount)
        {
            animator.Play("bossAtack4");
            powerMove =�@zero;
        }
        else if (moveSelect == 1)
        {
            animator.Play("bossAtack");
            powerMove++;
        }
        else if (moveSelect == 2)
        {
            animator.Play("bossAtack2");
            powerMove++;
        }
        else if (moveSelect == 3)
        {
            animator.Play("bossAtack3");
            powerMove++;
        }
            }
            else if (Vector2.Distance(transform.position, playerTr.position) < erea)
            {
                // �v���C���[�Ɍ����Đi��;
                transform.position = Vector2.MoveTowards(
                    transform.position,
               new Vector2(playerTr.position.x, transform.position.y),
                    speed * Time.deltaTime);
                animator.Play("bossWalk");
            }
            else
            {
                animator.Play("bossStand");
            }

            //�v���C���[�̕���������
            if (playerTr.position.x < transform.position.x)
            {
                transform.localScale = frontMove;
            }
            else if (playerTr.position.x > transform.position.x)
            {
                transform.localScale = backMove;
            }       
 

        }
    }
    }
