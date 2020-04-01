using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 게임에서 사용되는 이펙트 처리.
public class GameEffect : MonoBehaviour
{

	public Queue<GameObject> targetQueue;
	public float playTime;

	void OnEnable()
	{
		if (playTime >= 0.0f)
			Invoke("LifeEnd", playTime);
	}


	public void LifeEnd()
	{
		targetQueue.Enqueue(gameObject);
		transform.parent = ResourceManager.instance.transform.Find("EffectBox");
		gameObject.SetActive(false);
	}

	void OnDisable()
	{

	}

	IEnumerator StartEffect()
	{
		while (true)
		{
			if (!GetComponent<ParticleSystem>().isPlaying)
			{
				GetComponent<ParticleSystem>().Stop();
				gameObject.SetActive(false);
				StopCoroutine("StartEffect");
				break;
			}

			yield return null;
		}
	}

}
