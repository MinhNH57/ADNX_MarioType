using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    //public Guid? Id { get; set; } = Guid.NewGuid();
    //public string playerName { get; set; }
    //public int hightScore { get; set; } = 0;

    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public Guid? id ;

    [JsonProperty("playerName")]
    public string playerName;

    [JsonProperty("hightScore")]
    public int hightScore ;
}
