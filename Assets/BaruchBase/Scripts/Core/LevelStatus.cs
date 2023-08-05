namespace Baruch
{

    [System.Flags]
    public enum LevelStatus
    {
        HasMarbleInBottle = 1 << 0,
        NoMarbleInBottle = 1 << 1,
        TargetReached = 1 << 2,
        TargetFailed = 1 << 3,
        Success = NoMarbleInBottle | TargetReached,
    }


}