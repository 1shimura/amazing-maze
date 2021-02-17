namespace Client
{
    public interface IResettable
    {
        void PrewarmSetup();
        void Reset();
    }
}