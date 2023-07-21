using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smite : Skill
{
    private void Awake()
    {
        this.skillCoolDown = 0f;
        this.skillCost = 5f;
        this.coolDown = 5f;
        //InvokeRepeating("SkillCoolDown",1f,1f);
        Debug.Log("��Ÿ �߰���");
    }
    public override bool Ability(Player player)
    {
        if (this.skillCoolDown <= 0)
        {
            Debug.Log("��Ÿ ���");
            // �ִϸ��̼� ����, �ִϸ��̼ǿ��� ����Ʈ ����, ���� ����, ��ɽ���
            this.skillCoolDown = this.coolDown;
            this.skillImage.fillAmount = 0f;
            this.skillCoolDown = this.coolDown;
            //InvokeRepeating("SkillCoolDown", 1f, 1f);
            return true;
        }
        else
        {
            return false;
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