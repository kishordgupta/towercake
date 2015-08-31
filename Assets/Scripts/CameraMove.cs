using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	public Vector3 offset ;
	Transform targetTransform ;
	void Start () {
		

		
	}
	
	// Update is called once per frame
	void Update () {
		targetTransform = GameObject.FindWithTag("Cake").GetComponent<Transform> ();
//		transform.position = new Vector3( targetTransform.position.x+15 + offset.x,transform.position.y,transform.position.z  );
		transform.position = new Vector3( targetTransform.position.x,transform.position.y,transform.position.z  );
		
	}
}
