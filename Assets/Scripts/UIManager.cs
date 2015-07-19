using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour 
{
	public Text levelPrompt;

	void Update()
	{
		switch( GameManager.Get().gameState )
		{
		case GameManager.GameStates.Intro:
			if(!levelPrompt.enabled)
				levelPrompt.enabled = true;
			break;
		case GameManager.GameStates.Playing:
			if(levelPrompt.enabled)
				levelPrompt.enabled = false;
			break;
		case GameManager.GameStates.LevelComplete:
			if(!levelPrompt.enabled)
			{
				levelPrompt.GetComponent<Text>().text = "Level Complete";
				levelPrompt.enabled = true;
				GameManager.Get().player.GetComponent<MyCharacterController>().enabled = false;
			}
			break;
		default:
			break;
		}
	}
}
