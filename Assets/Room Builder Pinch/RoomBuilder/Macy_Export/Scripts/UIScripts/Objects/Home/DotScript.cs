using UnityEngine;
using System.Collections;

public class DotScript : MonoBehaviour {

    int numDots;
    //int currentCarouselTile = 0;
    int activeTileIndex = 0;

	// Use this for initialization
	void Start () {
		numDots = transform.parent.FindChild ("ScrollRect").FindChild ("CarouselTiles").childCount;

		// Spawn dots
		for (int i = 0; i < numDots; i++) {
			GameObject newDot = (GameObject) Instantiate (Resources.Load ("Dot"));
			newDot.transform.SetParent (transform);

			// Set transform
			newDot.transform.localPosition = Vector3.zero;
			newDot.transform.localRotation = Quaternion.Euler(Vector3.zero);
			newDot.transform.localScale = new Vector3 (1, 1, 1);

			if (i == 0) {
				// Set first dot to active
				newDot.GetComponent<CanvasGroup>().alpha = 1;
			}
		}
	}

	public void updateNumDots() {
		// Delete all dots
		for (int i = transform.childCount - 1; i >= 0; i--) {
			Destroy(transform.GetChild(i).gameObject);
		}

		// Remake all dots
		numDots = transform.parent.FindChild ("ScrollRect").FindChild ("CarouselTiles").childCount;
		for (int i = 0; i < numDots; i++) {
			GameObject newDot = (GameObject) Instantiate (Resources.Load ("Dot"));
			newDot.transform.SetParent (transform);
			
			// Set transform
			newDot.transform.localPosition = Vector3.zero;
			newDot.transform.localRotation = Quaternion.Euler(Vector3.zero);
			newDot.transform.localScale = new Vector3 (1, 1, 1);
		}

		// Set active dot
		setDotTransparency ();
	}

	public void setActiveTileIndex(int index) {
		activeTileIndex = index;

		setDotTransparency ();
	}

	void setDotTransparency() {
		// Make all dots half transparent
		for (int i = 0; i < numDots; i++) {
			transform.GetChild(i).gameObject.GetComponent<CanvasGroup>().alpha = 0.5f;
		}

		// Set active dot
		transform.GetChild (activeTileIndex).GetComponent<CanvasGroup> ().alpha = 1;
	}
}
