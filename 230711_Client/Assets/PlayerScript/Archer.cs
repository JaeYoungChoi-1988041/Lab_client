using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Archer : Player
{
    bool attack;
    public GameObject arrow;

    Transform arrowpos;

    WaitForSeconds WFS035 = new WaitForSeconds(0.35f);

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        weaponpos = gameObject.GetComponentInChildren<BoxCollider>(); //ȭ���� �߻�Ǵ� ��ġ
        arrowpos = weaponpos.GetComponentInChildren<BoxCollider>().transform; //ȭ�� ���� �� rigidbody.velocity����� ���� ȭ����ǥ
        //attackef.Stop();
    }

    protected override void Update()
    {
        base.Update();
        if(ST != StateType.Die && (AT == AnimType.Idle || AT == AnimType.Walk))
            AttackInput();
    }
    void AttackInput()
    {
        if (!attack && Input.GetMouseButton(0))
            StartCoroutine(AttackAction());
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            StartCoroutine(WindBlessing());
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            StartCoroutine(LonginusSpear());
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            StartCoroutine(SplitArrow());
    }

    /// <summary>
    /// �⺻����
    /// </summary>
    IEnumerator AttackAction() //���� �ִϸ��̼�
    {
        attack = true;
        AT = AnimType.Attack;
        anim.SetTrigger("Attack");
        anim.SetFloat("AttackSpeed", stats.attackSpeed);
        yield return WFS035;

        Shot();
        AT = AnimType.Idle;
        delay = 0;
        yield return new WaitUntil(() => delay > 1 / stats.attackSpeed);

        attack = false;
    }
    void Shot() //ȭ�����
    {
        GameObject weapon = Instantiate(arrow, arrowpos.position, arrowpos.rotation);
        Rigidbody weaponr = weapon.GetComponent<Rigidbody>();
        weaponr.velocity = Vector3.Lerp(arrowpos.position, arrowpos.forward*10, 1f);
    }

    /// <summary>
    /// ��ų
    /// </summary>
    /// <returns></returns>
    IEnumerator WindBlessing()//�ٶ��� �ູ
    {
        AT = AnimType.Skill1;
        Debug.Log("WindBlessing");
        anim.SetTrigger("WindBlessing");
        curMP -= 30;
        stats.attackSpeed = stats.attackSpeed * 2;
        stats.moveSpeed = stats.moveSpeed * 2;

        yield return null;

        AT = AnimType.Idle;
    }
    IEnumerator SplitArrow()//���ø��ַο�
    {
        AT = AnimType.Skill3;
        Debug.Log("SplitArrow");
        yield return null;

        AT = AnimType.Idle;
    }
    IEnumerator LonginusSpear() //�ձ⴩���� â
    {
        AT = AnimType.Skill2;
        Debug.Log("LonginusSpear");
        anim.SetTrigger("LonginusSpear");
        yield return null;

        AT = AnimType.Idle;
    }
}
