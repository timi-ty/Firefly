namespace Firefly.Core
{
    public interface IBaseBehaviour
    {
        public string Name { get; }
        public void Awaken();
    }
}