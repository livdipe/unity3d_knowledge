//This script controls the game, starting it, following game progress, and finishing it with game over.
//It also creates lanes with moving objects and items as the palyer progresses.
#if UNITY_5_3
import UnityEngine.SceneManagement;
#endif

#pragma strict

//Using the new Unity 4.6 UI
import UnityEngine.UI;

//The player object
var playerObjects:Transform[];
var currentPlayer:int = 0;

//The number of lives the player has. When the player dies, it loses one life. When lives reach 0, it's game over.
var lives:int = 1;

//The text object that shows how many lives we have left
var livesText:Transform;

//How many seconds to wait before respawning after the player dies
var respawnTime:float = 1.2;

//The object that replaces the player while respawning
var respawnObject:Transform;
static var targetPosition:Vector3;

//The camera that follows the player
var cameraObject:Transform;

//The object that holds the movement buttons. If swipe controls are used, this object is deactivated
var moveButtonsObject:Transform;

//Should swipe controls be used instead of click/tap controls?
var swipeControls:boolean = false;

//The start and end positions of the touches, when using swipe controls
internal var swipeStart:Vector3;
internal var swipeEnd:Vector3;

//The swipe distance; How far we need to swipe before detecting movement
var swipeDistance:float = 10;

//How long to wait before the swipe command is cancelled
var swipeTimeout:float = 1;
internal var swipeTimeoutCount:float;

// An array of powerups that can be activated
var powerups:Powerup[];

public class Powerup
{
	// The name of the function 
	var startFunctionA:String = "SetScoreMultiplier";
	var startParamaterA:float = 2;
	
	// The name of the function 
	var startFunctionB:String = "SetScoreMultiplier";
	var startParamaterB:float = 2;

	// The duration of this powerup. After it reaches 0, the end functions run
	var duration:float  = 10;
	internal var durationMax:float;

	// The name of the function 
	var endFunctionA:String = "SetScoreMultiplier";
	var endParamaterA:float = 1;

	// The name of the function 
	var endFunctionB:String = "SetScoreMultiplier";
	var endParamaterB:float = 1;

	// The icon of this powerup
	var icon:Transform;
}

// Stop the powerups when the player dies. Otherwise, the powerups will only be stopped on game over
var stopPowerupsOnDeath:boolean = false;

//A list of lanes that that randomly appear as the player moves forward
var lanes:Lane[];
internal var lanesList:Lane[];

public class Lane
{
	var laneObject:Transform;
	var laneChance:float = 1;
	var laneWidth:float = 1;
	var itemChance:float = 1;
}

// A lane that appears after the player passes the set number of lanes for victory. This is used in randomly-generated levels only.
var victoryLane:Transform;

// When the player passes this number of lanes in a level, we win. This is used in randomly-generated levels only.
var lanesToVictory:int = 0;

// The number of lanes we passed so far. This is used to check if we reached the victory lane. Only for randomly-generated levels.
internal var lanesCreated:int = 0;

//A list of the objects that can be dropped
var objectDrops:ObjectDrop[];
internal var objectDropList:Transform[];

public class ObjectDrop
{
	//The object that can be dropped
	var droppedObject:Transform;
	
	//The drop chance of the object
	var dropChance:int = 1;
}

//Should objects be dropped in sequence ( one after the other ) rather than randomly?
var dropInSequence:boolean = true;

//The index of the current drop, if we are using dropInSequence
internal var currentDrop:int = -1;

//The offset left and right on the lane
var objectDropOffset:int = 4;

//How many lanes to create before starting the game
var precreateLanes:int = 20;
var nextLanePosition:float = 1;

//The score and score text of the player
var score:int = 0;
var scoreText:Transform;
internal var highScore:int = 0;
internal var scoreMultiplier:int = 1;

//The player prefs record of the total coins we have ( not high score, but total coins we collected in all games )
var coinsPlayerPrefs:String = "Coins";

//The overall game speed
var gameSpeed:float = 1;

//How many points the player needs to collect before leveling up
var levelUpEveryCoins:int = 5;
internal var increaseCount:int = 0;

