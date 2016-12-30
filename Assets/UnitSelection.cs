using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public static class Utils
    {
        static Texture2D _whiteTexture;
        public static Texture2D WhiteTexture
        {
            get
            {
                if (_whiteTexture == null)
                {
                    _whiteTexture = new Texture2D(1, 1);
                    _whiteTexture.SetPixel(0, 0, Color.white);
                    _whiteTexture.Apply();
                }

                return _whiteTexture;
            }
        }

        public static void DrawScreenRect(Rect rect, Color color)
        {
            GUI.color = color;
            GUI.DrawTexture(rect, WhiteTexture);
            GUI.color = Color.white;
        }

        public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
        {
            Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
            Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
            Utils.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
            Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        }

        public static Rect GetScreenRect(Vector2 screenPosition1, Vector2 screenPosition2)
        {
            // Move origin from bottom left to top left
            screenPosition1.y = Screen.height - screenPosition1.y;
            screenPosition2.y = Screen.height - screenPosition2.y;

            // Calculate corners
            var topLeft = Vector2.Min(screenPosition1, screenPosition2);
            var bottomRight = Vector2.Max(screenPosition1, screenPosition2);

            // Create Rect
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }
    }

    bool isSelecting = false;
    Vector2 mousePosition1;

    void Start()
    {
    }

    void Update()
    {
        // If we press the left mouse button, save mouse location and begin selection
        if (Input.GetMouseButtonDown(0))
        {
            isSelecting = true;
            mousePosition1 = Input.mousePosition;
        }

        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;

            float maxX = Mathf.Max(mousePosition1.x, Input.mousePosition.x);
            float minX = Mathf.Min(mousePosition1.x, Input.mousePosition.x);
            float maxY = Mathf.Max(mousePosition1.y, Input.mousePosition.y);
            float minY = Mathf.Min(mousePosition1.y, Input.mousePosition.y);
            Rect selection = Rect.MinMaxRect(minX, minY, maxX, maxY);

            bool moveCommand = false;
            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (selection.width < 3 && selection.height < 3)
            {
                moveCommand = true;
            }

            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
				if (!player.GetComponent<Bot> ()) {
					continue;
				}
                bool selected = !moveCommand && selection.Contains(Camera.main.WorldToScreenPoint(player.transform.position));

                if (moveCommand && player.GetComponent<Bot>().selected)
                {
                    float angle = Random.value * Mathf.PI * 2;
                    player.GetComponent<Bot>().target = target + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.value;
                }

                player.GetComponent<Bot>().selected = selected;
            }
        }
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
}
