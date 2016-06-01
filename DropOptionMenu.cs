using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Items_And_Inventory;

public class DropOptionMenu : MonoBehaviour 
{
	#region Data Members

	public InputField amountText;

	private int amount;
	private bool dropOption;
	private GameObject slot;

	#endregion

	#region Setters & Getters

	#endregion

	#region Built-in Unity Methods

	// Use this for initialization
	void Start () 
	{
		amountText.onValueChanged.AddListener(delegate { ValueChangeUpdate(); });
		gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () {

	}

	#endregion

	#region Public Methods

	public void DropItemCall(bool drop, GameObject slotRef)
	{
		dropOption = drop;
		amountText.text = "1";
		amount = 1;
		slot = slotRef;
	}

	public void ChangeInputFieldValue(int val)
	{
		if(amount > slot.GetComponent<ISlottable>().Item_Quantity)
		{
			amount = slot.GetComponent<ISlottable>().Item_Quantity;
		}

		if((amount + val) > 0 && ((amount + val) <= slot.GetComponent<ISlottable>().Item_Quantity))
		{
			amount += val;
		}

		amountText.text = amount.ToString();
	}

	public void ConfirmDrop()
	{
		//Declaring local variables
		string id = slot.GetComponent<ISlottable>().Item_ID;

		SquadManager.Instance.Current_Character.GetComponent<CharacterInventory>().RemoveItem(id, amount, dropOption);

		gameObject.SetActive(false);
	}

	#endregion

	#region Private Methods

	private void ValueChangeUpdate()
	{
		amount = int.Parse(amountText.text);
	}

	#endregion
}
