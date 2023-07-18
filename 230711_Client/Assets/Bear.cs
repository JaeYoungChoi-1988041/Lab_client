using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bear : Skill
{
    private void Awake()
    {
        this.skillCoolDown = 0f;
        this.skillCost = 0f;
        this.coolDown = 45;
        //InvokeRepeating("SkillCoolDown",1f,1f);
        Debug.Log("���� Ȱ�� �߰���");
    }
    public override void Ability()
    {
        if (skillCoolDown <= 0)
        {
            Debug.Log("����Ȱ�� ���");
            // �ִϸ��̼� ����,����Ʈ ����, ���� ����
            // ��ɽ���
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