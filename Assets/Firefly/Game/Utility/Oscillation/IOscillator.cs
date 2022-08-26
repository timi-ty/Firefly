namespace Firefly.Game.Utility
{
    public interface IOscillator
    {
        void SetOscillationConfig(OscillationConfig newConfig);

        void Impulse(OscillationImpulse oscillationImpulse);
    }
}