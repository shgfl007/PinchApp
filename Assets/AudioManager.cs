using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

	private static AudioManager mInstance;
	public static AudioManager instance {get { return mInstance;}}

	public AudioClip[] aClips;
	private AudioSource aSource;

	// Use this for initialization
	void Start () {
		if (mInstance == null) {

			mInstance = this;

		}
		aSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void PlayAudio (int index) {

		aSource.clip = aClips[index];
		aSource.Play ();


	}

}
