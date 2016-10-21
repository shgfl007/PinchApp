
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KeyboardMaster : MonoBehaviour {

	public GameObject textBox;
    public GameObject root;
    private static KeyboardMaster mInstance;
    public static KeyboardMaster instance { get { return mInstance; } }
    
    // Use this for initialization
    void Start () {
   
        if (mInstance == null)
        {
            mInstance = this;
        }
     //   textBox = transform.parent.Find("KeyBoardInput").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public GameObject getTextBox() {
		return textBox;
	}

	public void setTextBox(GameObject textObject) {
		textBox = textObject;
	}
	
	public string addLetter(string letter) {
		string newText = textBox.GetComponent<InputField> ().text;
		newText += letter;
		textBox.GetComponent<InputField> ().text = newText;
		return newText;
	}
	
	public string backspace() {
		string newText = textBox.GetComponent<InputField> ().text;
		if (newText.Length > 0) {
			newText = newText.Remove (newText.Length - 1);
			textBox.GetComponent<InputField> ().text = newText;
		}
		return newText;
	}
}