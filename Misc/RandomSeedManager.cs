using UnityEngine;

public class RandomSeedManager : MonoBehaviour
{
    public static RandomSeedManager instance;

    public StringVariable seedPlayerprefsKey;
    public int cliemtDestinationSeed;
    public int optionalObjectiveSeed;
    public System.Random baseRandom;
    public System.Random cliemtDestinationRandom;
    public System.Random optionalObjectiveRandom;

    public int baseSeed;

    private void Awake()
    {
        instance = this;

        baseSeed = PlayerPrefs.GetInt(seedPlayerprefsKey.Value, -1);
        if(baseSeed < 0)
        {
            baseSeed = Random.Range(0, int.MaxValue);
        }
        baseRandom = new System.Random(baseSeed);

        cliemtDestinationSeed = baseRandom.Next();
        optionalObjectiveSeed = baseRandom.Next();
        cliemtDestinationRandom = new System.Random(cliemtDestinationSeed);
        optionalObjectiveRandom = new System.Random(optionalObjectiveSeed);
    }

    private void Start()
    {
        InitRandomClientDesiredDestination();
    }

    void InitRandomClientDesiredDestination()
    {
        NPC_Client[] clients = FindObjectsOfType<NPC_Client>();
        for (int i = 0; i < clients.Length; i++)
        {
            clients[i].SetDesiredDestination();
        }
    }
}
