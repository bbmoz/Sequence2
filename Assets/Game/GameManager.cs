using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// grid of nodes
	public GameObject[,] nodeGrid;
	public int nodeGridSizeX;
	public int nodeGridSizeY;
	public GameObject nodeObj;

	// node min, max range
	public int nodeMin;
	public int nodeMax;

	// node textures aka numbers
	public NodeTexture[] nodeTextures;

	// store dragging nodes
	public ArrayList nodesDragged;

	// singleton
	public static GameManager gm_Instance = null;
	public static GameManager Get() {
		if (!gm_Instance) {
			gm_Instance = (GameManager)FindObjectOfType(typeof(GameManager));
		}
		return gm_Instance;
	}

	// side-most position of grid
	public float sideX;
	public float sideY;

	// Use this for initialization
	void Start () {
		// grid should be generated at origin
		nodeGrid = new GameObject[nodeGridSizeX, nodeGridSizeY];
		nodesDragged = new ArrayList();
		sideX = (float)nodeGridSizeX / 2.0f;
		sideY = (float)nodeGridSizeY / 2.0f;

		// spawn nodes
		for (int x=0; x<nodeGridSizeX; x++) {
			for (int y=0; y<nodeGridSizeY; y++) {
				nodeGrid[x,y] = spawnNode(x-sideX, y-sideY);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Spawns the node.
	/// </summary>
	private GameObject spawnNode(float x, float y) {
		nodeObj.GetComponent<NodeManager>().posX = x;
		nodeObj.GetComponent<NodeManager>().posY = y;
		nodeObj.GetComponent<Transform>().position = new Vector3(x, y, 0.0f);
		return Instantiate(nodeObj) as GameObject;
	}
}
