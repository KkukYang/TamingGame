using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;//이벤트 핸들러 사용 using

public class MainMapSerch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Camera mainMapCamera;
    public Vector2 vec;
    public Vector2 preVec;

    public float value;

    void OnEnable()
    {
        vec = new Vector3(mainMapCamera.transform.position.x , mainMapCamera.transform.position.y , -10f);
    }

    //이벤트들
    #region event
    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        preVec = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //vec -= eventData.position - preVec;
        //preVec = eventData.position;
        //mainMapCamera.transform.localPosition += new Vector3(vec.x * value, vec.y * value, -10.0f);
    }


    public void OnDrag(PointerEventData eventData)
    {
        vec = eventData.position - preVec;
        preVec = eventData.position;
        mainMapCamera.transform.localPosition -= new Vector3(vec.x * value, vec.y * value, 0.0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
    #endregion


    void Update()
    {

    }

}