//This is the speed of the camera that keeps advancing on the player and kills him instantly if it reaches him
var deathLineObject:Transform;
internal var deathLineTargetPosX:float;
var deathLineSpeed:float = 1;
var deathLineSpeedIncrease:float = 1;
var deathLineSpeedMax:float = 1.5;

//Various canvases for the UI
var gameCanvas:Transform;
var pauseCanvas:Transform;
var gameOverCanvas:Transform;
var victoryCanvas:Transform;

//Is the game over?
internal var isGameOver:boolean = false;

//The level of the main menu that can be loaded after the game ends
var mainMenuLevelName:String = "MainMenu";

//Various sounds
var soundLevelUp:AudioClip;
var soundGameOver:AudioClip;
var soundVictory:AudioClip;

//The tag of the sound source
var soundSourceTag:String = "GameController";

var confirmButton:String = "Submit";

//The button that pauses the game. Clicking on the pause button in the UI also pauses the game
var pauseButton:String = "Cancel";
internal var isPaused:boolean = false;

internal var index:int = 0;

// The ad controller that will be used in this level
var adController:GameObject;

function Awake()
{
	if ( adController )    
	{
		// Create a new ad controller
		var newController:GameObject = Instantiate(adController);

		// Assign it to the scene so we can access it later
		adController = newController;
	}
}

function Start() 
{
	//Update the score and lives without changing them
	ChangeScore(0);
	ChangeLives(0);
	
	//Hide the game over canvas
	if ( gameOverCanvas )    gameOverCanvas.gameObject.SetActive(false);
	if( victoryCanvas )    gameOverCanvas.gameObject.SetActive(false);

	//Get the highscore for the player
	#if UNITY_5_3
	highScore = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_HighScore", 0);
	#else
	highScore = PlayerPrefs.GetInt(Application.loadedLevelName + "_HighScore", 0);
	#endif	
	
	//Calculate the chances for the lanes to appear
	var totalLanes:int = 0;
	var totalLanesIndex:int = 0;
	
	//Calculate the total number of drops with their chances
	for ( index = 0 ; index < lanes.Length ; index++ )
	{
		totalLanes += lanes[index].laneChance;
	}
	
	//Create a new list of the objects that can be dropped
	lanesList = new Lane[totalLanes];
	
	//Go through the list again and fill out each type of drop based on its drop chance
	for ( index = 0 ; index < lanes.Length ; index++ )
	{
		var laneChanceCount:int = 0;
		
		while ( laneChanceCount < lanes[index].laneChance )
		{
			lanesList[totalLanesIndex] = lanes[index];
			
			laneChanceCount++;
			
			totalLanesIndex++;
		}
	}
	
	//Calculate the chances for the objects to drop
	var totalDrops:int = 0;
	var totalDropsIndex:int = 0;
	
	//Calculate the total number of drops with their chances
	for ( index = 0 ; index < objectDrops.Length ; index++ )
	{
		totalDrops += objectDrops[index].dropChance;
	}
	
	//Create a new list of the objects that can be dropped
	objectDropList = new Transform[totalDrops];
	
	//Go through the list again and fill out each type of drop based on its drop chance
	for ( index = 0 ; index < objectDrops.Length ; index++ )
	{
		var dropChanceCount:int = 0;
		
		while ( dropChanceCount < objectDrops[index].dropChance )
		{
			objectDropList[totalDropsIndex] = objectDrops[index].droppedObject;
			
			dropChanceCount++;
			
			totalDropsIndex++;
		}
	}
	
	//Get the currently selected player from PlayerPrefs
	currentPlayer = PlayerPrefs.GetInt("CurrentPlayer", currentPlayer);
	
	//Set the current player object
	SetPlayer(currentPlayer);
	
	//If the player object is not already assigned, Assign it from the "Player" tag
	if ( cameraObject == null )    cameraObject = GameObject.FindGameObjectWithTag("MainCamera").transform;
	
	//Create a few lanes at the start of the game
	if ( lanesList.Length > 0 )    
	{
		// Count the number of lanes to create at the start of the game
		for ( index = 0 ; index < precreateLanes ; index++ )
		{
			// Create a lane only if we have an endless game, or (in case we have a victory condition) as long as we didn't reach the number of lanes to victory
			if ( victoryLane == null || (victoryLane && lanesToVictory > 0 && lanesCreated <= lanesToVictory) )    CreateLane();
		}
	}
	
	//Go through all the powerups and reset their timers
	for ( index = 0 ; index < powerups.Length ; index++ )
	{
		//Set the maximum duration of the powerup
		powerups[index].durationMax = powerups[index].duration;
		
		//Reset the duration counter
		powerups[index].duration = 0;
		
		//Deactivate the icon of the powerup
		powerups[index].icon.gameObject.SetActive(false);
	}
	
	//If swipe controls are on, deactivate button controls
	if ( swipeControls == true && moveButtonsObject )    moveButtonsObject.gameObject.SetActive(false);
	
	//Register the current death line position
	if ( deathLineObject )    deathLineTargetPosX = deathLineObject.position.x;
	
	//Pause the game at the start
	Pause();
	
	// These warnings appear if you set one of the attributes needed for a win condition in a randomly generated level, but don't set the rest
	if ( victoryLane && lanesToVictory <= 0 )    Debug.LogWarning("If you want the victory lane to appear you must set the number of lanes to victory higher than 0");
	if ( victoryLane == null && lanesToVictory > 0 )    Debug.LogWarning("You must assign a victory lane which will appear after you passed the number of lanes to victory", victoryLane);
	if ( victoryLane && lanesToVictory > 0 && victoryCanvas == null )    Debug.LogWarning("You must set a victory canvas from the scene that will appear when you win the game ( similar to how the game over canvas is set )");
}

