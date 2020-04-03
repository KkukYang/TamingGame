using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public Rigidbody rigidBody;
    public InGameManager inGameMgr;
    public Hero hero;
    public bool isTaming;
    public bool isNoticeHero;
    public MonsterState monsterState;
    public Animator animator;
    public Transform image;
    public Vector3 initPos;

    public int hp = 100;
    public int ap = 10;
    public int dp = 0;

    public float movingValue;
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    protected virtual void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        inGameMgr = InGameManager.instance;
        hero = inGameMgr.hero;
        movingValue = 1.0f;
        image = this.transform.Find("Image");
        animator = image.GetComponent<Animator>();

        Debug.Log("Monster");
    }

    protected virtual void OnEnable()
    {
        initPos = this.transform.position;
        monsterState = MonsterState.Idle;
        NextState();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        rigidBody.velocity = Vector3.zero;

        if (isTaming && hero.heroState == Hero.HeroState.Run)
        {
            if(Vector3.Dot(hero.direction, (this.transform.position - hero.transform.position).normalized)<0)
            {
                dampTime -= 0.01f;
                dampTime = Mathf.Clamp(dampTime, 0.1f, 0.16f);
            }
            else
            {
                dampTime += 0.01f;
                dampTime = Mathf.Clamp(dampTime, 0.1f, 0.16f);

            }
            FollowTarget(inGameMgr.hero.transform);
        }

        if(!isTaming && isNoticeHero)
        {
            //몬스터가 히어로 가까이 갔을때 부터, 히어로나 테이밍된 몬스터 공격.
            if(Vector3.Distance(this.transform.position, hero.transform.position) < 2.0f)
            {
                monsterState = MonsterState.Attack;
            }
            else
            {
                //히어로한테 감.
                transform.position = Vector3.Lerp(this.transform.position, hero.transform.position, 0.01f);
            }
        }
        else if(!isTaming && !isNoticeHero)
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

    protected virtual void NextState()
    {
        //empty.
    }

    protected void FollowTarget(Transform target)
    {
        Vector3 point = inGameMgr.mainCam.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - inGameMgr.mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);

    }

    public enum MonsterState
    {
        Idle,
        Attack,
        Run,
        Dead,
    }
}
