using System;
using System.Collections.Generic;

namespace BST.PMGR
{
    public class MetaDocument
    {
        public string Name { get; set; }
        public IDictionary<String, MetaActivity> producingActivities { get; set; } = new Dictionary<String, MetaActivity>();
        public IDictionary<String, MetaActivity> consumingActivities { get; set; } = new Dictionary<String, MetaActivity>();
        public IDictionary<String, MetaTool> producingTools { get; set; } = new Dictionary<String, MetaTool>();
        public IDictionary<String, MetaTool> consumingTools { get; set; } = new Dictionary<String, MetaTool>();
        public IDictionary<String, MetaProcess> producingProcesses { get; set; } = new Dictionary<String, MetaProcess>();
        public IDictionary<String, MetaProcess> consumingProcesses { get; set; } = new Dictionary<String, MetaProcess>();
        public IDictionary<String, MetaProcessGroup> producingProcessGroups { get; set; } = new Dictionary<String, MetaProcessGroup>();
        public IDictionary<String, MetaProcessGroup> consumingProcessGroups { get; set; } = new Dictionary<String, MetaProcessGroup>();
    }
}
