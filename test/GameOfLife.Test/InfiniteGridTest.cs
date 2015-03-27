﻿namespace GameOfLife
{
    using Xunit;
    
    public class InfiniteGridTest : InfiniteGrid<bool>
    {
        [Fact]
        public void can_get_set_cell()
        {
            Assert.True(this.SetCell( 10,  10, true));
            Assert.True(this.SetCell(110,  10, true));
            Assert.True(this.SetCell(210, 110, true));
            Assert.Equal(3, this.LoadedChunks);
            Assert.True(this.GetCell( 10,  10));
            Assert.True(this.GetCell(110,  10));
            Assert.True(this.GetCell(210, 110));
        }

        [Fact]
        public void world_to_chunk_match()
        {
            var pos = this.WorldToChunk(120, 160);
            Assert.Equal(pos.Item1, new Point(1, 1));
            Assert.Equal(pos.Item2, new Point(20, 60));
        }
    }
}
