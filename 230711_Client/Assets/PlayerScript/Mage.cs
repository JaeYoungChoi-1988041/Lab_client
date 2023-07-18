using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Mage : Player
{
    bool attack;
    public GameObject magic;
    public ParticleSystem attackef;

    Transform magicpos;
    GameObject weapon;

    WaitForSeconds WFS035 = new WaitForSeconds(0.35f);
    protected override void Start()
    {
        base.Start();
        weaponpos = gameObject.GetComponentInChildren<BoxCollider>();
        magicpos = weaponpos.GetComponentInChildren<BoxCollider>().transform; //��ü ���� �� rigidbody.velocity����� ���� ȭ����ǥ
        attackef.Stop();
    }

    protected override void Update()
    {
        base.Update();
        if (ST != StateType.Die && (AT == AnimType.Idle || AT == AnimType.Walk))
            AttackInput();
    }
    void AttackInput()
    {
        if (!attack && Input.GetMouseButton(0))
            StartCoroutine(AttackAction());
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            StartCoroutine(LotusExplosion());
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            StartCoroutine(ShiningWave());
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            StartCoroutine(ManaSpring());
    }
    /// <summary>
    /// �⺻����
    /// </summary>
    IEnumerator AttackAction() //�⺻���� �ִϸ��̼�
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
    void Shot() //�⺻���� ����
    {
        weapon = Instantiate(magic, magicpos.position, magicpos.rotation);
        Rigidbody weaponr = weapon.GetComponent<Rigidbody>();
        weaponr.velocity = Vector3.Lerp(magicpos.position, magicpos.forward * 10, 1f);
    }
    /// <summary>
    /// ��ų
    /// </summary>
    /// <returns></returns>
    IEnumerator LotusExplosion()//������ ����
    {
        AT = AnimType.Skill1;
        Debug.Log("LotusExplosion");
        anim.SetTrigger("LotusExplosion");
        yield return null;

        AT = AnimType.Idle;
    }
    IEnumerator ShiningWave() //���� �ĵ�
    {
        AT = AnimType.Skill2;
        Debug.Log("ShiningWave");
        anim.SetTrigger("ShiningWave");
        yield return null;

        AT = AnimType.Idle;
    }
    IEnumerator ManaSpring()//������ ��
    {
        AT = AnimType.Skill3;
        Debug.Log("ManaSpring");
        anim.SetTrigger("ManaSpring");
        yield return null;

        AT = AnimType.Idle;
    }
}
