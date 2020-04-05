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
    public List<Monster> attackMonster = new List<Monster>();

    public float maxHp = 100;
    public float hp = 0;
    public float ap = 10;
    public float dp = 0;

    public float movingValue;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

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
        attackMonster.Clear();

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

        }
        else
        {
            if (isNoticeTarget)
            {
                //몬스터가 히어로 가까이 갔을때 부터, 히어로나 테이밍된 몬스터 공격.
                if (Vector3.Distance(this.transform.position, hero.transform.position) < 2.0f)
                {
                    monsterState = MonsterState.Attack;
                }
                else
                {
                    //히어로한테 감.
                    transform.position = Vector3.Lerp(this.transform.position, hero.transform.position, 0.01f);
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

        //Debug.Log((this.transform.position - prePos).normalized);
        if ((this.transform.position - prePos).normalized.x >= 0)
        {
            image.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            image.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }


        prePos = this.transform.position;
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
            if (!other.gameObject.GetComponent<Monster>().isTaming
                && other.gameObject.GetComponent<Monster>().monsterState != Monster.MonsterState.Dead)  //죽었지만 테이밍 되기 전일수도
            {
                if (attackMonster.Find(_monster => _monster == other.gameObject.GetComponent<Monster>()) == null)
                {
                    attackMonster.Add(other.gameObject.GetComponent<Monster>());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            if (attackMonster.Find(_monster => _monster == other.gameObject.GetComponent<Monster>()) != null)
            {
                attackMonster.Remove(other.gameObject.GetComponent<Monster>());
            }
        }

    }

    protected void Attack()
    {
        if (Vector3.Distance(this.transform.position, hero.transform.position) < 2.0f)
        {

            hero.Hit(ap);
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

    public enum MonsterState
    {
        Idle,
        Attack,
        Run,
        Dead,
    }
}
