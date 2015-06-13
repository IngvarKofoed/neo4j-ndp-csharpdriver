using Microsoft.VisualStudio.TestTools.UnitTesting;
using Neo4j.NDP.CSharpDriver.Serialization;

namespace Neo4j.NDP.CSharpDriver.Test.Serialization
{
    [TestClass]
    public class MessageStructureExtensionsTest
    {
        [TestMethod]
        public void OutOfRangeTest()
        {
            // Initialize
            MessageStructure emptyStructue = new MessageStructure(StructureSignature.Init);

            // Run
            IMessageText result = MessageStructureExtensions.TryGetField<IMessageText>(emptyStructue, 1);

            // Validate
            Assert.IsNull(result);
        }

        [TestMethod]
        public void WrongTypeTest()
        {
            // Initialize
            MessageStructure textStructue = new MessageStructure(
                StructureSignature.Init, 
                new MessageText("Test")
            );

            // Run
            IMessageList result = MessageStructureExtensions.TryGetField<IMessageList>(textStructue, 0);

            // Validate
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetFieldTest()
        {
            // Initialize
            const string testString = "Test";
            MessageStructure textStructue = new MessageStructure(
                StructureSignature.Init,
                new MessageText(testString)
            );

            // Run
            IMessageText result = MessageStructureExtensions.TryGetField<IMessageText>(textStructue, 0);

            // Validate
            Assert.IsNotNull(result);
            Assert.AreEqual(testString, result.Text);
        }
    }
}
