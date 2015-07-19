using UnityEngine;
using System.Collections;

public class Deflection : MonoBehaviour 
{
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
		{
			Vector3 reflectForce = Vector3.Reflect(other.transform.forward, other.contacts[0].normal);
			other.rigidbody.velocity = Vector3.zero;
			other.rigidbody.angularVelocity = Vector3.zero;
			//other.transform.GetComponent<Laser>().Start = other.contacts[0].point;
			Quaternion rot = Quaternion.LookRotation(reflectForce);
			other.transform.rotation = rot;
			other.rigidbody.AddForce(reflectForce*20.0f, ForceMode.Impulse);
		}
	}
}
