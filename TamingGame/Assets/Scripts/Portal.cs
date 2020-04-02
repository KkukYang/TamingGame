using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Hero"))
        {
            SceneManager.activeSceneChanged += InitEachScene;
            SceneManager.LoadScene("MainInGame");

        }
    }

    public void InitEachScene(Scene current, Scene next)
    {
        if(SceneManager.GetActiveScene() == next)
        {
            //fog세팅
            InGameManager.instance.transform.Find("Fog").gameObject.SetActive(true);
            InGameManager.instance.mainCam.GetComponent<FogOfWarManager>().enabled = true;
        }
    }

    
}
