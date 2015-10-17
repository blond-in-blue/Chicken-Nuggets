# Chicken-Nuggets
A Unity3D project for the video game development club 2015 Fall Semester.

## Contributing
To contribute to the project please create a fork and open PRs to the master.  Make sure everyone know's what functionality your working on!

### Commits
When writing commit messages, try aiming for under 140 characters.  State why you made the changes you did more than what you did.

### Project Standards
These are just a few standards that we would like the repository to follow for consistancy's sake.  When you submit a PR we will make sure your changes follow these standards and if not ask you to make the appropriate changes so they do.

* Folders, file names, enums, and enum constants should be done in camel case beggining in a capital letter
  * ex: DemoAssets
  * ex: EnemyBehavior
  * ex: enum EnemyState { Searching, Persuing, Attacking, Fleeing }
* Function names are declared camel case and with starting letter lower case
  * ex: killSelf()
* If an enum is made public it should be put in it's own file that is named the enum
  * ex: public enum EnemyState will be put in EnemyState.cs, which only contains the enum decleration and using statements.
  * This is to make finding where the enum is declared easier, as well as an attempt at making merge conflicts easier.
* When you plan to have a class extend from monobehavior (such as a class meant for controlling camera movement), end the name of the class in Behavior
  * ex: CameraControlBehavior.cs
* Document every class and function!
  * To quickly add a summary to a class or function, move the cursor to above the declaration of a class slash function and enter '///'.  This in both Mono and VS start a nice body for the description of the class / function.
* **0 public variables.**  To get a variable's value or set it create the appropriate getter and setter methods.
  
### Programming Suggestions
These are just suggestions that have made Eli's time coding in Unity easier.
* When having a function being called inside of one of [MonoBehavior's messages](http://docs.unity3d.com/ScriptReference/MonoBehaviour.html) ( ex: Update() ), then that function should end in that message's name.
  * This makes programming state machine's easier.  The Update() contains a switch statement that calls the appropriate function based on the state.
  * ex: In Flee state, FleeUpdate() is a method that is called inside of [Update](http://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html).  
  * ex: In Flee state, FleeOnCollisionEnter() is a method that is called inside of [OnCollisionEnter](http://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter.html)
* Initialize Vector3 variables to Vector3.zero;
* If you want to edit a script's variable in monobehavior add the *[SerializeField]* line above the variable decleration.
* You should most likely be using a CharacterController rather than a rigid body for agent movement.
* Don't use Destroy( gameObject ) for removing the gameobject in the scene.  If it comes a time that you need to remove a gameobject from the scene localize that Destroy() statement to it's own method that can be called. Similar to destructors.


