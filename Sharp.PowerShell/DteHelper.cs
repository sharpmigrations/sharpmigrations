using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;

namespace Sharp.PowerShell {
    public class DteHelper {
        private DTE2 _dte;
        private List<Project> _projects; 

        public DteHelper(DTE2 dte) {
            _dte = dte;
            _projects = new List<Project>();
            FindAllProjects();
        }

        private void FindAllProjects() {
            var ps = _dte.Solution.Projects.Cast<Project>().ToList();
            FindSubProjects(ps);
        }

        private void FindSubProjects(List<Project> projects) {
            foreach (var project in projects) {
                try {
                    _projects.AddRange(GetProjects(project));
                }
                catch (Exception ex) { }
            }
        }

        public static List<Project> GetProjects(Project rootProject) {
            IEnumerator enumerator = rootProject.ProjectItems.GetEnumerator();
            var list = new List<Project>();
            if (IsProjectKind(rootProject)) {
                list.Add(rootProject);
            }
            while (enumerator.MoveNext()) {
                var item = enumerator.Current as ProjectItem;
                if (item == null) {
                    continue;                    
                }
                var project = item.SubProject;
                if (project == null) {
                    continue;
                }
                switch (project.Kind) {
                    case ("{66A26720-8FB5-11D2-AA7E-00C04F688DDE}"): //vsProjectKindSolutionFolder add subitems
                        list.AddRange(GetProjects(project));
                        break;
                    case ("{66A2671D-8FB5-11D2-AA7E-00C04F688DDE}"): //vsProjectKindMisc do not add
                        break;
                    default:
                        list.Add(project); // add
                        break;
                }
            }
            return list;
        }

        private static bool IsProjectKind(Project project) {
            return project.Kind != "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}" &&
                   project.Kind != "{66A2671D-8FB5-11D2-AA7E-00C04F688DDE}";
        }


        public void PlotProjects() {
            foreach (var project in _projects) {
                Console.WriteLine(project.Name);
            }
        }

        public Project GetProject(string name) {
            return _projects.FirstOrDefault(x => x.Name == name);
        }
    }
}
