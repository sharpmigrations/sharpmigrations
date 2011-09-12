namespace Sharp.Data {
    public class In {

        public string Name { get; set; }
        public object Value { get; set; }

        public static In Named(string name, object parameter) {
            return new In() { Value = parameter, Name = name };
        }

        public static In Par(object parameter) {
            return new In() { Value = parameter };
        }
    }
}
