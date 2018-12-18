using System;
using System.Collections.Generic;

namespace BST.PMGR
{
    public class SchemaActivity
    {
        public IList<String> inputs { get; set; } = new List<String>();
        public IList<String> outputs { get; set; } = new List<String>();
        public IList<String> tools { get; set; } = new List<String>();
        public String process { get; set; } = "process";
        public String processGroup { get; set; } = "processGroup";
    }
}
