namespace surgical_reports.helpers;

public static class Extensions
    {
         public static string UppercaseFirst(this String inputString)
        {
            if (string.IsNullOrEmpty(inputString)) { return string.Empty; }
            return char.ToUpper(inputString[0]) + inputString.Substring(1).ToLower();
        }
        public static string makeSureTwoChar(this String inputString)
        {
            var help = "";
            if(inputString.Length == 1){help = "0" + inputString;} else {help = inputString;}
            return help;
        }
    }
