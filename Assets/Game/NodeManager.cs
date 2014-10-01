using UnityEngine;
using System.Collections;

public class NodeManager : MonoBehaviour {
	// position of node
	public float posX;
	public float posY;

	// number of node
	public int number;

	// boolean to make sure OnMouseEnter is run once
	[HideInInspector]
	public bool nodeEntered;

	// Use this for initialization
	void Start () {
		randomizeNumber();
		nodeEntered = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Randomizes the number.
	/// </summary>
	/// <returns>The number.</returns>
	public void randomizeNumber() {
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
		foreach (GameObject node in GameManager.Get().nodesDragged) {
			node.renderer.material.shader = Shader.Find("Diffuse");
			node.GetComponent<NodeManager>().nodeEntered = false;
			node.GetComponent<NodeManager>().randomizeNumber();
		}


		// add score
		GameManager.Get().AddScore(fibbo(GameManager.Get().nodesDragged.Count));

		// clear nodes dragged array
		GameManager.Get().nodesDragged.Clear();


		// remove all instances of line renderer
		foreach(Object clone in GameObject.FindGameObjectsWithTag("linerenderer")) {
			Destroy(clone);
		}
	}

	// drag onto another node
	void OnMouseEnter() {
		if (!nodeEntered && GameManager.Get().nodesDragged.Count > 0) {
			bool nodeTest = true;
			ArrayList nodesDragged = GameManager.Get().nodesDragged;
			NodeManager nm = ((GameObject)nodesDragged.ToArray()[nodesDragged.Count - 1]).GetComponent<NodeManager>();

			// limit to immediate adjacent node
			if (nodeTest) {
				if ((posX == nm.posX || posX == nm.posX - 1.0f || posX == nm.posX + 1.0f) && (posY == nm.posY || posY == nm.posY - 1.0f || posY == nm.posY + 1.0f)) {
					// limit to number in next sequence and check if node was already included in the sequence
					if ((number == nm.number - 1 || number == nm.number + 1) && renderer.material.shader.name != "Mobile/Bumped Specular") {
						if (nodesDragged.Count > 1) {
							var i = renderer.material.shader;
						}
						addNodeToNodesDragged();
					}
				}
			}
		}
	}

	// exit hover over node
	void OnMouseExit() {
		nodeEntered = false;
	}

	// add node to nodes dragged array
	private void addNodeToNodesDragged() {
		nodeEntered = true;
		GameManager.Get().nodesDragged.Add(gameObject);
		renderer.material.shader = Shader.Find("Mobile/Bumped Specular");

		// add line renderer
		if (GameManager.Get().nodesDragged.Count > 1) {
			var nodesDragged = GameManager.Get().nodesDragged.ToArray();
			GameManager.Get().lineRenderer.SetPosition(0, (nodesDragged[nodesDragged.Length-2] as GameObject).transform.position);
			GameManager.Get().lineRenderer.SetPosition(1, (nodesDragged[nodesDragged.Length-1] as GameObject).transform.position);
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
