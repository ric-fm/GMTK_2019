using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
	public enum Type
	{
		Normal,
		Damage
	}

	public Type type;

	public Color NormalColor;
	public Color DamageColor;

	private void OnValidate()
	{
		Color color = new Color();
		switch(type)
		{
			case Type.Normal:
				color = NormalColor;
				break;
			case Type.Damage:
				color = DamageColor;
				break;
		}
		GetComponent<SpriteRenderer>().color = color;
	}
}
