using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    public Animator vacuumAnimator;
    private Skill currentSkill;// = new Vacuum();
    public void ChangeSkill(Skill newSkill)
    {
        currentSkill = newSkill;
        currentSkill.OnChange();
    }
    void Start()
    {
        ChangeSkill(new Vacuum());
    }
    private void FixedUpdate()
    {
        if ((GameObject.Find("Player").GetComponent<PlayerMoveManager>()._state & PlayerStateFlags.Stun) == 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (vacuumAnimator.gameObject.activeSelf)
                    currentSkill.OnStart();
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                if (vacuumAnimator.gameObject.activeSelf)
                    currentSkill.OnEnd();
            }
            else if (Input.GetKey(KeyCode.A))
            {
                if (vacuumAnimator.gameObject.activeSelf)
                    currentSkill.OnSkill();
            }
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        if ((GameObject.Find("Player").GetComponent<PlayerMoveManager>()._state & PlayerStateFlags.Stun) == 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (vacuumAnimator.gameObject.activeSelf)
                    vacuumAnimator.SetBool("isConsuming", true);
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                if (vacuumAnimator.gameObject.activeSelf)
                    vacuumAnimator.SetBool("isConsuming", false);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(currentSkill.OnFire());
            }
        }
    }
}