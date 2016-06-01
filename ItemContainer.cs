using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Items_And_Inventory;

public class ItemContainer : MonoBehaviour, IDropHandler
{
	#region Data Members

	private int gridValue;

	#endregion

	#region Setters & Getters

	public int Grid_Value
	{
		get { return gridValue; }
		set { gridValue = value; }
	}

	#endregion

	#region Built-in Unity Methods

	// Use this for initialization
	void Start ()
	{

	}
		
	#endregion

	#region Public Methods

	/// <summary>
	/// Raises the drop event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnDrop(PointerEventData eventData)
	{
		//This for the case when an Item Slot is dragged and dropped onto a slot that is occupied by another.
		if(transform.GetComponentInChildren<ISlottable>() != null)
		{
			GameObject tempSlot = transform.GetChild(0).gameObject;
			Transform tempParent = eventData.pointerDrag.gameObject.GetComponent<ISlottable>().Slot_Parent;
			int tempGridIndex = eventData.pointerDrag.gameObject.GetComponent<ISlottable>().Grid_Position;
			tempSlot.GetComponent<ISlottable>().ReInitializeTransform(tempParent, tempGridIndex);
			eventData.pointerDrag.GetComponent<ISlottable>().ReInitializeTransform(transform, gridValue);
		}
		else
		{
			eventData.pointerDrag.GetComponent<ISlottable>().ReInitializeTransform(transform, gridValue);
		}
	}

	#endregion

	#region Private Methods

	#endregion
}
