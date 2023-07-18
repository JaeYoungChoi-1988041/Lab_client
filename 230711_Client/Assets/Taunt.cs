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
        Debug.Log("도발 추가됨");
    }
    public override void Ability()
    {
        if (this.skillCoolDown <= 0)
        {
            Debug.Log("도발 사용");
            // 애니메이션 실행,이펙트 실행, 사운드 실행
            // 기능실행, 플레이어 마나감소
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