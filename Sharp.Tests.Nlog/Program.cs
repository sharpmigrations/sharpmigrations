using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharp.Migrations.Runners;

namespace Sharp.Tests.Nlog {
    class Program {
        static void Main(string[] args) {
            var runner = new ChooseDbConsoleRunner("","");
            runner.Start();

        }
    }
}
