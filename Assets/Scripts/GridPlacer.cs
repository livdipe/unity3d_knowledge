using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GridPlacer : MonoBehaviour 
{
	public GameObject prefab;
	public GameObject tagPrefab;
	public Transform trPlane;
	Transform trDrag;
	Image imgDrag;
	int rowNum = 18;
	int colNum = 18;
	int size = 1;
	int space = 2;
	List<CNode> nodes = new List<CNode>();
	List<CNode> blocks = new List<CNode>();
	List<CPlane> planes = new List<CPlane>();
	List<CPlane> planesOrg = new List<CPlane>();
	bool isGenerated = false;
	Vector2 nodeSize;
	Vector3 leftTopPos;

	void Start () 
	{
		InitPlane();
		InitDrag();
		InitGrid();		
	}

	void InitPlane()
	{
		for (int i = 0; i < rowNum; i++)
		{
			for (int j = 0; j < colNum; j++)
			{
				GameObject obj = Instantiate(prefab) as GameObject;
				obj.name = i.ToString() + "_" + j.ToString();
				obj.transform.SetParent(trPlane);

				CNode node = new CNode(obj, j, i);
				nodes.Add (node);
			}
		}
	}

	void InitDrag()
	{
		nodeSize = nodes[0].obj.GetComponent<RectTransform>().sizeDelta;
		leftTopPos = new Vector3((-Screen.width + nodeSize.x) * 0.5f,
		                         (Screen.height - nodeSize.y) * 0.5f,
		                         0);
		GameObject obj = Instantiate(prefab) as GameObject;
		trDrag = obj.transform;
		trDrag.SetParent(trPlane.parent);
		float w = nodeSize.x * size + (size - 1) * space;
		obj.GetComponent<RectTransform>().sizeDelta = new Vector2(w, w);
		imgDrag = obj.GetComponent<Image>();
	}

	void InitGrid()
	{
		planes.Clear();	
		for (int i = 0; i < nodes.Count; i++)
		{
			ProcessNode(nodes[i]);
		}
		if (!isGenerated)
			isGenerated = true;
	}

	void ProcessNode(CNode node)
	{
		if (node.x + size - 1 >= colNum ||
		    node.y + size - 1 >= rowNum)
			return ;

		for (int x = node.x; x <= node.x + size - 1; x++)
		{
			for (int y = node.y; y <= node.y + size - 1; y++)
			{
				if (IsInBlock(x, y))
					return ;
			}
		}

//		if (size == 2)
		{
			CPlane p = new CPlane();
			p.x1 = node.x;
			p.y1 = node.y;
			p.x2 = node.x + size - 1;
			p.y2 = node.y + size - 1;
			Vector2 pos1 = GetNodePos(p.x1, p.y1);
			Vector2 pos2 = GetNodePos(p.x2, p.y2);
			p.center = new Vector2((pos1.x + pos2.x) * 0.5f, (pos1.y + pos2.y) * 0.5f);

//			GenerateTag(p.center);

			planes.Add (p);
			if (!isGenerated)
				planesOrg.Add (p);
		}
	}

	bool IsInBlock(int x, int y)
	{
		for (int i = 0; i < blocks.Count; i++)
		{
			if (x == blocks[i].x && y == blocks[i].y)
				return true;
		}
		return false;
	}


	Vector2 GetNodePos(int x, int y)
	{
		float w = leftTopPos.x + x * nodeSize.x + x * space;
		float h = leftTopPos.y - y * nodeSize.y - y * space;
		return new Vector2(w, h);
	}

	void GenerateTag(Vector2 pos)
	{
		GameObject obj = Instantiate(tagPrefab) as GameObject;
		obj.transform.SetParent(trPlane.parent);
		obj.transform.localPosition = new Vector3(pos.x, pos.y, 0);
	}


	CPlane GetMinDis(Vector2 pos)
	{
		float min = 1000000;
		CPlane plane = new CPlane();
		for (int i = 0; i < planes.Count; i++)
		{
			float dis = Vector2.Distance(pos, planes[i].center);
			if (dis < min)
			{
				min = dis;
				plane = planes[i];
			}
		}

		return plane;
	}

	CPlane GetMinDisInOrg(Vector2 pos)
	{
		float min = 1000000;
		CPlane plane = new CPlane();
		for (int i = 0; i < planesOrg.Count; i++)
		{
			float dis = Vector2.Distance(pos, planesOrg[i].center);
			if (dis < min)
			{
				min = dis;
				plane = planesOrg[i];
			}
		}
		
		return plane;
	}

	CNode GetNodeByXY(int x, int y)
	{
		for (int i = 0; i < nodes.Count; i++)
		{
			if (x == nodes[i].x && y == nodes[i].y)
			{
				return nodes[i];
			}
		}
		return null;
	}

	void Place(CPlane plane)
	{
		for (int x = plane.x1; x <= plane.x2; x++)
		{
			for (int y = plane.y1; y <= plane.y2; y++)
			{
				CNode node = GetNodeByXY(x, y);
				if (node != null)
				{
					node.obj.GetComponent<Image>().color = Color.gray;
					blocks.Add(node);
				}
			}
		}
	}


	void Update ()
	{
		Vector2 pos = new Vector2(Input.mousePosition.x - Screen.width * 0.5f,
		                          Input.mousePosition.y - Screen.height * 0.5f);

		if (planes.Count == 0)
		{
			trDrag.localPosition = GetMinDisInOrg(pos).center;
			imgDrag.color = Color.red;
			return ;
		}

		CPlane plane = GetMinDis(pos);
		trDrag.localPosition = plane.center;
		imgDrag.color = Color.green;

		if (Input.GetMouseButtonDown(0))
		{
			Place(plane);

			InitGrid();
		}
	}
}

class CNode
{
	public GameObject obj;
	public int x;
	public int y;

	public CNode(GameObject _obj, int _x, int _y)
	{
		obj = _obj;
		x = _x;
		y = _y;
	}
}

class CPlane
{
	public int x1;
	public int y1;
	public int x2;
	public int y2;
	public Vector2 center;
}
