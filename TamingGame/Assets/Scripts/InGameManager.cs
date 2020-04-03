using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    private static InGameManager s_instance = null;
    public static InGameManager instance
    {
        get
        {
            if (null == s_instance)
            {
                s_instance = FindObjectOfType(typeof(InGameManager)) as InGameManager;
                if (null == s_instance)
                {
                    //
                }
            }
            return s_instance;
        }
    }

    public JoyStick joyStick;
    public Hero hero;
    public Camera mainCam;
    public GameObject UICamera;
    public GameObject MainMapCamera;
    public GameObject ground;
    public bool isMainMap;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (joyStick.m_JoyStickBackGround.activeSelf)
        {
            hero.SetMove(joyStick);
        }

    }

    public void SwitchCam()
    {
        Debug.Log("SwitchCam()");

        if(isMainMap)
        {
            UICamera.SetActive(true);
            MainMapCamera.SetActive(false);
        }
        else
        {
            MainMapCamera.transform.position = new Vector3(hero.transform.position.x
                , hero.transform.position.y
                , MainMapCamera.transform.position.z);

            UICamera.SetActive(false);
            MainMapCamera.SetActive(true);
        }

        isMainMap = !isMainMap;
    }


}
