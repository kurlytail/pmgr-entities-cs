using System;
using System.Collections.Generic;

namespace BST.PMGR
{
    public class MetaProcessGroup
    {
        public string Name { get; set; }
        public IDictionary<String, MetaActivity> activities { get; set; } = new Dictionary<String, MetaActivity>();
        public IDictionary<String, MetaDocument> producedDocuments { get; set; } = new Dictionary<String, MetaDocument>();
        public IDictionary<String, MetaDocument> consumedDocuments { get; set; } = new Dictionary<String, MetaDocument>();
        public IDictionary<String, MetaTool> tools { get; set; } = new Dictionary<String, MetaTool>();
        public IDictionary<String, MetaProcess> processes { get; set; } = new Dictionary<String, MetaProcess>();
    }
}
