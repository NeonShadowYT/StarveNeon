using System;
using UnityEngine;
public class InspectionCamera : MonoBehaviour
{
    [NonSerialized] public InspectableObject inspectableObject;
    [SerializeField] private Transform _objectRotator;
    [SerializeField] private Vector3 _targetPosition;
    private Vector3 _targetRotation;
    [SerializeField] private Vector3 _initialSpawnOffset = Vector3.down*5f;
    [Header("Поворот и зум настройки")]
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _zoomSpeed = 10f;
    void Update()
    {
        if(inspectableObject == null) return;
        ZoomInOut();
        RotateObject();
    }
    private void RotateObject()
    {
        if (Input.GetKey(KeyCode.Mouse0)) _objectRotator.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * _rotationSpeed, Space.World);
    }
    private void ZoomInOut()
    {
        _targetPosition = new Vector3(_targetPosition.x, _targetPosition.y, Mathf.Clamp(_targetPosition.z - Input.GetAxis("Mouse ScrollWheel"), inspectableObject.minMaxZoom.x, inspectableObject.minMaxZoom.y));
        _objectRotator.localPosition = Vector3.Lerp(_objectRotator.localPosition, _targetPosition, Time.deltaTime * _zoomSpeed);
    }
    private void OnEnable()
    {
        if (inspectableObject == null) return;
        _targetPosition.z = inspectableObject.defaultZoomValue;
        _objectRotator.localPosition = _initialSpawnOffset;
        _objectRotator.localRotation = Quaternion.Euler(Vector3.zero + _targetRotation);
    }
}
