using UnityEngine;
using System.Collections;

public class Menu : VRObject {
     GameObject menu;
    public VRButton cancleButton;

    // Use this for initialization
    void Start () {
        menu = transform.FindChild("CreatingMenu").gameObject; //get the menu object
	}
	
	// Update is called once per frame
	void Update () {
        cancleButton.onClicked += destroyThis;
	}

    public void destroyThis(VRObject vrObject, Cursor cursorInput) {
        menu.SetActive(false);
    }

    public override void start(ref Cursor cursor)
    {
        if (menu.activeInHierarchy)
        {
            menu.SetActive(false);
        }
        else {
            menu.SetActive(true);
        }
    }
}
