using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using Items_And_Inventory;

public class ItemSlot : MonoBehaviour, ISlottable, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	#region Data Members

	private string itemID;
	private int itemQuantity;
	private int itemIndexInInventory;
	private string itemName;

	[SerializeField]
	private int gridPosition;

	[SerializeField]
	private int isSlotted;

	private Vector2 offsetFromMouseCursor;

	public Transform rootTransform;
	public Transform parentTransform;
	public Text text;
	public Image icon;

	#endregion

	#region Setters & Getters

	public string Item_ID
	{
		get { return itemID; }
		set { itemID = value; }
	}

	public int Item_Quantity
	{
		get { return itemQuantity; }
		set { itemQuantity = value; }
	}

	public int Is_On_Slot
	{
		get { return isSlotted; }
		set { isSlotted = value; }
	}

	public int Grid_Position
	{
		get { return gridPosition; }
		set { gridPosition = value; }
	}

	public Transform Root_Transform
	{
		get { return rootTransform; }
		set { rootTransform = value; }
	}

	public Transform Slot_Parent
	{
		get { return parentTransform; }
		set { parentTransform = value; }
	}

	#endregion

	#region Built-in Unity Methods

	//n/a

		
	#endregion

	#region Public Methods

	/// <summary>
	/// Initializes the item slot.
	/// </summary>
	/// <param name="item">Item.</param>
	public void InitializeItemSlot(IStoreable item)
	{
		itemID = item.Inventory_Unique_ID;
		itemQuantity = item.Item_Quantity;
		itemName = item.Item_Name;
		isSlotted = item.Quickbar_Index;
		gridPosition = item.Grid_Index;

		//Initialize the quantity text
		text.text = itemQuantity.ToString();

		//Initialize the icon
		icon.sprite = item.Item_Sprite;	//Set the quick slot icon with the item sprite
		icon.color = new Vector4(255f, 255f, 255f, 255f); //Make sure the alpha value is turned 
														  //all the way up
	}

	/// <summary>
	/// Called by the menu whenever the item 
	/// quantity is updated.
	/// </summary>
	/// <param name="qty">Qty.</param>
	public void UpdateQuantity(int qty)
	{
		if(qty < 0)
		{
			qty *= -1;
		}

		itemQuantity = qty;
		text.text = qty.ToString();
	}

	/// <summary>
	/// Raises the begin drag event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnBeginDrag(PointerEventData eventData)
	{
		offsetFromMouseCursor = eventData.position - new Vector2(transform.position.x, transform.position.y);
		transform.SetParent(rootTransform);
		transform.position = eventData.position;
	}

	/// <summary>
	/// Raises the drag event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnDrag(PointerEventData eventData)
	{
		transform.position = eventData.position - offsetFromMouseCursor;
		gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
	}

	/// <summary>
	/// Raises the end drag event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnEndDrag(PointerEventData eventData)
	{
		Debug.Log(eventData.pointerEnter);

		//When the item slot is dragged onto an empty area.
		if(eventData.pointerEnter == null)
		{
			gameObject.transform.SetParent(parentTransform);
			gameObject.transform.position = parentTransform.position;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
		else if(eventData.pointerEnter.GetComponent<IContainable>() == null)
		{
			gameObject.transform.SetParent(parentTransform);
			gameObject.transform.position = parentTransform.position;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
	}

	/// <summary>
	/// Sets the new transform for the Item Slot to latch
	/// on to. Called by OnDrop() method of the GameObject
	/// that has it implemented.
	/// </summary>
	/// <param name="newParent">New parent.</param>
	/// <param name="gridIndex">Grid index.</param>
	public void ReInitializeTransform(Transform newParent, int _gridIndex)
	{
		//Checks if the new parent is the Grid or a QuickBar,
		//since both GameObjects are utilizing the same interface.
		if(newParent.parent.gameObject.GetComponent<IContainable>() != null)
		{
			parentTransform = newParent;
			transform.SetParent(newParent);
			gameObject.transform.position = newParent.transform.position;
			gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;

			if(newParent.parent.GetComponent<IContainable>().Is_Inventory_Grid)
			{
				Debug.Log("Grid");
				gridPosition = _gridIndex;
				isSlotted = -1;
			}
			else
			{
				Debug.Log("Quick");
				isSlotted = _gridIndex;
				gridPosition = -1;
			}
		}
	}

	#endregion

	#region Private Methods

	#endregion
}
