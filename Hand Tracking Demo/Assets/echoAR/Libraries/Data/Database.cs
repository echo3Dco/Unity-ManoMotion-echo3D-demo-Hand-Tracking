/**************************************************************************
* Copyright (C) echoAR, Inc. 2018-2020.                                   *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at           *
* https://www.echoar.xyz/terms, or another agreement                      *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Database
{
    // API key
    private string apiKey;

    // Database data structure (the API key's entries)
    private Dictionary<string, Entry> db;

    /**
     * Constructor
     * @param apiKey An API key
     */
    public Database(string apiKey)
    {
        this.apiKey = apiKey;
        this.db = new Dictionary<string, Entry>();
    }

    public string getApiKey()
    {
        return apiKey;
    }

    public void setApiKey(string apiKey)
    {
        this.apiKey = apiKey;
    }

    public void addEntry(Entry entry)
    {
        db.Add(entry.getId(), entry);
    }

    public HashSet<Entry> getEntries()
    {
        return new HashSet<Entry>(db.Values);
    }
}

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    // save the dictionary to lists
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // load dictionary from lists
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}

[System.Serializable]
public class DictionaryOfStringAndString : SerializableDictionary<string, string> { }