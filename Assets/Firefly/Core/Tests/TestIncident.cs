using Firefly.Core.Incident;

namespace Firefly.Core.Tests
{
    public class TestIncident : Incident<TestIncident.Data>
    {
        public static TestIncident Instance { get; } = new();
        private TestIncident(){}
        
        public readonly struct Data
        {
            public bool IsMobile { get; }
            public double d1 { get; }
            public double d2 { get; }
            public double d3 { get; }
            public double d4 { get; }
            public double d5 { get; }
            public double d6 { get; }
            public double d7 { get; }
            public double d8 { get; }
            public double d9 { get; }
            public double d10 { get; }
            public double d11 { get; }
            public double d12 { get; }
            public double d13 { get; }
            public double d14 { get; }
            public double d15 { get; }
            public double d16 { get; }
            public double d17 { get; }
            public double d18 { get; }
            public double d19 { get; }
            public double d20 { get; }
            public double d21 { get; }
            public double d22 { get; }
            

            public Data(bool isMobile)
            {
                IsMobile = isMobile;
                d1 = 0;
                d2 = 0;
                d3 = 0;
                d4 = 0;
                d5 = 0;
                d6 = 0;
                d7 = 0;
                d8 = 0;
                d9 = 0;
                d10 = 0;
                d11 = 0;
                d12 = 0;
                d13 = 0;
                d14 = 0;
                d15 = 0;
                d16 = 0;
                d17 = 0;
                d18 = 0;
                d19 = 0;
                d20 = 0;
                d21 = 0;
                d22 = 0;
            }
        }
    }
}