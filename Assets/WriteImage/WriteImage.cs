using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class WriteImage : MonoBehaviour 
{
	Texture2D texture;
	public SpriteRenderer bg;
	public Color color1;
	public Color color2;
	float worldWidth, worldHeight;
	void Start () 
	{
		texture = new Texture2D(400, 400, TextureFormat.ARGB32, false);
		worldWidth = texture.width * 0.01f;
		worldHeight = texture.height * 0.01f;
//		texture.SetPixel(0, 0, Color.red);
//		texture.SetPixel(1, 0, Color.blue);
//		texture.SetPixel(0, 1, Color.green);
//		texture.SetPixel(1, 1, Color.black);
		for (int x = 0; x < texture.width; x ++)
		{
			for(int y = 0; y < texture.height; y ++)
			{
				texture.SetPixel(x, y, Color.white);
			}
		}
		texture.Apply();

		Sprite sprite = new Sprite();
		sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0,0));
		bg.sprite = sprite;
	}

//	void Set()
//	{
//		int height = Random.Range(0, texture.height);
//		for (int x = 0; x < texture.width; x ++)
//		{
//			for(int y = 0; y < texture.height; y ++)
//			{
//				if (y >= height)
//				{
//					texture.SetPixel(x, y, color1);
//				}
//				else
//				{
//					texture.SetPixel(x, y, color2);
//				}
//			}
//		}
//		texture.SetPixel(0,410,color1);
//		texture.Apply();
//		width = texture.width * 0.01f;
//		height = texture.height * 0.01f;
//	}


	void Update()
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if (pos.x <= bg.transform.position.x + worldWidth &&
			pos.x >= bg.transform.position.x &&
			pos.y <= bg.transform.position.y + worldHeight &&
			pos.y >= bg.transform.position.y)
		{
			int x = (int)(pos.x * 100);
			int y = (int)(pos.y * 100);
			for (int i = x - 10; i < x + 10; i++)
			{
				for (int j = y - 10; j < y + 10; j++)
				{
					texture.SetPixel(i, j, Color.black);
				}
			}
			texture.Apply();
		}
	}
}
