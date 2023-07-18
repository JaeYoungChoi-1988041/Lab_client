using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : Skill
{
    private void Awake()
    {
        this.skillCoolDown = 0f;
        this.skillCost = 15f;
        this.coolDown = 15;
        //InvokeRepeating("SkillCoolDown",1f,1f);
        Debug.Log("���� �߰���");
    }
    public override void Ability()
    {
        if (this.skillCoolDown <= 0)
        {
            Debug.Log("���� ���");
            // �ִϸ��̼� ����,����Ʈ ����, ���� ����
            // ��ɽ���, �÷��̾� ��������
            this.skillCoolDown = this.coolDown;
            this.skillImage.fillAmount = 0f;
            this.skillCoolDown = this.coolDown;
            //InvokeRepeating("SkillCoolDown", 1f, 1f);
            return;
        }
    }

    void SkillCoolDown()
    {
        if (this.skillCoolDown <= 0)
        {
            this.skillCoolDown = 0;
            CancelInvoke("SkillCoolDown");
            return;
        }
        this.skillCoolDown--;
        this.skillImage.fillAmount = (this.coolDown - this.skillCoolDown) / this.coolDown;
    }
}