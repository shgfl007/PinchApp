using UnityEngine;
using System.Collections;

public class RoomReset : MonoBehaviour {

	public GameObject[] RoomPrefabs;

	public Transform centerPos; 

	private int currentIndex;
	private GameObject[] RoomList;

	//for debug only
	public bool isReset = false;

	public static bool toReset = false;

	public Layout_Changer lc;


	// Use this for initialization
	void Start () {
		
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isReset || toReset) {
			
			currentIndex = lc.getCurrentIndex ();
			print (currentIndex);
			RoomList = lc.getRoomList ();
			RoomList [currentIndex].SetActive (false);
			GameObject temp = RoomList [currentIndex];
			//Destroy (RoomList [currentIndex]);
			//DestroyImmediate (RoomList [currentIndex]);
			Quaternion rotation = Quaternion.Euler (0,0,0);
			GameObject newRoom = (GameObject)Instantiate (RoomPrefabs [currentIndex], centerPos.transform.position, rotation);
			newRoom.transform.SetParent (GameObject.Find("Room Layouts and Light").transform);
			newRoom.transform.position = centerPos.transform.position;
			newRoom.transform.rotation = centerPos.transform.rotation;
			newRoom.transform.localScale = centerPos.transform.localScale;
			RoomList [currentIndex] = RoomPrefabs [currentIndex];
			lc.setRoomList (RoomList);
			isReset = false;
			Destroy (temp);
		}
	}
}
