using UnityEngine;
using System.Collections;

public class TurnOffPanels : MonoBehaviour {
	private static TurnOffPanels mInstance;                             // LKU: singleton
	public static TurnOffPanels instance { get { return mInstance; } }
	public GameObject currentPanel;
	public GameObject transport;

	public void resetPanel(GameObject nextPanel) {
	
		if (currentPanel != null) {
			currentPanel.gameObject.SetActive (false);
			currentPanel = nextPanel;
			currentPanel.gameObject.SetActive (true);
		} else {
		
			currentPanel = nextPanel;
			currentPanel.gameObject.SetActive (true);
		
		}
	
	}
	// Use this for initialization
	void Start () {
	
		if (mInstance == null) {
		
			mInstance = this;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
