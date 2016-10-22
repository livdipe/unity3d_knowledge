using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Main : MonoBehaviour 
{
	public GameObject prefab;

	float startX = -100;
	float startY = 180;
	float stepX = 11;
	float stepY = -11;

	int row = 30;
	int col = 20;
	public Transform trParent;
	Color cAlive = Color.green;
	Color cDie = Color.white;
	Image[,] images;
	int[,] states;
	int[,] cacheStates;
	void Start()
	{
		InitImages();
		InitStates();
		InitColor();

		UpdateState();
	}

	void Update()
	{
		if (Time.frameCount % 20 == 0)
		{
			UpdateState();
		}
	}

	void InitImages()
	{
		images = new Image[row, col];
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				obj.SetActive(true);
				Transform tr = obj.transform;
				tr.SetParent(trParent, true);
				tr.localPosition = new Vector3(startX + stepX * j, startY + stepY * i);
				images[i,j] = obj.GetComponent<Image>();
			}
		}
	}

	void InitStates()
	{
		states = new int[row, col];
		cacheStates = new int[row, col];
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				if (Random.value < 0.1f)
				{
					cacheStates[i, j] = states[i, j] = 1;
				}
				else
				{
					cacheStates[i,j] = states[i, j] = 0;
				}
			}
		}
	}

	void InitColor()
	{
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				SetColor(i, j);
			}
		}
	}

	void SetColor(int i, int j)
	{
		images[i,j].color = states[i,j] == 1 ? cAlive : cDie;
	}


	void UpdateState()
	{
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				cacheStates[i,j] = states[i,j];
				int cnt = GetAroundLife(i, j);
				// 当前为存活状态
				if (states[i, j] == 1)
				{
					if (cnt < 2 || cnt > 3)
					{
						cacheStates[i, j] = 0;
					}
				}
				else
				{
					if (cnt >= 3)
					{
						cacheStates[i, j] = 1;
					}
				}
//				Debug.LogError(i +  "   " + j +  "   " +  cnt);
			}
		}
		for (int i = 0; i < row; i++)
		{
			for (int j = 0; j < col; j++)
			{
				states[i,j] = cacheStates[i,j] ;
				SetColor(i, j);
			}
		}
	}

	// 获取周围存活的细胞数量
	int GetAroundLife(int r, int c)
	{
		int cnt = 0;
		// 左上
		if (r - 1 >=0 && c - 1 >= 0)
		{
			cnt += states[r - 1, c - 1];
		}
		// 上
		if (r - 1 >=0)
		{
			cnt += states[r - 1, c];
		}
		// 右上
		if (r - 1 >=0 && c + 1 < col)
		{
			cnt += states[r - 1, c + 1];
		}
		// 左
		if (c - 1 >= 0)
		{
			cnt += states[r, c-1];
		}
		// 右
		if (c + 1 < col)
		{
			cnt += states[r, c + 1];
		}
		// 左下
		if (r + 1 < row && c - 1 >= 0)
		{
			cnt += states[r + 1, c - 1];
		}
		// 上
		if (r + 1 < row)
		{
			cnt += states[r + 1, c];
		}
		// 右上
		if (r + 1 < row && c + 1 < col)
		{
			cnt += states[r + 1, c + 1];
		}
		return cnt;
	}
}
