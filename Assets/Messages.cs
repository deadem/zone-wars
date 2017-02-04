using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messages
{
	class Target
	{
		public float x;
		public float y;
	}

	List<Target> gameOver;

	public void YouWin()
	{
	}

	public void GameOver()
	{
		Bot[] bots = GameObject.FindObjectsOfType(typeof(Bot)) as Bot[];

		int index = 0;

		foreach (Bot bot in bots) {
			bot.isActive = false;

			Target target = gameOver[index];
			Vector2 coords;
			coords.x = Screen.width / 2 * target.x;
			coords.y = Screen.height / 2 + Screen.height / 4 * target.y;

			Vector3 moveTo = Camera.main.ScreenToWorldPoint(coords);
			bot.speed = 5;
			bot.target = moveTo;
			index = (index + 1) % gameOver.Count;
		}
	}

	public void init()
	{
		gameOver = new List<Target>();

		Texture2D gameOverImage;
		gameOverImage = Resources.Load("GameOver") as Texture2D;

		for (int x = 0; x < gameOverImage.width; ++x) {
			for (int y = 0; y < gameOverImage.height; ++y) {
				Color color = gameOverImage.GetPixel(x, y);

				//Debug.Log(color);

				if (color.r == 0) {
					Target target = new Target();
					target.x = (float)x / gameOverImage.width;
					target.y = (float)y / gameOverImage.height;
					gameOver.Add(target);
				}
			}
		}
	}
}
