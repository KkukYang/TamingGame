using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_4 : Monster
{
    protected override void Awake()
    {
        base.Awake();

        Debug.Log("Monster_4");
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        NextState();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }


    void NextState()
    {
        string methodName = monsterState.ToString() + "State";
        StartCoroutine(methodName);
    }

    IEnumerator IdleState()
    {
        animator.Play("Idle");

        while (monsterState == MonsterState.Idle)
        {
            yield return null;
        }

        NextState();
    }

    //IEnumerator RunState()
    //{
    //    animator.Play("Run", 0.3f);



    //    while (monsterState == MonsterState.Run)
    //    {
    //        yield return null;
    //    }

    //    NextState();
    //}
    IEnumerator AttackState()
    {
        animator.Play("Attack");
        image.GetComponent<AnimationEvent>().add = new AnimationEvent.Add(Attack);

        while (monsterState == MonsterState.Attack)
        {
            yield return null;
        }

        NextState();
    }
    IEnumerator DeadState()
    {
        animator.Play("Dead");
        Death();

        while (monsterState == MonsterState.Dead)
        {
            yield return null;
        }

        NextState();
    }
}
