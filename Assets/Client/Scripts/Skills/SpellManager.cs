using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{
    enum KeyPressed
    {
        NONE, UP, DOWN, ING
    }

    private KeyPressed _keyPressed = KeyPressed.NONE;
    private Skill currentSkill;
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
            switch (_keyPressed)
            {
                case KeyPressed.DOWN:
                {
                    currentSkill.OnStart();
                    break;
                }
                case KeyPressed.UP:
                {
                    currentSkill.OnEnd();
                    break;
                }
                case KeyPressed.ING:
                {
                    currentSkill.OnSkill();
                    break;
                }
                default:
                {
                    break;
                }
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
                _keyPressed = KeyPressed.DOWN;
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                _keyPressed = KeyPressed.UP;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _keyPressed = KeyPressed.ING;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(currentSkill.OnFire());
            }
        }
    }
}