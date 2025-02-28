using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Item : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Vector2Int _size = Vector2Int.one;

    RectTransform _rectTransform;
    Vector2Int _currentPosition;

    private InputSystem_Actions _inputSystem;
    bool _isDragged = false;

    private void OnEnable()
    {
        _inputSystem.UI.Enable();

        _inputSystem.UI.RightClick.performed += Rotate;
    }

    private void OnDisable()
    {
        _inputSystem.UI.RightClick.performed -= Rotate;

        _inputSystem.UI.Disable();
    }

    void Awake()
    {
        _inputSystem = new InputSystem_Actions();
        _rectTransform = GetComponent<RectTransform>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        if (_isDragged)
        {
            _size = new Vector2Int(_size.y, _size.x);

            Quaternion currentRotation = _rectTransform.rotation;
            Vector3 eulerAngles = currentRotation.eulerAngles;
            float rotation = _rectTransform.rotation.z;

            if (rotation == 0.0f)
            {
                eulerAngles.z = -90.0f;
            }
            else
            {
                eulerAngles.z = 0.0f;
            }

            currentRotation.eulerAngles = eulerAngles;
            _rectTransform.rotation = currentRotation;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        _isDragged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        Vector2Int gridPos = InventoryManager.Instance.GetGridPositionFromUI(eventData.position, _size);
        InventoryManager.Instance.HighlightSlots(gridPos, _size);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2Int gridPos = InventoryManager.Instance.GetGridPositionFromUI(eventData.position, _size);
        bool isAvailablePlace = InventoryManager.Instance.IsAvailablePlace(gridPos, _size);

        if (isAvailablePlace)
        {
            _rectTransform.anchoredPosition = InventoryManager.Instance.GetPositionInGrid(gridPos, _size);
            InventoryManager.Instance.PlaceItem(gridPos, _size, _currentPosition);

            _currentPosition = gridPos;
        }
        else
        {
            _rectTransform.anchoredPosition = InventoryManager.Instance.GetPositionInGrid(_currentPosition, _size);
        }

        _isDragged = false;
        InventoryManager.Instance.TurnOffAllHighlights();
    }

    public Vector2Int GetItemSize()
    {
        return _size;
    }

    public void SetStartPosition(Vector2Int value)
    {
        _currentPosition = value;
    }
}
