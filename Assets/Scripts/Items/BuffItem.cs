using UnityEngine;
using System;
using System.Collections;

public class BuffItem : Item {
	private Hashtable buffs;
	
	public BuffItem() {
		buffs = new Hashtable();
	}
	
	public BuffItem(Hashtable ht) {
		buffs = ht;
	}
	
	public void AddBuff(BaseStat stat, int mod) {
		try {
			buffs.Add(stat.Name, mod);
		}
		catch (Exception e) {
			Debug.LogWarning(e.ToString());
		}
	}
	
	public void RemoveBuff(BaseStat stat) {
		buffs.Remove(stat.Name);
	}
	
	public int BuffCount() {
		return buffs.Count;
	}
	
	public Hashtable GetBuffs() {
		return buffs;
	}
}
