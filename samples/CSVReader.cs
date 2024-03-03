using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSVReader : MonoBehaviour
{
    [SerializeField] private TextAsset textAssetData;
    private RankingInfo[] rankingDataList = new RankingInfo[0];

    private void Start()
    {
        ReadCSV();
    }

    private void ReadCSV()
    {
        // split data into strings
        string[] data = textAssetData.text.Split(new string[] { ";", "\n" }, StringSplitOptions.None);

        // determine the table size, ignore the first row
        int tableSize = data.Length / 3 - 1;
        Debug.Log(tableSize);

        // set the array length to match table size
        rankingDataList = new RankingInfo[tableSize];

        // assign data into ranking data instances
        for(int i = 0; i < tableSize; i++)
        {
            rankingDataList[i] = new RankingInfo();
            rankingDataList[i].index = int.Parse(data[3 * (i + 1)]);
            rankingDataList[i].title = data[3 * (i + 1) + 1];
            rankingDataList[i].description = data[3 * (i + 1) + 2];
        }
    }

    public RankingInfo GetRankingData(int index)
    {
        return rankingDataList[index];
    }
}

[Serializable]
public class RankingInfo
{
    public int index;
    public string title;
    public string description;
}
