using System;
using System.Text;

namespace Sharp.Data {
    public static class ExceptionHelper {

        public static string GetAllErrors(Exception ex) {
            var allErrors = new StringBuilder();
            GetAllErrors(allErrors, ex);
            return allErrors.ToString();
        }
        private static void GetAllErrors(StringBuilder sb, Exception ex) {
            sb.AppendLine();
            sb.Append("Error: " + ex.Message);
            if (ex.InnerException != null) {
                GetAllErrors(sb, ex.InnerException);
            }
        }
    }
}