using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Base : MonoBehaviour
{
	public Bot botPrefab;
	private float maxpower = 20;
	public float power = 20;
	private const float baseUnitRadius = 10f;
	private const int attackTeamSize = 25;

	private static readonly Dictionary<string, Color> colors = new Dictionary<string, Color> {
		{ "Player", Color.yellow },
		{ "EnemyGreen", Color.green },
		{ "EnemyRed", Color.red },
		{ "Neutral", Color.gray },
		{ "", Color.gray }
	};

	private Color getColor()
	{
		foreach (KeyValuePair<string, Color> color in colors) {
			if (color.Key == tag || color.Key == "") {
				return color.Value;
			}
		}

		return Color.cyan;
	}

	// Use this for initialization
	void Start()
	{
		InvokeRepeating("CloneBot", 1f, 1f);
	}

	public void Shot(string player)
	{
		--power;
		if (power <= 0) {
			power += maxpower;
			tag = player;
		}
	}

	void CloneBot()
	{
		Color color = getColor();
		if (color != Color.gray) {
			float angle = Random.value * Mathf.PI * 2;
			Bot bot = (Bot)Instantiate(botPrefab, transform.position, Quaternion.identity);
			bot.GetComponent<SpriteRenderer>().color = getColor();
			bot.gameObject.tag = tag;

			Vector3 position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 1.5f * (Random.value + 1f);
			bot.GetComponent<Bot>().target = transform.position + position;

			bot.gameObject.SetActive(true);
		}

		if (color != Color.yellow) {
			// calc nearest Bots
			GameObject[] army = GameObject.FindGameObjectsWithTag(tag);
			List<GameObject> team = new List<GameObject>();
			foreach (GameObject bot in army) {
				if ((bot.transform.position - transform.position).sqrMagnitude <= baseUnitRadius) {
					team.Add(bot);
				}
			}

			if (team.Count >= attackTeamSize) {
				Debug.Log("ready: " + team.Count);
				GameObject nearest = findTarget(team.Count);

				if (nearest) {
					foreach (GameObject obj in team) {
						Bot bot = obj.GetComponent<Bot>();
						if (bot) {
							bot.Attack(nearest.transform.position);
						}
					}
				}
			}
		}
	}

	private GameObject testCandidate(GameObject target, int teamSize, GameObject[] candidates)
	{
		float distance = Mathf.Infinity;
		if (target) {
			distance = (target.transform.position - transform.position).sqrMagnitude;
		}

		foreach (GameObject element in candidates) {
			if (!element.GetComponent<Base>()) {
				continue;
			}

			int baseStrength = 0;

			foreach (KeyValuePair<string, Color> color in colors) {
				if (color.Key != tag && color.Key != "") {
					GameObject[] enemies = GameObject.FindGameObjectsWithTag(color.Key);
					foreach (GameObject bot in enemies) {
						if ((bot.transform.position - element.transform.position).sqrMagnitude <= baseUnitRadius) {
							++baseStrength;
						}
					}
				}
			}

			Debug.Log("base strength: " + baseStrength);

			if (baseStrength <= teamSize || teamSize >= attackTeamSize * 1.7) {
				float diff = (element.transform.position - transform.position).sqrMagnitude;
				if (diff <= distance) {
					target = element;
					distance = diff;
				}
			}
		}
		return target;
	}

	private GameObject findTarget(int teamSize)
	{
		GameObject target = null;

		foreach (KeyValuePair<string, Color> color in colors) {
			if (color.Key != tag && color.Key != "") {
				target = testCandidate(target, teamSize, GameObject.FindGameObjectsWithTag(color.Key));
			}
		}

		return target;
	}

	
	// Update is called once per frame
	void Update()
	{
		GetComponent<SpriteRenderer>().color = getColor();
	}

	void OnGUI()
	{
		Texture2D progressBarEmpty = new Texture2D(10, 10);
		// Texture2D progressBarFull = new Texture2D(10, 10);

		Vector2 size = new Vector2(60, 5);
		Vector3 position = Camera.main.WorldToScreenPoint(transform.position);

		Rect pos = new Rect();

		pos.x = position.x - size.x / 2;
		pos.y = Screen.height - position.y + 20;
		pos.width = Mathf.FloorToInt(power / maxpower * size.x);
		pos.height = size.y;

		GUI.color = getColor();
		GUI.DrawTexture(pos, progressBarEmpty);
		GUI.color = Color.white;
	}

}
