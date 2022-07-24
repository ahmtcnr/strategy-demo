using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ProductPanel : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform productGroup;

    [SerializeField] private List<Transform> items;
    [SerializeField] private GameObject itemPrefab;


    private bool canScroll;

    private void Awake()
    {
    }

    private void OnEnable()
    {
        scrollRect.onValueChanged.AddListener(OnScrolling);
    }


    private void OnDisable()
    {
        scrollRect.onValueChanged.RemoveListener(OnScrolling);
    }

    private void Update()
    {
        if (canScroll)
        {
            Debug.Log("aaa");
        }
    }

    private void OnScrolling(Vector2 scrollPos)
    {
        
        if (scrollPos.y < 0.25)
        {
            Debug.Log("Create");
            GameObject spawned = Instantiate(itemPrefab, productGroup);
            //Destroy(items[0].gameObject);

            items.RemoveAt(0);
            Destroy(productGroup.GetChild(0).gameObject);
        }

        // if (productGroup.localPosition.y > 100)
        // {
        //    
        //     productGroup.localPosition = new Vector3(0, 0, 0);
        //     items[0].SetSiblingIndex(productGroup.childCount);
        //
        //     var tempTransform = items[0];
        //     items.RemoveAt(0);
        //     items.Add(tempTransform);
        //     scrollRect.LayoutComplete(); 
        // }
    }
}