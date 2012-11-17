using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class myGUI : MonoBehaviour {
	public float lootWindowHeight = 90;
	
	public float buttonWidth = 40;
	public float buttonHeight = 40;
	public float closeButtonWidth = 17;
	public float clostButtonHeight = 17;
	
	//private List<Item> _lootItems;
	private bool _displayLootWindow = false;
	private const int LOOT_WINDOW_ID = 0;
	private float _offset = 10;
	private Rect _lootWindowRect = new Rect(0,0,0,0);
	private Vector2 _lootWindowSlider = Vector2.zero;
	public static Chest chest;
	
	private string _toolTip = "";
	
	//*****************************
	//* Inventory window variable
	//*****************************
	private bool _displayInventory = false;
	private const int INVENTORY_WINDOW_ID = 1;
	private Rect _inventoryWindowRect = new Rect(10,10,170,265);
	private int _inventoryRows = 6;
	private int _inventoryCols = 4;
	
	private float _doubleClickTimer = 0;
	private const float DOUBLE_CLICK_TIMER_THRESHHOLD = .5f;
	private Item _selectedItem;
	
	//*****************************
	//* Character window variable
	//*****************************
	private bool _displayCharacterWindow = false;
	private const int CHARACTER_WINDOW_ID = 2;
	private Rect _characterWindowRect = new Rect(210,10,170,265);
	private int _characterPanel = 0;
	private string[] _characterPanelNames = new string[] {"Equip", "Attr", "Skills"};
	
	// Use this for initialization
	void Start () {
		PC.Instance.Initialize();
		//_lootItems = new List<Item>();
	}
	
	private void OnEnable() {
		Messenger.AddListener("DisplayLoot", DisplayLoot);
		Messenger.AddListener("ToggleInventory", ToggleInventoryWindow);
		Messenger.AddListener("CloseChest", ClearWindow);
		Messenger.AddListener("ToggleCharacterWindow", ToggleCharacterWindow);
	}
	
	private void OnDisable() {
		Messenger.RemoveListener("DisplayLoot", DisplayLoot);
		Messenger.RemoveListener("ToggleInventory", ToggleInventoryWindow);
		Messenger.RemoveListener("CloseChest", ClearWindow);
		Messenger.RemoveListener("ToggleCharacterWindow", ToggleCharacterWindow);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnGUI() {
		if (_displayLootWindow)
			_lootWindowRect = GUI.Window(LOOT_WINDOW_ID, new Rect(_offset, Screen.height - (_offset + lootWindowHeight), Screen.width - (_offset * 2), lootWindowHeight), LootWindow, "Loot Window");
		
		if (_displayCharacterWindow)
			_characterWindowRect = GUI.Window(CHARACTER_WINDOW_ID, _characterWindowRect, CharacterWindow, "Character");
		
		if (_displayInventory)
			_inventoryWindowRect = GUI.Window(INVENTORY_WINDOW_ID, _inventoryWindowRect, InventoryWindow, "Inventory");
	
		DisplayToolTip();
		
		if (GUI.Button(new Rect(Screen.width - 95, Screen.height-120, 85, 25), "Character(C)")) {
			_displayCharacterWindow = !_displayCharacterWindow;
		}
		
		if (GUI.Button(new Rect(Screen.width - 95, Screen.height-90, 85, 25), "Inventory(I)")) {
			_displayInventory = !_displayInventory;
		}
	
		if (GUI.Button(new Rect(Screen.width - 95, Screen.height-60, 85, 25), "Main Menu")) {
			GameSetting2.SavePlayerPosition(PC.Instance.transform.position);
			Application.LoadLevel(GameSetting2.levelNames[0]);
		}
		
		if (GUI.Button(new Rect(Screen.width - 95, Screen.height-30, 85, 25), "Exit(ESC)")) {
			GameSetting2.SavePlayerPosition(PC.Instance.transform.position);
			GameObject cc = GameObject.FindWithTag("CharColor");
			ChangingRoom go = cc.GetComponent<ChangingRoom>();
			go.SaveCharacterColor();
			Debug.Log("myGUI: save pos and color");
			Application.Quit();
		}
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
			GameSetting2.SavePlayerPosition(PC.Instance.transform.position);
			Application.Quit();
			Debug.Log("myGUI: save pos");
			Debug.Log("exit_esc");
		}
	}
	
	private void LootWindow(int id) {
		if (GUI.Button(new Rect(_lootWindowRect.width - 20 - _offset, 3, closeButtonWidth, clostButtonHeight), "x")) {
			ClearWindow();
		}
		
		if (chest == null)
			return;
		
		if (chest.loot.Count == 0) {
			ClearWindow();
			return;
		}
		
		_lootWindowSlider = GUI.BeginScrollView(new Rect(_offset * .5f, 15, _lootWindowRect.width - 10, 70), _lootWindowSlider, new Rect(0, 0, (chest.loot.Count * buttonWidth) + _offset, buttonHeight + _offset));
		
		for (int cnt = 0; cnt < chest.loot.Count; cnt++) {
			if (GUI.Button(new Rect(5 + (buttonWidth * cnt), _offset, buttonWidth, buttonHeight), new GUIContent(chest.loot[cnt].Icon, chest.loot[cnt].ToolTip()))) {
				PC.Instance.Inventory.Add (chest.loot[cnt]);
				chest.loot.RemoveAt(cnt);
			}
		}
		
		GUI.EndScrollView();
		
		SetToolTip();
	}
	
	private void DisplayLoot() {
		_displayLootWindow = true;
	}
	
	private void ClearWindow() {
		//_lootItems.Clear();
		
		//if (chest != null)
		chest.OnMouseUp();
		
		chest = null;
		_displayLootWindow = false;
	}
	
	public void InventoryWindow(int id) {
		int cnt = 0;
		
		for (int y = 0; y < _inventoryRows; y++) {
			for (int x = 0; x < _inventoryCols; x++) {
				if (cnt < PC.Instance.Inventory.Count) {
					if (GUI.Button(new Rect(5 + (x * buttonWidth), 20 + (y * buttonHeight), buttonWidth, buttonHeight), new GUIContent(PC.Instance.Inventory[cnt].Icon, PC.Instance.Inventory[cnt].ToolTip()))) {
						if  (_doubleClickTimer != 0 && _selectedItem != null) {
							if (Time.time - _doubleClickTimer < DOUBLE_CLICK_TIMER_THRESHHOLD) {
								
								if (typeof(Weapon) == PC.Instance.Inventory[cnt].GetType()) {
									if (PC.Instance.EquipedWeapon == null) {
										PC.Instance.EquipedWeapon = PC.Instance.Inventory[cnt];
										PC.Instance.Inventory.RemoveAt(cnt);
									} else {
										Item temp = PC.Instance.EquipedWeapon;
										PC.Instance.EquipedWeapon = PC.Instance.Inventory[cnt];
										PC.Instance.Inventory[cnt] = temp;
									}
								} else if (typeof(Armor) == PC.Instance.Inventory[cnt].GetType()) {
									Armor arm = (Armor)(PC.Instance.Inventory[cnt]);
									switch(arm.Slot) {
									case EquipmentSlot.Head:
										if (PC.Instance.EquipedHeadGear == null) {
											PC.Instance.EquipedHeadGear = PC.Instance.Inventory[cnt];
											PC.Instance.Inventory.RemoveAt(cnt);
										} else {
											Item temp = PC.Instance.EquipedHeadGear;
											PC.Instance.EquipedHeadGear = PC.Instance.Inventory[cnt];
											PC.Instance.Inventory[cnt] = temp;
										}
										break;
									
									case EquipmentSlot.OffHand:
										if (PC.Instance.EquipedShield == null) {
											PC.Instance.EquipedShield = PC.Instance.Inventory[cnt];
											PC.Instance.Inventory.RemoveAt(cnt);
										} else {
											Item temp = PC.Instance.EquipedShield;
											PC.Instance.EquipedShield = PC.Instance.Inventory[cnt];
											PC.Instance.Inventory[cnt] = temp;
										}
										break;
									
									default:
										Debug.Log("No Defined Equipment Slot");
										break;
									}
									
								}
								
								_doubleClickTimer = 0;
								_selectedItem = null;
							} else {
								_doubleClickTimer = Time.time;
							}
						} else {
							_doubleClickTimer = Time.time;
							_selectedItem = PC.Instance.Inventory[cnt];
						}
					}
				} else {
					GUI.Label(new Rect(5 + (x * buttonWidth), 20 + (y * buttonHeight), buttonWidth, buttonHeight), "", "box");
				}
				
				cnt++;
			}
		}
		
		SetToolTip();
		GUI.DragWindow();
	}
	
	public void ToggleInventoryWindow() {
		_displayInventory = !_displayInventory;
	}
	
	public void CharacterWindow(int id) {
		_characterPanel = GUI.Toolbar(new Rect(5, 25, _characterWindowRect.width - 10, 50), _characterPanel, _characterPanelNames);
		
		switch(_characterPanel) {
		case 0:
			DisplayEquipment();
			break;
		case 1:
			DisplayAttributes();
			break;
		case 2:
			DisplaySkills();
			break;
		}
		
		GUI.DragWindow();
	}
	
	public void ToggleCharacterWindow() {
		_displayCharacterWindow = !_displayCharacterWindow;
	}
	
	private void DisplayEquipment() {
		if (PC.Instance.EquipedWeapon == null) {
			GUI.Button(new Rect(5, 100, 40, 40), "X");
		} else {
			if (GUI.Button(new Rect(5, 100, 40, 40), new GUIContent(PC.Instance.EquipedWeapon.Icon, PC.Instance.EquipedWeapon.ToolTip()))) {
				PC.Instance.Inventory.Add(PC.Instance.EquipedWeapon);
				PC.Instance.EquipedWeapon = null;
			}
		}
		
		if (PC.Instance.EquipedShield == null) {
			GUI.Button(new Rect(55, 100, 40, 40), "X");
		} else {
			if (GUI.Button(new Rect(55, 100, 40, 40), new GUIContent(PC.Instance.EquipedShield.Icon, PC.Instance.EquipedShield.ToolTip()))) {
				PC.Instance.Inventory.Add(PC.Instance.EquipedShield);
				PC.Instance.EquipedShield = null;
			}
		}
		
		if (PC.Instance.EquipedHeadGear == null) {
			GUI.Button(new Rect(105, 100, 40, 40), "X");
		} else {
			if (GUI.Button(new Rect(105, 100, 40, 40), new GUIContent(PC.Instance.EquipedHeadGear.Icon, PC.Instance.EquipedHeadGear.ToolTip()))) {
				PC.Instance.Inventory.Add(PC.Instance.EquipedHeadGear);
				PC.Instance.EquipedHeadGear = null;
			}
		}
		
		SetToolTip();
	}
	
	private void DisplayAttributes() {
		int lineHeight = 17;
		int valueDisplayWidth = 50;
		
		GUI.BeginGroup( new Rect(5, 75, _characterWindowRect.width-(_offset*2), 
			_characterWindowRect.height - 75) );
		
		// Display the attributes
		for (int cnt = 0; cnt < PC.Instance.primaryAttribute.Length; cnt++) {
			GUI.Label( new Rect(0,cnt*lineHeight,_characterWindowRect.width-(_offset*2)-valueDisplayWidth-5,25), 
				((AttributeName)cnt).ToString() );
			GUI.Label( new Rect(_characterWindowRect.width-(_offset*2)-valueDisplayWidth,
								cnt*lineHeight,
								valueDisplayWidth,
								25), 
						PC.Instance.GetPrimaryAttribute(cnt).BaseValue.ToString() );
			
		}
		
		// Display the vitals
		for (int cnt = 0; cnt < PC.Instance.vital.Length; cnt++) {
			GUI.Label( new Rect(0,(cnt+PC.Instance.primaryAttribute.Length)*lineHeight,_characterWindowRect.width-(_offset*2)-valueDisplayWidth-5,25), 
				((VitalName)cnt).ToString() );
			GUI.Label( new Rect(_characterWindowRect.width-(_offset*2)-valueDisplayWidth,
								(cnt+PC.Instance.primaryAttribute.Length)*lineHeight,
								valueDisplayWidth,
								25), 
						PC.Instance.GetVital(cnt).CurValue.ToString() + "/" +
						PC.Instance.GetVital(cnt).AdjustedBaseValue.ToString());
			
		}
		
		GUI.EndGroup();
	}
	
	private void DisplaySkills() {
		int lineHeight = 17;
		int valueDisplayWidth = 50;
		
		GUI.BeginGroup( new Rect(5, 75, _characterWindowRect.width-(_offset*2), 
			_characterWindowRect.height - 75) );
		
		// Display the skills
		for (int cnt = 0; cnt < PC.Instance.skill.Length; cnt++) {
			GUI.Label( new Rect(0,cnt*lineHeight,_characterWindowRect.width-(_offset*2)-valueDisplayWidth-5,25), 
				((SkillName)cnt).ToString() );
			GUI.Label( new Rect(_characterWindowRect.width-(_offset*2)-valueDisplayWidth,
								cnt*lineHeight,
								valueDisplayWidth,
								25), 
						PC.Instance.GetSkill(cnt).AdjustedBaseValue.ToString() );
			
		}
		
		GUI.EndGroup();
	}
	
	private void SetToolTip() {
		if (Event.current.type == EventType.Repaint && GUI.tooltip != _toolTip) {
			if (_toolTip != "")
				_toolTip = "";
			
			if (GUI.tooltip != "")
				_toolTip = GUI.tooltip;
		}
	}
	
	private void DisplayToolTip() {
		if (_toolTip != "")
			GUI.Box(new Rect(Screen.width / 2 - 100, 10, 200, 100), _toolTip);
	}
}
