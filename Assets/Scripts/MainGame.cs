using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using UnityEngine.EventSystems;

public class MainGame : MonoBehaviour
{
		public Text scoreText;
	public Button pause;


		// Use this for initialization
		void Start ()
		{
				GameEventManager.GameStart += GameStart;
				GameEventManager.GameOver += GameOver;
				GameEventManager.GamePointCalculation += GamePoint;
				GameEventManager.TriggerGameStart ();


				
				
		}

		void Awake ()
		{
				if (PlayerPrefs.GetInt ("music") == 1) {

						Camera.main.GetComponent<AudioSource> ().enabled = false;
				} else {

						Camera.main.GetComponent<AudioSource> ().enabled = true;
				}
		}

		void GamePoint ()
		{
				scoreText.text =  Cake.score.ToString();
		}

		void GameStart ()
		{

//				print ("GameStart in agdum bagdum");
		}
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.Escape)) 
						Application.Quit (); 
		}

		void OnDestroy ()
		{
				GameEventManager.GameStart -= GameStart;
				GameEventManager.GameOver -= GameOver;
				GameEventManager.GamePointCalculation -= GamePoint;
		}

		void GameOver ()
		{
				
		}
}
