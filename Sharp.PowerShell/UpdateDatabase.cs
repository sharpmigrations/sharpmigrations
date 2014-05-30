using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Microsoft.SqlServer.Server;
using Sharp.Migrator;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Sharp.PowerShell {
    [Cmdlet(VerbsData.Update, "Database")]
    public class UpdateDatabase : PSCmdlet {
        private long _version = -1;

        [Parameter(
            Mandatory = false,
            Position = 0,
            HelpMessage = "Version to migrate to"
            )]
        [Alias("v")]
        public long Version {
            get { return _version; }
            set { _version = value; }
        }

        [Parameter(
            Mandatory = false,
            Position = 1,
            HelpMessage = "This is the connection string of the target database"
            )]
        [Alias("c")]
        public string ConnectionString { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 2,
            HelpMessage = "Assembly with migrations"
            )]
        [Alias("assembly", "a")]
        public string AssemblyPath { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 3,
            HelpMessage = "Name of the migration group"
            )]
        [Alias("group", "g")]
        public string MigrationGroup { get; set; }

        [Parameter(
            Mandatory = false,
            Position = 4,
            HelpMessage = "This is the database provider"
            )]
        [Alias("provider", "p")]
        public string DatabaseProvider { get; set; }

        private DteHelper _dte;

        protected override void ProcessRecord() {
            base.ProcessRecord();
            Execute();
        }

        public void Execute() {
            //$installPath is the path to the folder where the package is installed
            //$toolsPath is the path to the tools directory in the folder where the package is installed
            //$package is a reference to the package object.
            //$project is a reference to the EnvDTE project object and represents the project the package is installed into. Note: This will be null in Init.ps1. In that case doesn't have a 
            //var project = (Project) InvokeCommand.InvokeScript("Get-Project", false, PipelineResultTypes.None, null, null)[0].BaseObject;
            
            //project.ConfigurationManager.ActiveConfiguration.OutputGroups.Item(0).
            var projectName = "Sharp.Tests.Northwind";
            _dte = new DteHelper((DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.12.0"));
            var project = FindCurrentProject(projectName);
            AssemblyPath = GetProjectAssemblyPath(project);
            
            var appConfig = ConfigurationManager.OpenExeConfiguration(AssemblyPath);
            ConnectionString = appConfig.ConnectionStrings.ConnectionStrings[0].ConnectionString;
            DatabaseProvider = appConfig.ConnectionStrings.ConnectionStrings[0].ProviderName;

            //string dllConfigData = appConfig.AppSettings.Settings["dllConfigData"].Value;

            var args = new ArgsBuilder()
                .Add("-a", AssemblyPath)
                .Add("-m", "auto")
                .Add("-v", Version)
                .Add("-c", ConnectionString)
                .Add("-p", DatabaseProvider);
            Sharp.Migrator.Program.Main(args.ToArgs());
        }

        private Project FindCurrentProject(string projectName = null) {
            if (projectName == null) {
                return (Project) InvokeCommand.InvokeScript("Get-Project", false, PipelineResultTypes.None, null, null)[0].BaseObject;
            }
            return _dte.GetProject(projectName);
        }

        private static string GetProjectAssemblyPath(Project project) {
            string outputPath =
                project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
            string assemblyName = project.Properties.Item("AssemblyName").Value.ToString();
            string outputType = project.Properties.Item("OutputType").Value.ToString(); // 1 = exe
            string projectFolder = Path.GetDirectoryName(project.FullName);
            string absoluteOutputPath = Path.Combine(projectFolder, outputPath);
            string assemblyPath = Path.Combine(absoluteOutputPath, assemblyName) + (outputType == "1" ? ".exe" : ".dll");
            return assemblyPath;
        }

        //private void GetAssemblyPath() {
        //    var dlls = Directory.EnumerateFiles("..\\", "*.dll", SearchOption.AllDirectories);
        //    dlls = dlls.Concat(Directory.EnumerateFiles(".", "*.exe", SearchOption.AllDirectories));
        //    if (!String.IsNullOrEmpty(AssemblyPath)) return;
        //    foreach (var dll in dlls) {
        //        var assembly = Assembly.Load(dll);
        //        if (assembly.GetTypes()
        //            .Any(x => x.BaseType != null && (x.BaseType.Name == "SchemaMigration" || x.BaseType.Name == "DataMigration"))) {
        //            AssemblyPath = dll;
        //        }
        //    }
        //}
    }

    public class ArgsBuilder {
        private StringBuilder _sb = new StringBuilder();

        public ArgsBuilder Add(string name, object value) {
            if (value == null || value.ToString() == "") {
                return this;
            }
            _sb.Append(name).Append("|").Append(value);
            return this;
        }

        public string[] ToArgs() {
            return _sb.ToString().Split('|');
        }
    }
}