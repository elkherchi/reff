[System.Serializable]
public class LevelData
{
    public int Level;
    public int SeedsCollected;

    public LevelData(int level, int seeds)
    {
        Level = level;
        SeedsCollected = seeds;
    }
}