using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float coolDown;
    public float atkPower;
    public float skillCoolDown;
    public float skillCost;
    public AudioClip skillSound;
    public ParticleSystem skillEffect;
    public AnimationClip skillAnim;
    public UnityEngine.UI.Image skillImage;

    public virtual void Ability()
    {

    }
}
