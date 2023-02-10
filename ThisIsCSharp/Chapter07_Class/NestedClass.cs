using System;
using System.Collections.Generic;

namespace ThisIsCSharp.Chapter07_Class
{
    class Configuration
    {
        private class ItemValue
        {
            private string item;
            private string value;

            public void SetValue(Configuration config, string item, string value)
            {
                this.item = item;
                this.value = value;

                bool found = false;
                for (int i = 0; i < config.listConfig.Count; i++)
                {
                    if (config.listConfig[i].item == item)
                    {
                        config.listConfig[i] = this;
                        found = true;
                        break;
                    }
                    if (found == false) config.listConfig.Add(this);
                }
            }
            public string GetItem() { return item; }
            public string GetValue() { return value; }
        }
        List<ItemValue> listConfig = new List<ItemValue>();

        public void setConfig(string item, string value)
        {
            ItemValue iv = new ItemValue();
            iv.SetValue(this, item, value);
        }

        public string GetConfig(string item)
        {
            foreach (ItemValue iv in listConfig)
            {
                if (iv.GetItem() == item) return iv.GetValue();
            }
            return "";
        }
    }
    class NestedClass
    {
        static void Main(string[] args)
        {
            Configuration config = new Configuration();
            config.setConfig("Version", "V 5.0");
            config.setConfig("Size", "665,324 KB");

            Console.WriteLine(config.GetConfig("Version"));
            Console.WriteLine(config.GetConfig("Size"));

            config.setConfig("Version", "V 5.0.1");
            Console.WriteLine(config.GetConfig("Version"));
        }
    }
}
