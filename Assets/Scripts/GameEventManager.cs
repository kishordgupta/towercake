using UnityEngine;
using System.Collections;

public static class GameEventManager {

	public delegate void GameEvent();
	public static event GameEvent GamePointCalculation,GameStart, GameOver;
	public static void TriggerGamePointCalculation(){
		if(GamePointCalculation != null){
			GamePointCalculation();
		}
	}
	
	public static void TriggerGameStart(){
		if(GameStart != null){
			GameStart();
		}
	}
	public static void TriggerGameOver()
	{
		if(GameOver !=null)
			GameOver();
	}


}
