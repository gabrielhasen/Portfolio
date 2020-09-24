using UnityEngine;

public class Spell_Bar_UI : MonoBehaviour {
	
	public static Spell_Bar_UI instance;
	void Awake()
	{
		if(instance != null)
		{
			Debug.LogWarning("More than 1 instance of Spell_Bar_UI");
			return;
		}
		instance = this;
	}
	
	public Transform itemsParent;

	Spell_Bar spellBar;

	//The whole hand UI
	public GameObject spellUI;
	public GameObject Highlight;

	Spell_Bar_Slot[] slots;

	// Use this for initialization
	void Start() {
		spellBar = Spell_Bar.instance;
		//listening for the delegate to get triggered
		//and saying UpdateUI function is listening
		spellBar.OnItemChangedCallBack += UpdateUI;

		//finds all children to this parent
		//and looks for the InventorySlot script on the children
		slots = itemsParent.GetComponentsInChildren<Spell_Bar_Slot>();
	}
	
	void Update() {
		//Toggles hand
		if(Input.GetKeyDown(KeyCode.H))
		{
			spellUI.SetActive(!spellUI.activeSelf);
		}
	}

	void UpdateUI()
	{
		//loops through all the slots of the hand
		for (int i = 0; i < slots.Length; i++)
		{
			//if there is an item to add
			if(i < spellBar.spells.Count)
			{
				slots[i].AddItem(spellBar.spells[i]);
			}
			//if there is no item to add
			else
			{
				slots[i].ClearSlot();
			}
		}
	}

	public void UpdateHighlight(int slotNum)
	{
		Highlight.transform.position = slots[slotNum].transform.position;
	}
}
