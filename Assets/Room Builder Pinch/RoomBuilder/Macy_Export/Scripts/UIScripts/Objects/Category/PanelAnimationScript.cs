using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelAnimationScript : VRObject {

	Object[] textures = null;
	System.Random random;
	Vector3 originalLocalPosition;

	Transform animationImage, image;

	int timeBetweenAnimation;
	int timeSinceLastAnim = 0;

	// Use this for initialization
	void Start () {
		//random = new System.Random ();
		//if (textures == null)
		//	textures = transform.parent.parent.parent.parent.GetComponent<LoadCategoriesScript> ().textures; // Images and random will be set by the loadCategoriesScript
		timeBetweenAnimation = random.Next (800, 1700);

		animationImage = transform.FindChild ("ImageAnimation");
		image = transform.FindChild ("Image");
		originalLocalPosition = animationImage.localPosition;

		animationComponentsActive (false);
	}

	public void initialize(Object[] inTex, System.Random inRand) {
		textures = inTex;
		random = inRand;
	}

	// Update is called once per frame
	void Update () {
		timeSinceLastAnim++;
		if (timeSinceLastAnim == timeBetweenAnimation) {
			timeSinceLastAnim = 0;

			// Randomize image to switch to
			int randomImageIndex = random.Next(0, textures.Length-1);

			// Set child
			animationImage.GetComponent<RawImage>().texture = (Texture) textures[randomImageIndex];

			// Start animation
			StartCoroutine(animate ());
		}
	}

	void animationComponentsActive(bool isActive) {
		transform.GetComponent<Mask> ().enabled = isActive;
		transform.GetComponent<Image> ().enabled = isActive;
		animationImage.gameObject.SetActive (isActive);
	}

	IEnumerator animate() {
		animationComponentsActive (true);

		int intervals = 40;
		for (int i = 1; i <= intervals; i++) {
			Vector3 newLocalPos = animationImage.localPosition;
			newLocalPos.x = originalLocalPosition.x - easeInOut(originalLocalPosition.x, intervals, i);
			animationImage.localPosition = newLocalPos;

			yield return new WaitForSeconds(0.016f);
		}
		image.GetComponent<RawImage> ().texture = animationImage.GetComponent<RawImage> ().texture;
		animationImage.localPosition = originalLocalPosition;

		animationComponentsActive (false);
	}
}
