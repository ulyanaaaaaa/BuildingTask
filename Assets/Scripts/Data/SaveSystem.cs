using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static string _path = Application.persistentDataPath + "/gridSave.dat";

    public static void Save(ISaveable saveable)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(_path, FileMode.Create))
        {
            GridSaveData data = new GridSaveData();
            saveable.Save(data);
            formatter.Serialize(stream, data);
        }
    }

    public static GridSaveData Load()
    {
        if (File.Exists(_path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(_path, FileMode.Open))
            {
                GridSaveData data = formatter.Deserialize(stream) as GridSaveData;
                return data;
            }
        }
        else
        {
            return null;
        }
    }
}