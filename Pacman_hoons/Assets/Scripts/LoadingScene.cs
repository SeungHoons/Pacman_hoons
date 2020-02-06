using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
	[SerializeField]
	Image loadingBar;

    // Start is called before the first frame update
    void Start()
    {
		loadingBar.fillAmount = 0;
		StartCoroutine(ProgressLoadScene());
    }

	IEnumerator ProgressLoadScene()
	{
		yield return null;
		AsyncOperation asyncScene = SceneManager.LoadSceneAsync(2);
		asyncScene.allowSceneActivation = false;
		float time = 0;

		while(!asyncScene.isDone)
		{
			yield return null;
			time += Time.deltaTime;
			if(asyncScene.progress >= 0.9f)
			{
				loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1, time);
				if(loadingBar.fillAmount == 1.0f)
				{
					asyncScene.allowSceneActivation = true;
				}
			}
			else
			{
				loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, asyncScene.progress, time);
				if(loadingBar.fillAmount >= asyncScene.progress)
				{
					time = 0f;
				}
			}
		}
	}
}
