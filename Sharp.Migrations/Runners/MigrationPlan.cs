using System.Collections.Generic;
using System.Linq;

namespace Sharp.Migrations.Runners {
    public class MigrationPlan {
        public Direction Direction {
            get { return CurrentVersion <= TargetVersion ? Direction.Up : Direction.Down; }
        }

        public bool IsUp {
            get { return Direction == Direction.Up; }
        }

        public long TargetVersion { get; private set; }
        public long CurrentVersion { get; private set; }
        public List<MigrationInfo> OrderedMigrationsToRun { get; private set; }

        public MigrationPlan(long currentVersion, long targetVersion, List<MigrationInfo> orderedMigrationsToRun) {
            TargetVersion = targetVersion;
            CurrentVersion = currentVersion;
            OrderedMigrationsToRun = orderedMigrationsToRun;
        }
    }
}