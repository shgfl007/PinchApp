using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Layout_Changer : MonoBehaviour {


	public Transform centerLocationPanel;
	public Transform centerLocationRoom;

	public Transform LeftPanel;
	public Transform RightPanel;

	int currentIndex;

	public GameObject[] layout3D_list;
	public GameObject[] layoutPanels;

	private bool isChanged = false;

	// Use this for initialization
	void Start () {

		//currentIndex = 0;

		//init
		init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void init(){
		print ("current index is " + currentIndex);
		//init the scene with ui shown properly 
		for(int i = 0; i < layoutPanels.Length; i++){
			Color tempColor = layoutPanels [i].GetComponent<Image> ().color;
			if (i != currentIndex) {
				tempColor.a = 0.2f;
				layoutPanels [i].GetComponent<Image> ().color = tempColor;
				layoutPanels [i].transform.GetChild (0).gameObject.SetActive (false);
			} else {
				tempColor.a = 0.8f;
				layoutPanels [i].GetComponent<Image> ().color = tempColor;
				layoutPanels [i].transform.GetChild (0).gameObject.SetActive (true);

			}
		}

		//move panels
		iTween.MoveTo(layoutPanels[currentIndex], centerLocationPanel.transform.position,2f);
		if (currentIndex == layoutPanels.Length - 1) {
			iTween.MoveTo (layoutPanels [currentIndex - 1], LeftPanel.transform.position, 2f);
			iTween.MoveTo (layoutPanels [0], RightPanel.transform.position, 2f);
		} else if (currentIndex == 0) {
			iTween.MoveTo (layoutPanels [currentIndex + 1], RightPanel.transform.position, 2f);
			iTween.MoveTo (layoutPanels [currentIndex + 2], LeftPanel.transform.position, 2f);
		} else {
			iTween.MoveTo (layoutPanels [currentIndex - 1], LeftPanel.transform.position, 2f);
			iTween.MoveTo (layoutPanels [currentIndex + 1], RightPanel.transform.position, 2f);
		}

		for (int i = 0; i < layout3D_list.Length; i++) {
			if (i != currentIndex) {
				layout3D_list [i].SetActive (false);
			} else {
				layout3D_list [i].SetActive (true);

				layout3D_list [i].transform.position = centerLocationRoom.position;
			}
		}

	}

	public void pressLeft(){
		print ("press left");
		changeLayout (-1);
	}

	public void pressRight(){
		print ("press right");
		changeLayout (1);
	}

	void changeLayout(int direction){
		isChanged = true;
		int tempIndex = currentIndex + direction;
		//handle the index
		if (tempIndex < 0) {
			tempIndex = layout3D_list.Length - 1;	
		} else if (tempIndex >= layout3D_list.Length) {
			tempIndex = 0;
		}
		currentIndex = tempIndex;
		//change layout here
		init();

	
	}

	public void panelsFadeOut(){
		//hide all layout panels


		//hide left and right handles first
		GameObject.Find("Left").SetActive(false);
		GameObject.Find ("Right").SetActive (false);
		//then zoom current layout
		iTween.ScaleTo(layoutPanels[currentIndex], new Vector3(1.5f,1.5f,0f),2f);
		//WaitForSeconds (2);
		//wait for 1 seconds to disable layout canvas
		StartCoroutine(delayToHideLayouts());

	}

	IEnumerator delayToHideLayouts(){
		yield return new WaitForSeconds (1);
		GameObject.Find("Layouts").SetActive(false);

		//call light change when done 
		LightChange();
	}

	void LightChange(){
		//light up the background light when a layout is selected
		GameObject bkLight = GameObject.Find("RoomLight");
		Color lightColor = bkLight.GetComponent<Light> ().color;
		Color newLightColor = new Color (1f,0.9f,0.83f,0.8f);
		iTween.ColorTo (bkLight, newLightColor, 1f);
	}

	public int getCurrentIndex(){
		int index = currentIndex;
		return index;
	}

	public GameObject[] getRoomList(){
		return layout3D_list;
	}

	public void setRoomList(GameObject[] RoomList){
		layout3D_list = RoomList;
	}



}
