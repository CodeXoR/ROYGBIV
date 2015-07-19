using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public enum GameStates { Intro = 0, Playing = 1, LevelComplete = 2 };
	public GameStates gameState = GameStates.Intro;
	public MyCharacterController player;
	public GameObject[] levelAreas;
	public TransparencyTracker[] transTrackers;
	public static GameManager gameManager;
	public static GameManager Get() { return gameManager; }

	void Awake()
	{
		gameManager = this;
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<MyCharacterController>();
		levelAreas = GameObject.FindGameObjectsWithTag ("LevelArea");
	}

	void Update()
	{
		ApplyObjectTransparency ();

		/*switch(gameState)
		{
		case GameStates.Intro:
			break;
		case GameStates.Playing:
			break;
		case GameStates.LevelComplete:
			break;
		default:
			break;
		}*/
	}

	void ApplyObjectTransparency()
	{
		switch(player.GetComponent<MyCharacterController>().lastArea)
		{
		case "Area1":
			transTrackers[1].transform.position = new Vector3(-15.64f,-15.86f,7.33f);
			transTrackers[0].transform.position = new Vector3(4.27f,-15.86f,14.01f);
			transTrackers[0].GetComponent<BoxCollider>().center = new Vector3(-11.16f,-6.52f,72.17f);
			transTrackers[0].GetComponent<BoxCollider>().size = new Vector3(18.63f,24.5f,240.0f);
			break;
		case "Area3":
			transTrackers[1].transform.position = new Vector3(-15.64f,-15.86f,4.6f);
			transTrackers[4].transform.position = new Vector3(4.15f,-15.86f,-12.87f);
			transTrackers[4].GetComponent<BoxCollider>().center = new Vector3(-11.16f,-6.52f,72.17f);
			transTrackers[4].GetComponent<BoxCollider>().size = new Vector3(14f,24.5f,178.0f);
			break;
		case "Area4":
			transTrackers[4].transform.position = new Vector3(7.25f,-15.86f,-16.92f);
			transTrackers[4].GetComponent<BoxCollider>().center = new Vector3(-11.16f,-6.52f,72.17f);
			transTrackers[4].GetComponent<BoxCollider>().size = new Vector3(14f,24.5f,226.0f);
			transTrackers[7].transform.position = new Vector3(5.76f,-15.82f,3.79f);
			transTrackers[7].GetComponent<BoxCollider>().center = new Vector3(-11.16f,-6.52f,72.17f);
			transTrackers[7].GetComponent<BoxCollider>().size = new Vector3(14f,24.5f,165.0f);
			break;
		case "Area2":
			transTrackers[0].transform.position = new Vector3(6.58f,-15.86f,13.14f);
			transTrackers[0].GetComponent<BoxCollider>().center = new Vector3(-11.16f,-6.52f,72.17f);
			transTrackers[0].GetComponent<BoxCollider>().size = new Vector3(18.63f,24.5f,250.0f);
			transTrackers[7].transform.position = new Vector3(7.2f,-15.82f,6.13f);
			transTrackers[7].GetComponent<BoxCollider>().center = new Vector3(-11.16f,-6.52f,72.17f);
			transTrackers[7].GetComponent<BoxCollider>().size = new Vector3(14f,24.5f,190.0f);
			break;
		default:
			break;
		}
	}
}
