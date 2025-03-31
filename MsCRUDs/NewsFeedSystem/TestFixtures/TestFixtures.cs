using AutoFixture;

namespace TestFixtures
{
    public partial class TestFixtures
    {
        public static uint GenerateId() => new Fixture().Create<uint>();

        public static string GenerateString() => new Fixture().Create<string>();
    }
}