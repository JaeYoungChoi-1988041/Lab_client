using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarriorSkills : MonoBehaviour
{
    // ��ų ����� �� ���� �� �˸��߰�. while�� ���� �� �ִ��� , ��Ÿ�� �ڷ�ƾ �ι� ȣ��Ǵ°� Ȯ��
    Skill smite;
    Skill bear;
    Skill taunt;

    public List<Skill> skills;
    public List<Skill> coolDownSkills;
    public GameObject skillPanel;
    /// <summary> ��ų�ý��ۿ��� �ҷ����� �÷��̾� ��ũ��Ʈ </summary>
    public Player player;

    Image[] SkillSlots;

    /// <summary> ��Ÿ�� �����ֱ� </summary>
    private float decrease = 0.1f;
    private void Awake()
    {
        SkillSlots = skillPanel.GetComponentsInChildren<Image>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
    }
    public void AddSkill3()
    {
        if (taunt != null) return;
        taunt = gameObject.AddComponent<Taunt>();
        skills.Add(taunt);
        taunt.skillImage = SkillSlots[3];
        taunt.skillImage.fillAmount = 1f;
    }
    public void UseSkill(int i)
    {
        if (skills[i].Ability(player))
        {
            coolDownSkills.Add(skills[i]);
            if (coolDownSkills.Count == 1) StartCoroutine(SkillCoolDown());
        }
    }
    IEnumerator SkillCoolDown()
    {
        while (coolDownSkills.Count != 0)
        {

            for (int i = coolDownSkills.Count - 1; i >= 0; i--)
            {
                Skill skill = coolDownSkills[i];
                skill.skillCoolDown-= decrease;
                skill.skillImage.fillAmount = (skill.coolDown - skill.skillCoolDown) / skill.coolDown;

                if (skill.skillCoolDown <= 0)
                {
                    skill.skillCoolDown = 0;
                    coolDownSkills.RemoveAt(i);
                }
            }
            yield return new WaitForSeconds(decrease);
        }
    }
}