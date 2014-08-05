namespace Uma.Eservices.TestHelpers
{
    using System;

    public static class ClassPropertyInitializator
    {
        public static T SetProperties<T>(T input) where T : class
        {
            T generic = (T)Activator.CreateInstance<T>();

            var cla = input.GetType().GetProperties();

            foreach (var item in cla)
            {
                if (!item.CanWrite)
                {
                    continue;
                }

                string propType = item.PropertyType.FullName;

                if (string.IsNullOrEmpty(propType))
                {
                    continue;
                }

                switch (propType)
                {
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                        item.SetValue(generic, RandomData.GetInteger(10, 100));
                        break;
                    case "System.String":
                        item.SetValue(generic, RandomData.GetStringWordProper());
                        break;
                    case "System.Boolean":
                        item.SetValue(generic, RandomData.GetBool());
                        break;
                    case "System.Uri":
                        item.SetValue(generic, new Uri("http://localhost/" + RandomData.GetString()));
                        break;
                    default:
                        break;
                }
            }

            return input = generic;
        }
    }
}