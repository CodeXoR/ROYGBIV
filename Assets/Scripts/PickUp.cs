using UnityEngine;
using System.Collections;

public class PickUp : MonoBehaviour {

	public enum PickUpStates { Carried = 0, CanPickUp =1 }
	public PickUpStates state = PickUpStates.CanPickUp;
	public Transform indicatorPos;

	void LateUpdate()
	{
		switch(state)
		{
		case PickUpStates.CanPickUp:
			Vector3 relPos = new Vector3( Camera.main.transform.position.x, indicatorPos.position.y, Camera.main.transform.position.z ) - indicatorPos.position;
			indicatorPos.rotation = Quaternion.LookRotation(relPos);
		    break;
		case PickUpStates.Carried:
			break;
		default:
			break;
		}
	}
}
