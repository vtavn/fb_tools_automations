using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Tool_Request.Helpers
{
    public class JSON_Settings
    {
        public JSON_Settings(string jsonStringOrPathFile, bool isJsonString = false)
        {
            if (isJsonString)
            {
                bool flag = jsonStringOrPathFile.Trim() == "";
                if (flag)
                {
                    jsonStringOrPathFile = "{}";
                }
                this.json = JObject.Parse(jsonStringOrPathFile);
            }
            else
            {
                try
                {
                    this.PathFileSetting = "settings\\" + jsonStringOrPathFile + ".json";
                    bool flag2 = !File.Exists(this.PathFileSetting);
                    if (flag2)
                    {
                        using (File.AppendText(this.PathFileSetting))
                        {
                        }
                    }
                    this.json = JObject.Parse(File.ReadAllText(this.PathFileSetting));
                }
                catch
                {
                    this.json = new JObject();
                }
            }
        }

        public JSON_Settings()
        {
            this.json = new JObject();
        }

        public string GetValue(string key, string valueDefault = "")
        {
            string result = valueDefault;
            try
            {
                result = ((this.json[key] == null) ? valueDefault : this.json[key].ToString());
            }
            catch
            {
            }
            return result;
        }

        public static Dictionary<string, object> ConvertToDictionary(JObject jObject)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            try
            {
                dic = jObject.ToObject<Dictionary<string, object>>();
                List<string> list = (from h__TransparentIdentifier0 in dic.Select(delegate (KeyValuePair<string, object> r)
                {
                    KeyValuePair<string, object> keyValuePair2 = r;
                    return new
                    {
                        r = r,
                        key = keyValuePair2.Key
                    };
                })
                                     let value = h__TransparentIdentifier0.r.Value
                                     where value.GetType() == typeof(JObject)
                                     select h__TransparentIdentifier0.key).ToList();


                List<string> list2 = (from h__TransparentIdentifier0 in dic.Select(delegate (KeyValuePair<string, object> r)
                {
                    KeyValuePair<string, object> keyValuePair = r;
                    return new
                    {
                        r = r,
                        key = keyValuePair.Key
                    };
                })
                                      let value = h__TransparentIdentifier0.r.Value
                                      where value.GetType() == typeof(JArray)
                                      select h__TransparentIdentifier0.key).ToList();
                list2.ForEach(delegate (string key)
                {
                    dic[key] = (from x in ((JArray)dic[key]).Values()
                                select ((JValue)x).Value).ToArray();
                });
                list.ForEach(delegate (string key)
                {
                    dic[key] = ConvertToDictionary(dic[key] as JObject);
                });
            }
            catch
            {
            }
            return dic;
        }

        public List<string> GetValueList(string key, int typeSplitString = 0)
        {
            List<string> list = new List<string>();
            try
            {
                bool flag = typeSplitString == 0;
                if (flag)
                {
                    list = this.GetValue(key, "").Split(new char[]
                    {
                        '\n'
                    }).ToList<string>();
                }
                else
                {
                    list = this.GetValue(key, "").Split(new string[]
                    {
                        "\n|\n"
                    }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                }
                list = Common.RemoveEmptyItems(list);
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
                result = ((this.json[key] == null) ? valueDefault : Convert.ToInt32(this.json[key].ToString()));
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
                result = ((this.json[key] == null) ? valueDefault : Convert.ToBoolean(this.json[key].ToString()));
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
                bool flag = !this.json.ContainsKey(key);
                if (flag)
                {
                    this.json.Add(key, value);
                }
                else
                {
                    this.json[key] = value;
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Update(string key, object value)
        {
            try
            {
                this.json[key] = value.ToString();
            }
            catch
            {
            }
        }

        public void Update(string key, List<string> lst)
        {
            try
            {
                this.json[key] = string.Join("\n", lst).ToString();
            }
            catch
            {
            }
        }

        public void Remove(string key)
        {
            try
            {
                this.json.Remove(key);
            }
            catch
            {
            }
        }

        public void Save(string pathFileSetting = "")
        {
            try
            {
                bool flag = pathFileSetting == "";
                if (flag)
                {
                    pathFileSetting = this.PathFileSetting;
                }
                File.WriteAllText(pathFileSetting, this.json.ToString());
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
                result = this.json.ToString().Replace("\r\n", "");
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        private string PathFileSetting;

        private JObject json;
    }
}
