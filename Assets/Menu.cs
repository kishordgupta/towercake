using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour
{
		public Button  start, fbShare, music;

	public Sprite musicOnTexture,musicOffTexture;

		public Text highScore;

	public Text score;
		private string lastResponse = null;
		// Use this for initialization
		void Awake ()
		{
				if (PlayerPrefs.GetInt ("music") == 1) {
				
		
			music.GetComponent<Image>().sprite=musicOnTexture;
						Camera.main.GetComponent<AudioSource> ().enabled = false;
				} else {
					
			music.GetComponent<Image>().sprite=musicOffTexture;
			Camera.main.GetComponent<AudioSource> ().enabled = true;
				}
		}

		void Start ()
		{
				highScore.text = "Best : " + PlayerPrefs.GetInt ("score");
				score.text=Cake.score.ToString();
				FB.Init (SetInit, OnHideUnity);
				start.onClick.AddListener (() => {
						Application.LoadLevel (1);
				});

				if (PlayerPrefs.GetInt ("music") == 1) {
			music.GetComponent<Image>().sprite=musicOnTexture;
			Camera.main.GetComponent<AudioSource> ().enabled = false;
				} else {
			music.GetComponent<Image>().sprite=musicOffTexture;
			Camera.main.GetComponent<AudioSource> ().enabled = true;
				}
				music.onClick.AddListener (() => {

						if (PlayerPrefs.GetInt ("music") == 1) {
				music.GetComponent<Image>().sprite=musicOffTexture;
				Camera.main.GetComponent<AudioSource> ().enabled = true;
								PlayerPrefs.SetInt ("music", 0);
								PlayerPrefs.Save ();
					 
						} else {
								music.GetComponent<Image>().sprite=musicOnTexture;
								Camera.main.GetComponent<AudioSource> ().enabled = false;
								PlayerPrefs.SetInt ("music", 1);
								PlayerPrefs.Save ();

						}

				});
				fbShare.onClick.AddListener (() => {
						if (!FB.IsLoggedIn)
								CallFBLogin ();
			
						FB.Feed (
							link: "https://www.siliconorchard.com",
							linkName: "The Westeros Runner",
							linkCaption: "Enjoy the game",
							linkDescription: "My Score is " + PlayerPrefs.GetInt ("score") + ", What is yours?",
							picture: "http://www.siliconorchard.com/silicon_src2/fb_imgforcake.png",
							callback: LogCallback
						);
			
				});
		}

		private void CallFBLogin ()
		{
				FB.Login ("email,publish_actions", LoginCallback);
		}

		void LogCallback (FBResult response)
		{
				Debug.Log (response.Text);
		}
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.Escape)) 
						Application.Quit (); 
		}

		void LoginCallback (FBResult result)
		{
				if (result.Error != null)
						lastResponse = "Error Response:\n" + result.Error;
				else if (!FB.IsLoggedIn) {
						lastResponse = "Login cancelled by Player";
				} else {
						lastResponse = "Login was successful!";
				}
		}

		private void SetInit ()
		{
				enabled = true; 
				// "enabled" is a magic global; this lets us wait for FB before we start rendering
		}
	
		private void OnHideUnity (bool isGameShown)
		{
				if (!isGameShown) {
						// pause the game - we will need to hide
						Time.timeScale = 0;
				} else {
						// start the game back up - we're getting focus again
						Time.timeScale = 1;
				}
		}
}
