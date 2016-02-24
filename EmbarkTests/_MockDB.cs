using Embark;
using Embark.Interaction;
using EmbarkTests._Mocks;
using Xunit;

namespace EmbarkTests
{
    public class _MockDB
    {
        internal static Client SharedRuntimeClient = SetupEnvironmentAndGetTestClient();
        //internal static Client SharedDiskClient;

        internal static Collection RuntimeBasicCollection;
        internal static DataEntryCollection<Sound> RuntimeDataEntryCollection;

       

        internal static TestDiskFolder GetTestDiskFolder() => new TestDiskFolder();

        static Client SetupEnvironmentAndGetTestClient()
        {
            SharedRuntimeClient = Client.GetRuntimeDB();

            RuntimeBasicCollection = SharedRuntimeClient.Basic;
            RuntimeDataEntryCollection = SharedRuntimeClient.GetDataEntryCollection<Sound>("ConventionTests");

            Assert.NotNull(RuntimeBasicCollection);

            return SharedRuntimeClient;
        }
    }
}
