using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public HeroState heroState;
    public float movingSpeed;
    private Transform image;

    public List<Monster> havingMonster = new List<Monster>();
    public GameObject sampleSpot;

    public Rigidbody rigidBody;
    public Animator animator;
    public Vector3 direction;
    public Vector3 prePos;

    public int hp = 100;
    public int ap = 30;
    public int dp = 5;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        image = transform.Find("Image");
        animator = image.GetComponent<Animator>();
        rigidBody = this.GetComponent<Rigidbody>();

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
        prePos = this.transform.position;
        heroState = HeroState.Idle;
        NextState();
    }

    void Update()
    {
        rigidBody.velocity = Vector3.zero;

        if(heroState == HeroState.Run)
        {
            for (int i = 0; i < havingMonster.Count; i++)
            {
                havingMonster[i].image.localScale = this.image.localScale;
            }

            direction = (this.transform.position - prePos).normalized;

            prePos = this.transform.position;

        }

    }

    public void SetMove(JoyStick joyStick)
    {
        heroState = HeroState.Run;

        Rect rectRange = joyStick.m_JoyStickBackGround.GetComponent<RectTransform>().rect;
        Vector3 joyStickLocalpos = joyStick.m_JoyStick.transform.localPosition;
        Vector3 resultMoving = Vector3.zero;

        resultMoving += new Vector3(joyStickLocalpos.x / (rectRange.width * 0.5f), joyStickLocalpos.y / (rectRange.height * 0.5f));
        resultMoving *= movingSpeed;
        this.transform.position += resultMoving;

        SetLeftRight(joyStickLocalpos.x);

        Invoke("TempResetState", 0.5f);
    }

    public void SetLeftRight(float joysticLocalPosX)
    {
        if(joysticLocalPosX > 0)
        {
            //Right
            image.localScale = new Vector3(-1.0f, 1.0f);
        }
        else if(joysticLocalPosX < 0)
        {
            //Left
            image.localScale = new Vector3(1.0f, 1.0f);
        }
    }

    void TempResetState()
    {
        Debug.Log("TempResetState()");
        heroState = HeroState.Idle;
    }

    protected void NextState()
    {
        string methodName = heroState.ToString() + "State";
        StartCoroutine(methodName);
    }

    IEnumerator IdleState()
    {
        animator.CrossFade("Idle", 0.3f);

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

    IEnumerator RunState()
    {
        animator.CrossFade("Run", 0.3f);

        for (int i = 0; i < havingMonster.Count; i++)
        {
            havingMonster[i].monsterState = Monster.MonsterState.Run;
            havingMonster[i].image.localScale = this.image.localScale;
        }

        while (heroState == HeroState.Run)
        {
            yield return null;
        }

        NextState();
    }
    IEnumerator AttackState()
    {
        animator.CrossFade("Attack", 0.3f);

        while (heroState == HeroState.Attack)
        {
            yield return null;
        }

        NextState();
    }
    IEnumerator DeadState()
    {
        animator.CrossFade("Dead", 0.3f);

        while (heroState == HeroState.Dead)
        {
            yield return null;
        }

        NextState();
    }


    public enum HeroState
    {
        Idle,
        Run,
        Attack,
        Dead,

    }
}