function Update() 
{
	//If the game is over, listen for the Restart and MainMenu buttons
	if ( isGameOver == true )
	{
		//The jump button restarts the game
		if ( Input.GetButtonDown(confirmButton) )
		{
			Restart();
		}
		
		//The pause button goes to the main menu
		if ( Input.GetButtonDown(pauseButton) )
		{
			MainMenu();
		}
	}
	else
	{
		//Toggle pause/unpause in the game
		if ( Input.GetButtonDown(pauseButton) )
		{
			if ( isPaused == true )    Unpause();
			else    Pause();
		}
		
		//Using swipe controls to move the player
		if ( swipeControls == true )
		{
			if ( swipeTimeoutCount > 0 )    swipeTimeoutCount -= Time.deltaTime;
			
			//Check touches on the screen
			for ( var touch:Touch in Input.touches )
	        {
	            //Check the touch position at the beginning
	            if ( touch.phase == TouchPhase.Began )
	            {
	                swipeStart = touch.position;
	                swipeEnd = touch.position;
	                
	                swipeTimeoutCount = swipeTimeout;
	            }
	           	
	           	//Check swiping motion
	            if ( touch.phase == TouchPhase.Moved )
	            {
	                swipeEnd = touch.position;
	            }
	           
	            //Check the touch position at the end, and move the player accordingly
	            if( touch.phase == TouchPhase.Ended && swipeTimeoutCount > 0 )
	            {
	                if( (swipeStart.x - swipeEnd.x) > swipeDistance && (swipeStart.y - swipeEnd.y) < -swipeDistance ) //Swipe left
	                {
	                    MovePlayer("left");
	                }
	                else if((swipeStart.x - swipeEnd.x) < -swipeDistance && (swipeStart.y - swipeEnd.y) > swipeDistance ) //Swipe right
	                {
	                    MovePlayer("right");
	                }
	                else if((swipeStart.y - swipeEnd.y) < -swipeDistance && (swipeStart.x - swipeEnd.x) < -swipeDistance ) //Swipe up
	                {
	                    MovePlayer("forward");
	                }
	                else if((swipeStart.y - swipeEnd.y) > swipeDistance && (swipeStart.x - swipeEnd.x) > swipeDistance ) //Swipe down
	                {
	                    MovePlayer("backward");
	              	}
	            }
			}
		}
	}
	
	//If the camera moved forward enough, create another lane
	if ( lanesList.Length > 0 && nextLanePosition - cameraObject.position.x < precreateLanes )
	{ 
		// Create a lane only if we have an endless game, or (in case we have a victory condition) as long as we didn't reach the number of lanes to victory
		if ( victoryLane == null || (victoryLane && lanesToVictory > 0 && lanesCreated <= lanesToVictory) )    CreateLane();
	}
	
	if ( cameraObject )
	{
		//Make the camera chase the player in all directions
		if ( playerObjects[currentPlayer] && playerObjects[currentPlayer].gameObject.activeSelf == true )    
		{
			cameraObject.position.x = Mathf.Lerp( cameraObject.position.x, playerObjects[currentPlayer].position.x, Time.deltaTime * 3);
			cameraObject.position.z = Mathf.Lerp( cameraObject.position.z, playerObjects[currentPlayer].position.z, Time.deltaTime * 3);
		}
		else if ( respawnObject && respawnObject.gameObject.activeSelf == true )
		{
			cameraObject.position.x = Mathf.Lerp( cameraObject.position.x, respawnObject.position.x, Time.deltaTime * 3);
			cameraObject.position.z = Mathf.Lerp( cameraObject.position.z, respawnObject.position.z, Time.deltaTime * 3);		
		}
		
		//Make the death line chase the player forward only
		if ( deathLineObject )    
		{
			if ( isGameOver == false )    deathLineTargetPosX += deathLineSpeed * Time.deltaTime;
			
			if ( cameraObject.position.x > deathLineTargetPosX )    deathLineTargetPosX = cameraObject.position.x;
			
			deathLineObject.position.x = Mathf.Lerp( deathLineObject.position.x, deathLineTargetPosX, Time.deltaTime * 0.5 );
		}
	}
}

