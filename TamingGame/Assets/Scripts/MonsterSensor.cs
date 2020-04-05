using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSensor : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Debug.Log("Notice");
            if(other.gameObject.GetComponent<Monster>().monsterState != Monster.MonsterState.Dead)
            {
                if (other.gameObject.GetComponent<Monster>().isTaming == false)
                {
                    //야생일때. 공격상태로.
                    other.gameObject.GetComponent<Monster>().isNoticeTarget = true;
                    other.gameObject.GetComponent<Monster>().attackTarget = this.transform.parent.gameObject;
                    other.gameObject.GetComponent<Monster>().monsterState = Monster.MonsterState.Idle;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Debug.Log("out of Notice");

            if (other.gameObject.GetComponent<Monster>().isTaming == false)
            {
                //야생일때. idle상태로
                other.gameObject.GetComponent<Monster>().isNoticeTarget = false;
                //other.gameObject.GetComponent<Monster>().monsterState = Monster.MonsterState.Idle;    //무리로 돌아간 후, Idle.

            }
        }

    }
}
