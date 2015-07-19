using UnityEngine;
using System.Collections;

public class PlatformCamera : MonoBehaviour 
{
	public static PlatformCamera craneCam;
	private float angleOfError;
	public Transform pivot;
	public float rotSpeed;

	public static PlatformCamera Get(){return craneCam;}
	void Awake()
	{
		craneCam = this;
		angleOfError = 0.25f;
	}

	public IEnumerator CraneUp( float yPos, float moveSpeed )
	{
		while(true)
		{
			if( Mathf.Abs(yPos - transform.position.y) > angleOfError )
			{
				transform.position = Vector3.MoveTowards (transform.position, 
				                                          new Vector3(transform.position.x,
				                                                      yPos,
				                                                      transform.position.z), 
				                                          moveSpeed * Time.deltaTime);
				yield return 0;
			}
			else
				yield break;
		}
	}

	public IEnumerator CraneMove(Transform target, float moveSpeed)
	{
		while(true)
		{
			if(Input.GetMouseButtonDown(0))
			{
				CameraManager.Get().skip = true;
				GameManager.Get().player.lastArea = GameManager.Get().player.currentArea;
				yield break;
			}
			if( Vector3.Angle(transform.position, target.position) > angleOfError )
			{
				transform.position = Vector3.MoveTowards (transform.position, 
				                                          target.position, 
				                                          moveSpeed * Time.deltaTime);
				yield return 0;
			}
			else
			{
				GameManager.Get().player.lastArea = GameManager.Get().player.currentArea;
				yield break;
			}
		}
	}

	public IEnumerator RotateToTarget(Transform target, float rotSpeed)
	{
		while(true)
		{
			if( Vector3.Angle(transform.eulerAngles, target.eulerAngles) > angleOfError )
			{
				transform.rotation = Quaternion.RotateTowards (transform.rotation, 
				                                               target.rotation, 
				                                               rotSpeed * Time.deltaTime);
				yield return 0;
			}
			else
			{
				yield break;
			}
		}
	}

	public IEnumerator Rotate(float targetAngle)
	{
		float dir = Mathf.DeltaAngle (transform.eulerAngles.y, targetAngle);
		dir = dir > 0 ? 1 : -1;
		while(true)
		{
			float delta = Mathf.Abs(transform.eulerAngles.y - targetAngle);
 			delta = Mathf.Abs(delta);
			//Debug.Log(delta);
			if(delta > 2.0f)
			{
				transform.RotateAround (pivot.position, pivot.forward, 
				                        dir * rotSpeed * Time.smoothDeltaTime);
				yield return 0;
			}
			else
			{
				transform.eulerAngles = new Vector3(transform.eulerAngles.x,
				                                    targetAngle,
				                                    transform.eulerAngles.z);
				GameManager.Get().player.lastArea = GameManager.Get().player.currentArea;
				yield break;
			}
		}
	}

	public IEnumerator SkipToTarget(Transform target, float moveSpeed)
	{
		while(true)
		{
			if( Vector3.Angle(transform.position, target.position) > angleOfError )
			{
				transform.position = Vector3.MoveTowards (transform.position, 
				                                          target.position,
				                                          moveSpeed * Time.deltaTime);
				yield return 0;
			}
			else
			{
				//GameManager.Get().player.lastArea = GameManager.Get().player.currentArea;
				yield break;
			}
		}
	}


}
