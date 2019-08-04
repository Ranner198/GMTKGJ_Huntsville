using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorToCenter : MonoBehaviour
{
    public Texture2D cursorTexture;
    private Vector2 cursorHotspot;
     
    public static MouseCursorToCenter instance;
    void Awake()
    {
        if (this == instance)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        Destroy(instance);
    }

    void Start () {
        cursorHotspot = new Vector2 (cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }

    void Update () {
        Vector3 currentMouse = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay (currentMouse);
        RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction);
        Debug.DrawLine (ray.origin, hit.point);
    }
}
