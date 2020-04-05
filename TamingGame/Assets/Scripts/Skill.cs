using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Skill : MonoBehaviour
{

    public GameObject target; //외부지정.
    public Monster master;


    // Start is called before the first frame update
    void OnEnable()
    {
        this.GetComponent<AnimationEvent>().end = new AnimationEvent.End(SkillEnd);

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, target.transform.position, 0.5f);
    }

    public void ShootSkill()
    {

    }

    public void SkillEnd(GameObject _this)
    {
        //스킬 끝나고 이벤트
        Debug.Log("SkillEnd()");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            if(master.isTaming)
            {
                if(other.gameObject.GetComponent<Monster>().isTaming == false)
                {
                    other.gameObject.GetComponent<Monster>().Hit(master.ap);
                }
            }
            else
            {
                if (other.gameObject.GetComponent<Monster>().isTaming == true)
                {
                    other.gameObject.GetComponent<Monster>().Hit(master.ap);
                }
            }
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer("Hero"))
        {
            if(master.isTaming == false)
            {
                try
                {
                    other.transform.parent.GetComponent<Hero>().Hit(master.ap);
                }
                catch(Exception e)
                {
                    other.transform.GetComponent<Hero>().Hit(master.ap);
                }
            }
        }

    }
}
