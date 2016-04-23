//This script defines the player, its speed and movement limits, as well as the different types of deaths it may suffer.
#pragma strict

internal var thisTransform:Transform;
internal var gameController:GameObject;

//The player's movement speed, and variables that check if the player is moving now, where it came from, and where it's going to
var speed:float = 0.1;
static var speedMultiplier:float = 1;
internal var isMoving:boolean = false;
internal var previousPosition:Vector3;
static var targetPosition:Vector3;

//The movement limits object. This object contains some colliders that bounce the player back into the game area
var moveLimits:Transform;

//The player can't move or perform actions for a few seconds
var moveDelay:float = 0;

// Holds the next move the player should have. This is recorded if you try to move while the player is already moving, so the next move executes immediately after this move is finished
internal var nextMove:String = "stop";
internal var currentMove:String;

// The height at which the player is currently moving. This is used to allow the player to move on higher/lower platforms
internal var moveHeight:float = 0;

//Various animations for the player
var animationMove:AnimationClip;
var animationCoin:AnimationClip;
var animationSpawn:AnimationClip;
var animationVictory:AnimationClip;

// The Animator controller for the player. This is used to replace the Animation system.
var animatorObject:Animator;

//Death effects that show when the player is killed
var deathEffect:Transform[];

//Is this player invulnerable? If so, then you don't die when recieving a Die() function call
var isInvulnerable:boolean = false;

// The object that this player is currently standing on, such as a platform
var attachedToObject:Transform;

// Did the player win the game?
internal var isVictorious:boolean = false;

//Various sounds and their source
var soundMove:AudioClip[];
var soundCoin:AudioClip[];
var soundSourceTag:String = "GameController";

function Start() 
{
	speedMultiplier = 1;

	thisTransform = transform;
	
	targetPosition = thisTransform.position;
	
	gameController = GameObject.FindGameObjectWithTag("GameController");
}

function Update() 
{
	if ( isVictorious == false )
	{
		//Keep the move limits object moving forward/backward along with the player
		if ( moveLimits )    moveLimits.position.x = thisTransform.position.x;
		
		//Count down the move delay
		if ( moveDelay > 0 )    moveDelay -= Time.deltaTime * speedMultiplier;
		
		//If the player is not already moving, it can move
		if ( Time.timeScale > 0 )
		{
			//You can move left/right only if you are not already moving forward/backwards
			if ( Input.GetAxisRaw("Vertical") == 0 )
			{
				//Moving right
				if ( Input.GetAxisRaw("Horizontal") > 0 )
				{
					//Move one step to the right
					Move("right");
				}
				
				//Moving left
				if ( Input.GetAxisRaw("Horizontal") < 0 )
				{
					//Move one step to the left
					Move("left");
				}
			}
			
			//You can move forward/backwards only if you are not already moving left/right
			if ( Input.GetAxisRaw("Horizontal") == 0 )
			{
				//Moving forward
				if ( Input.GetAxisRaw("Vertical") > 0 )
				{
					//Move one step forward
					Move("forward");
				}
				
				//Moving backwards
				if ( Input.GetAxisRaw("Vertical") < 0 )
				{
					//Move one step backwards
					Move("backward");
				}
			}
		}
		
		// If the player is moving, move it to its target
		if ( isMoving == true )
		{
			// Keep moving towards the target position until we reach it
			if ( attachedToObject == null )
			{
				if( Vector3.Distance( thisTransform.position, targetPosition) > 0.1 )
				{
					// Move this object towards the target position
					thisTransform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed * speedMultiplier);
				}
				else
				{
					thisTransform.position = targetPosition;

					// The object is not moving anymore
					isMoving = false;
				}
			}
		}
		else if ( nextMove != "stop" && nextMove != currentMove ) // If there is a next move recorded, move the player to it and clear it
		{
			// Move the player to the next move
			Move(nextMove);
			
			// Clear the next move
			nextMove = "stop";
		}
	}
}

