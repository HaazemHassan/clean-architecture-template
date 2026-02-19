using System.Reflection;

namespace Template.Application.Common.Behaviors.Trimming
{
    public static class TrimExtensions
    {

        public static void ApplyTrim(this object target)
        {
            if (target == null) return;

            var props = target.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p =>
                    p.PropertyType == typeof(string) &&
                    p.CanRead &&
                    p.CanWrite &&
                    !p.IsDefined(typeof(NoTrimAttribute), true)
                );

            foreach (var prop in props)
            {
                var value = prop.GetValue(target) as string;
                if (value != null)
                {
                    prop.SetValue(target, value.Trim());
                }
            }
        }
    }

}
