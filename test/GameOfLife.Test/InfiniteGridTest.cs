namespace GameOfLife
{
    using Xunit;
    
    public class InfiniteGridTest : InfiniteGrid<bool>
    {
        [Fact]
        public void can_get_set_cell()
        {
            this.SetCell( 10,  10, true);
            this.SetCell(110,  10, true);
            this.SetCell(210, 110, true);

            Assert.Equal(3, this.LoadedChunks);
            Assert.True(this.GetCell( 10,  10));
            Assert.True(this.GetCell(110,  10));
            Assert.True(this.GetCell(210, 110));
        }

        [Fact]
        public void world_to_chunk_match()
        {
            var pos = this.WorldToChunk(120, 160);
            Assert.Equal(new Point(1, 1), pos.Item1);
            Assert.Equal(new Point(20, 60), pos.Item2);
        }
    }
}
