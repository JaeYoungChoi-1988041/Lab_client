using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public StateType ST;
    public AnimType AT; 
    public static JobType JT;

    public Stats stats = new Stats();
    private All all = new All();
    public Stats Stats
    {
        get { return stats; }
        set { stats = value; }
    }
    public All All
    {
        get { return  all; }
        set { all = value; }
    }

    public float curHP;
    public float curMP;
    public int curexp;

    MeshRenderer[] meshs;
    Animator anim;
    Rigidbody rigid;

    WaitForSeconds WFS01 = new WaitForSeconds(0.1f);
    WaitForSeconds WFS3 = new WaitForSeconds(3f);
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        meshs = GetComponentsInChildren<MeshRenderer>();
    }
    void Start()
    {
        anim.SetBool("Live",true);
        curHP = stats.maxHp;
        curMP = stats.maxMp;
    }

    void Update()
    {
        Recovery();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MonsterAttack"))
        {
            if (ST == StateType.Playing) //Playing������ ���� ����� �޵���
            {
                TS ma = other.GetComponent<TS>(); //�׽�Ʈ�� ���߿� �ٲ� ��
                curHP -= ma.damage;
                Debug.Log(curHP);
                StartCoroutine(DamageAction());
            }
        }
    }


    void Recovery()
    {
        StartCoroutine(RegainHp());
        StartCoroutine(RegainMp());
    }

    IEnumerator RegainHp()
    {
        if (curHP < stats.maxHp && ST != StateType.Die)
            curHP += stats.maxHp * stats.genHp;
        else if(curHP > stats.maxHp)
            curHP = stats.maxHp;
        yield return null;
    }
    IEnumerator RegainMp()
    {
        if (curMP < stats.maxMp && ST != StateType.Die)
            curMP += stats.maxMp * stats.genMp;
        else if(curMP > stats.maxMp)
            curMP = stats.maxMp;
        yield return null;
    }

    IEnumerator DamageAction()
    {
        ST = StateType.Unbeatable; //�ǰݽ� ����

        foreach (MeshRenderer mesh in meshs)
            mesh.material.color = Color.red;
        yield return WFS01; //0.1��

        if (curHP > 0)
        {
            ST = StateType.Playing;
            foreach (MeshRenderer mesh in meshs)
                mesh.material.color = new Color(1, 1, 1);
        }
        else
        {
            ST = StateType.Die;
            anim.SetBool("Live",false);
            anim.SetTrigger("Die"); //����ִϸ��̼�
            gameObject.layer = 6;//����� ���̾� ����
            yield return WFS3;

            //transform.position = Translate(); //����� ������ �̵�
            gameObject.layer = 7;
            ST = StateType.Playing;
            anim.SetBool("Live",true);
            curHP = stats.maxHp;
            curMP = stats.maxMp;
        }
    }
}