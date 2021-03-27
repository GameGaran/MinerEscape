using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activity : MonoBehaviour
{

    [SerializeField]
    private List<Transform> spotPositions;

    [SerializeField]
    private bool isEnabled = true;

    [SerializeField]
    public ActivityType Type { get; set; }

    private Dictionary<Transform, Miner> occupierMap = new Dictionary<Transform, Miner>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetFreeSpot(Miner occupier)
	{
        if (!isEnabled)
            return null;
        if (new HashSet<Miner>(occupierMap.Values).Contains(occupier))
            return null;
		foreach (var spot in spotPositions)
		{
            if (occupierMap.ContainsKey(spot))
                continue;
            occupierMap.Add(spot, occupier);

            return spot;
		}
        return null;
	}

    public void FreeSpot(Transform transform)
	{
        occupierMap.Remove(transform);
	}

    public void StopActivity()
	{
		foreach (var miner in occupierMap.Values)
		{
            miner.SetActivity(null);
		}
	}

    public enum ActivityType
	{
        PileOfRock,
        WaterPump,
        Gardening,
        Sleeping
	}
}
