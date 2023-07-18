using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorSkills : MonoBehaviour
{
    Skill smite;
    Skill bear;
    Skill taunt;

    public List<Skill> skills;
    public List<Skill> coolDownSkills;
    public GameObject skillPanel;
    Image[] SkillSlots;
    private void Awake()
    {
        SkillSlots = skillPanel.GetComponentsInChildren<Image>();

        smite = gameObject.AddComponent<Smite>();
        skills.Add(smite);
        smite.skillImage = SkillSlots[1];
        smite.skillImage.fillAmount = 1f;
        coolDownSkills.Add(smite);
    }
    private void Start()
    {
        StartCoroutine(SkillCoolDown());
        
    }
    public void AddSkill2()
    {
        if (bear != null) return;
        bear = gameObject.AddComponent<Bear>();
        skills.Add(bear);
        bear.skillImage = SkillSlots[2];
        bear.skillImage.fillAmount = 1f;
        coolDownSkills.Add(bear);
    }
    public void AddSkill3()
    {
        if (taunt != null) return;
        taunt = gameObject.AddComponent<Taunt>();
        skills.Add(taunt);
        taunt.skillImage = SkillSlots[3];
        taunt.skillImage.fillAmount = 1f;
        coolDownSkills.Add(taunt);
    }
    public void UseSkill(int i)
    {
        skills[i].Ability();
        coolDownSkills.Add(skills[i]);
    }
    IEnumerator SkillCoolDown()
    {
        while (coolDownSkills.Count != 0)
        {

            for (int i = coolDownSkills.Count - 1; i >= 0; i--)
            {
                Skill skill = coolDownSkills[i];
                skill.skillCoolDown--;
                skill.skillImage.fillAmount = (skill.coolDown - skill.skillCoolDown) / skill.coolDown;

                if (skill.skillCoolDown <= 0)
                {
                    skill.skillCoolDown = 0;
                    coolDownSkills.RemoveAt(i);
                }
            }
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);
    }
}