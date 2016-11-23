using UnityEngine;
using System.Collections;
using LitJson;  //Allows the use of JSON data and for it to be converted into a C# object
using System.Collections.Generic;//Allows the use of Lists
using System.IO; //Allows access to system files

public class Stats
{
    public int Power { get; set; }
    public int Defence { get; set; }
    public int Vitality { get; set; }

    public Stats(JsonData data)
    {
        Power = (int)data["power"];
        Defence = (int)data["defence"];
        Vitality = (int)data["vitality"];
    }
    
}
public class Item
{
    public string Title { get; set; }
    public int Value { get; set; }
    public Stats Stats { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public int Rarity { get; set; }
    public Sprite Sprite { get; set; }
    public GameObject gameObject { get; set; }

public Item(JsonData data)
    {
        Title = data["title"].ToString();
        Value = (int)data["value"];
        Stats = new Stats(data["stats"]);
        Description = data["description"].ToString();
        Rarity = (int)data["rarity"];
        string fileName = data["sprite"].ToString();
        Sprite = Resources.Load<Sprite>("Sprites/Item/" + fileName);
        Stackable = (bool)data["stackable"];
    }
    public override string ToString()
    {
        return "Title:" + Title + "\n" +
            "value:" + Value + "\n" +
            "Stats:" + Stats + "\n" +
            "\t" + "Power:" + Stats.Power + "\n" +
            "\t" + "Defence:" + Stats.Defence + "\n" +
            "\t" + "Vitality:" + Stats.Vitality + "\n" +
            "Description:" + Description + "\n" +
            "Stackable:" + Stackable.ToString() + "\n" +
            "Rarity:" + Rarity + "\n";
    }
}

public class ItemDatabase : MonoBehaviour
{   //Stores all of the items in a database
    private Dictionary<string, Item> database = new Dictionary<string, Item>();//Use this variable to store all of the items to duplicate in the inventory

    private JsonData itemData; //Holds the JSON database that is pulled in from the scene

    private static ItemDatabase instance = null; //Make the class a singleton

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            string jsonFilePath = Application.dataPath + "/StreamingAssets/Items.json"; //Obtain the JSON file path
            string jsontext = File.ReadAllText(jsonFilePath); //Read the file and store the data into a string
            itemData = JsonMapper.ToObject(jsontext); //Load in the JSON data
            ConstructDatabase(); //Construct the item database
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void ConstructDatabase()
    {
        for (int i = 0; i < itemData.Count; i++) //Loop through all of the items in itemData
        {
            JsonData data = itemData[i];//Obtain the item data
            Item newItem = new Item(data); //Create new item
            database.Add(newItem.Title, newItem); //Add item to database
        }
    }
    public static Item GetItem(string itemName) //Finds and returns specifiedd item
    {
        Dictionary<string, Item> database = instance.database;//Create a dictionary to store private variables

        if(database.ContainsKey(itemName)) //Check if the item exists in the database
        {
            return database[itemName];
        }
        return null; //otherwise return null
    }
    public static Dictionary<string,Item> GetDataBase()
    {
        return instance.database;
    }
}
