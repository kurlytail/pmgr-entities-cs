using NUnit.Framework;
using System;
using BST.PMGR;

namespace PmgrTest
{
    [TestFixture()]
    public class SchemaServiceTest
    {
        [Test()]
        public void TestSchemaIntrospection()
        {
            var schemaService = new SchemaService();
            Assert.IsNotNull(schemaService.activities);
        }
    }
}
