using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public GenericDictionary<string, int> stats;
    public List<int> startingStats = new List<int>();
    
    public int GetStat(string stat)
    {
        if (stats.TryGetValue(stat, out int value)) {
            return value;
        }
        else {
            Debug.Log($"No stat value found for {stat} on {this.name}");
            return 0;
        }
    }

    public int ChangeStat(string stat, int amount)
    {
        if (startingStats.Count == 0) {
            foreach (string _statName in stats.Keys) {
                startingStats.Add(stats[_statName]);
            }
        }
        if (stats.TryGetValue(stat, out int value)) {
            stats[stat] += amount;
            return stats[stat];
        }
        else {
            Debug.Log($"No stat value found for {stat} on {this.name}");
            return -1;
        }
    }

    public void SetStatsToDefault() // HACK -- REWORK
    {
        for (int i = 0; i < startingStats.Count; i++) {
            stats[stats.Keys.ElementAt(i)] = startingStats[i];
        }
    }
}
