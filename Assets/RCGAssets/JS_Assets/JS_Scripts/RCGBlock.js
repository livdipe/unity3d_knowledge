//This script defines a block which can interact with the player in various ways. A block may be a rock or a wall that
//bounces the player back, or it can be an enemy that kills the player, or it can be a coin that can be collected.
#pragma strict

//The tag of the object that can touch this block
var touchTargetTag:String = "Player";

//A list of functions that run when this block is touched by the target
var touchFunctions:TouchFunction[];

public class TouchFunction
{
	//The name of the function that will run
	var functionName:String = "CancelMove";
	
	//The tag of the target that the function will run on
	var targetTag:String = "Player";
	
	//A parameter that is passed along with the function
	var functionParameter:float = 0;
}

//Remove this object after a ceratin amount of touches
var removeAfterTouches:int = 0;
internal var isRemovable:boolean = false;

//The animation that plays when this object is touched
var hitAnimation:AnimationClip;

//The sound that plays when this object is touched
var soundHit:AudioClip;
var soundSourceTag:String = "GameController";


//The effect that is created at the location of this object when it is destroyed
var deathEffect:Transform;

function Start()
{
	//If removeAfterTouches is higher than 0, make this object removable after one or more touches
	if ( removeAfterTouches > 0 )    isRemovable = true;
}

//This function runs when this obstacle touches another object with a trigger collider
function OnTriggerEnter(other:Collider) 
{	
	//Check if the object that was touched has the correct tag
	if ( other.tag == touchTargetTag )
	{
		//Go through the list of functions and runs them on the correct targets
		for ( var touchFunction in touchFunctions )
		{
			//Check that we have a target tag and function name before running
			if ( touchFunction.functionName != String.Empty )
			{
				// If the targetTag is "TouchTarget", it means that we apply the function on the object that ouched this lock
				if ( touchFunction.targetTag == "TouchTarget" )
				{
					// Run the function
					other.SendMessage(touchFunction.functionName, transform);
				}
				else if ( touchFunction.targetTag != String.Empty )
				{
					//Run the function
					GameObject.FindGameObjectWithTag(touchFunction.targetTag).SendMessage(touchFunction.functionName, touchFunction.functionParameter);
				}
			}
		}
		
		//If there is an animation, play it
		if ( GetComponent.<Animation>() && hitAnimation )    
		{
			//Stop the animation
			GetComponent.<Animation>().Stop();
			
			//Play the animation
			GetComponent.<Animation>().Play(hitAnimation.name);
		}
		
		//If this object is removable, count down the touches and then remove it
		if ( isRemovable == true )
		{
			//Reduce the number of times this object was touched by the target
			removeAfterTouches--;
			
			if ( removeAfterTouches <= 0 )    
			{
				if ( deathEffect )    Instantiate( deathEffect, transform.position, Quaternion.identity);
				
				Destroy(gameObject);
			}
		}
		
		//If there is a sound source and a sound assigned, play it
		if ( soundSourceTag != "" && soundHit )    GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent.<AudioSource>().PlayOneShot(soundHit);
	}
}