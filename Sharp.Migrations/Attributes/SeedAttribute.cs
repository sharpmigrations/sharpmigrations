using System;

namespace Sharp.Migrations.Attributes {
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SeedAttribute : Attribute {
        public string Name { get; set; }

        public SeedAttribute(string name) {
            Name = name;
        }
    }
}