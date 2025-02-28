using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Vector2Int _size = new Vector2Int(15, 8);
    [SerializeField] GameObject _inventoryGrid;
    [SerializeField] Inventory _inventory;

    private static InventoryManager _instance;
    RectTransform _inventoryTransform;
    float _slotSize;

    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject inventoryManager = GameObject.Find("Inventory Manager");

                _instance = inventoryManager.GetComponent<InventoryManager>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);

        Assert.IsNotNull(_inventoryGrid);

        SetPanelSettings();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Assert.IsNotNull(_inventory);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetPanelSettings()
    {
        GridLayoutGroup gridLayoutGroup = _inventoryGrid.GetComponent<GridLayoutGroup>();

        _inventoryTransform = (RectTransform)_inventoryGrid.transform;
        _slotSize = gridLayoutGroup.cellSize.x;
    }

    public Vector2Int GetGridPositionFromUI(Vector3 mousePosition, Vector2Int itemSize)
    {
        Vector2Int gridPosition = _inventory.GetInventoryGrid().GetGridPositionFromUI(mousePosition, itemSize);

        return gridPosition;
    }

    public Vector2 GetPositionInGrid(Vector2Int gridCoordinates, Vector2Int itemSize)
    {
        Vector2 gridPosition = _inventory.GetInventoryGrid().GetPositionInGrid(gridCoordinates, _slotSize, itemSize);

        return gridPosition;
    }

    public RectTransform GetInventoryPanel()
    {
        return _inventoryTransform;
    }

    public Vector2Int GetInventorySize()
    {
        return _size;
    }

    public float GetSlotSize()
    {
        return _slotSize;
    }

    public bool IsAvailablePlace(Vector2Int gridCoordinates, Vector2Int itemSize)
    {
        bool isAvailablePlace = _inventory.IsAvailablePlace(gridCoordinates, itemSize);

        return isAvailablePlace;
    }

    public void PlaceItem(Vector2Int gridCoordinates, Vector2Int itemSize, Vector2Int? previousCoordinates = null)
    {
        _inventory.PlaceItem(gridCoordinates, itemSize, previousCoordinates);
    }

    public void HighlightSlots(Vector2Int gridCoordinates, Vector2Int itemSize)
    {
        _inventory.HighlightSlots(gridCoordinates, itemSize);
    }

    public void TurnOffAllHighlights()
    {
        _inventory.GetInventoryGrid().TurnOffAllHighlights();
    }

    public GameObject[,] GetSlots()
    {
        GameObject[,] slots = _inventory.GetInventoryGrid().GetSlots();

        return slots;
    }
}
