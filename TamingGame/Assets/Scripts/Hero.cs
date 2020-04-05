using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public HeroState heroState;
    public float movingSpeed;
    private Transform image;

    public List<Monster> havingMonster = new List<Monster>();
    public List<Monster> attackMonster = new List<Monster>();
    public GameObject sampleSpot;

    public Rigidbody rigidBody;
    public Animator animator;
    public Vector3 direction;
    public Vector3 prePos;

    public HPBar hpBar;

    public float maxHp = 100;
    public float hp = 100;
    public float ap = 30;
    public float dp = 5;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        image = transform.Find("Image");
        animator = image.GetComponent<Animator>();
        rigidBody = this.GetComponent<Rigidbody>();
        hpBar = this.transform.Find("HPBar").GetComponent<HPBar>();

        float _val = 360.0f / havingMonster.Count;

        for (int i = 0; i < havingMonster.Count; i++)
        {
            GameObject _obj = Instantiate(sampleSpot) as GameObject;
            _obj.transform.parent = this.transform.Find("SpotGroup");
            _obj.transform.name = i.ToString();
            _obj.transform.position = new Vector3() + this.transform.position;

            _obj.SetActive(true);


        }


        Debug.Log("Hero");
    }

    private void OnEnable()
    {
        attackMonster.Clear();
        //havingMonster.Clear();

        hp = maxHp;
        hpBar.Init();
        hpBar.gameObject.SetActive(false);

        prePos = this.transform.position;
        heroState = HeroState.Idle;
        NextState();
    }

    void Update()
    {

        //if (heroState == HeroState.Run)
        //{
        //    for (int i = 0; i < havingMonster.Count; i++)
        //    {
        //        havingMonster[i].image.localScale = this.image.localScale;
        //    }
        //}
        direction = (this.transform.position - prePos).normalized;
        rigidBody.velocity = Vector3.zero;

        if (heroState == HeroState.Idle)
        {
            if (Vector3.Distance(this.transform.position, prePos) > 0.02f)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
                {
                    animator.Play("Run");
                }
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    animator.Play("Idle");
                }
            }
        }

        prePos = this.transform.position;
    }

    public void SetMove(JoyStick joyStick)
    {
        if (heroState != HeroState.Dead)
        {
            if(attackMonster.Count > 0)
            {
                heroState = HeroState.Attack;
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    animator.Play("Attack");
                }

            }
            else
            {

                heroState = HeroState.Idle;
                Invoke("ResetState", 0.5f);

            }

            Rect rectRange = joyStick.m_JoyStickBackGround.GetComponent<RectTransform>().rect;
            Vector3 joyStickLocalpos = joyStick.m_JoyStick.transform.localPosition;
            Vector3 resultMoving = Vector3.zero;

            resultMoving += new Vector3(joyStickLocalpos.x / (rectRange.width * 0.5f), joyStickLocalpos.y / (rectRange.height * 0.5f));
            resultMoving *= movingSpeed;
            this.transform.position += resultMoving;

            SetLeftRight(joyStickLocalpos.x);


        }
    }

    public void SetLeftRight(float joysticLocalPosX)
    {
        if (joysticLocalPosX > 0)
        {
            //Right
            image.localScale = new Vector3(-1.0f, 1.0f);
        }
        else if (joysticLocalPosX < 0)
        {
            //Left
            image.localScale = new Vector3(1.0f, 1.0f);
        }
    }

    void ResetState()
    {
        heroState = HeroState.Idle;
        foreach(Monster _monster in havingMonster)
        {
            _monster.rigidBody.velocity = Vector3.zero;
        }
    }

    public void Hit(float _damage)
    {
        hp -= ((_damage - dp) < 0) ? 0.0f : (_damage - dp);
        hpBar.gameObject.SetActive(true);
        hpBar.UpdateHP(hp / maxHp);

        if (hp <= 0)
        {
            heroState = HeroState.Dead;
        }
    }

    public void Attack()
    {
        //리스트에 등록된 몬스터들중 하나를 팬다.
        if (attackMonster.Count > 0)
        {
            attackMonster[Random.Range(0, attackMonster.Count)].Hit(ap);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            //일단 죽지 않아야. //테이밍 되지 않은 상태.
            if(!other.gameObject.GetComponent<Monster>().isTaming 
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


    protected void NextState()
    {
        string methodName = heroState.ToString() + "State";
        StartCoroutine(methodName);
    }

    IEnumerator IdleState()
    {
        animator.Play("Idle");

        for (int i = 0; i < havingMonster.Count; i++)
        {
            havingMonster[i].monsterState = Monster.MonsterState.Idle;
        }

        while (heroState == HeroState.Idle)
        {
            yield return null;
        }

        NextState();
    }

    //IEnumerator RunState()
    //{
    //    animator.CrossFade("Run", 0.3f);

    //    for (int i = 0; i < havingMonster.Count; i++)
    //    {
    //        havingMonster[i].monsterState = Monster.MonsterState.Run;
    //        havingMonster[i].image.localScale = this.image.localScale;
    //    }

    //    while (heroState == HeroState.Run)
    //    {
    //        yield return null;
    //    }

    //    NextState();
    //}
    IEnumerator AttackState()
    {
        animator.Play("Attack");
        image.GetComponent<AnimationEvent>().add = new AnimationEvent.Add(Attack);

        while (heroState == HeroState.Attack)
        {
            yield return null;
        }

        NextState();
    }
    IEnumerator DeadState()
    {
        animator.Play("Dead");

        while (heroState == HeroState.Dead)
        {
            yield return null;
        }

        NextState();
    }


    public enum HeroState
    {
        Idle,
        //Run,
        Attack,
        Dead,

    }
}
