using UnityEngine;

public class TrackRigidbody2D : MonoBehaviour
{
    public float radius = 1.0f;
    public float zoomFactor = 2.0f;
    public float trackingSpeed = 5.0f;
    public float zoomSpeed = 5.0f;

    private Rigidbody2D _selectedRigidbody;
    private Camera _mainCamera;
    private float _lastClickTime;
    private float _doubleClickThreshold = 0.3f;
    private Vector3 _originalCameraPosition;
    private float _originalCameraSize;
    private bool _isTracking = false;
    private bool _isZooming = false;
    private bool _isResetting = false;
    private float _targetOrthographicSize;

    void Start()
    {
        _mainCamera = Camera.main;
        _originalCameraPosition = _mainCamera.transform.position;
        _originalCameraSize = _mainCamera.orthographicSize;
    }

    void Update()
    {
        Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastClick = Time.time - _lastClickTime;
            _lastClickTime = Time.time;

            if (timeSinceLastClick <= _doubleClickThreshold)
            {
                Rigidbody2D clickedRigidbody = FindNearestRigidbody(mousePosition);
                if (clickedRigidbody != null)
                {
                    _selectedRigidbody = clickedRigidbody;
                    _isTracking = true;
                    _isZooming = true;
                    _targetOrthographicSize = _originalCameraSize / zoomFactor;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && _isTracking)
        {
            _isTracking = false;
            _selectedRigidbody = null;
            _isZooming = true; // Start zooming out
            _isResetting = true; // Start resetting the camera
        }

        if (_isZooming)
        {
            AnimateCameraZoom();
        }

        if (_isResetting)
        {
            ResetCamera();
        }

        if (_isTracking && _selectedRigidbody != null)
        {
            TrackRigidbody();
        }
    }

    private Rigidbody2D FindNearestRigidbody(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);
        Rigidbody2D nearestRigidbody = null;
        float minDistance = float.MaxValue;

        foreach (var collider in colliders)
        {
            Rigidbody2D rb = collider.attachedRigidbody;
            if (rb != null)
            {
                float distance = Vector2.Distance(position, rb.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestRigidbody = rb;
                }
            }
        }

        return nearestRigidbody;
    }

    private void AnimateCameraZoom()
    {
        float targetSize = _isTracking ? _targetOrthographicSize : _originalCameraSize;
        _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, targetSize, zoomSpeed * Time.deltaTime);

        if (Mathf.Abs(_mainCamera.orthographicSize - targetSize) < 0.01f)
        {
            _isZooming = false;
            if (!_isTracking)
            {
                _isResetting = true;
            }
        }
    }

    private void TrackRigidbody()
    {
        Vector3 targetPosition = _selectedRigidbody.transform.position;
        targetPosition.z = _mainCamera.transform.position.z;
        _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, targetPosition, trackingSpeed * Time.deltaTime);
    }

    private void ResetCamera()
    {
        _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, _originalCameraPosition, trackingSpeed * Time.deltaTime);
        _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, _originalCameraSize, zoomSpeed * Time.deltaTime);

        if (Vector3.Distance(_mainCamera.transform.position, _originalCameraPosition) < 0.01f && 
            Mathf.Abs(_mainCamera.orthographicSize - _originalCameraSize) < 0.01f)
        {
            _isResetting = false;
        }
    }
}
