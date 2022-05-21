using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{

    public TMP_Text name;

    public TMP_Text info;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        UpdataPos();
    }

    private void Update()
    {
        UpdataPos();
    }

    void UpdataPos()
    {
        Vector3 pos = Input.mousePosition;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        float width = corners[3].x - corners[0].x;
        float height = corners[1].y - corners[0].y;

        if(pos.y < height)
        {
            rectTransform.position = pos + Vector3.up * height * 0.6f;
        }
        else if(Screen.width - pos.x > width)
        {
            rectTransform.position = pos + Vector3.right * width * 0.6f;
        }
        else
        {
            rectTransform.position = pos + Vector3.left * width * 0.6f;
        }

    }

    public void SetUpItemTooltip(ItemData_SO itemData)
    {
        name.text = itemData.itemName;
        info.text = itemData.itemDescription;
    }
}
