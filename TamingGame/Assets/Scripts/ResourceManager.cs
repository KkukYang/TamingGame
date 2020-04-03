using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// 게임에서 사용되는 각종 리소스 관리.
public class ResourceManager : MonoBehaviour
{

    private static ResourceManager s_instance = null;
    //private PopUpManager popUpMgr = null;

    public static ResourceManager instance
    {
        get
        {
            if (null == s_instance)
            {
                s_instance = FindObjectOfType(typeof(ResourceManager)) as ResourceManager;
                if (null == s_instance)
                {
                    //
                }
            }
            return s_instance;
        }
    }

    public Dictionary<string, GameObject> monster = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> effect = new Dictionary<string, GameObject>();

    public GameObject effectBox;
    public Dictionary<string, Queue<GameObject>> effectDic = new Dictionary<string, Queue<GameObject>>();

    public GameObject monsterBox;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        effectBox = transform.Find("EffectBox").gameObject;
        monsterBox = transform.Find("MonsterBox").gameObject;

        effectBox.SetActive(false);
        monsterBox.SetActive(false);

        // 적 셋팅.
        object[] monsterObj = Resources.LoadAll("Prefabs/Monster");
        for (int i = 0; i < monsterObj.Length; i++)
        {
            GameObject obj = monsterObj[i] as GameObject;
            obj.SetActive(false);
            monster.Add(obj.name, obj);
        }

        // 이펙트 셋팅.
        object[] effObj = Resources.LoadAll("Prefabs/Effect");
        for (int i = 0; i < effObj.Length; i++)
        {
            GameObject obj = effObj[i] as GameObject;
            obj.SetActive(false);
            effect.Add(obj.name, obj);
        }

        Resources.UnloadUnusedAssets();

    }


    public GameObject CreateEffectObj(string name, Vector3 pos, float playTime = 0.0f)
    {
        GameObject targetObj = null;
        Queue<GameObject> targetQueue = new Queue<GameObject>();

        if (effectDic.ContainsKey(name))
            targetQueue = effectDic[name];
        else
            effectDic.Add(name, targetQueue);

        if (targetQueue.Count > 0)
        {
            targetObj = targetQueue.Dequeue();
            targetObj.transform.parent = effectBox.transform;
            targetObj.GetComponent<GameEffect>().targetQueue = targetQueue;
            targetObj.GetComponent<GameEffect>().playTime = playTime;
        }
        else
        {
            targetObj = Instantiate(effect[name]) as GameObject;
            targetObj.transform.parent = effectBox.transform;
            targetObj.GetComponent<GameEffect>().targetQueue = targetQueue;
            targetObj.GetComponent<GameEffect>().playTime = playTime;
        }

        targetObj.transform.position = pos;

        return targetObj;
    }

    public GameObject GetMonster(string name)
    {
        GameObject _obj;

        if (monsterBox.transform.Find(name) == null)
        {

            _obj = Instantiate(monster[name]) as GameObject;
        }
        else
        {
            _obj = monsterBox.transform.Find(name).gameObject;
        }
        
        _obj.transform.name = name;
        _obj.SetActive(false);

        if (_obj.GetComponent<Monster>() == null)
        {
            _obj.AddComponent<Monster>();
        }

        return _obj;
    }

    public void ReleaseMonster(GameObject _obj)
    {
        _obj.transform.parent = monsterBox.transform;
        _obj.SetActive(false);

    }


    public void DisableAllObj()
    {
        // 리소스, 가비지콜렉터 정리.
        System.GC.Collect();


        effectDic.Clear();

        //each child set off in ResourceManager
        foreach (Transform tmObj in transform.Find("MonsterBox"))
        {
            Destroy(tmObj.gameObject);
        }

        foreach (Transform tmObj in transform.Find("EffectBox"))
        {
            Destroy(tmObj.gameObject);
        }
    }

}
