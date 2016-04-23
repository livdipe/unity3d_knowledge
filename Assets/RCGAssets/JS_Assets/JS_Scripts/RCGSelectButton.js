//This script selects a certain button when this canvas/panel/object is enabled
#pragma strict

import UnityEngine.UI;
import UnityEngine.EventSystems;

//The button that will be selected when this object is activated
var selectedButton:GameObject;

function OnEnable() 
{
	if ( EventSystem.current && selectedButton )    
	{
		// Select the button
		EventSystem.current.SetSelectedGameObject(selectedButton);
	}
}