//This function creates a lane, sometimes reversing the paths of the moving objects in it
function CreateLane()
{
	// If we have a victory lane and we passed the needed number of lanes, create the victory lane.
	if ( victoryLane && lanesToVictory > 0 && lanesCreated >= lanesToVictory )
	{
		Instantiate( victoryLane, Vector3(nextLanePosition,0,0), Quaternion.identity);
	}
	else //Othewise, create a random lane from the list of available lanes
	{
		//Choose a random lane from the list
		var randomLane:int = Mathf.Floor(Random.Range(0, lanesList.Length));
		
		//Create a random lane from the list of available lanes
		var newLane = Instantiate( lanesList[randomLane].laneObject, Vector3(nextLanePosition,0,0), Quaternion.identity);
		
		if ( Random.value < lanesList[randomLane].itemChance )
		{ 
			if ( dropInSequence == true )    
			{
				if ( currentDrop < objectDropList.Length - 1 )    currentDrop++;
				else    currentDrop = 0;
			}
			else
			{
				currentDrop = Mathf.FloorToInt(Random.Range(0, objectDropList.Length));
			}
			
			var newObject = Instantiate( objectDropList[currentDrop] );
			
			newObject.position = newLane.position;
			
			newObject.position.z += Mathf.Round(Random.Range(-objectDropOffset,objectDropOffset));
		}
		
		//Go to the next lane position, based on the width of the current lane
		nextLanePosition += lanesList[randomLane].laneWidth;
	}
	
	lanesCreated++;
}

//This function changes the score of the player
function ChangeScore( changeValue:int )
{
	//Change the score
	score += changeValue * scoreMultiplier;
	
	//Update the score text
	if ( scoreText )    scoreText.GetComponent(Text).text = score.ToString();
	
	//Increase the counter to the next level
	increaseCount += changeValue;
	
	//If we reached the required number of points, level up!
	if ( increaseCount >= levelUpEveryCoins )
	{
		increaseCount -= levelUpEveryCoins;
		
		LevelUp();
	}
}

//This function sets the score multiplier ( When the player picks up coins he gets 1X,2X,etc score )
function SetScoreMultiplier( setValue:int )
{
	// Set the score multiplier
	scoreMultiplier = setValue;
}

