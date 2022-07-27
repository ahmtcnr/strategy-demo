using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProductionPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform productGroupParent;
    [Range(0.01f,0.03f)]
    [SerializeField] private float inputSensitivity = 0.0212f;
    [Range(10f,70f)]
    [SerializeField] private float deAccelerationRate = 30f;
    
    private List<GameObject> buildings = new List<GameObject>();
    private ObjectPool buildingPool = new ObjectPool();

    private float _positiveThresholdTracker, _negativeThresholdTracker;

    private float _positiveDragVelocity, _negativeDragVelocity;
    private int _tailIndex, _headIndex;

    private float initialOffset;
    public float offset;

    private void Awake()
    {
        initialOffset = productGroupParent.GetChild(0).localPosition.y;
        _tailIndex = productGroupParent.childCount;

        foreach (Transform building in productGroupParent)
        {
            buildings.Add(building.gameObject);
        }
    }

    private void Update()
    {
        if (_negativeDragVelocity < 0)
        {
            productGroupParent.transform.localPosition += new Vector3(0, _negativeDragVelocity, 0);
            _negativeThresholdTracker += _negativeDragVelocity;
            var overflowTracker = Mathf.Abs(_negativeThresholdTracker / offset);
            _negativeDragVelocity += Time.deltaTime * deAccelerationRate;
            if (overflowTracker > 1)
            {
                _negativeThresholdTracker %= offset;

                for (int i = 0; i < overflowTracker - 1; i++)
                {
                    
                    buildingPool.Push(buildings[buildings.Count - 1]);
                    //Destroy(items[items.Count - 1]);
                    buildings.RemoveAt(buildings.Count - 1);
                    var pooled = buildingPool.Pull();
                    pooled.transform.localPosition = new Vector3(0, _headIndex * offset + initialOffset, 0);
                    buildings.Insert(0, pooled);
                    _headIndex++;
                    _tailIndex--;
                }
            }
        }


        if (_positiveDragVelocity > 0)
        {
            productGroupParent.transform.localPosition += new Vector3(0, _positiveDragVelocity, 0);

            _positiveThresholdTracker += _positiveDragVelocity;
            var overflowTracker = _positiveThresholdTracker / offset;
            _positiveDragVelocity -= Time.deltaTime * deAccelerationRate;
            if (overflowTracker > 1)
            {
                _positiveThresholdTracker %= offset;

                for (int i = 0; i < overflowTracker - 1; i++)
                {
                    buildingPool.Push(buildings[0]);
                    buildings.RemoveAt(0);
                    var spawned = buildingPool.Pull();
                    spawned.transform.localPosition = new Vector3(0, -_tailIndex * offset + initialOffset, 0);
                    buildings.Add(spawned);
                    _tailIndex++;
                    _headIndex--;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _positiveDragVelocity = 0;
        _negativeDragVelocity = 0;
        
        
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (_positiveDragVelocity < 0)
        {
            _positiveDragVelocity = 0;
        }

        if (_negativeDragVelocity> 0)
        {
            _negativeDragVelocity = 0;
        }
        
        
        _positiveDragVelocity += eventData.delta.y * inputSensitivity;
        _negativeDragVelocity += eventData.delta.y * inputSensitivity;
    }
    
    
    
    
}