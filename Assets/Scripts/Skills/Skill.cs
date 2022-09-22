using System.Collections;
using UnityEngine;

public abstract class Skill
{
    public abstract void OnStart(); // change animation
    public abstract void OnSkill(); // attack
    public abstract IEnumerator OnFire();
    public abstract void OnEnd();   // return animation
}
