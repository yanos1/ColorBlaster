namespace Interfaces
{
    public interface IChildElement : IObstacleElement
    {
        public void NotifyParent();
        public void SetParent();
    }
}