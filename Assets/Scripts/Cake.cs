using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
//using Vectrosity;

public class Cake : MonoBehaviour
{
		
		
		private float minX, maxX, minY, maxY;
		private static int 	cameraMoveIndicator = 0;
		private float sizeOfGameObjectY ;
		float sizeX;
		Vector2 newScale ;
		static int counter ;
		bool firstTime ;
		public 	static int score;
		public GameObject background;


		public GameObject awesomeSprite;
		// Use this for initialization
		void OnDisable ()
		{
				GameEventManager.GameStart -= GameStart;
				GameEventManager.GameOver -= GameOver;
				GameEventManager.GamePointCalculation -= GamePoint;
		
		}

		void Awake ()
		{
				/* screen resolution for all devices */
				float camDistance = Vector3.Distance (transform.position, Camera.main.transform.position);
				Vector2 bottomCorner = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, camDistance));
				Vector2 topCorner = Camera.main.ViewportToWorldPoint (new Vector3 (1, 1, camDistance));
				minX = bottomCorner.x;
				maxX = topCorner.x - topCorner.x/3;
				minY = bottomCorner.y;
				maxY = topCorner.y;
		print ("maxX " + maxX + " the boundary " );
//				
		}

		void OnEnable ()
		{
				GameEventManager.GameStart += GameStart;
				GameEventManager.GameOver += GameOver;
				GameEventManager.GamePointCalculation += GamePoint;
		}

		void GamePoint ()
		{
				print (score++);
				

				
		}

		void Start ()
		{		
				sizeOfGameObjectY = gameObject.rigidbody2D.renderer.bounds.size.y;
				
		}
	
		void Update ()
		{ 
				
		}

		void GameStart ()
		{		
				newScale = new Vector2 (1, 1f);
				counter = 0;
				firstTime = false;
				score = 1;
				FirstMove (gameObject);
		}

		void GameOver ()
		{		
				if (score > PlayerPrefs.GetInt ("score"))
						PlayerPrefs.SetInt ("score", score);
				PlayerPrefs.Save ();
				Application.LoadLevel (0);
				GoogleAdController.ShowInterstital ();
		}
	
		void FirstMove (GameObject ob)
		{
		
				float dis = maxX;
				Hashtable paramTable = new Hashtable (); 
				paramTable.Add ("direction", (int)-1);
				paramTable.Add ("distance", (float)dis);
				paramTable.Add ("obj", (GameObject)ob);
				iTween.MoveTo ((GameObject)ob, iTween.Hash ("position", new Vector3 (dis, ob.transform.position.y, ob.transform.position.z), "speed", 5f, "easetype", iTween.EaseType.linear, "oncomplete", "Movement", "onupdate", "HaultMoving", "onupdatetarget", this.gameObject, "onupdateparams", ob, "oncompletetarget", this.gameObject, "oncompleteparams", paramTable));
		
		}
	
		void Movement (object vals)
		{
		
				Hashtable paramTable = new Hashtable (); 
		
				Hashtable ht = (Hashtable)vals;
				int dir = (int)ht ["direction"];
				float dis = (float)ht ["distance"];
				GameObject obj = (GameObject)ht ["obj"];
				paramTable.Add ("direction", (int)-1);
				paramTable.Add ("distance", dir * dis);
				paramTable.Add ("obj", (GameObject)ht ["obj"]);
				iTween.MoveTo ((GameObject)ht ["obj"], iTween.Hash ("position", new Vector3 (dir * dis, obj.transform.position.y, obj.transform.position.z), "speed", 5f, "easetype", iTween.EaseType.linear, "onupdate", "HaultMoving", "onupdatetarget", this.gameObject, "onupdateparams", (GameObject)ht ["obj"], "oncomplete", "Movement", "oncompletetarget", this.gameObject, "oncompleteparams", paramTable));
		
		}
	
		void HaultMoving (GameObject obj)
		{
				#if UNITY_ANDROID || UNITY_IPHONE
				if (Input.touchCount > 0)
				if( Input.GetTouch (0).phase == TouchPhase.Began) {
					print ("Touch working in android ");
					ScaleDown(obj);
				}
		
				#endif
		#if UNITY_WEBPLAYER
		if (Input.GetMouseButtonDown (0)) {
			
			ScaleDown(obj);
			
		}
		
		#endif
				#if UNITY_EDITOR
				if (Input.GetMouseButtonDown (0)) {
					
					ScaleDown(obj);

				}
				#endif
		}

		IEnumerator VanishPerfect ()
		{
				yield return new WaitForSeconds (1.0f);
			

		awesomeSprite.GetComponent<Image>().enabled=false;
				
			

		}

		void ScaleDown (GameObject obj)
		{		
		
				GameObject right = obj.transform.FindChild ("right").gameObject;
				GameObject left = obj.transform.FindChild ("left").gameObject;


		
		//		VectorLine.SetRay (Color.blue, GameObject.FindWithTag ("Cake").transform.FindChild ("right").gameObject.transform.position, Vector3.down);
				iTween.Stop ((GameObject)obj);
				obj.rigidbody2D.velocity = Vector2.zero;


				RaycastHit2D hitRight = Physics2D.Raycast (right.transform.position, -Vector2.up, 1.0f, 1 << 9);
				RaycastHit2D hitLeft = Physics2D.Raycast (left.transform.position, -Vector2.up, 1.0f, 1 << 9);
				/* Right raycast hit */
				if (hitRight.collider != null && hitLeft.collider != null) {
						print ("Perfect");
						
						awesomeSprite.GetComponent<Image>().enabled=true;
						StartCoroutine (VanishPerfect ());
						sizeX = 1f;
						GameEventManager.TriggerGamePointCalculation ();						
				} else if (hitRight.collider != null) {
						
						Vector3 posOfLeft = new Vector3 (hitRight.collider.transform.FindChild ("left").position.x, right.transform.position.y);
						float size = (right.transform.position - posOfLeft).magnitude;
						sizeX = size / hitRight.collider.bounds.size.x;
						
						GameEventManager.TriggerGamePointCalculation ();
						obj.transform.FindChild ("leftside").GetComponent<SpriteRenderer> ().enabled = true;
						iTween.MoveBy ((GameObject)obj.transform.FindChild ("leftside").gameObject, iTween.Hash ("y", -100, "speed", 1.5f));
						iTween.ScaleBy ((GameObject)obj, iTween.Hash ("amount", new Vector3 (sizeX, 1), "time", 0.1f));
						
						iTween.MoveTo ((GameObject)obj, iTween.Hash ("x", (right.transform.position.x + posOfLeft.x) / 2, "time", 0.1f));
						
				}

				/* Left Raycast hit */
				
				else if (hitLeft.collider != null) {
						Vector3 posOfRight = new Vector3 (hitLeft.collider.transform.FindChild ("right").position.x, left.transform.position.y);
						float size = (left.transform.position - posOfRight).magnitude;
						
						GameEventManager.TriggerGamePointCalculation ();
						sizeX = size / hitLeft.collider.bounds.size.x;
						
						obj.transform.FindChild ("rightside").GetComponent<SpriteRenderer> ().enabled = true;
						iTween.MoveBy ((GameObject)obj.transform.FindChild ("rightside").gameObject, iTween.Hash ("y", -100, "speed", 1.5f));
						iTween.ScaleBy ((GameObject)obj, iTween.Hash ("amount", new Vector3 (sizeX, 1), "time", 0.1f));
						

						iTween.MoveTo ((GameObject)obj, iTween.Hash ("x", (left.transform.position.x + posOfRight.x) / 2, "time", 0.1f));
			
				} else {
						if (firstTime) {
								GameEventManager.TriggerGameOver ();
								print ("Game over");
						}
						firstTime = true;
						sizeX = 1f;
				
				}

				counter++;
				if (counter > 3)
						CameraMove (obj);
//				NewCake (obj);
				
				newScale.x *= sizeX;
				// new cake generation
				obj.tag = "CakeGround";
				obj.layer = 9; // Layer value of CakeGround
				obj.GetComponent<Cake> ().enabled = false;
				NewToppings ();
				GameObject newCake = Instantiate (Resources.Load ("Cake")) as GameObject;
				newCake.tag = "Cake";
				newCake.layer = 8;

				newCake.transform.localScale = newScale;
				newCake.transform.position = new Vector2 (obj.transform.position.x, obj.transform.position.y + obj.renderer.bounds.size.y +0.045f);
				FirstMove ((GameObject)newCake);

		
		
		}

		void NewToppings ()
		{
				GameObject newToppings = Instantiate (Resources.Load ("straw2")) as GameObject;
				newToppings.transform.position = new Vector2 (Random.Range (maxX, -maxX), maxY);
				iTween.MoveBy ((GameObject)newToppings, iTween.Hash ("y", -100, "speed", 0.5f));
		}
		/*	
		void NewCake (GameObject obj)
		{
				obj.tag = "CakeGround";
				obj.layer = 9; // Layer value of CakeGround
				//				obj.GetComponent<Cake> ().enabled = false;
				GameObject newCake = new GameObject ("Cake");

				newCake.AddComponent<SpriteRenderer> ();
				Sprite[] sprite;
				sprite = Resources.LoadAll<Sprite> ("cake");
				newCake.GetComponent<SpriteRenderer> ().sprite = sprite [0];
//		newCake.AddComponent<iTween>();
//				iTween.ScaleBy ((GameObject)newCake, iTween.Hash ("x", 0.5f, "time", 0.1f));
				iTween.ScaleBy ((GameObject)newCake, iTween.Hash ("amount", new Vector3 (sizeX, 1), "time", 0.1f));
//				newCake.transform.localScale = new Vector3 (PlayerPrefs.GetFloat ("sizeX"), 1f);// newCake.GetComponent<SpriteRenderer> ().sprite.bounds.size;
//				newCake.transform.localScale = new Vector3 (PlayerPrefs.GetFloat ("sizeX"), 1f);// newCake.GetComponent<SpriteRenderer> ().sprite.bounds.size;
		
				newCake.AddComponent<Rigidbody2D> ();
				newCake.GetComponent<Rigidbody2D> ().fixedAngle = true;
				newCake.GetComponent<Rigidbody2D> ().gravityScale = 1;
				newCake.AddComponent<BoxCollider2D> ();
				newCake.tag = "Cake";
				newCake.layer = 8;
				
				newCake.transform.position = new Vector2 (obj.transform.position.x, obj.transform.position.y + obj.renderer.bounds.size.y);
				newCake.AddComponent<Cake> ();
				GameObject rightChild = new GameObject ("right"); 
				GameObject leftChild = new GameObject ("left"); 
				Sprite[] spriteChild;
				spriteChild = Resources.LoadAll<Sprite> ("ray");
				rightChild.AddComponent<SpriteRenderer> ();
				rightChild.GetComponent<SpriteRenderer> ().sprite = spriteChild [0];
				rightChild.transform.parent = newCake.transform;
				rightChild.transform.localPosition = new Vector2 (2.46f, -0.618f);
				
				leftChild.AddComponent<SpriteRenderer> ();
				leftChild.GetComponent<SpriteRenderer> ().sprite = spriteChild [0];
				leftChild.transform.parent = newCake.transform;
				leftChild.transform.localPosition = new Vector2 (-2.46f, -0.618f);

				FirstMove ((GameObject)newCake);
		}
	 */
	
		void CameraMove (GameObject obj)
		{
				float smooth = 1.5f; 
				Vector3 cameraPos;
				if (cameraMoveIndicator > 2) {		
						cameraPos = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y + gameObject.renderer.bounds.size.y, Camera.main.transform.position.z);
			
			
				} else {
						cameraMoveIndicator ++;
						cameraPos = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y + gameObject.renderer.bounds.size.y - 0.50f, Camera.main.transform.position.z);
			
				}
				iTween.MoveTo ((GameObject)Camera.main.gameObject, iTween.Hash ("position", cameraPos, "speed", 1));
				iTween.MoveTo ((GameObject)background, iTween.Hash ("position", new Vector3 (cameraPos.x, cameraPos.y, 5), "time", 0.1));
				
		
		}

		// Update is called once per frame
		

	
	
}
