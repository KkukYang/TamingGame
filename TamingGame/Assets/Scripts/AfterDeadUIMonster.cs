using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AfterDeadUIMonster : MonoBehaviour
{
    public InGameManager inGameMgr;
    public ResourceManager resourceMgr;
    public Transform target;
    Camera mainCam;
    public Button tamingButton;
    public Button rewardButton;
    public Action endUICallback;
    public Hero hero;

    private void Awake()
    {
        tamingButton = this.transform.Find("TamingButton").GetComponent<Button>();
        rewardButton = this.transform.Find("RewardButton").GetComponent<Button>();
    }

    private void OnEnable()
    {
        mainCam = inGameMgr.mainCam;
        hero = inGameMgr.hero;
        tamingButton.onClick.AddListener(TamingMonster);
        rewardButton.onClick.AddListener(GetReward);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mainCam != null && inGameMgr != null)
        {
            Vector3 viewportPos = mainCam.WorldToViewportPoint(target.position);
            RectTransform _rectTmCanvas = inGameMgr.UICamera.transform.Find("Canvas").GetComponent<RectTransform>();
            Vector3 uiPosInCanvas = new Vector3((viewportPos.x * _rectTmCanvas.rect.width) - (_rectTmCanvas.rect.width * 0.5f),
                (viewportPos.y * _rectTmCanvas.rect.height) - (_rectTmCanvas.rect.height * 0.5f), 0.0f);

            this.transform.localPosition = uiPosInCanvas;
        }
    }

    public void TamingMonster()
    {
        Debug.Log("TamingMonster()");
        target.GetComponent<Monster>().isTaming = true;
        target.GetComponent<Monster>().hp = target.GetComponent<Monster>().maxHp;
        target.GetComponent<Monster>().monsterState = Monster.MonsterState.Idle;
        //target.GetComponent<Monster>().attackMonster.Clear();
        
        foreach(Collider _col in target.GetComponents<Collider>())
        {
            _col.enabled = false;
            _col.enabled = true;
        }
        ////히어로 공격목록에 있으면 삭제
        //if (hero.attackMonster.Find(_monster => _monster == target.GetComponent<Monster>()) != null)
        //{
        //    hero.attackMonster.Remove(target.GetComponent<Monster>());
        //}

        //히어로 테이밍 리스트로 들어감.
        hero.havingMonster.Add(target.GetComponent<Monster>());

        CloseUI();
    }

    public void GetReward()
    {
        Debug.Log("GetReward()");
        CloseUI();
    }

    void CloseUI()
    {
        this.transform.parent = resourceMgr.uiBox.transform;
        this.gameObject.SetActive(false);
    }
    


}
