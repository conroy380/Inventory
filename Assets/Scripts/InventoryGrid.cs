using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class InventoryGrid : MonoBehaviour
{
    [SerializeField] GameObject _slot;

    RectTransform _rectTransform;
    GridLayoutGroup _gridLayoutGroup;
    GameObject[,] _slots;
    HashSet<GameObject> _currentHighlightedSlots = new HashSet<GameObject>();
    HashSet<GameObject> _highlightedSlots = new HashSet<GameObject>();

    Color _slotDefaultColor;

    private void Awake()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        _rectTransform = (RectTransform)transform;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Assert.IsNotNull(_gridLayoutGroup);
        Assert.IsNotNull(_slot);

        Image image = _slot.GetComponent<Image>();
        _slotDefaultColor = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfigGridLayout(float cellSize, int columns)
    {
        _gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        _gridLayoutGroup.constraintCount = columns;
    }

    public void GenerateSlots(Vector2Int inventorySize)
    {
        _slots = new GameObject[inventorySize.x, inventorySize.y];

        for (int j = 0; j < inventorySize.y; ++j)
        {
            for (int i = 0; i < inventorySize.x; ++i)
            {
                GameObject slot = Instantiate(_slot, transform);
                int indexY = inventorySize.y - j - 1;

                _slots[i, indexY] = slot;
            }
        }
    }

    public Vector2Int GetGridPositionFromUI(Vector3 mousePosition, Vector2Int itemSize)
    {
        Vector2 localPoint;

        Vector2Int inventorySize = InventoryManager.Instance.GetInventorySize();
        float slotSize = InventoryManager.Instance.GetSlotSize();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, mousePosition, null, out localPoint);

        int gridX = Mathf.FloorToInt((localPoint.x - _rectTransform.rect.xMin) / slotSize);
        int gridY = Mathf.FloorToInt((localPoint.y - _rectTransform.rect.yMin) / slotSize);

        gridX = (int)Mathf.Clamp(gridX, 0, inventorySize.x - itemSize.x);
        gridY = (int)Mathf.Clamp(gridY, 0, inventorySize.y - itemSize.y);

        Vector2Int gridCoordinates = new Vector2Int(gridX, gridY);

        return gridCoordinates;
    }

    public Vector2 GetPositionInGrid(Vector2Int gridCoordinates, float slotSize, Vector2Int itemSize)
    {
        float deltaPosX = (gridCoordinates.x * slotSize) + ((slotSize / 2.0f) * itemSize.x);
        float deltaPosY = (gridCoordinates.y * slotSize) + ((slotSize / 2.0f) * itemSize.y);

        Vector2 newPosition = new Vector2(deltaPosX, deltaPosY);

        return newPosition;
    }

    public void HiglightSlot(int x, int y, Color color)
    {
        GameObject slot = _slots[x, y];
        bool isNotHighlighted = _highlightedSlots.Add(slot);

        if (isNotHighlighted)
        {
            Image image = slot.GetComponent<Image>();

            image.color = color;
            _highlightedSlots.Add(slot);
        }
    }

    public void SaveHighlightedSlot(int x, int y)
    {
        GameObject slot = _slots[x, y];

        _currentHighlightedSlots.Add(slot);
    }

    public void TurnOffHighlights()
    {
        foreach (GameObject slot in _highlightedSlots)
        {
            bool isContained = _currentHighlightedSlots.Contains(slot);

            if (!isContained)
            {
                Image image = slot.GetComponent<Image>();

                image.color = _slotDefaultColor;
            }
        }

        (_highlightedSlots, _currentHighlightedSlots) = (_currentHighlightedSlots, _highlightedSlots);
        _currentHighlightedSlots.Clear();
    }

    public void TurnOffAllHighlights()
    {
        foreach (GameObject slot in _highlightedSlots)
        {
            Image image = slot.GetComponent<Image>();

            image.color = _slotDefaultColor;
        }

        _highlightedSlots.Clear();
    }

    public GameObject[,] GetSlots()
    {
        return _slots;
    }
}
