using UnityEngine;
using System.Collections;

public class NodeManager : MonoBehaviour {
	// position of node
	public float posX;
	public float posY;

	// number of node
	public int number;

	// boolean to make sure OnMouseEnter is run once
	private bool nodeEntered;

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
		foreach (GameObject node in GameManager.Get().nodesDragged) {
			node.renderer.material.shader = Shader.Find("Diffuse");
			node.GetComponent<NodeManager>().nodeEntered = false;
			node.GetComponent<NodeManager>().randomizeNumber();
		}
		GameManager.Get().nodesDragged.Clear();
	}

	// drag onto another node
	void OnMouseEnter() {
		if (!nodeEntered && GameManager.Get().nodesDragged.Count > 0) {
			ArrayList nodesDragged = GameManager.Get().nodesDragged;
			NodeManager nm = ((GameObject)nodesDragged.ToArray()[nodesDragged.Count - 1]).GetComponent<NodeManager>();
			// limit to immediate adjacent node
			if ((posX == nm.posX || posX == nm.posX - 1.0f || posX == nm.posX + 1.0f) && (posY == nm.posY || posY == nm.posY - 1.0f || posY == nm.posY + 1.0f)) {
				// limit to number in next sequence
				if (number == nm.number - 1 || number == nm.number + 1) {
					addNodeToNodesDragged();
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
		renderer.material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
	}
}
