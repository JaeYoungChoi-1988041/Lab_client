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
        weaponpos = gameObject.GetComponentInChildren<BoxCollider>(); //화살이 발사되는 위치
        arrowpos = weaponpos.GetComponentInChildren<BoxCollider>().transform; //화살 생성 후 rigidbody.velocity사용을 위한 화살좌표
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
    /// 기본공격
    /// </summary>
    IEnumerator AttackAction() //공격 애니메이션
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
    void Shot() //화살생성
    {
        GameObject weapon = Instantiate(arrow, arrowpos.position, arrowpos.rotation);
        Rigidbody weaponr = weapon.GetComponent<Rigidbody>();
        weaponr.velocity = Vector3.Lerp(arrowpos.position, arrowpos.forward*10, 1f);
    }

    /// <summary>
    /// 스킬
    /// </summary>
    /// <returns></returns>
    IEnumerator WindBlessing()//바람의 축복
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
    IEnumerator SplitArrow()//스플릿애로우
    {
        AT = AnimType.Skill3;
        Debug.Log("SplitArrow");
        yield return null;

        AT = AnimType.Idle;
    }
    IEnumerator LonginusSpear() //롱기누스의 창
    {
        AT = AnimType.Skill2;
        Debug.Log("LonginusSpear");
        anim.SetTrigger("LonginusSpear");
        yield return null;

        AT = AnimType.Idle;
    }
}
