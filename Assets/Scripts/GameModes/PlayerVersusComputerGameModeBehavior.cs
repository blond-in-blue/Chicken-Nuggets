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
	bool AIOnly = false;

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

	int teamOnePoints = 0;
	int teamTwoPoints = 0;
	void OnGUI(){

		GUI.Box (new Rect(10,10,110,90),"");

		GUI.Label (new Rect(20,20, 100,20), "Team One: "+teamOnePoints );

		GUI.Label (new Rect(20,50, 100,20), "Team Two: "+teamTwoPoints );

	}

	/// <summary>
	/// Starts the game off!
	/// Spawns all players appropriately
	/// </summary>
	void startGame(){

		GameState.getInstance ().setGameModeBeingPlayed (this);



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
						if(currentTeamSpawning == Team.One && teamMemberIndex == 0 && !AIOnly){
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

		Vector3 spawnLocation = spawnPoints [Random.Range(0, spawnPoints.Length)].point.transform.position;

		if (chicken.getChickensTeam () == ChickenTeam.Blue) {
			teamTwoPoints ++;
		} else {
			teamOnePoints ++;
		}

		if (chicken.gameObject.GetComponent<PlayerBehavior> () != null) {
			ChickenFactory.createPlayerChicken(spawnLocation, chicken.getChickensTeam());
		} else {
			ChickenFactory.createChicken(spawnLocation, chicken.getChickensTeam(), 0);
		}


	}

}
