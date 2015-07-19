using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour 
{
	public bool skip = false;

	// starting position || Area1 position
	public Transform posDown;
	// level intro position
	public Transform posUp;

	public static CameraManager cameraManager;
	
	public static CameraManager Get() { return cameraManager; }

	void Start()
	{
		cameraManager = this;
		StartCoroutine (LevelIntro ());
	}
	
	IEnumerator CameraUpdate()
	{
		skip = false;
		while(true)
		{
			if(GameManager.Get().player.transform.position.y > -12.5f)
			{
				PlatformCamera.Get().GetComponent<Camera>().cullingMask = -1;
				float z = Camera.main.WorldToScreenPoint(GameManager.Get().player.transform.position).z;
				posUp.position = z > 40.0f ? new Vector3(posUp.position.x, 10.0f, posUp.position.z) : new Vector3(posUp.position.x, 5.7f, posUp.position.z);
				yield return StartCoroutine(PlatformCamera.Get().CraneUp(posUp.position.y, 8f));
				yield return 0;
			}
			else if(GameManager.Get().player.transform.position.y < -12.5f)
			{
				PlatformCamera.Get().GetComponent<Camera>().cullingMask = 1 << 14;
				PlatformCamera.Get().GetComponent<Camera>().cullingMask = ~PlatformCamera.Get().GetComponent<Camera>().cullingMask;
				float z = Camera.main.WorldToScreenPoint(GameManager.Get().player.transform.position).z;
				posDown.position = z > 40.0f ? new Vector3(posUp.position.x, 2.5f, posUp.position.z) : new Vector3(posUp.position.x, -2.5f, posUp.position.z);
				yield return StartCoroutine(PlatformCamera.Get().CraneUp(posDown.position.y, 8f));
				yield return 0;
			}
			if(GameManager.Get().player.currentArea == "Area2" &&
			   GameManager.Get().player.lastArea != "Area2")
			{
				yield return StartCoroutine(PlatformCamera.Get().Rotate(30.0f));
				GameManager.Get().player.currentArea = "";
				yield return 0;
			}
			else if(GameManager.Get().player.currentArea == "Area1" && 
			        GameManager.Get().player.lastArea != "Area1")
			{
				yield return StartCoroutine(PlatformCamera.Get().Rotate(315.0f));
				GameManager.Get().player.currentArea = "";
				yield return 0;
			}
			else if(GameManager.Get().player.currentArea == "Area3" &&
			   		GameManager.Get().player.lastArea != "Area3")
			{
				yield return StartCoroutine(PlatformCamera.Get().Rotate(230.0f));
				GameManager.Get().player.currentArea = "";
				yield return 0;
			}
			else if(GameManager.Get().player.currentArea == "Area4" && 
			        GameManager.Get().player.lastArea != "Area4")
			{
				yield return StartCoroutine(PlatformCamera.Get().Rotate(140.0f));
				GameManager.Get().player.currentArea = "";
				yield return 0;
			}
			else
				yield return 0;
		}
	}

	IEnumerator LevelIntro()
	{
		yield return new WaitForSeconds(1.0f);
		yield return StartCoroutine(PlatformCamera.Get().CraneMove(posUp, 1f));
		if(skip==true)
		{
			yield return StartCoroutine(PlatformCamera.Get().SkipToTarget(posDown,8f));
			GameManager.Get().gameState = GameManager.GameStates.Playing;
			GameManager.Get().player.enabled = true;
			skip = false;
			yield return StartCoroutine(CameraUpdate());
		}
		yield return new WaitForSeconds(.25f);
		yield return StartCoroutine(PlatformCamera.Get().CraneMove(posDown, 2f));
		GameManager.Get().gameState = GameManager.GameStates.Playing;
		GameManager.Get().player.enabled = true;
		yield return StartCoroutine(CameraUpdate());
	}
}
