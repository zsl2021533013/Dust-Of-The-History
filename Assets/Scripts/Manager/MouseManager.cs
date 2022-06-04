using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class MouseManager : Singleton<MouseManager>
{
    public event Action<Vector3> OnMouseClick;

    public event Action<GameObject> OnEnemyClick;

    public Texture2D point, doorway, attack, target, arrow;

    public bool isClickPortal, isClickTrigger;

    public RaycastHit hitInfo;

    public bool isClickArtillery;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetCursorTexture();
        if (IsInteractWithUI())
        {
            return;
        }
        MouseControl();
    }

    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                Debug.Log(hitInfo.collider.gameObject.name);
            }
            switch(hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(0, 0), CursorMode.Auto);
                    break;
                case "Attackable":
                    Cursor.SetCursor(point, new Vector2(0, 0), CursorMode.Auto);
                    break;
                case "Portal":
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Item":
                    Cursor.SetCursor(point, new Vector2(0, 0), CursorMode.Auto);
                    break;
                case "Trigger":
                    Cursor.SetCursor(point, new Vector2(0, 0), CursorMode.Auto);
                    break;
                case "Artillery":
                    Cursor.SetCursor(point, new Vector2(0, 0), CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(arrow, new Vector2(0, 0), CursorMode.Auto);
                    break;
            }
        }
    }

    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
            {
                OnMouseClick?.Invoke(hitInfo.point);
            }

            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClick?.Invoke(hitInfo.collider.gameObject);
            }

            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
            {
                OnEnemyClick?.Invoke(hitInfo.collider.gameObject);
            }

            if (hitInfo.collider.gameObject.CompareTag("Item"))
            {
                OnMouseClick?.Invoke(hitInfo.point);
            }

            if (hitInfo.collider.gameObject.CompareTag("Portal"))
            {
                isClickPortal = true;
                OnMouseClick?.Invoke(hitInfo.point);
            }
            else
            {
                isClickPortal = false;
            }

            if (hitInfo.collider.gameObject.CompareTag("Trigger"))
            {
                isClickTrigger = true;
                OnMouseClick?.Invoke(hitInfo.point);
            }
            else
            {
                isClickTrigger = false;
            }

            if (hitInfo.collider.gameObject.CompareTag("Artillery"))
            {
                isClickArtillery = true;
                OnMouseClick?.Invoke(hitInfo.point);
            }
            else
            {
                isClickArtillery = false;
            }
        }
    }

    bool IsInteractWithUI()
    {
        if(EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }
}
