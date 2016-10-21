using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {

	private static SceneLoader mInstance;                             // LKU: singleton
	public static SceneLoader instance { get { return mInstance; } }

    public GameObject blackPanel;

    public  bool changeScenes = false;

    float ctr = 0;

	public  int levelName;

	// Use this for initialization
	void Start () {
		if (mInstance == null) {

			mInstance = this;
		}
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(blackPanel);
    }

	public  void LoadScene(int sceneName)
    {
        levelName = sceneName;
        FadeBlack.instance.fadeInBetween = true;
        //SceneManager.LoadScene(sceneName);
        changeScenes = true;
        //if (blackPanel.transform.GetChild(0).GetComponent<Image>().color.a > 0.99f)
          //  SceneManager.LoadScene(sceneName);
    }

 //   public static IEnumerator ChangeScenes(float timeDelay)
  //  {
   //     yield return new WaitForSeconds(timeDelay);
        //sceneState = caseNumber;
        //changeSceneState();
        //isSpaceBuilder = !isSpaceBuilder;
        //	print("State 2 switch");
   // }

    // Update is called once per frame
    void Update () {

        if (changeScenes)
        {
            if (ctr < 1.0f)
                ctr += 1.0f * Time.deltaTime;
            else
            {
                blackPanel.transform.parent = transform;
                SceneManager.LoadScene(levelName);
                changeScenes = false;
            }


            //  Debug.Log("CTR: " + ctr);
        }
        //       if (Input.GetKeyUp(KeyCode.T))
        //      {
        //         LoadScene("EmptyScene");
        //    }

    }
}
