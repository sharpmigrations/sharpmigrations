using System.Collections.Generic;
using System.Linq;

namespace SharpMigrations.Runners {
    public class MigrationPlan {
        public Direction Direction => CurrentVersion <= TargetVersion ? Direction.Up : Direction.Down;
        public bool IsUp => Direction == Direction.Up;
        public long TargetVersion { get; }
        public long CurrentVersion { get; }
        public List<MigrationPlanStep> OrderedSteps { get; }

        public MigrationPlan(List<long> migrationsFromDatabase, 
                             List<MigrationInfo> migrationsFromAssembly,
                             long targetVersion) {
            CurrentVersion = GetCurrentVersion(migrationsFromDatabase);
            TargetVersion = GetTargetVersion(targetVersion, migrationsFromAssembly);
            OrderedSteps = new List<MigrationPlanStep>();

            if (IsMigratingDown()) {
                AddDowns(migrationsFromDatabase, migrationsFromAssembly, TargetVersion);
            }
            AddUps(migrationsFromDatabase, migrationsFromAssembly, TargetVersion);
        }

        private void AddDowns(List<long> migrationsFromDatabase, List<MigrationInfo> migrationsFromAssembly, long targetVersion) {
            OrderedSteps.AddRange(migrationsFromAssembly
                .Where(m => m.Version > targetVersion && migrationsFromDatabase.Contains(m.Version))
                .Select(m => new MigrationPlanStep(Direction.Down, m))
                .Reverse());
        }

        private void AddUps(List<long> migrationsFromDatabase, List<MigrationInfo> migrationsFromAssembly, long targetVersion) {
            OrderedSteps.AddRange(migrationsFromAssembly
                .Where(m => !migrationsFromDatabase.Contains(m.Version) && m.Version <= targetVersion)
                .Select(m => new MigrationPlanStep(Direction.Up, m)));
        }

        private static long GetCurrentVersion(List<long> migrationsFromDatabase) {
            return migrationsFromDatabase.Count == 0 ? 0 : migrationsFromDatabase.Last();
        }

        private static long GetTargetVersion(long targetVersion, List<MigrationInfo> migrationsFromAssembly) {
            if (targetVersion < 0) {
                targetVersion = migrationsFromAssembly.Last().Version;
            }
            return targetVersion;
        }

        private bool IsMigratingDown() {
            return TargetVersion < CurrentVersion;
        }
    }
}