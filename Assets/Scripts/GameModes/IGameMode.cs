using UnityEngine;
using System.Collections;

public interface IGameMode {

	void onLivingThingDeath(ChickenControlBehavior chicken);

	void gameModeStart();

}
