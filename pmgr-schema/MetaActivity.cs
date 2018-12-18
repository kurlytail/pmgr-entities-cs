using System;
using System.Collections.Generic;

namespace BST.PMGR
{
    public class MetaActivity
    {
        public string Name { get; set; }
        public IDictionary<String, MetaDocument> inputs { get; set; } = new Dictionary<String, MetaDocument>();
        public IDictionary<String, MetaDocument> outputs { get; set; } = new Dictionary<String, MetaDocument>();
        public IDictionary<String, MetaTool> tools { get; set; } = new Dictionary<String, MetaTool>();
        public MetaProcess process { get; set; }
        public MetaProcessGroup processGroup { get; set; }
    }
}
