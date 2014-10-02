using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	// grid of nodes
	public GameObject[,] nodeGrid;
	public int nodeGridSizeX;
	public int nodeGridSizeY;
	public GameObject nodeObj;

	// Score variables
	public GUIText scoreText;
	private int score;

	// node min, max range
	public int nodeMin;
	public int nodeMax;

	// node textures aka numbers
	public NodeTexture[] nodeTextures;

	// store dragging nodes
	public ArrayList nodesDragged;

	// line renderer
	public LineRenderer lineRenderer;
	public CapsuleCollider lineRendererCollider;

	// singleton
	public static GameManager gm_Instance = null;
	public static GameManager Get() {
		if (!gm_Instance) {
			gm_Instance = (GameManager)FindObjectOfType(typeof(GameManager));
		}
		return gm_Instance;
	}

	// side-most position of grid
	[HideInInspector]
	public float sideX;
	[HideInInspector]
	public float sideY;

	// shader for tree zapper
	[HideInInspector]
	public Shader zapShader;

	// Use this for initialization
	void Start () {
		// grid should be generated at origin
		nodeGrid = new GameObject[nodeGridSizeX, nodeGridSizeY];
		nodesDragged = new ArrayList();
		sideX = (float)nodeGridSizeX / 2.0f;
		sideY = (float)nodeGridSizeY / 2.0f;
		zapShader = Shader.Find("Reflective/Specular");

		// initialize score
		score = 0;
		UpdateScore();

		// spawn nodes
		for (int x=0; x<nodeGridSizeX; x++) {
			for (int y=0; y<nodeGridSizeY; y++) {
				nodeGrid[x,y] = spawnNode(x-sideX, y-sideY);
			}
		}

		// set line renderer width
		lineRenderer.SetWidth(0.2f,0.2f);
		lineRendererCollider = lineRenderer.gameObject.AddComponent("CapsuleCollider") as CapsuleCollider;
		lineRendererCollider.radius = 0.2f/2.0f;
		lineRendererCollider.center = Vector3.zero;
		lineRendererCollider.direction = 2; // z-axis for easier lookat orientation
	}
	
	// Update is called once per frame
	void Update () {
		// tree zapper
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			treeZap(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			treeZap(2);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			treeZap(3);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			treeZap(4);
		}
		if (Input.GetKeyDown(KeyCode.Alpha5)) {
			treeZap(5);
		}
		if (Input.GetKeyDown(KeyCode.Alpha6)) {
			treeZap(6);
		}
	}

	// initial tree zap
	private void treeZap(int number) {
		int nodeNumber = 0;
		int neighborNumOne,neighborNumTwo,neighborNumThree,neighborNumFour,neighborNumFive,neighborNumSix,neighborNumSeven,neighborNumEight;
		for (int i=0; i<nodeGridSizeX; i++) {
			for (int j=0; j<nodeGridSizeY; j++) {
				nodeNumber = nodeGrid[i,j].GetComponent<NodeManager>().number;
				if (nodeNumber == number) {
					nodeGrid[i,j].renderer.material.shader = zapShader;
					// get valid neighbors
					neighborNumOne=neighborNumTwo=neighborNumThree=neighborNumFour=neighborNumFive=neighborNumSix=neighborNumSeven=neighborNumEight = 0;
					try {
						neighborNumOne = nodeGrid[i-1,j-1].GetComponent<NodeManager>().number;
						if (neighborNumOne == nodeNumber-1 || neighborNumOne == nodeNumber+1) {
							treeZapRecurse(i-1, j-1);
						}
					} catch {}
					try {
						neighborNumTwo = nodeGrid[i-1,j].GetComponent<NodeManager>().number;
						if (neighborNumTwo == nodeNumber-1 || neighborNumTwo == nodeNumber+1) {
							treeZapRecurse(i-1, j);
						}
					} catch {}
					try {
						neighborNumThree = nodeGrid[i-1,j+1].GetComponent<NodeManager>().number;
						if (neighborNumThree == nodeNumber-1 || neighborNumThree == nodeNumber+1) {
							treeZapRecurse(i-1, j+1);
						}
					} catch {}
					try {
						neighborNumFour = nodeGrid[i,j-1].GetComponent<NodeManager>().number;
						if (neighborNumFour == nodeNumber-1 || neighborNumFour == nodeNumber+1) {
							treeZapRecurse(i, j-1);
						}
					} catch {}
					try {
						neighborNumFive = nodeGrid[i+1,j-1].GetComponent<NodeManager>().number;
						if (neighborNumFive == nodeNumber-1 || neighborNumFive == nodeNumber+1) {
							treeZapRecurse(i+1, j-1);
						}
					} catch {}
					try {
						neighborNumSix = nodeGrid[i+1,j].GetComponent<NodeManager>().number;
						if (neighborNumSix == nodeNumber-1 || neighborNumSix == nodeNumber+1) {
							treeZapRecurse(i+1, j);
						}
					} catch {}
					try {
						neighborNumSeven = nodeGrid[i,j+1].GetComponent<NodeManager>().number;
						if (neighborNumSeven == nodeNumber-1 || neighborNumSeven == nodeNumber+1) {
							treeZapRecurse(i, j+1);
						}
					} catch {}
					try {
						neighborNumEight = nodeGrid[i+1,j+1].GetComponent<NodeManager>().number;
						if (neighborNumEight == nodeNumber-1 || neighborNumEight == nodeNumber+1) {
							treeZapRecurse(i+1, j+1);
						}
					} catch {}
				}
			}
		}
	}

	// tree zap recursion
	private void treeZapRecurse(int i, int j) {
		int nodeNumber = nodeGrid[i,j].GetComponent<NodeManager>().number;
		nodeGrid[i,j].renderer.material.shader = zapShader;

		// get valid neighbors
		int neighborNumOne,neighborNumTwo,neighborNumThree,neighborNumFour,neighborNumFive,neighborNumSix,neighborNumSeven,neighborNumEight;
		neighborNumOne=neighborNumTwo=neighborNumThree=neighborNumFour=neighborNumFive=neighborNumSix=neighborNumSeven=neighborNumEight = 0;
		try {
			if (nodeGrid[i-1,j-1].renderer.material.shader.name != zapShader.name) {
				neighborNumOne = nodeGrid[i-1,j-1].GetComponent<NodeManager>().number;
				if (neighborNumOne == nodeNumber-1 || neighborNumOne == nodeNumber+1) {
					treeZapRecurse(i-1, j-1);
				}
			}
		} catch {}
		try {
			if (nodeGrid[i-1,j].renderer.material.shader.name != zapShader.name) {
				neighborNumTwo = nodeGrid[i-1,j].GetComponent<NodeManager>().number;
				if (neighborNumTwo == nodeNumber-1 || neighborNumTwo == nodeNumber+1) {
					treeZapRecurse(i-1, j);
				}
			}
		} catch {}
		try {
			if (nodeGrid[i-1,j+1].renderer.material.shader.name != zapShader.name) {
				neighborNumThree = nodeGrid[i-1,j+1].GetComponent<NodeManager>().number;
				if (neighborNumThree == nodeNumber-1 || neighborNumThree == nodeNumber+1) {
					treeZapRecurse(i-1, j+1);
				}
			}
		} catch {}
		try {
			if (nodeGrid[i,j-1].renderer.material.shader.name != zapShader.name) {
				neighborNumFour = nodeGrid[i,j-1].GetComponent<NodeManager>().number;
				if (neighborNumFour == nodeNumber-1 || neighborNumFour == nodeNumber+1) {
					treeZapRecurse(i, j-1);
				}
			}
		} catch {}
		try {
			if (nodeGrid[i+1,j-1].renderer.material.shader.name != zapShader.name) {
				neighborNumFive = nodeGrid[i+1,j-1].GetComponent<NodeManager>().number;
				if (neighborNumFive == nodeNumber-1 || neighborNumFive == nodeNumber+1) {
					treeZapRecurse(i+1, j-1);
				}
			}
		} catch {}
		try {
			if (nodeGrid[i+1,j].renderer.material.shader.name != zapShader.name) {
				neighborNumSix = nodeGrid[i+1,j].GetComponent<NodeManager>().number;
				if (neighborNumSix == nodeNumber-1 || neighborNumSix == nodeNumber+1) {
					treeZapRecurse(i+1, j);
				}
			}
		} catch {}
		try {
			if (nodeGrid[i,j+1].renderer.material.shader.name != zapShader.name) {
				neighborNumSeven = nodeGrid[i,j+1].GetComponent<NodeManager>().number;
				if (neighborNumSeven == nodeNumber-1 || neighborNumSeven == nodeNumber+1) {
					treeZapRecurse(i, j+1);
				}
			}
		} catch {}
		try {
			if (nodeGrid[i+1,j+1].renderer.material.shader.name != zapShader.name) {
				neighborNumEight = nodeGrid[i+1,j+1].GetComponent<NodeManager>().number;
				if (neighborNumEight == nodeNumber-1 || neighborNumEight == nodeNumber+1) {
					treeZapRecurse(i+1, j+1);
				}
			}
		} catch {}
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

	// Increases the score value
	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		if (score < 0) { score = 0; }
		UpdateScore();
	}
	// Updates the score gui text
	void UpdateScore()
	{
		scoreText.text = "Score: " + score;
	}
}
