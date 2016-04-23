#pragma strict

internal var thisTransform:Transform;
internal var cameraObject:Transform;

var distance:float = 20;

function Start() 
{
	thisTransform = transform;
	cameraObject = GameObject.FindGameObjectWithTag("MainCamera").transform;
}

function Update() 
{
	if ( thisTransform.position.x < cameraObject.position.x - distance )    Destroy(gameObject);
}