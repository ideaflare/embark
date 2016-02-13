using Xunit;
using Embark.Interaction.Concurrency;
using System.Linq;

namespace EmbarkTests.Interaction.Concurrency
{
    public class TestHashLock
    {
        [Fact]
        public void CreatedLocks_AreShared()
        {
            // arrange
            var hl = new HashLock(2);

            // act
            var lock1 = hl.GetLock(1);
            var lock2 = hl.GetLock(2);
            var lock3 = hl.GetLock(3);
            var lock4 = hl.GetLock(4);

            // assert
            Assert.Same(lock1, lock3);
            Assert.Same(lock2, lock4);

            Assert.NotSame(lock1, lock2);
            Assert.NotSame(lock3, lock4);

            Enumerable.Range(1,4)
                .ToList()
                .ForEach(i => Assert.Equal(i, i.GetHashCode()));
        }

        [Fact]
        public void NullObject_ReturnsFirstLock()
        {
            var hl = new HashLock(10);

            var firstLock = hl.GetLock(0);
            var nullLock = hl.GetLock(null);

            Assert.Same(firstLock, nullLock);
            Assert.Equal(0, 0.GetHashCode());
        }

        [Fact]
        public void LockObject_CannotBeModified()
        {
            var hl = new HashLock(1);

            var lock1 = hl.GetLock("");
            var lock2 = hl.GetLock("");

            Assert.Same(lock1, lock2);

            lock1 = 5;
            Assert.NotSame(lock1, lock2);

            Assert.Same(lock2, hl.GetLock(""));
        }

        [Fact]
        public void LockCount_CannotBeLessThanOne()
        {
            Assert.Throws<System.ArgumentException>(() =>
            {
                var hl = new HashLock(0);
            });
        }
    }
}