//This function levels up, and increases the difficulty of the game
function LevelUp()
{
	//Increase the speed of the death line ( the moving fog ), but never above the maximum allowed value
	if ( deathLineSpeed + deathLineSpeedIncrease < deathLineSpeedMax )    deathLineSpeed += deathLineSpeedIncrease;	
	
	//If there is a source and a sound, play it from the source
	if ( soundSourceTag != String.Empty && soundLevelUp )    GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent.<AudioSource>().PlayOneShot(soundLevelUp);
}

//This function pauses the game
function Pause()
{
	isPaused = true;
	
	//Set timescale to 0, preventing anything from moving
	Time.timeScale = 0;
	
	//Show the pause screen and hide the game screen
	if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(true);
	if ( gameCanvas )    gameCanvas.gameObject.SetActive(false);
}

function Unpause()
{
	isPaused = false;
	
	//Set timescale back to the current game speed
	Time.timeScale = gameSpeed;
	
	//Hide the pause screen and show the game screen
	if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(false);
	if ( gameCanvas )    gameCanvas.gameObject.SetActive(true);
}

//This function changes the number of lives the player has
function ChangeLives( changeValue:int )
{
	//Change the number of lives the player has
	lives += changeValue;
	
	//Update the lives text
	if ( livesText )    livesText.GetComponent(Text).text = lives.ToString();
	
	//If we ran out of lives, run the game over function
	if ( lives <= 0 )    GameOver(0.5);
	else if ( playerObjects[currentPlayer] && changeValue < 0 )    
	{
		// Stop all powerups
		if ( stopPowerupsOnDeath == true )
		{
			//Go through all the powerups and nullify their timers, making them end
			for ( index = 0 ; index < powerups.Length ; index++ )
			{
				//Set the duration of the powerup to 0
				powerups[index].duration = 0;
			}
		}
		
		//Show the respawn object, allowing it to move
		if ( respawnObject )    
		{
			respawnObject.gameObject.SetActive(true);
			
			respawnObject.position = playerObjects[currentPlayer].position;
			
			respawnObject.rotation = playerObjects[currentPlayer].rotation;
			
			respawnObject.SendMessage("Spawn");
		}
		
		yield WaitForSeconds(respawnTime);
		
		//Activate the player object
		if ( playerObjects[currentPlayer].gameObject.activeSelf == false )
		{
			playerObjects[currentPlayer].gameObject.SetActive(true);
			
			//Respawn the player object
			playerObjects[currentPlayer].SendMessage("Spawn");
			
			//If there is a respawn object, place the player at its position, and hide the respawn object
			if ( respawnObject )   
			{
				targetPosition = respawnObject.position;
				
				playerObjects[currentPlayer].position = targetPosition;
				
				playerObjects[currentPlayer].rotation = respawnObject.rotation;
							
				respawnObject.gameObject.SetActive(false);
			}
		}
	}
}

//This function handles the game over event
function GameOver( delay:float )
{
	//Go through all the powerups and nullify their timers, making them end
	for ( index = 0 ; index < powerups.Length ; index++ )
	{
		//Set the duration of the powerup to 0
		powerups[index].duration = 0;
	}
	
	yield WaitForSeconds(delay);
	
	isGameOver = true;
	
	//Remove the pause and game screens
	if ( pauseCanvas )    Destroy(pauseCanvas.gameObject);
	if ( gameCanvas )    Destroy(gameCanvas.gameObject);
	
	//Get the number of coins we have
	var totalCoins:int = PlayerPrefs.GetInt( coinsPlayerPrefs, 0);
	
	//Add to the number of coins we collected in this game
	totalCoins += score;
	
	//Record the number of coins we have
	PlayerPrefs.SetInt( coinsPlayerPrefs, totalCoins);
	
	//Show the game over screen
	if ( gameOverCanvas )    
	{
		//Show the game over screen
		gameOverCanvas.gameObject.SetActive(true);
		
		//Write the score text
		gameOverCanvas.Find("TextScore").GetComponent(Text).text = "SCORE " + score.ToString();
		
		//Check if we got a high score
		if ( score > highScore )    
		{
			highScore = score;
			
			//Register the new high score
			#if UNITY_5_3
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_HighScore", score);
			#else
			PlayerPrefs.SetInt(Application.loadedLevelName + "_HighScore", score);
			#endif
		}
		
		//Write the high sscore text
		gameOverCanvas.Find("TextHighScore").GetComponent(Text).text = "HIGH SCORE " + highScore.ToString();
	}
	
	// If there is a source and a sound, play it from the source
	if( soundSourceTag != String.Empty && soundGameOver )    GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent.<AudioSource>().PlayOneShot(soundGameOver);
	
	// If there is an ad controller, Try to run it
	if ( adController )    adController.SendMessage("TryAd");
}

