using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Tool_Request.Helpers
{
    internal class JsonHelper
    {
        private string PathFileSetting;

        private JObject json;

        public JsonHelper(string jsonStringOrPathFile, bool isJsonString = false)
        {
            if (isJsonString)
            {
                if (jsonStringOrPathFile.Trim() == "")
                {
                    jsonStringOrPathFile = "{}";
                }
                json = JObject.Parse(jsonStringOrPathFile);
                return;
            }
            try
            {
                PathFileSetting = "settings\\" + jsonStringOrPathFile + ".json";
                if (!File.Exists(PathFileSetting))
                {
                    using (File.AppendText(PathFileSetting))
                    {
                    }
                }
                json = JObject.Parse(File.ReadAllText(PathFileSetting));
            }
            catch
            {
                json = new JObject();
            }
        }

        public JsonHelper()
        {
            json = new JObject();
        }

        public string GetValue(string key, string valueDefault = "")
        {
            string result = valueDefault;
            try
            {
                result = ((json[key] == null) ? valueDefault : json[key].ToString());
            }
            catch
            {
            }
            return result;
        }

        public List<string> GetValueList(string key, int typeSplitString = 0)
        {
            List<string> list = new List<string>();
            try
            {
                list = ((typeSplitString != 0) ? GetValue(key).Split(new string[1] { "\n|\n" }, StringSplitOptions.RemoveEmptyEntries).ToList() : GetValue(key).Split('\n').ToList());
                list = Helpers.Common.RemoveEmptyItems(list);
            }
            catch
            {
            }
            return list;
        }

        public int GetValueInt(string key, int valueDefault = 0)
        {
            int result = valueDefault;
            try
            {
                result = ((json[key] == null) ? valueDefault : Convert.ToInt32(json[key].ToString()));
            }
            catch
            {
            }
            return result;
        }

        public bool GetValueBool(string key, bool valueDefault = false)
        {
            bool result = valueDefault;
            try
            {
                result = ((json[key] == null) ? valueDefault : Convert.ToBoolean(json[key].ToString()));
            }
            catch
            {
            }
            return result;
        }

        public void Add(string key, string value)
        {
            try
            {
                if (!json.ContainsKey(key))
                {
                    json.Add(key, value);
                }
                else
                {
                    json[key] = value;
                }
            }
            catch (Exception)
            {
            }
        }

        public void Update(string key, object value)
        {
            try
            {
                json[key] = value.ToString();
            }
            catch
            {
            }
        }

        public void Update(string key, List<string> lst)
        {
            try
            {
                json[key] = string.Join("\n", lst).ToString();
            }
            catch
            {
            }
        }

        public void Remove(string key)
        {
            try
            {
                json.Remove(key);
            }
            catch
            {
            }
        }

        public void Save(string pathFileSetting = "")
        {
            try
            {
                if (pathFileSetting == "")
                {
                    pathFileSetting = PathFileSetting;
                }
                File.WriteAllText(pathFileSetting, json.ToString());
            }
            catch
            {
            }
        }

        public string GetFullString()
        {
            string result = "";
            try
            {
                result = json.ToString().Replace("\r\n", "");
            }
            catch (Exception)
            {
            }
            return result;
        }
    }

}
