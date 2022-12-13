using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipmentInventory : Inventory
{
    [SerializeField] private Transform _parent;

    public void Initialize()
    {
    }

    #region Serialization
    public override SerializableData SaveData()
    {
        EquipmentInventoryData data = new EquipmentInventoryData();

        return data;
    }

    public override void LoadData(SerializableData data)
    {
        if (data == null) return;
    }

    public override void ResetData()
    {
    }

    [System.Serializable]
    private class EquipmentInventoryData : SerializableData
    {
        public List<SingleEquipmentData> equipments;

        public EquipmentInventoryData()
        {
            equipments = new List<SingleEquipmentData>();
        }

        public SingleEquipmentData this[int index]
        {
            get
            {
                if (index > equipments.Count)
                {
                    return null;
                }
                else return equipments[index];
            }
        }
        /*
        public void Add(Equipment equipment)
        {
            SingleEquipmentData data = new SingleEquipmentData();

            data.id = equipment.ID;
            data.level = (int)equipment.Level.Value;
            data.isEquipped = equipment.isEquiped;

            equipments.Add(data);
        }*/

        [System.Serializable]
        public class SingleEquipmentData
        {
            public int id;
            public int level;
            public bool isEquipped;
        }
    }
    #endregion
}
