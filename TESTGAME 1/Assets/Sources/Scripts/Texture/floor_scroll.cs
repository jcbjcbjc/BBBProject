using UnityEngine;
using System.Collections;

public class floor_scroll : MonoBehaviour
{
	public float scrollSpeed = 1f;

	void Update()
	{
		float offset = Time.time * scrollSpeed;
		GetComponent<Renderer>().material.SetTextureOffset("_DetailAlbedoMap", new Vector2(0, -offset));
	}
}
