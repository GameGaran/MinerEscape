using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CharacterSelection : MonoBehaviour
{

	[SerializeField]
	private Miner currentMiner;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

		if (Input.GetMouseButtonUp(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.collider != null)
			{
				var selectResult = SelectCharacter(hit.collider.gameObject);
				if (!selectResult && currentMiner != null)
				{
					DoActivity(hit.collider.gameObject);
				}
			}
			else
				Unselect();

		}

	}

	public void Unselect()
	{
		if (currentMiner == null)
			return;
		var renderer = currentMiner.gameObject.GetComponent<SpriteRenderer>();
		currentMiner = null;
		Color.RGBToHSV(renderer.color, out var hue, out var sat, out var _);

		renderer.color = Color.HSVToRGB(hue, sat, 1);
		return;
	}

	public bool SelectCharacter(GameObject obj)
	{
		if (!obj.TryGetComponent<Miner>(out var miner))
			return false;
		Unselect();
		var renderer = obj.GetComponent<SpriteRenderer>();
		this.currentMiner = miner;
		Color.RGBToHSV(renderer.color, out var hue, out var sat, out var val);

		renderer.color = Color.HSVToRGB(hue, sat, 0.5f);
		return true;
	}

	public bool DoActivity(GameObject obj)
	{
		if (!obj.TryGetComponent<Activity>(out var activity))
			return false;
		return currentMiner.SetActivity(activity);

	}
}
