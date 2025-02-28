using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsGenerator : MonoBehaviour
{
    [SerializeField] int _itemsToGenerate = 1;

    GameObject[] _items;
    List<GameObject> _slots = new List<GameObject>();

    private void Awake()
    {
        _items = Resources.LoadAll<GameObject>("Items");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Initialize());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Initialize()
    {
        yield return new WaitForSeconds(0.2f);

        GetSlots();
        GenerateItems();
    }

    void GetSlots()
    {
        GameObject[,] slots = InventoryManager.Instance.GetSlots();

        foreach (GameObject slot in slots)
        {
            _slots.Add(slot);
        }
    }

    void GenerateItems()
    {
        for (int i = 0; i < _itemsToGenerate; ++i)
        {
            int index = Random.Range(0, _itemsToGenerate);

            GameObject item = Instantiate(_items[index], transform);
            PlaceItems(item);
        }
    }

    void PlaceItems(GameObject item)
    {
        Item itemS = item.GetComponent<Item>();
        RectTransform itemRectTransform = (RectTransform)item.transform;
        Vector2Int itemSize = itemS.GetItemSize();

        foreach (GameObject slot in _slots)
        {
            Vector2 slotPos = slot.transform.position;
            Vector2Int gridPos = InventoryManager.Instance.GetGridPositionFromUI(slotPos, itemSize);
            bool isAvailablePlace = InventoryManager.Instance.IsAvailablePlace(gridPos, itemSize);

            if (isAvailablePlace)
            {
                itemRectTransform.anchoredPosition = InventoryManager.Instance.GetPositionInGrid(gridPos, itemSize);
                InventoryManager.Instance.PlaceItem(gridPos, itemSize);
                itemS.SetStartPosition(gridPos);

                break;
            }
        }
    }
}
