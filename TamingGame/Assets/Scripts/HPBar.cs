using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//HPBar/HP 의 스케일을 조절하여 HP표시 100=초록 70=노랑 30=빨강
public class HPBar : MonoBehaviour
{
    public Transform hpControlImage;
    //public int maxHp;
    public float showingTime = 0.0f;

    private void Awake()
    {
        hpControlImage = this.transform.Find("HP");
        //foreach(Transform _tm in hpControlImage.GetComponentsInChildren<Transform>())
        //{
        //    _tm.gameObject.layer = LayerMask.NameToLayer("")
        //}
    }

    private void OnEnable()
    {
            
    }

    public void Init()
    {
        hpControlImage = this.transform.Find("HP");

        UpdateHP(1.0f);
    }

    public void UpdateHP(float _hpRate)
    {
        //color and scale.
        if(_hpRate>0.7f)
        {
            hpControlImage.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if(_hpRate>0.3f)
        {
            hpControlImage.GetChild(0).GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else
        {
            hpControlImage.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        }

        hpControlImage.localScale = new Vector3(Mathf.Clamp(_hpRate, 0.0f, 1.0f), 1.0f, 1.0f);

        showingTime += 2.0f;
        showingTime = Mathf.Clamp(showingTime, 0.0f, 2.0f);
    }

    private void Update()
    {
        showingTime -= Time.deltaTime;

        if(showingTime <=0.0f)
        {
            this.gameObject.SetActive(false);
        }
    }

}
