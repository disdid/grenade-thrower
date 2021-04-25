using System;
using GrenadeThrower.Ballistics;
using GrenadeThrower.Character;
using Unity.Collections;
using UnityEngine;

namespace GrenadeThrower.Grenades
{
    /// <summary>
    /// Launches <see cref="IThrowable"/> objects
    /// </summary>
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(LineRenderer))]
    public class ThrowerScript : MonoBehaviour
    {
        public GameObject throwableObjectPrefab;
        
        [Tooltip("The source from which the object is thrown")]
        [SerializeField]
        private Transform throwSource;
        
        [Tooltip("This object which will be placed at landing point of throwable. Part of trajectory preview.")]
        [SerializeField]
        private GameObject targetingObject;
        
        [SerializeField]
        private float throwSpeed = 25;

        [Tooltip("Defines maximum preview line length")]
        [SerializeField]
        private float previewTime = 5;

        [Tooltip("Defines how accurate preview line will be")]
        [SerializeField]
        private float previewTimeStep = 0.05f;

        [SerializeField]
        private LayerMask previewHitTestLayerMask;

        public event Action<GameObject> ObjectThrownEvent;

        private PlayerInputHandler _playerInputHandler;
        private LineRenderer _lineRenderer;

        private bool _isHoldingFireButton;
        private BallisticCurve _currentCurve;
        private NativeArray<Vector3> _curvePoints;

        private void Awake()
        {
            _curvePoints = new NativeArray<Vector3>((int)(previewTime / previewTimeStep), Allocator.Persistent);
        }

        private void Start()
        {
            _playerInputHandler = GetComponent<PlayerInputHandler>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (_isHoldingFireButton)
            {
                UpdateCurve();
                UpdatePreview();
                if (_playerInputHandler.GetFireButtonUp())
                {
                    _isHoldingFireButton = false;
                    _playerInputHandler.EnableMovementInput = true;
                    ResetPreview();
                    Throw();
                }
            }
            else
            {
                if (_playerInputHandler.GetFireButtonDown())
                {
                    _isHoldingFireButton = true;
                    _playerInputHandler.EnableMovementInput = false;
                    UpdateCurve();
                    UpdatePreview();
                }
            }
        }

        private void OnDisable()
        {
            ResetPreview();
            _isHoldingFireButton = false;
            _playerInputHandler.EnableMovementInput = true;
        }

        private void OnDestroy()
        {
            _curvePoints.Dispose();
            ObjectThrownEvent = null;
        }
        
        private void UpdateCurve()
        {
            _currentCurve = new BallisticCurve(throwSource, throwSpeed);
        }
        
        private void UpdatePreview()
        {
            var pointCount = _currentCurve.TraceCurve(_curvePoints, previewTimeStep, previewHitTestLayerMask,
                out var collisionPoint);

            _lineRenderer.positionCount = pointCount;
            _lineRenderer.SetPositions(_curvePoints);
            
            if (collisionPoint.HasValue)
            {
                targetingObject.transform.position = collisionPoint.Value;
                targetingObject.SetActive(true);
            }
            else
            {
                targetingObject.SetActive(false);
            }
        }

        private void ResetPreview()
        {
            _lineRenderer.positionCount = 0;
            targetingObject.SetActive(false);
        }

        private void Throw()
        {
            if (throwableObjectPrefab == null)
            {
                Debug.LogWarning("Throwable object prefab is not specified", this);
                return;
            }

            var go = Instantiate(throwableObjectPrefab, throwSource.position, throwSource.rotation);
            go.GetComponent<IThrowable>()?.SetTrajectory(_currentCurve);
            
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), go.GetComponent<Collider>());
            
            ObjectThrownEvent?.Invoke(go);
        }
    }
}