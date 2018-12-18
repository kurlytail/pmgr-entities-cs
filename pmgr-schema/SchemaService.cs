using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace BST.PMGR
{
    public class SchemaService
    {
        public IDictionary<String, SchemaActivity> schemaMap { get; set; }
            = new Dictionary<String, SchemaActivity>();
        public IDictionary<String, MetaActivity> activities { get; set; }
            = new Dictionary<String, MetaActivity>();
        public IDictionary<String, MetaDocument> documents { get; set; }
            = new Dictionary<String, MetaDocument>();
        public IDictionary<String, MetaTool> tools { get; set; }
            = new Dictionary<String, MetaTool>();
        public IDictionary<String, MetaProcess> processes { get; set; }
            = new Dictionary<String, MetaProcess>();
        public IDictionary<String, MetaProcessGroup> processGroups { get; set; }
            = new Dictionary<String, MetaProcessGroup>();

        public SchemaService()
        {
            var stream = typeof(SchemaService).GetTypeInfo().Assembly.GetManifestResourceStream("pmgrschema.schema.json");
            var streamReader = new StreamReader(stream);
            var jsonString = streamReader.ReadToEnd();

            this.CreateSkeletonMaps();
            this.FixupActivities();
            this.fixupDocuments();
            this.fixupTools();
            this.fixupProcesses();
            this.fixupProcessGroups();
        }

        private void CreateSkeletonMaps() 
        {
            foreach(var entry in this.schemaMap) {
                var name = entry.Key;
                var meta = entry.Value;

                var metaActivity = new MetaActivity { Name = name };
                this.activities.Add(name, metaActivity);

                foreach (var inputName in meta.inputs) {
                    var metaDocument = new MetaDocument { Name = name };
                    this.documents.Add(inputName, metaDocument);
                }

                foreach (var outputName in meta.outputs) {
                    var metaDocument = new MetaDocument { Name = name };
                    this.documents.Add(outputName, metaDocument);
                }

                foreach (var toolName in meta.tools) {
                    var metaTool = new MetaTool { Name = name };
                    this.tools.Add(toolName, metaTool);
                }

                var metaProcessGroup = new MetaProcessGroup() { Name = meta.processGroup };
                this.processGroups.Add(meta.processGroup, metaProcessGroup);

                var metaProcess = new MetaProcess() { Name = meta.process };
                this.processes.Add(meta.process, metaProcess);
            }
        }

        private void FixupActivities()
        {
            foreach (var entry in this.activities) {
                var name = entry.Key;
                var metaActivity = entry.Value;

                var schemaActivity = this.schemaMap[name];
                foreach(var inputName in schemaActivity.inputs) {
                    metaActivity.inputs.Add(inputName, this.documents[inputName]);
                }

                foreach (var outputName in schemaActivity.outputs) {
                    metaActivity.outputs.Add(outputName, this.documents[outputName]);
                }

                foreach (var toolName in schemaActivity.tools) {
                    metaActivity.tools.Add(toolName, this.tools[toolName]);
                }

                metaActivity.process = this.processes[schemaActivity.process];
                metaActivity.processGroup = this.processGroups[schemaActivity.processGroup];
            }
        }

        private void fixupDocuments()
        {
            foreach (var entry in this.activities) {
                var name = entry.Key;
                var metaActivity = entry.Value;
                var schemaActivity = this.schemaMap[name];

                foreach(var inputName in schemaActivity.inputs) {
                    var metaDocument = this.documents[inputName];
                    metaDocument.consumingActivities.Add(inputName, metaActivity);
                    foreach(var toolName in schemaActivity.tools) {
                        metaDocument.consumingTools.Add(toolName, this.tools[toolName]);
                    }
                    metaDocument.consumingProcesses.Add(schemaActivity.process, this.processes[schemaActivity.process]);
                    metaDocument.consumingProcessGroups.Add(schemaActivity.processGroup, this.processGroups[schemaActivity.processGroup]);
                }

                foreach (var outputName in schemaActivity.outputs) {
                    var metaDocument = this.documents[outputName];
                    metaDocument.producingActivities.Add(outputName, metaActivity);
                    foreach (var toolName in schemaActivity.tools) {
                        metaDocument.producingTools.Add(toolName, this.tools[toolName]);
                    }
                    metaDocument.producingProcesses.Add(schemaActivity.process, this.processes[schemaActivity.process]);
                    metaDocument.producingProcessGroups.Add(schemaActivity.processGroup, this.processGroups[schemaActivity.processGroup]);
                }
            }
        }

        private void fixupProcesses()
        {
            foreach (var entry in this.activities) {
                var name = entry.Key;
                var metaActivity = entry.Value;
                var schemaActivity = this.schemaMap[name];

                var process = schemaActivity.process;
                var metaProcess = this.processes[process];

                metaProcess.activities.Add(name, metaActivity);
                foreach (var inputName in schemaActivity.inputs) {
                    metaProcess.consumedDocuments.Add(inputName, this.documents[inputName]);
                }
                foreach (var outputName in schemaActivity.outputs) {
                    metaProcess.producedDocuments.Add(outputName, this.documents[outputName]);
                }
                metaProcess.processGroup = processGroups[schemaActivity.process];
                foreach(var toolName in schemaActivity.tools) {
                    metaProcess.tools.Add(toolName, this.tools[toolName]);
                }
            }
        }

        private void fixupProcessGroups()
        {

        }

        private void fixupTools()
        {

        }
    }
}
