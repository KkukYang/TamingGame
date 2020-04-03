using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_2 : Monster
{
    protected override void Awake()
    {
        base.Awake();

        Debug.Log("Monster_2");
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }


    protected override void NextState()
    {
        string methodName = monsterState.ToString() + "State";
        StartCoroutine(methodName);
    }

    IEnumerator IdleState()
    {
        animator.CrossFade("Idle", 0.3f);

        while (monsterState == MonsterState.Idle)
        {
            yield return null;
        }

        NextState();
    }

    IEnumerator RunState()
    {
        animator.CrossFade("Run", 0.3f);



        while (monsterState == MonsterState.Run)
        {
            yield return null;
        }

        NextState();
    }
    IEnumerator AttackState()
    {
        animator.CrossFade("Attack", 0.3f);

        while (monsterState == MonsterState.Attack)
        {
            yield return null;
        }

        NextState();
    }
    IEnumerator DeadState()
    {
        animator.CrossFade("Dead", 0.3f);

        while (monsterState == MonsterState.Dead)
        {
            yield return null;
        }

        NextState();
    }

}
