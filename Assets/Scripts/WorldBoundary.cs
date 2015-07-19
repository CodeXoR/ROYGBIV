using UnityEngine;
using System.Collections;

public class WorldBoundary : MonoBehaviour {
	
	// Update is called once per frame
	void LateUpdate () 
	{
		Vector3 blah = Camera.main.WorldToViewportPoint(transform.position);
		Vector3 boom = Camera.main.ViewportToWorldPoint (blah);
		if (blah.x <= 0.01f)
		{
			float x = Camera.main.ViewportToWorldPoint (blah).x;
			boom.x = Mathf.Clamp (boom.x, x+.05f, 10);
			transform.position = boom;
		}
		if (blah.x >= 0.99f)
		{
			float x = Camera.main.ViewportToWorldPoint (blah).x;
			boom.x = Mathf.Clamp (boom.x, -10, x-.05f);
			transform.position = boom;
		}
		if (blah.y <= 0.01f)
		{
			float z = Camera.main.ViewportToWorldPoint (blah).z;
			boom.z = Mathf.Clamp (boom.x, -10, z+.05f);
			transform.position = boom;
		}
		if (blah.y >= 0.99f)
		{
			float z = Camera.main.ViewportToWorldPoint (blah).z;
			boom.z = Mathf.Clamp (boom.x, z-.05f, 10);
			transform.position = boom;
		}
	}
}
