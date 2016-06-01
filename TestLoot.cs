using UnityEngine;
using System.Collections;
using Items_And_Inventory;

public class TestLoot : MonoBehaviour, ILootable
{
	private ItemInfo lootInfo;

	public ItemInfo Item_Info
	{
		get { return lootInfo; }
	}

	public void SetLootContents(ItemInfo itm)
	{
		lootInfo = itm;
		gameObject.GetComponent<SpriteRenderer>().sprite = lootInfo.Item_Info.Item_Sprite;
		gameObject.transform.localScale = new Vector3(.25f, .25f, 1.0f);
		StartCoroutine(TimerForDestroy());
	}

	private IEnumerator TimerForDestroy()
	{
		yield return new WaitForSeconds(5.0f);
		Destroy(this.gameObject);
	}
}