//This function handles the game over event
function Victory( delay:float )
{
	//Go through all the powerups and nullify their timers, making them end
	for ( index = 0 ; index < powerups.Length ; index++ )
	{
		//Set the duration of the powerup to 0
		powerups[index].duration = 0;
	}
	
	//Activate the player object
	playerObjects[currentPlayer].gameObject.SetActive(true);

	//If there is a respawn object, place the player at its position, and hide the respawn object
	if ( respawnObject && respawnObject.gameObject.activeSelf == true )
	{
		targetPosition = respawnObject.position;
		
		playerObjects[currentPlayer].position = targetPosition;
		
		playerObjects[currentPlayer].rotation = respawnObject.rotation;
		
		respawnObject.gameObject.SetActive(false);
	}

	// Call the victory function on the player
	if ( playerObjects[currentPlayer] )    playerObjects[currentPlayer].SendMessage("Victory");
	
	yield WaitForSeconds(delay);
	
	isGameOver = true;
	
	//Remove the pause and game screens
	if ( pauseCanvas )    Destroy(pauseCanvas.gameObject);
	if ( gameCanvas )    Destroy(gameCanvas.gameObject);
	
	//Get the number of coins we have
	var totalCoins:int = PlayerPrefs.GetInt( coinsPlayerPrefs, 0);
	
	//Add to the number of coins we collected in this game
	totalCoins += score;
	
	//Record the number of coins we have
	PlayerPrefs.SetInt( coinsPlayerPrefs, totalCoins);
	
	//Show the game over screen
	if ( victoryCanvas )    
	{
		//Show the game over screen
		victoryCanvas.gameObject.SetActive(true);
		
		//Write the score text
		victoryCanvas.Find("TextScore").GetComponent(Text).text = "SCORE " + score.ToString();
		
		//Check if we got a high score
		if ( score > highScore )    
		{
			highScore = score;
			
			//Register the new high score
			#if UNITY_5_3
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_HighScore", score);
			#else
			PlayerPrefs.SetInt(Application.loadedLevelName + "_HighScore", score);
			#endif
		}
		
		//Write the high sscore text
		victoryCanvas.Find("TextHighScore").GetComponent(Text).text = "HIGH SCORE " + highScore.ToString();
	}
	
	// If there is a source and a sound, play it from the source
	if( soundSourceTag != String.Empty && soundVictory )    GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent.<AudioSource>().PlayOneShot(soundVictory);
	
	// If there is an ad controller, Try to run it
	if ( adController )    adController.SendMessage("TryAd");
}

//This function restarts the current level
function Restart()
{
	#if UNITY_5_3
	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	#else
	Application.LoadLevel(Application.loadedLevelName);
	#endif
}

//This function returns to the Main Menu
function MainMenu()
{
	#if UNITY_5_3
	SceneManager.LoadScene(mainMenuLevelName);
	#else
	Application.LoadLevel(mainMenuLevelName);
	#endif
}

//This function activates the selected player, while deactivating all the others
function SetPlayer( playerNumber:int )
{
	//Hide the respawn object
	if ( respawnObject )    respawnObject.gameObject.SetActive(false);
	
	//Go through all the players, and hide each one except the current player
	for(index = 0; index < playerObjects.Length; index++)
	{
		if ( index != playerNumber )    playerObjects[index].gameObject.SetActive(false);
		else    playerObjects[index].gameObject.SetActive(true);
	}
}

