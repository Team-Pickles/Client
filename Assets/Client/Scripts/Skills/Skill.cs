using System.Collections;
using UnityEngine;

public abstract class Skill
{
    public abstract void OnChange(); // change animation
    public abstract void OnStart(); // start frame
    public abstract void OnSkill(); // attack
    public abstract IEnumerator OnFire();
    public abstract void OnEnd();   // return animation
}
