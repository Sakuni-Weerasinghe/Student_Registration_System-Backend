namespace Student_Registration_System_Backend.Helpers
{
    public class HelpingMethods
    {
        public static void TrimStringProperties(object obj)
        {
            // Use reflection to iterate through properties
            foreach (var property in obj.GetType().GetProperties())
            {
                // Check if the property is of type string
                if (property.PropertyType == typeof(string))
                {
                    // Trim leading and trailing spaces from the string property
                    var value = (string)property.GetValue(obj);
                    property.SetValue(obj, value?.Trim());
                }
            }
        }
    }
}
