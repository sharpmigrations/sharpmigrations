using System.Reflection;

namespace Sharp.Data.Util {
    public class ReflectionHelper {
        public static BindingFlags NoRestrictions = BindingFlags.Public |
                                                    BindingFlags.NonPublic |
                                                    BindingFlags.Static |
                                                    BindingFlags.Instance;
    }
}