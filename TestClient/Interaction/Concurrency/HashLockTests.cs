using Microsoft.VisualStudio.TestTools.UnitTesting;
using Embark.Interaction.Concurrency;
using System.Linq;

namespace TestClient.Interaction.Concurrency
{
    [TestClass]
    public class HashLockTests
    {
        [TestMethod]
        public void CreatedLocks_AreShared()
        {
            //Arrange
            var hl = new HashLock(2);

            //Act
            var lock1 = hl.GetLock(1);
            var lock2 = hl.GetLock(2);
            var lock3 = hl.GetLock(3);
            var lock4 = hl.GetLock(4);

            //Assert
            Assert.AreSame(lock1, lock3);
            Assert.AreSame(lock2, lock4);

            Assert.AreNotSame(lock1, lock2);
            Assert.AreNotSame(lock3, lock4);

            Enumerable.Range(1,4)
                .ToList()
                .ForEach(i => Assert.AreEqual(i, i.GetHashCode()));
        }

        [TestMethod]
        public void NullObject_ReturnsFirstLock()
        {
            var hl = new HashLock(10);

            var firstLock = hl.GetLock(0);
            var nullLock = hl.GetLock(null);

            Assert.AreSame(firstLock, nullLock);
            Assert.AreEqual(0, 0.GetHashCode());
        }

        [TestMethod]
        public void LockObject_CannotBeModified()
        {
            var hl = new HashLock(1);

            var lock1 = hl.GetLock("");
            var lock2 = hl.GetLock("");

            Assert.AreSame(lock1, lock2);

            lock1 = 5;
            Assert.AreNotSame(lock1, lock2);

            Assert.AreSame(lock2, hl.GetLock(""));
        }
        
    }
}
