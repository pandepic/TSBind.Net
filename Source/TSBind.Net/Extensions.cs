namespace TSBindDotNet
{
    public static class Extensions
    {
        public static bool AddIfNotContains<T>(this List<T> list, T value)
        {
            if (list.Contains(value))
                return false;

            list.Add(value);
            return true;
        }

        public static void AddRangeIfNotContains<T>(this List<T> list, List<T> values)
        {
            foreach (var value in values)
                list.AddIfNotContains(value);
        }
    }
}
