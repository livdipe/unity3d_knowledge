using UnityEngine;
using System.Collections;

public class DelegateTest : MonoBehaviour 
{
	void Start()
	{
		GameObject obj = new GameObject("FileUploader");
		FileUploader fileUploader = obj.AddComponent<FileUploader>();
		fileUploader.FileUploaded += Handler;
		fileUploader.SetStart();
	}

	void Handler(int fileProgress)
	{
		Debug.Log("fileProgress:" + fileProgress);
	}
}
