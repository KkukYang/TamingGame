using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Rigidbody rigidBody;
    public InGameManager inGameMgr; //외부에서 할당
    public ResourceManager resourceMgr; //외부에서 할당.
    public Hero hero;
    public bool isTaming;
    public bool isNoticeTarget;
    public MonsterState monsterState;
    public Animator animator;
    public Transform image;
    public Vector3 initPos;
    public Vector3 prePos;
    public HPBar hpBar;

    public GameObject attackTarget;
    //public List<Monster> attackMonster = new List<Monster>();

    public float maxHp = 100;
    public float hp = 0;
    public float ap = 10;
    public float dp = 0;

    public float movingValue;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    public bool isBoss = false;

    protected virtual void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        //inGameMgr = InGameManager.instance;
        //resourceMgr = ResourceManager.instance;
        movingValue = 1.0f;
        image = this.transform.Find("Image");
        animator = image.GetComponent<Animator>();
        hpBar = this.transform.Find("HPBar").GetComponent<HPBar>();

        Debug.Log("Monster");
    }

    protected virtual void OnEnable()
    {
        //attackMonster.Clear();

        if (inGameMgr == null)
        {
            inGameMgr = InGameManager.instance;
        }
        if (resourceMgr == null)
        {
            resourceMgr = ResourceManager.instance;
        }
        hero = inGameMgr.hero;

        hp = maxHp;
        hpBar.Init();
        hpBar.gameObject.SetActive(false);
        //initPos = this.transform.position;
        monsterState = MonsterState.Idle;
    }

    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        rigidBody.velocity = Vector3.zero;
        
        if(monsterState == MonsterState.Dead)
        {
            return;
        }

        if (isTaming)
        {
            //if (hero.heroState == Hero.HeroState.Run)
            {
                if (Vector3.Dot(hero.direction, (this.transform.position - hero.transform.position).normalized) < 0)
                {
                    dampTime -= 0.01f;
                    dampTime = Mathf.Clamp(dampTime, 0.14f, 0.16f);
                }
                else
                {
                    dampTime += 0.01f;
                    dampTime = Mathf.Clamp(dampTime, 0.14f, 0.16f);

                }
                FollowTarget(inGameMgr.hero.transform);
            }

            if (attackTarget == null)
            {
                if (hero.attackMonster.Count > 0)
                {
                    attackTarget = hero.attackMonster[Random.Range(0, hero.attackMonster.Count)].gameObject;
                    monsterState = MonsterState.Attack;
                }
                else
                {
                    monsterState = MonsterState.Idle;
                }
            }
            else
            {
                monsterState = MonsterState.Idle;
            }
        }
        else
        {
            if (attackTarget == null)
            {
                if (hero.havingMonster.Count > 0)
                {
                    attackTarget = hero.havingMonster[Random.Range(0, hero.havingMonster.Count)].gameObject;
                }
                else
                {
                    monsterState = MonsterState.Idle;
                }
            }
            else
            {
                monsterState = MonsterState.Attack;
            }
            
            AttackAction();


        }

        //Debug.Log((this.transform.position - prePos).normalized);
        if ((this.transform.position - prePos).normalized.x >= 0)
        {
            image.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            image.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        if(monsterState == MonsterState.Idle)
        {
            if(Vector3.Distance(this.transform.position, prePos) > 0.02f)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    animator.CrossFade("Run", 0.2f);
                }
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    animator.CrossFade("Idle", 0.2f);
                }
            }
        }


        prePos = this.transform.position;
    }

    void AttackAction()
    {
        if (isNoticeTarget)
        {
            //몬스터가 히어로 가까이 갔을때 부터, 히어로나 테이밍된 몬스터 공격.
            if (Vector3.Distance(this.transform.position, attackTarget.transform.position) < 2.0f)
            {
                monsterState = MonsterState.Attack;
            }
            else
            {
                //히어로한테 감.
                transform.position = Vector3.Lerp(this.transform.position, attackTarget.transform.position, 0.01f);
            }
        }
        else if (!isNoticeTarget)
        {
            if (Vector3.Distance(this.transform.position, initPos) < 1.0f)
            {
                monsterState = MonsterState.Idle;
            }
            else
            {
                //제자리로 감.
                transform.position = Vector3.Lerp(this.transform.position, initPos, 0.01f);
            }

        }

    }

    protected void FollowTarget(Transform target)
    {
        Vector3 point = inGameMgr.mainCam.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - inGameMgr.mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            //테이밍이 따른 다른편을 공격해야함.  
            if (!other.gameObject.GetComponent<Monster>().isTaming && this.isTaming
                && other.gameObject.GetComponent<Monster>().monsterState != Monster.MonsterState.Dead)  
            {
                attackTarget = other.gameObject;
            }
            else if (other.gameObject.GetComponent<Monster>().isTaming && !this.isTaming
                && other.gameObject.GetComponent<Monster>().monsterState != Monster.MonsterState.Dead)  
            {
                attackTarget = other.gameObject;
            }
        }
    }


    /*
        private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            //테이밍이 따른 다른편을 공격해야함.
            if (!other.gameObject.GetComponent<Monster>().isTaming && this.isTaming
                && other.gameObject.GetComponent<Monster>().monsterState != Monster.MonsterState.Dead)  
            {
                if (attackMonster.Find(_monster => _monster == other.gameObject.GetComponent<Monster>()) == null)
                {
                    attackMonster.Add(other.gameObject.GetComponent<Monster>());
                    other.gameObject.GetComponent<Monster>().isNoticeTarget = true;
                    other.gameObject.GetComponent<Monster>().attackTarget = this.gameObject;
                }
            }
            else if (other.gameObject.GetComponent<Monster>().isTaming && !this.isTaming
                && other.gameObject.GetComponent<Monster>().monsterState != Monster.MonsterState.Dead)  
            {
                if (attackMonster.Find(_monster => _monster == other.gameObject.GetComponent<Monster>()) == null)
                {
                    attackMonster.Add(other.gameObject.GetComponent<Monster>());
                    other.gameObject.GetComponent<Monster>().isNoticeTarget = true;
                    other.gameObject.GetComponent<Monster>().attackTarget = this.gameObject;
                }
            }
        }
    }

        */
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
    //    {

    //        if (!other.gameObject.GetComponent<Monster>().isTaming && this.isTaming
    //            && other.gameObject.GetComponent<Monster>().monsterState != Monster.MonsterState.Dead)  //죽었지만 테이밍 되기 전일수도
    //        {
    //            if (attackMonster.Find(_monster => _monster == other.gameObject.GetComponent<Monster>()) != null)
    //            {
    //                attackMonster.Remove(other.gameObject.GetComponent<Monster>());
    //                other.gameObject.GetComponent<Monster>().isNoticeTarget = false;
    //                if (attackMonster.Count > 0)
    //                {
    //                    attackTarget = attackMonster[Random.Range(0, attackMonster.Count)].gameObject;
    //                }
    //                else
    //                {
    //                    attackTarget = null;
    //                }
    //            }
    //        }
    //        else if (other.gameObject.GetComponent<Monster>().isTaming && !this.isTaming
    //            && other.gameObject.GetComponent<Monster>().monsterState != Monster.MonsterState.Dead)  //죽었지만 테이밍 되기 전일수도
    //        {
    //            if (attackMonster.Find(_monster => _monster == other.gameObject.GetComponent<Monster>()) != null)
    //            {
    //                attackMonster.Remove(other.gameObject.GetComponent<Monster>());
    //                other.gameObject.GetComponent<Monster>().isNoticeTarget = false;
    //                if (attackMonster.Count > 0)
    //                {
    //                    attackTarget = attackMonster[Random.Range(0, attackMonster.Count)].gameObject;
    //                }
    //                else
    //                {
    //                    attackTarget = null;
    //                }
    //            }
    //        }
    //    }

    //}

    protected void Attack()
    {
        if (attackTarget != null)
        {
            if (Vector3.Distance(this.transform.position, attackTarget.transform.position) < 2.0f)
            {
                if(attackTarget.GetComponent<Monster>() == null)
                {
                    hero.Hit(ap);
                }
                else
                {
                    attackTarget.GetComponent<Monster>().Hit(ap);
                }
                //if (isTaming)
                //{
                //    attackTarget.GetComponent<Monster>().Hit(ap);
                //}
                //else
                //{
                //    hero.Hit(ap);
                //}
            }

            if(isBoss)
            {
                //스킬생성 / 타겟 지정 / SetActive True
                GameObject _skill = resourceMgr.CreateEffectObj(this.transform.name, this.transform.position, 3.0f);
                _skill.transform.parent = null;
                _skill.transform.name = this.transform.name;
                _skill.GetComponent<Skill>().target = attackTarget;
                _skill.GetComponent<Skill>().master = this;
                _skill.SetActive(true);
            }
        }

    }
    public void Hit(float _damage)
    {
        hp -= ((_damage - dp) < 0) ? 0.0f : (_damage - dp);
        hpBar.gameObject.SetActive(true);
        hpBar.UpdateHP(hp / maxHp);

        if (hp <= 0)
        {
            monsterState = MonsterState.Dead;
        }

    }

    public void UpdateHp()
    {
        hpBar.UpdateHP(hp / maxHp);
    }

    protected void Death()
    {
        isNoticeTarget = false;

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

        if(isTaming)
        {
            resourceMgr.ReleaseMonster(this.gameObject);
        }
        else
        {
            Vector3 viewportPos = inGameMgr.mainCam.WorldToViewportPoint(this.transform.position);
            RectTransform _rectTmCanvas = inGameMgr.UICamera.transform.Find("Canvas").GetComponent<RectTransform>();
            Vector3 uiPosInCanvas = new Vector3((viewportPos.x * _rectTmCanvas.rect.width) - (_rectTmCanvas.rect.width * 0.5f),
                (viewportPos.y * _rectTmCanvas.rect.height) - (_rectTmCanvas.rect.height * 0.5f), 0.0f);

            GameObject _ui = null;
            if(resourceMgr.uiBox.transform.Find("AfterDeadUIMonster")!= null)
            {
                _ui = resourceMgr.uiBox.transform.Find("AfterDeadUIMonster").gameObject;
            }
            else
            {
                _ui = Instantiate(resourceMgr.ui["AfterDeadUIMonster"]) as GameObject;
            }
            _ui.transform.name = "AfterDeadUIMonster";
            _ui.transform.parent = _rectTmCanvas.Find("WorldUIBox");
            _ui.transform.localScale = Vector3.one;
            _ui.transform.localPosition = uiPosInCanvas;

            _ui.GetComponent<AfterDeadUIMonster>().inGameMgr = inGameMgr;
            _ui.GetComponent<AfterDeadUIMonster>().resourceMgr = resourceMgr;
            _ui.GetComponent<AfterDeadUIMonster>().target = this.transform;
            _ui.SetActive(true);
        }
    }

    public enum MonsterState
    {
        Idle,
        Attack,
        //Run,
        Dead,
    }
}