//This function sends a move command to the current player
function MovePlayer( moveDirection:String )
{
	if ( playerObjects[currentPlayer] && playerObjects[currentPlayer].gameObject.activeSelf == true )    playerObjects[currentPlayer].SendMessage("Move", moveDirection);
	else if ( respawnObject && respawnObject.gameObject.activeSelf == true )    respawnObject.SendMessage("Move", moveDirection);
}

//This function changes the speed of the game ( Time.timeScale )
function SetGameSpeed( setValue:float )
{
	gameSpeed = setValue;

	//Set the overall speed of the scene
	Time.timeScale = gameSpeed;

	//Toggle between a low pitch for the slowmotion time, and normal pitch when the slowmotion ends
	if ( GetComponent.<AudioSource>().pitch == 1 )    GetComponent.<AudioSource>().pitch = 0.5f;
	else    GetComponent.<AudioSource>().pitch = 1;
}

//This function changes the speed of the player
function SetPlayerSpeed( setValue:float )
{
	if ( playerObjects[currentPlayer] && playerObjects[currentPlayer].gameObject.activeSelf == true )    playerObjects[currentPlayer].SendMessage("SetPlayerSpeed", setValue);
	else if ( respawnObject && respawnObject.gameObject.activeSelf == true )    respawnObject.SendMessage("SetPlayerSpeed", setValue);
}

//This function kills the player
function KillPlayer( killType:int )
{
	if ( playerObjects[currentPlayer] && playerObjects[currentPlayer].gameObject.activeSelf == true )     playerObjects[currentPlayer].gameObject.SendMessage("Die", killType);
}

//This function resets the position of the death line to the camera
function ResetDeathLine()
{
	if ( deathLineObject && cameraObject )   deathLineTargetPosX = cameraObject.position.x;
}

//This function activates a power up from a list of available power ups
function ActivatePowerup( powerupIndex:int )
{
	//If there is already a similar powerup running, refill its duration timer
	if ( powerups[powerupIndex].duration > 0 )
	{
		//Refill the duration of the powerup to maximum
		powerups[powerupIndex].duration = powerups[powerupIndex].durationMax;
	}
	else //Otherwise, activate the power up functions
	{
		//Activate the powerup icon
		powerups[powerupIndex].icon.gameObject.SetActive(true);

		//Run up to two start functions from the gamecontroller
		if ( powerups[powerupIndex].startFunctionA != String.Empty )    SendMessage(powerups[powerupIndex].startFunctionA, powerups[powerupIndex].startParamaterA);
		if ( powerups[powerupIndex].startFunctionB != String.Empty )    SendMessage(powerups[powerupIndex].startFunctionB, powerups[powerupIndex].startParamaterB);

		//Fill the duration timer to maximum
		powerups[powerupIndex].duration = powerups[powerupIndex].durationMax;
		
		//Count down the duration of the powerup
		while ( powerups[powerupIndex].duration > 0 )
		{
			yield WaitForSeconds(Time.deltaTime);

			powerups[powerupIndex].duration -= Time.deltaTime;

			//Animate the powerup timer graphic using fill amount
			powerups[powerupIndex].icon.Find("FillAmount").GetComponent(Image).fillAmount = powerups[powerupIndex].duration/powerups[powerupIndex].durationMax;
		}

		//Run up to two end functions from the gamecontroller
		if ( powerups[powerupIndex].endFunctionA != String.Empty )    SendMessage(powerups[powerupIndex].endFunctionA, powerups[powerupIndex].endParamaterA);
		if ( powerups[powerupIndex].endFunctionB != String.Empty )    SendMessage(powerups[powerupIndex].endFunctionB, powerups[powerupIndex].endParamaterB);

		//Deactivate the powerup icon
		powerups[powerupIndex].icon.gameObject.SetActive(false);
	}
}

function OnDrawGizmos()
{
	//Draw the position of the next lane in red
	Gizmos.color = Color.red;
	Gizmos.DrawLine( Vector3(nextLanePosition,0,-10), Vector3(nextLanePosition,0,10) );
}
