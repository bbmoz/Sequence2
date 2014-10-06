using UnityEngine;
using System.Collections;

public class NodeManager : MonoBehaviour {
	// position of node
	public float posX;
	public float posY;

	// number of node
	public int number;
	public int nodesRemaining;
	static public int[] remainArray = new int[3];

	// boolean to make sure OnMouseEnter is run once
	[HideInInspector]
	public bool nodeEntered;
	
	// checks if same number consecutive
	private bool sameNum;

	// Use this for initialization
	void Start () {
		randomizeNumber();
		nodeEntered = false;
		sameNum = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Randomizes the number.
	/// </summary>
	/// <returns>The number.</returns>
	private void randomizeNumber() {
		number = Random.Range(GameManager.Get().nodeMin, GameManager.gm_Instance.nodeMax + 1);
		// set texture to node
		renderer.material.mainTexture = GameManager.Get().nodeTextures[0].numbers[number - 1];
	}

	// click node
	void OnMouseDown() {
		if (!nodeEntered) {
			addNodeToNodesDragged();
		}
	}

	// unclick node
	void OnMouseUp() {
		// clear nodes
		object[] nodesDraggedArray = GameManager.Get().nodesDragged.ToArray();
		ArrayList nodesToRandomize = new ArrayList();
		for (int i=0; i<nodesDraggedArray.Length; i++) {
			if (((GameObject)nodesDraggedArray[i]).GetComponent<NodeManager>().sameNum) {
				// if neighbor node is not the same number, then just deactivate the current node
				if (i > 0 && i < nodesDraggedArray.Length-1) {
					int curNum = ((GameObject)nodesDraggedArray[i]).GetComponent<NodeManager>().number;
					int prevNum = ((GameObject)nodesDraggedArray[i-1]).GetComponent<NodeManager>().number;
					int postNum = ((GameObject)nodesDraggedArray[i+1]).GetComponent<NodeManager>().number;
					if (prevNum == curNum + 1 || prevNum == curNum - 1 || postNum == curNum + 1 || postNum == curNum - 1) {
						((GameObject)nodesDraggedArray[i]).SetActive(false);
						continue;
					}
				}
				// if same number with no different neighbors
				nodesToRandomize.Add(nodesDraggedArray[i]);
			} else {
				((GameObject)nodesDraggedArray[i]).SetActive(false);
			}
		}
		foreach (GameObject nodeToRandomize in nodesToRandomize) {
			nodeToRandomize.renderer.material.shader = Shader.Find("Diffuse");
			nodeToRandomize.GetComponent<NodeManager>().nodeEntered = false;
			nodeToRandomize.GetComponent<NodeManager>().randomizeNumber();
			nodeToRandomize.GetComponent<NodeManager>().sameNum = false;
		}

		// add score
		GameManager.Get().AddScore(fibbo(GameManager.Get().nodesDragged.Count));

		// clear nodes dragged array
		GameManager.Get().nodesDragged.Clear();


		// remove all instances of line renderer
		foreach(Object clone in GameObject.FindGameObjectsWithTag("linerenderer")) {
			Destroy(clone);
		}
		nodesRemaining = 0;
		remainArray [0] = 0;
		remainArray [1] = 0;
		remainArray [2] = 0;
		foreach(Object nodeClone in GameObject.FindGameObjectsWithTag("node")) {
			print(((GameObject)nodeClone).GetComponent<NodeManager>().number + "num");
				if( ((GameObject)nodeClone).activeSelf == true) {
					if(nodesRemaining < 3) {
						print ("hery" + nodesRemaining);
						remainArray[nodesRemaining] = ((GameObject)nodeClone).GetComponent<NodeManager>().number;
					}
				}
				nodesRemaining += 1;

		}
		if (nodesRemaining < 3) {

		}

		print(nodesRemaining);
		print("arr:"+remainArray[0]);
		print("arr:"+remainArray[1]);
		print("arr:"+remainArray[2]);
	}

	// drag onto another node
	void OnMouseEnter() {
		if (!nodeEntered && GameManager.Get().nodesDragged.Count > 0) {
			bool nodeTest = true;
			ArrayList nodesDragged = GameManager.Get().nodesDragged;
			NodeManager nm = ((GameObject)nodesDragged.ToArray()[nodesDragged.Count - 1]).GetComponent<NodeManager>();

			// limit to immediate adjacent node
			if (nodeTest) {
				//if ((posX == nm.posX || posX == nm.posX - 1.0f || posX == nm.posX + 1.0f) && (posY == nm.posY || posY == nm.posY - 1.0f || posY == nm.posY + 1.0f)) {
					// limit to number in next sequence and check if node was already included in the sequence
					if ((number == nm.number - 1 || number == nm.number + 1 || number == nm.number) && renderer.material.shader.name != "Mobile/Bumped Specular") {
						if (number == nm.number) {
							sameNum = true;
							if (!nm.sameNum) {
								nm.sameNum = true;
							}
						}
						if (checkLineCollision()) {
							addNodeToNodesDragged();
						}
					}
				//}
			}
		}
	}

	// exit hover over node
	void OnMouseExit() {
		nodeEntered = false;
	}

	// checks if colliding TODO SEE CAPSULE COLLIDER
	private bool checkLineCollision() {
		/*
		RaycastHit[] hits;
		var nodesDragged = GameManager.Get().nodesDragged.ToArray();
		Vector3 start = (nodesDragged[nodesDragged.Length-1] as GameObject).transform.position;
		Vector3 end = transform.position;
		hits = Physics.RaycastAll(start, (end-start).normalized);
		if (hits.Length > 1) {
			return false;
		}
		*/
		return true;
	}

	// add node to nodes dragged array
	private void addNodeToNodesDragged() {
		nodeEntered = true;
		renderer.material.shader = Shader.Find("Mobile/Bumped Specular");
		GameManager.Get().nodesDragged.Add(gameObject);

		// add line renderer
		if (GameManager.Get().nodesDragged.Count > 1) {
			var nodesDragged = GameManager.Get().nodesDragged.ToArray();
			Vector3 startPos = (nodesDragged[nodesDragged.Length-2] as GameObject).transform.position;
			Vector3 endPos = (nodesDragged[nodesDragged.Length-1] as GameObject).transform.position;
			GameManager.Get().lineRenderer.SetPosition(0, startPos);
			GameManager.Get().lineRenderer.SetPosition(1, endPos);
			GameManager.Get().lineRendererCollider.transform.position = startPos+(endPos-startPos)/2.0f;
			GameManager.Get().lineRendererCollider.transform.LookAt(startPos);
			GameManager.Get().lineRendererCollider.height = (endPos-startPos).magnitude;
			Instantiate(GameManager.Get().lineRenderer);
		}
	}

	// returns the fibonacci sum for a number
	private int fibbo(int a) {
		if (a==1) { return -50; }
		int x = 0;
		int y = 100;
		for (int i = 1 ; i < a; i++) {
			x += y;
			y += 100;
		}
		return x;
	}
}
