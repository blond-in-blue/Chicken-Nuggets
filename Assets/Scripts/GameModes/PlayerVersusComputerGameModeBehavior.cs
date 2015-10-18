using UnityEngine;
using System.Collections;

/// <summary>
/// This is the game mode behavior that keeps up with how player versus computer
/// should be executed.  Makes sure characters are appropriately spawned and
/// keeps up with score and respawning, and when the game is over
/// </summary>
public class PlayerVersusComputerGameModeBehavior : MonoBehaviour {

	enum Team{ One, Two }

	[System.Serializable]
	struct SpawnPoint{
		public GameObject point;
		public Team teamAtStart;
		public int priority;
	}

	[SerializeField]
	SpawnPoint[] spawnPoints;

	[SerializeField]
	int teamSize = 1;

	[SerializeField]
	GameObject[] scenicCameras;
	

	// Use this for initialization
	void Start () {

		//Can't play the game if we don't have enough spawn points for everyone!
		if(teamSize * 2 > spawnPoints.Length){
			Debug.LogError("Not enough spawn points for all players!");
			return;
		}

		startGame ();

	}

	/// <summary>
	/// Starts the game off!
	/// Spawns all players appropriately
	/// </summary>
	void startGame(){

		Team currentTeamSpawning = Team.One;

		bool spawningTeams = true;

		while (spawningTeams){

			ChickenTeam chickenTeam = ChickenTeam.Blue;
			if(currentTeamSpawning == Team.Two){
				chickenTeam = ChickenTeam.Red;
			}

			for(int teamMemberIndex = 0; teamMemberIndex < teamSize; teamMemberIndex ++){

				for(int i = 0; i < spawnPoints.Length; i ++){

					if(spawnPoints[i].priority == teamMemberIndex && spawnPoints[i].teamAtStart == currentTeamSpawning){

						//Spawn the players as the first chicken on the first team
						if(currentTeamSpawning == Team.One && teamMemberIndex == 0){
							ChickenFactory.createPlayerChicken(spawnPoints[i].point.transform.position, chickenTeam);
						} 
						else //Spawn a computer player
						{
							ChickenFactory.createChicken(spawnPoints[i].point.transform.position, chickenTeam, Random.Range(0f,1f));
						}

					}

				}

			}

			//Breaks us from the while loop if we've spawned both teams
			if(currentTeamSpawning == Team.One){
				currentTeamSpawning = Team.Two;
			} else {
				spawningTeams = false;
			}

		}

	}
	
	// Update is called once per frame
	void Update () {

	}


	public void OnLivingThingDeath(ChickenControlBehavior chicken){

	}

}
