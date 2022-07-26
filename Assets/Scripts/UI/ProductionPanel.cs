using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProductionPanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [SerializeField] private RectTransform productGroupParent;
    [Range(0.01f, 0.03f)] [SerializeField] private float inputSensitivity = 0.0212f;
    [Range(10f, 70f)] [SerializeField] private float deAccelerationRate = 30f;

    private List<GameObject> _buildings = new List<GameObject>();
    private ObjectPool _buildingPool = new ObjectPool();

    private float _positiveThresholdTracker, _negativeThresholdTracker;

    private float _positiveDragVelocity, _negativeDragVelocity;
    private int _tailIndex, _headIndex;

    private float _initialOffset;
    [SerializeField] private float trackOffset;

    private void Awake()
    {
        _initialOffset = productGroupParent.GetChild(0).localPosition.y;
        _tailIndex = productGroupParent.childCount;

        foreach (Transform building in productGroupParent)
        {
            _buildings.Add(building.gameObject);
        }
    }

    private void Update()
    {
        if (_negativeDragVelocity < 0)
        {
            productGroupParent.transform.localPosition += new Vector3(0, _negativeDragVelocity, 0);
            _negativeThresholdTracker += _negativeDragVelocity;
            var overflowTracker = Mathf.Abs(_negativeThresholdTracker / trackOffset);
            _negativeDragVelocity += Time.deltaTime * deAccelerationRate;
            if (overflowTracker > 1)
            {
                _negativeThresholdTracker %= trackOffset;
                GameObject pulledObject;
                for (int i = 0; i < overflowTracker - 1; i++)
                {
                    _buildingPool.Push(_buildings[_buildings.Count - 1]);
                    _buildings.RemoveAt(_buildings.Count - 1);
                    pulledObject = _buildingPool.Pull();
                    pulledObject.transform.localPosition = new Vector3(0, _headIndex * trackOffset + _initialOffset, 0);
                    _buildings.Insert(0, pulledObject);
                    _headIndex++;
                    _tailIndex--;
                }
            }
        }


        if (_positiveDragVelocity > 0)
        {
            productGroupParent.transform.localPosition += new Vector3(0, _positiveDragVelocity, 0);

            _positiveThresholdTracker += _positiveDragVelocity;
            var overflowTracker = _positiveThresholdTracker / trackOffset;
            _positiveDragVelocity -= Time.deltaTime * deAccelerationRate;
            if (overflowTracker > 1)
            {
                _positiveThresholdTracker %= trackOffset;
                GameObject pulledObject;
                for (int i = 0; i < overflowTracker - 1; i++)
                {
                    _buildingPool.Push(_buildings[0]);
                    _buildings.RemoveAt(0);
                    pulledObject = _buildingPool.Pull();
                    pulledObject.transform.localPosition = new Vector3(0, -_tailIndex * trackOffset + _initialOffset, 0);
                    _buildings.Add(pulledObject);
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

    public void OnDrag(PointerEventData eventData)
    {
        if (_positiveDragVelocity < 0)
        {
            _positiveDragVelocity = 0;
        }

        if (_negativeDragVelocity > 0)
        {
            _negativeDragVelocity = 0;
        }


        _positiveDragVelocity += eventData.delta.y * inputSensitivity;
        _negativeDragVelocity += eventData.delta.y * inputSensitivity;
    }
}