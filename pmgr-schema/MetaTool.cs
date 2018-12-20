using System;
using System.Collections.Generic;

namespace BST.PMGR
{
    public class MetaTool
    {
        public string Name { get; set; }
        public IDictionary<String, MetaActivity> activities { get; set; } = new Dictionary<String, MetaActivity>();
        public IDictionary<String, MetaDocument> producedDocuments { get; set; } = new Dictionary<String, MetaDocument>();
        public IDictionary<String, MetaDocument> consumedDocuments { get; set; } = new Dictionary<String, MetaDocument>();
        public IDictionary<String, MetaProcessGroup> processGroups { get; set; } = new Dictionary<String, MetaProcessGroup>();
        public IDictionary<String, MetaProcess> processes { get; set; } = new Dictionary<String, MetaProcess>();
    }
}
