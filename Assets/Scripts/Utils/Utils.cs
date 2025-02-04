using System.Text.RegularExpressions;
using UnityEngine;

public class Utils
{
	public static Vector2 WorldToViewport(Vector2 worldPosition)
	{
		return Camera.main.WorldToViewportPoint(worldPosition);
	}

	public static Vector2 ViewportToWorld(Vector2 viewportPosition)
	{
		return Camera.main.ViewportToWorldPoint(viewportPosition);
	}

	public static bool CheckStringValid(string input, string regex)
	{
		return Regex.IsMatch(input, regex);
	}
}