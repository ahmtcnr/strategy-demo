using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using Units.Base;
using UnityEngine;

public class BannerHandler : MonoBehaviour
{
    [SerializeField] private GameObject bannerParent;

    private bool canSetBanner;
    private BaseProducer _currentProducer;

    private void Awake()
    {
        HideBanner();
    }

    private void OnEnable()
    {
        Actions.OnUnitSelected += SetBannerStatus;

        Actions.OnRightClick += PlaceBanner;

        Actions.OnUnitDeselected += HideBanner;
    }


    private void OnDisable()
    {
        Actions.OnUnitSelected -= SetBannerStatus;

        Actions.OnRightClick -= PlaceBanner;
        
        Actions.OnUnitDeselected -= HideBanner;
    }

    private void HideBanner()
    {
        canSetBanner = false;
        bannerParent.SetActive(false);
    }


    private void SetBannerStatus(BaseUnit baseUnit)
    {
        if (baseUnit != null && baseUnit.TryGetComponent(out BaseProducer baseProducer))
        {
            bannerParent.SetActive(true);
            canSetBanner = true;
            _currentProducer = baseProducer;
            transform.position = baseProducer.bannerNode.PivotWorldPosition;
        }
        else
        {
            HideBanner();
        }
    }

    private void PlaceBanner()
    {
        if (!canSetBanner) return;

        var targetNode = GridSystem.Instance.GetNodeOnCursor();
        _currentProducer.bannerNode = targetNode;
        transform.position = targetNode.PivotWorldPosition;
    }
}