//This function moves an object from its current position to a target position, over time
function Move( moveDirection:String )
{
	if ( isVictorious == false )
	{
		if ( isMoving == false && moveDelay <= 0 )
		{
			currentMove = moveDirection;
			
			// If the player is attached to an object, detach from it
			if ( attachedToObject )    Detach();
					
			//The object is moving
			isMoving = true;
			
			// If we are moving in a different direction, switch to that direction
			//if ( moveDirection )   
			
			switch ( moveDirection )
		    {
			    case "forward":
			        //Turn to the front
					thisTransform.eulerAngles.y = 0;
					
					//Set the new target position to move to
					targetPosition = thisTransform.position + Vector3(1,0,0);
					
					//Make sure the player lands on the grid 
					targetPosition.x = Mathf.Round(targetPosition.x);
					targetPosition.z = Mathf.Round(targetPosition.z);
					
					//Register the last position the player was at, so we can return to it if the path is blocked
					previousPosition = thisTransform.position;
						
			        break;
			    
			    case "backward":
			        //Turn to the back
					thisTransform.eulerAngles.y = 180;
					
					//Register the last position the player was at, so we can return to it if the path is blocked
					previousPosition = thisTransform.position;
					
					//Make sure the player lands on the grid 
					targetPosition.x = Mathf.Round(targetPosition.x);
					targetPosition.z = Mathf.Round(targetPosition.z);
					
					//Set the new target position to move to
					targetPosition = thisTransform.position + Vector3(-1,0,0);
			        
			        break;
			        
			    case "right":
			        //Turn to the right
					thisTransform.eulerAngles.y = 90;
					
					//Register the last position the player was at, so we can return to it if the path is blocked
					previousPosition = thisTransform.position;
					
					//Make sure the player lands on the grid 
					targetPosition.x = Mathf.Round(targetPosition.x);
					targetPosition.z = Mathf.Round(targetPosition.z);
					
					//Set the new target position to move to
					targetPosition = thisTransform.position + Vector3(0,0,-1);
			        
			        break;
			        
			    case "left":
		        	//Turn to the left
					thisTransform.eulerAngles.y = -90;
					
					//Register the last position the player was at, so we can return to it if the path is blocked
					previousPosition = thisTransform.position;
					
					//Make sure the player lands on the grid 
					targetPosition.x = Mathf.Round(targetPosition.x);
					targetPosition.z = Mathf.Round(targetPosition.z);
					
					//Set the new target position to move to
					targetPosition = thisTransform.position + Vector3(0,0,1);
						
			        break;
			        
			    default:
			        //Turn to the front
					thisTransform.eulerAngles.y = 0;
					
					//Set the new target position to move to
					targetPosition = thisTransform.position + Vector3(1,0,0);
					
					targetPosition.Normalize();
					
					//Register the last position the player was at, so we can return to it if the path is blocked
					previousPosition = thisTransform.position;

			        break;
		    }
		    
			targetPosition.y = moveHeight;
		    
			// If we are using an Animator Object, use it for animation
			if ( animatorObject )
			{
				//print(animatorObject.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
				//animatorObject["Jump"].time = 0f;

				animatorObject.Play("Jump", -1, 0);			
				//print("F");

				//if ( animator && !animator.GetCurrentAnimatorStateInfo(0).IsName("walk") )    animator.Play("walk", -1, 0f);

			}
			else if ( GetComponent.<Animation>() && animationMove )    // Otherwise, if there is an animation component, play it
			{
				// Stop the animation

				GetComponent.<Animation>().Stop();
			
				// Play the animation
				GetComponent.<Animation>().Play(animationMove.name);
			
				// Set the animation speed base on the movement speed
				GetComponent.<Animation>()[animationMove.name].speed = speed * speedMultiplier;
			}
			
			//If there is a sound source and more than one sound assigned, play one of them from the source
			if ( soundSourceTag != String.Empty && soundMove.Length > 0 )    GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent.<AudioSource>().PlayOneShot(soundMove[Mathf.Floor(Random.value * soundMove.Length)]);
		}
		else
		{
			// If we are still moving, record the next move for smoother controls
			nextMove = moveDirection;
		}
	}
}

