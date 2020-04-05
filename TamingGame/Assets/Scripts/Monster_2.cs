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
        image.GetComponent<AnimationEvent>().add = new AnimationEvent.Add(Attack);

        while (monsterState == MonsterState.Attack)
        {
            yield return null;
        }

        NextState();
    }
    IEnumerator DeadState()
    {
        animator.CrossFade("Dead", 0.3f);
        isNoticeTarget = false;

        Vector3 viewportPos = inGameMgr.mainCam.WorldToViewportPoint(this.transform.position);
        RectTransform _rectTmCanvas = inGameMgr.UICamera.transform.Find("Canvas").GetComponent<RectTransform>();
        Vector3 uiPosInCanvas = new Vector3((viewportPos.x * _rectTmCanvas.rect.width) - (_rectTmCanvas.rect.width * 0.5f),
            (viewportPos.y * _rectTmCanvas.rect.height) - (_rectTmCanvas.rect.height * 0.5f), 0.0f);

        GameObject _ui = Instantiate(resourceMgr.ui["AfterDeadUIMonster"]) as GameObject;
        _ui.transform.name = "AfterDeadUIMonster";
        _ui.transform.parent = _rectTmCanvas.Find("WorldUIBox");
        _ui.transform.localScale = Vector3.one;
        _ui.transform.localPosition = uiPosInCanvas;

        _ui.GetComponent<AfterDeadUIMonster>().inGameMgr = inGameMgr;
        _ui.GetComponent<AfterDeadUIMonster>().resourceMgr = resourceMgr;
        _ui.GetComponent<AfterDeadUIMonster>().target = this.transform;
        _ui.SetActive(true);

        //히어로 공격목록에 있으면 삭제
        if (hero.attackMonster.Find(_monster => _monster == this) != null)
        {
            hero.attackMonster.Remove(this);
        }

        //테이밍된 몬스터일 경우
        if (hero.havingMonster.Find(_monster => _monster == this) != null)
        {
            hero.havingMonster.Remove(this);
        }

        while (monsterState == MonsterState.Dead)
        {
            yield return null;
        }

        NextState();
    }

}
