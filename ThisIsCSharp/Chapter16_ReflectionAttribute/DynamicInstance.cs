using System;
using System.Reflection;

namespace ThisIsCSharp.Chapter16_ReflectionAttribute
{
    class Profile
    {
        private string name;
        private string phone;
        public Profile()
        {
            name = ""; phone = ""; 
        }

        public Profile(string name, string phone)
        {
            this.name = name;
            this.phone = phone;
        }

        public void Print()
        {
            Console.WriteLine($"{name}, {phone}");
        }

        public string Name
        {
            get { return name; }
            set { name = Name; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = Phone; }
        }
    }

    class DynamicInstance
    {
        static void Main(string[] args)
        {
            Type type = Type.GetType("ThisIsCSharp.Chapter16_ReflectionAttribute.Profile");
            MethodInfo methodInfo = type.GetMethod("Print");
            PropertyInfo nameProperty = type.GetProperty("Name");
            PropertyInfo phoneProperty = type.GetProperty("Phone");

            object profile = Activator.CreateInstance(type, "김세훈", "133-1314");
            methodInfo.Invoke(profile, null);

            profile = Activator.CreateInstance(type);
            nameProperty.SetValue(profile, "박찬호", null);
            phoneProperty.SetValue(profile, "997-1001", null);

            Console.WriteLine("{0}, {1}", 
                nameProperty.GetValue(profile, null), 
                phoneProperty.GetValue(profile, null));
        }
    }
}
