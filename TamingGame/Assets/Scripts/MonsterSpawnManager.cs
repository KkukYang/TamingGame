using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    public ResourceManager resourceMgr;
    public InGameManager inGameMgr;

    public GameObject ground;
    public int maxMonsterGroup = 15;
    public Collider groundCollider;

    public Transform liveMonster;
    public Dictionary<int, List<Monster>> dicMonsterEachGroupNum = new Dictionary<int, List<Monster>>();

    private void Awake()
    {
        resourceMgr = ResourceManager.instance;
        inGameMgr = InGameManager.instance;
        ground = inGameMgr.ground;
        groundCollider = ground.transform.GetChild(0).GetComponent<Collider>();

        liveMonster = this.transform.Find("LiveMonster");
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector3 _spawnPos = Vector3.zero;

        for (int i = 0; i < maxMonsterGroup; i++)
        {
            //그라운드에 포인트 하나 잡아서 부대생성하기.
            _spawnPos = new Vector3(Random.Range(-groundCollider.bounds.size.x * 0.5f, groundCollider.bounds.size.x * 0.5f)
                , Random.Range(-groundCollider.bounds.size.y * 0.5f, groundCollider.bounds.size.y * 0.5f), 0.0f);
            List<Monster> _listMonster = new List<Monster>();
            int _maxCountInGroup = Random.Range(4, 8);
            int _monsterNum = Random.Range(1, 3+1);

            for (int j = 0; j < _maxCountInGroup; j++)
            {
                GameObject _monster = resourceMgr.GetMonster("Monster_" + _monsterNum.ToString());
                _monster.transform.parent = liveMonster;
                _monster.transform.localScale = Vector3.one;
                _monster.transform.position = _spawnPos + new Vector3(Random.Range(-1.0f, 1.0f)
                    , Random.Range(-1.0f, 1.0f)
                    , 0.0f);

                _monster.GetComponent<Monster>().isTaming = false;

                _monster.SetActive(true);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
