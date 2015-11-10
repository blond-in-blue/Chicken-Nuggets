using UnityEngine;
using System.Collections;

public class MatchmakingBehavior : Photon.PunBehaviour, IGameMode {

	// Use this for initialization
	void Start()
	{
		PhotonNetwork.ConnectUsingSettings("0.1");
	}
	
	void OnGUI()
	{

		GUILayout.Label(PhotonNetwork.connectionState.ToString() + " - " + PhotonNetwork.connectionStateDetailed.ToString());


		if(Input.GetKey(KeyCode.Keypad1)){

			int leaderboardsWidth = 500;
			int leaderboardsHeight = 500;

			int startingX= (Screen.width / 2) - (leaderboardsWidth/2);
			int startingY = (Screen.height / 2) - (leaderboardsHeight/2);

			GUI.Box(new Rect(startingX, startingY,leaderboardsWidth,leaderboardsHeight),"Players: "+PhotonNetwork.playerList.Length);
			
			for(int i = 0; i < PhotonNetwork.playerList.Length; i ++){
				GUI.Box(new Rect(startingX+10, startingY + 20+(i*30), leaderboardsWidth-20, 30),PhotonNetwork.playerList[i].name);
			}

		}

		PhotonNetwork.playerName = GUI.TextField (new Rect(10,Screen.height-30,100,20), PhotonNetwork.playerName);

	}


	public override void OnJoinedLobby()
	{
		Debug.Log ("Joined Lobby!");
		PhotonNetwork.JoinRandomRoom();
	}


	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		PhotonNetwork.CreateRoom(null);
	}

	void OnJoinedRoom()
	{
		Debug.Log ("Joined Room!");
		ChickenFactory.createNetworkPlayerChicken (new Vector3(0, 1, 0), ChickenTeam.None);
	}




	public void onLivingThingDeath(ChickenControlBehavior chicken){

	}


	public void gameModeStart(){

	}

}
