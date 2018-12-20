using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;

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

            schemaMap = JsonConvert.DeserializeObject<Dictionary<String, SchemaActivity>>(jsonString);

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
                    this.documents[inputName] = metaDocument;
                }

                foreach (var outputName in meta.outputs) {
                    var metaDocument = new MetaDocument { Name = name };
                    this.documents[outputName] = metaDocument;
                }

                foreach (var toolName in meta.tools) {
                    var metaTool = new MetaTool { Name = name };
                    this.tools[toolName] = metaTool;
                }

                var metaProcessGroup = new MetaProcessGroup() { Name = meta.processGroup };
                this.processGroups[meta.processGroup] = metaProcessGroup;

                var metaProcess = new MetaProcess() { Name = meta.process };
                this.processes[meta.process] = metaProcess;
            }
        }

        private void FixupActivities()
        {
            foreach (var entry in this.activities) {
                var name = entry.Key;
                var metaActivity = entry.Value;

                var schemaActivity = this.schemaMap[name];
                foreach(var inputName in schemaActivity.inputs) {
                    metaActivity.inputs[inputName] = this.documents[inputName];
                }

                foreach (var outputName in schemaActivity.outputs) {
                    metaActivity.outputs[outputName] = this.documents[outputName];
                }

                foreach (var toolName in schemaActivity.tools) {
                    metaActivity.tools[toolName] = this.tools[toolName];
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
                    metaDocument.consumingActivities[inputName] = metaActivity;
                    foreach(var toolName in schemaActivity.tools) {
                        metaDocument.consumingTools[toolName] = this.tools[toolName];
                    }
                    metaDocument.consumingProcesses[schemaActivity.process] = this.processes[schemaActivity.process];
                    metaDocument.consumingProcessGroups[schemaActivity.processGroup] = this.processGroups[schemaActivity.processGroup];
                }

                foreach (var outputName in schemaActivity.outputs) {
                    var metaDocument = this.documents[outputName];
                    metaDocument.producingActivities[outputName] = metaActivity;
                    foreach (var toolName in schemaActivity.tools) {
                        metaDocument.producingTools[toolName] = this.tools[toolName];
                    }
                    metaDocument.producingProcesses[schemaActivity.process] = this.processes[schemaActivity.process];
                    metaDocument.producingProcessGroups[schemaActivity.processGroup] = this.processGroups[schemaActivity.processGroup];
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

                metaProcess.activities[name] = metaActivity;
                foreach (var inputName in schemaActivity.inputs) {
                    metaProcess.consumedDocuments[inputName] = this.documents[inputName];
                }
                foreach (var outputName in schemaActivity.outputs) {
                    metaProcess.producedDocuments[outputName] = this.documents[outputName];
                }
                metaProcess.processGroup = processGroups[schemaActivity.processGroup];
                foreach(var toolName in schemaActivity.tools) {
                    metaProcess.tools[toolName] = this.tools[toolName];
                }
            }
        }

        private void fixupProcessGroups()
        {
            foreach (var entry in this.activities) {
                var name = entry.Key;
                var metaActivity = entry.Value;
                var schemaActivity = this.schemaMap[name];

                var processGroupName = schemaActivity.processGroup;
                var metaProcessGroup = this.processGroups[processGroupName];
                metaProcessGroup.activities[name] = metaActivity;
                foreach (var inputName in schemaActivity.inputs) {
                    metaProcessGroup.consumedDocuments[inputName] = this.documents[inputName];
                }
                foreach (var outputName in schemaActivity.outputs) {
                    metaProcessGroup.producedDocuments[outputName] = this.documents[outputName];
                }
                metaProcessGroup.processes[schemaActivity.process] = this.processes[schemaActivity.process];
                foreach (var toolName in schemaActivity.tools) {
                    metaProcessGroup.tools[toolName] = this.tools[toolName];
                }
            }
        }

        private void fixupTools()
        {
            foreach (var entry in this.activities) {
                var name = entry.Key;
                var metaActivity = entry.Value;
                var schemaActivity = this.schemaMap[name];

                foreach (var toolName in schemaActivity.tools) {
                    var metaTool = this.tools[toolName];
  
                    metaTool.activities[name] = metaActivity;
                    foreach (var inputName in schemaActivity.inputs) {
                        metaTool.consumedDocuments[inputName] = this.documents[inputName];
                    }
                    foreach (var outputName in schemaActivity.outputs) {
                        metaTool.producedDocuments[outputName] = this.documents[outputName];
                    }
                    metaTool.processes[schemaActivity.process] = this.processes[schemaActivity.process];
                    metaTool.processGroups[schemaActivity.processGroup] = this.processGroups[schemaActivity.processGroup];
                }
            }
        }
    }
}
