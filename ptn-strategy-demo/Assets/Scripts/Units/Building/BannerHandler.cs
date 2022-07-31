using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using Units.Base;
using UnityEngine;

public class BannerHandler : MonoBehaviour
{
    [SerializeField] private GameObject bannerParent;

    private bool _canSetBanner;
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
        _canSetBanner = false;
        bannerParent.SetActive(false);
    }


    private void SetBannerStatus(BaseUnit baseUnit)
    {
        if (baseUnit != null && baseUnit.TryGetComponent(out BaseProducer baseProducer))
        {
            bannerParent.SetActive(true);
            _canSetBanner = true;
            _currentProducer = baseProducer;
            transform.position = baseProducer.BannerNode.PivotWorldPosition;
        }
        else
        {
            HideBanner();
        }
    }

    private void PlaceBanner()
    {
        if (!_canSetBanner) return;

        var targetNode = GridSystem.Instance.GetNodeOnCursor();
        _currentProducer.BannerNode = targetNode;
        transform.position = targetNode.PivotWorldPosition;
    }
}