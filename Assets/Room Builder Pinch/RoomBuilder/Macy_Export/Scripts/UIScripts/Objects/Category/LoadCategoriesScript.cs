using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class LoadCategoriesScript : MonoBehaviour {

	public System.Random random = new System.Random();
	public Object[] textures;

	// Start loading categories
	void Start ()
	{
		// Load images
		textures = Resources.LoadAll("HomePage", typeof(Texture2D));

		// For each texture
		for (int i = 0; i < textures.Length; i++)
		{
			// Generate a category
			GameObject newCategory = (GameObject) Instantiate (Resources.Load ("HomePageCategory"));
			newCategory.transform.SetParent (transform);

			// Set initial position/rotation
			newCategory.transform.position = Vector3.zero;
			newCategory.transform.rotation = Quaternion.Euler(new Vector3(0, i * -360/(textures.Length), 0));
			newCategory.transform.localScale = new Vector3(1, 1, 1);

			// Set alphas
			newCategory.transform.FindChild("Canvas").FindChild("PopupPanel").GetComponent<CanvasGroup>().alpha = 0;

			// Compress texture
			//((Texture2D) textures[i]).Compress(false);

			// Set texture
			newCategory.transform.FindChild("Canvas").FindChild("Panel").FindChild("Mask").FindChild("Image").GetComponent<RawImage>().texture = (Texture2D) textures[i];
			newCategory.transform.FindChild("Canvas").FindChild("PopupPanel").FindChild("Image").GetComponent<RawImage>().texture = (Texture2D) textures[i];

			// Set category name
			newCategory.transform.FindChild("Canvas").FindChild("Panel").FindChild("Title").GetComponent<Text>().text = textures[i].name;
			newCategory.transform.FindChild("Canvas").FindChild("PopupPanel").FindChild("Title").GetComponent<Text>().text = textures[i].name;
			newCategory.transform.FindChild("Canvas").FindChild("Consistent").FindChild("Title2").GetComponent<Text>().text = textures[i].name;
			//newCategory.transform.FindChild("Canvas").FindChild("Category Interface").FindChild("CenterUI").FindChild("Title").GetComponent<Text>().text = textures[i].name;

			newCategory.transform.FindChild("Canvas").FindChild("Panel").FindChild("Mask").GetComponent<PanelAnimationScript>().initialize(textures,random);
		}
	}
}
