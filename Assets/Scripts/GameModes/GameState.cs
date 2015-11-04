using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The current state of a game, used for keeping up with what game mode is being played
/// Also keeps up with things AI and Gamemodes care about like what's alive
/// </summary>
public class GameState {

	private static GameState instance = null;

	/// <summary>
	/// Used to get an instance of the class.
	/// This is used to make sure only one single instance of this class is ever created
	/// </summary>
	/// <returns>The instance.</returns>
	public static GameState getInstance(){

		if(instance == null){
			instance = new GameState();
		}

		return instance;

	}

	GameModeType currentGameBeingPlayed = GameModeType.None;

	PlayerVersusComputerGameModeBehavior gameModeBeingPlayed;


	/// <summary>
	/// Gets the current game mode *object* being played.
	/// This object is the behavior of the game mode which keeps up with spawning
	/// and what not.
	/// </summary>
	/// <returns>The current mode being played.</returns>
	public PlayerVersusComputerGameModeBehavior getCurrentModeBeingPlayed(){
		return gameModeBeingPlayed;
	}


	/// <summary>
	/// Sets the game mode being played.
	/// Allows it to talk to the game mode to update it when a character dies,
	/// or something else happpens such as ending game early.
	/// </summary>
	/// <param name="gameMode">Game mode.</param>
	public void setGameModeBeingPlayed(PlayerVersusComputerGameModeBehavior gameMode){
		this.gameModeBeingPlayed = gameMode;
	}


	//private constructor so only the GameState can create itself 
	GameState(){
		charactersInScene = new List<ChickenControlBehavior>();
	}

	List<ChickenControlBehavior> charactersInScene = null;

	/// <summary>
	/// Adds a character to the current game state.
	/// </summary>
	/// <param name="chicken">Chicken.</param>
	public void addCharacter(ChickenControlBehavior chicken){

		if(chicken == null){
			return;
		}

		charactersInScene.Add (chicken);

	}

	/// <summary>
	/// Removes a character from the current game state.
	/// Generally moved upon death
	/// </summary>
	/// <param name="chicken">Chicken.</param>
	public void removeCharacter(ChickenControlBehavior chicken){

		if(chicken == null){
			return;
		}

		if(charactersInScene.Contains(chicken)){

			//If we're playing a game right now..
			if(gameModeBeingPlayed != null){
				gameModeBeingPlayed.OnLivingThingDeath(chicken);
			}

			charactersInScene.Remove(chicken);
		}

	}

	/// <summary>
	/// Get all characters that are alive in the current game state
	/// </summary>
	/// <returns>The all characters.</returns>
	public ChickenControlBehavior[] getAllCharacters(){
		return charactersInScene.ToArray ();
	}

	/// <summary>
	/// Based on all characters in the game state, we return those that are not on 
	/// the team that is specified
	/// </summary>
	/// <returns>The all characters not on team.</returns>
	/// <param name="teamTheirNotOn">Team their not on.</param>
	public ChickenControlBehavior[] getAllCharactersNotOnTeam(ChickenTeam teamTheirNotOn){

		List<ChickenControlBehavior> charactersNotOnTeam = new List<ChickenControlBehavior>();

		foreach (ChickenControlBehavior character in charactersInScene) {
			if(character.getChickensTeam() != teamTheirNotOn){
				charactersNotOnTeam.Add(character);
			}
		}

		return charactersNotOnTeam.ToArray();

	}

}
