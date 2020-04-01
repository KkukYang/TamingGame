using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public HeroState heroState;
    public float movingSpeed;
    private Transform image;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        image = this.transform.Find("Image");
        Debug.Log("Hero");
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    public void SetMove(Vector3 joyStickLocalPos)
    {
        //Debug.Log(joyStickLocalPos);
        Vector3 resultMoving = Vector3.zero;
        if(joyStickLocalPos.x >0)
        {
            resultMoving += new Vector3(1.0f, 0.0f);
        }
        else if (joyStickLocalPos.x < 0)
        {
            resultMoving += new Vector3(-1.0f, 0.0f);
        }

        if (joyStickLocalPos.y >0)
        {
            resultMoving += new Vector3(0.0f, 1.0f);
        }
        else if (joyStickLocalPos.y < 0)
        {
            resultMoving += new Vector3(0.0f, -1.0f);
        }

        resultMoving *= movingSpeed;
        this.transform.position += resultMoving;
    }
    */


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

    protected void NextState()
    {
        string methodName = heroState.ToString() + "State";
        StartCoroutine(methodName);
    }



    public enum HeroState
    {
        Idle,
        Run,
        Attack,
        Dead,

    }
}
