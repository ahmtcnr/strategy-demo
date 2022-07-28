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

    private void Awake()
    {
        DeactivateBanner();
    }

    private void OnEnable()
    {
        Actions.OnUnitSelected += SetBannerStatus;

        Actions.OnRightClick += PlaceBanner;

        Actions.OnDeselectUnit += DeactivateBanner;
    }


    private void OnDisable()
    {
        Actions.OnUnitSelected -= SetBannerStatus;
        
        Actions.OnRightClick -= PlaceBanner;

        Actions.OnDeselectUnit -= DeactivateBanner;
    }

    private void DeactivateBanner()
    {
        canSetBanner = false;
        bannerParent.SetActive(false);
    }


    private void SetBannerStatus(BaseUnitData bd)
    {
        if (bd.GetType() == typeof(ProducerData))
        {
            bannerParent.SetActive(true);
            canSetBanner = true;
            transform.position = ((ProducerData)bd).bannerNode.PivotWorldPosition;
        }
    }

    private void PlaceBanner()
    {
        if (!canSetBanner) return;
        transform.position = GridSystem.Instance.GetNodeOnCursor().PivotWorldPosition;
    }
}