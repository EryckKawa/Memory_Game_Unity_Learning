using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
	[SerializeField] GameObject targetObject;
	[SerializeField] string targetMessage;
	public Color highlightColor = Color.cyan; // Mudou a cor para ciano

	private SpriteRenderer spriteRenderer;

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void OnMouseEnter()
	{
		if (spriteRenderer != null)
		{
			spriteRenderer.color = highlightColor;
		}
	}

	public void OnMouseExit()
	{
		if (spriteRenderer != null)
		{
			spriteRenderer.color = Color.white;
		}
	}

	public void OnMouseDown()
	{
		transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
	}

	public void OnMouseUp()
	{
		transform.localScale = Vector3.one;
		if (targetObject != null)
		{
			targetObject.SendMessage(targetMessage);
		}
	}
}
