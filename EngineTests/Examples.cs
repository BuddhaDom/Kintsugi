namespace EngineTests
{
    public class Examples
    {
        [Fact]
        public void Test1()
        {
            Assert.Equal(0, 232 * 0);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(223, 223)]
        public void Test2(int i1, int i2)
        {
            Assert.Equal(i1, 1 * i2);
        }

    }
}