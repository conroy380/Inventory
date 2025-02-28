using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Vector2Int _inventorySize = new Vector2Int(15, 8);
    [SerializeField] float _slotSize = 100;
    [SerializeField] InventoryGrid _inventoryGrid;

    RectTransform _rectTransform;
    int[,] _grid;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _grid = new int[_inventorySize.x, _inventorySize.y];
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Assert.IsNotNull(_inventoryGrid);

        Vector2 recstSize = new Vector2();

        recstSize.x = _inventorySize.x * _slotSize;
        recstSize.y = _inventorySize.y * _slotSize;

        _rectTransform.sizeDelta = recstSize;

        _inventoryGrid.ConfigGridLayout(_slotSize, _inventorySize.x);
        _inventoryGrid.GenerateSlots(_inventorySize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2Int GetInventorySize()
    {
        return _inventorySize;
    }

    public InventoryGrid GetInventoryGrid()
    {
        return _inventoryGrid;
    }

    public bool IsAvailablePlace(Vector2Int gridCoordinates, Vector2Int itemSize)
    {
        for (int i = 0; i < itemSize.x; ++i)
        {
            for (int j = 0; j < itemSize.y; ++j)
            {
                int xIndex = gridCoordinates.x + i;
                int yIndex = gridCoordinates.y + j;

                if (_grid[xIndex, yIndex] != 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void PlaceItem(Vector2Int gridCoordinates, Vector2Int itemSize, Vector2Int? previousCoordinates = null)
    {
        for (int i = 0; i < itemSize.x; ++i)
        {
            for (int j = 0; j < itemSize.y; ++j)
            {
                int xIndex = gridCoordinates.x + i;
                int yIndex = gridCoordinates.y + j;

                _grid[xIndex, yIndex] = 1;     
            }
        }

        if (previousCoordinates != null)
        {
            RemoveItem(previousCoordinates.Value, itemSize);
        }
    }

    public void RemoveItem(Vector2Int gridCoordinates, Vector2Int itemSize)
    {
        for (int i = 0; i < itemSize.x; ++i)
        {
            for (int j = 0; j < itemSize.y; ++j)
            {
                int xIndex = gridCoordinates.x + i;
                int yIndex = gridCoordinates.y + j;

                _grid[xIndex, yIndex] = 0;
            }
        }
    }

    public void HighlightSlots(Vector2Int gridCoordinates, Vector2Int itemSize)
    {
        Color color = new Color(0.214012f, 0.5176471f, 0.1882353f, 0.533333f);

        for (int i = 0; i < itemSize.x; ++i)
        {
            for (int j = 0; j < itemSize.y; ++j)
            {
                int xIndex = gridCoordinates.x + i;
                int yIndex = gridCoordinates.y + j;

                if (_grid[xIndex, yIndex] == 1)
                {
                    color = new Color(0.5188679f, 0.1884567f, 0.1884567f, 0.533333f);
                }

                _inventoryGrid.HiglightSlot(xIndex, yIndex, color);
                _inventoryGrid.SaveHighlightedSlot(xIndex, yIndex);
            }
        }

        _inventoryGrid.TurnOffHighlights();
    }
}
