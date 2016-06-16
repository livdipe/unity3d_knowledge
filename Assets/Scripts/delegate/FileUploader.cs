using UnityEngine;
using System.Collections;

public class FileUploader : MonoBehaviour 
{
	public delegate void FileUploadedHandler(int progress);
	public event FileUploadedHandler FileUploaded;
	// 如果不加event会存在两个问题:
	// 		1. 如果在主线程中另起一个工作线程，该工作线程则可以将FileProgress委托链置为空:
	// 			f1.FileUploaded = null
	//		    这会导致主线程不能再接收到FileUpdater对象的通知
	//		1. 可以在外部直接调用FileUploaded， 如
	//		    f1.FileUploaded(10);

	int fileProgress = 100;
	float time = 0.2f;
	float timer = 0;
	bool startUpload = false;

	void Start()
	{
		fileProgress = 100;
	}

	public void SetStart()
	{
		startUpload = true;
	}

	void Update()
	{
		if (!startUpload)
		{
			return ;
		}

		if (Time.time > timer && fileProgress > 0)
		{
			timer = Time.time + time;
			//传输代码，省略
			fileProgress--;
			if (FileUploaded != null)
			{
				FileUploaded(fileProgress);
			}
		}
	}
}