//This function cancels the player's current move, bouncing it back to it's previous position 
function CancelMove( moveDelayTime:float )
{
	//If there is an animation, play it
	if ( GetComponent.<Animation>() && animationMove )    
	{
		//Set the animation speed base on the movement speed
		GetComponent.<Animation>()[animationMove.name].speed = -speed * speedMultiplier;
	}
	
	//Set the previous positoin as the target position to move to
	targetPosition = previousPosition;
	
	//If there is a move delay, prevent movement for a while
	moveDelay = moveDelayTime;
}

/// This function changes the height of the player. Used to allow the player to move to higher/lower platforms
function ChangeHeight( targetHeight:Transform )
{
	targetPosition.y = moveHeight = targetHeight.position.y;
}

//This function animates a coin added to the player
function AddCoin( addValue:int )
{
	gameController.SendMessage("ChangeScore", addValue);
	
	//If there is an animation, play it
	if ( GetComponent.<Animation>() && animationCoin && animationMove )    
	{
		//Play the animation
		//animation.Play(animationCoin.name);
		
		//animation.Play(animationMove.name); 
		GetComponent.<Animation>()[animationCoin.name].layer = 1; 
		GetComponent.<Animation>().Play(animationCoin.name); 
		GetComponent.<Animation>()[animationCoin.name].weight = 0.5f;
	}
	
	//If there is a sound source and more than one sound assigned, play one of them from the source
	if ( soundSourceTag != String.Empty && soundCoin.Length > 0 )    GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent.<AudioSource>().PlayOneShot(soundCoin[Mathf.Floor(Random.value * soundCoin.Length)]);
}

//This function destroys the player, and triggers the game over event
function Die( deathType:int )
{
	// If the player is attached to an object, detach from it
	if ( attachedToObject )    Detach();
	
	//If you are invulnerable, don't die
	if ( isInvulnerable == false )
	{
		//Change the number of lives the player has, and check for game over
		gameController.SendMessage("ChangeLives", -1);
		
		//If we have death effects...
		if ( deathEffect.Length > 0 )
		{
			//Create the correct death effect
			Instantiate(deathEffect[deathType], thisTransform.position, thisTransform.rotation);
		}
		
		//Deactivate the player object
		gameObject.SetActive(false);
	}
}

//This function spawns the player
function Spawn()
{
	//Activate the player object
	gameObject.SetActive(true); 
	
	// If there is an animation, update the animation speed based on speedMultiplier
	if( GetComponent.<Animation>() && animationSpawn )    GetComponent.<Animation>().Play(animationSpawn.name);
}

//This function changes the speed of the player
function SetPlayerSpeed( setValue:float )
{
	speedMultiplier = setValue;

	// If there is an animation, update the animation speed base on speedMultiplier
	if( GetComponent.<Animation>() && animationMove )    GetComponent.<Animation>()[animationMove.name].speed = speed * speedMultiplier;

	// If there is an animation, update the animation speed base on speedMultiplier
	if( GetComponent.<Animation>() && animationCoin )    GetComponent.<Animation>()[animationCoin.name].speed = speed * speedMultiplier;
}

// Detach this object from the object it is currently attached to
function AttachToThis( attachedObject:Transform )
{
	// Set the current attached object
	attachedToObject = attachedObject;
	
	// Set the object we are attached to as the parent
	thisTransform.parent = attachedToObject;
	
	// Set the position to 0 locally
	thisTransform.localPosition = Vector3.zero;
	
	// The player is not moving
	isMoving = false;
}

/// Detach this object from the object it is currently attached to
function Detach()
{
	// No object is attached
	attachedToObject = null;
	
	// The player has no parent
	thisTransform.parent = null;
}

/// Runs when the player wins the level
function Victory()
{
	isVictorious = true;
	
	// If there is a victory animation, play it
	if( GetComponent.<Animation>() && animationVictory )    GetComponent.<Animation>().Play(animationVictory.name);
}