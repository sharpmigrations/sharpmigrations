namespace SharpMigrations.Runners {
    public class MigrationPlanStep {

        public Direction Direction { get; set; }
        public MigrationInfo MigrationInfo { get; set; }
        public bool ShouldUpdateVersion { get; set; }

        public MigrationPlanStep(Direction down, MigrationInfo migrationInfo, bool shouldUpdateVersion = true) {
            Direction = down;
            MigrationInfo = migrationInfo;
            ShouldUpdateVersion = shouldUpdateVersion;
        }
    }
}