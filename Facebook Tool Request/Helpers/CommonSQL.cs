using Facebook_Tool_Request.core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Facebook_Tool_Request.Helpers
{
    internal class CommonSQL
    {
        public static bool CheckExistTable(string table)
        {
            return Connector.Instance.ExecuteScalar("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='" + table + "';") > 0;
        }
        public static bool CheckUidPageAndUidCloneActiveInvite(string uidClone, string uidPage)
        {
            return Connector.Instance.ExecuteScalar("SELECT COUNT(*) FROM invitePages WHERE (uid_clone='" + uidClone + "' AND uid_page='"+uidPage+"');") > 0;
        }
        public static bool InsertInvitePageToDatabase(string uid_clone, string uid_page)
        {
            bool result = true;
            try
            {
                string query = string.Concat(new string[]
                {
                    "insert into invitePages values(null,'",
                    uid_clone,
                    "','",
                    uid_page,
                    "','",
                    "','",
                    "0','",
                    "','",
                    "','",
                    DateTime.Now.ToString(),
                    "','')"
                });
                Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public static bool CheckExitsFile(string name)
        {
            return Connector.Instance.ExecuteScalar("SELECT COUNT(*) FROM files WHERE name='" + name + "' AND active=1;") > 0;
        }

        public static bool InsertFileToDatabase(string namefile)
        {
            bool result = true;
            try
            {
                string query = string.Concat(new string[]
                {
                    "insert into files values(null,'",
                    namefile,
                    "','",
                    DateTime.Now.ToString(),
                    "',1)"
                });
                Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static DataTable GetAllFilesFromDatabase(bool isShowAll = false)
        {
            DataTable result = new DataTable();
            try
            {
                bool flag = !isShowAll;
                string query;
                if (flag)
                {
                    query = "select id, name from files where active=1";
                }
                else
                {
                    query = string.Concat(new string[]
                    {
                        "select id, name from files where active=1 UNION SELECT -1 AS id, '",
                        Language.GetValue("[Tất cả thư mục]"),
                        "' AS name UNION SELECT 999999 AS id, '",
                        Language.GetValue("[Chọn nhiều thư mục]"),
                        "' AS name ORDER BY id ASC"
                    });
                }
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }

        public static DataTable GetAllFilesFromDatabaseForBin(bool isShowAll = false)
        {
            DataTable result = new DataTable();
            try
            {
                bool flag = !isShowAll;
                string query;
                if (flag)
                {
                    query = "select id, name from files WHERE id IN (SELECT DISTINCT idfile FROM accounts WHERE active=0)";
                }
                else
                {
                    query = string.Concat(new string[]
                    {
                        "select id, name from files WHERE id IN (SELECT DISTINCT idfile FROM accounts WHERE active=0) UNION SELECT -1 AS id, '",
                        Language.GetValue("[Tất cả thư mục]"),
                        "' AS name UNION SELECT 999999 AS id, '",
                        Language.GetValue("[Chọn nhiều thư mục]"),
                        "' AS name ORDER BY id ASC"
                    });
                }
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }

        public static DataTable GetAllInfoFromAccount(List<string> lstIdFile, bool isGetActive = true)
        {
            DataTable result = new DataTable();
            try
            {
                bool flag = lstIdFile == null || lstIdFile.Count == 0;
                string text;
                if (flag)
                {
                    text = "where active=" + (isGetActive ? 1 : 0).ToString();
                }
                else
                {
                    text = "where idfile IN (" + string.Join(",", lstIdFile) + ") AND active=" + (isGetActive ? 1 : 0).ToString();
                }
                string query = string.Concat(new string[]
                {
                    "SELECT '-1' as id, '",
                    Language.GetValue("[Tất cả tình trạng]"),
                    "' AS name UNION select DISTINCT '0' as id,info from accounts ",
                    text,
                    " ORDER BY id ASC"
                });
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }

        public static DataTable GetAccFromFile(List<string> lstIdFile = null, string info = "", bool isGetActive = true)
        {
            DataTable result = new DataTable();
            try
            {
                string text = "WHERE ";
                string text2 = (lstIdFile != null && lstIdFile.Count > 0) ? ("t1.idFile IN (" + string.Join(",", lstIdFile) + ")") : "";
                bool flag = text2 != "";
                if (flag)
                {
                    text = text + text2 + " AND ";
                }
                string text3 = (info != "") ? ("t1.info = '" + info + "'") : "";
                bool flag2 = text3 != "";
                if (flag2)
                {
                    text = text + text3 + " AND ";
                }
                string str = string.Format("t1.active = '{0}'", isGetActive ? 1 : 0);
                text += str;
                string query = "SELECT t1.*, t2.name AS nameFile FROM accounts t1 JOIN files t2 ON t1.idfile=t2.id " + text + " ORDER BY t1.idfile";
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }
        public static bool DeleteFileToDatabase(string idFile)
        {
            bool result = false;
            try
            {
                bool flag = Connector.Instance.ExecuteScalar("SELECT COUNT(idfile) FROM accounts WHERE idfile=" + idFile) == 0;
                bool flag2 = flag;
                if (flag2)
                {
                    result = (Connector.Instance.ExecuteNonQuery("delete from files where id=" + idFile) > 0);
                }
                else
                {
                    bool flag3 = Connector.Instance.ExecuteNonQuery("UPDATE files SET active=0 where id=" + idFile) > 0;
                    if (flag3)
                    {
                        result = CommonSQL.DeleteAccountByIdFile(idFile);
                    }
                }
            }
            catch
            {
            }
            return result;
        }
        public static bool DeleteAllPageById(string uidAdmin)
        {
            bool result = false;
            try
            {
                result = (Connector.Instance.ExecuteNonQuery("delete from pagess where uid=" + uidAdmin) > 0);

            }
            catch { }
            return result;
        }
        public static bool UpdateFileNameToDatabase(string idFile, string nameFile)
        {
            try
            {
                string query = "UPDATE files SET name='" + nameFile + "' where id=" + idFile;
                return Connector.Instance.ExecuteNonQuery(query) > 0;
            }
            catch
            {
            }
            return false;
        }
        public static bool DeleteAccountByIdFile(string idFile)
        {
            bool result = true;
            try
            {
                bool flag = Connector.Instance.ExecuteNonQuery("UPDATE accounts SET active=0, dateDelete='" + DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + "' where idfile=" + idFile) > 0;
                result = flag;
            }
            catch
            {
            }
            return result;
        }
        public static List<string> ConvertToSqlInsertAccount(List<string> lstSqlStatement)
        {
            List<string> list = new List<string>();
            try
            {
                int num = 100;
                int num2 = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal((double)lstSqlStatement.Count * 1.0 / (double)num)));
                for (int i = 0; i < num2; i++)
                {
                    string item = "INSERT INTO accounts(uid, pass,token,cookie1,email,name,friends,groups,birthday,gender,info,fa2,idfile,passmail,useragent,proxy,dateImport,active) VALUES " + string.Join(",", lstSqlStatement.GetRange(num * i, (num * i + num <= lstSqlStatement.Count) ? num : (lstSqlStatement.Count % num)));
                    list.Add(item);
                }
            }
            catch
            {
            }
            return list;
        }
        public static string ConvertToSqlInsertAccount(string uid, string pass, string token, string cookie, string email, string name, string friends, string groups, string birthday, string gender, string info, string fa2, string idFile, string passMail, string useragent, string proxy)
        {
            string text = "";
            try
            {
                text = "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}',1)";
                text = string.Format(text, new object[]
                {
                    uid,
                    pass.Replace("'", "''"),
                    token,
                    cookie,
                    email,
                    name.Replace("'", "''"),
                    friends,
                    groups,
                    birthday,
                    gender,
                    info,
                    fa2,
                    idFile,
                    passMail,
                    useragent,
                    proxy,
                    DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                });
            }
            catch
            {
            }
            return text;
        }

        public static bool CheckColumnIsExistInTable(string table, string column)
        {
            return Connector.Instance.ExecuteScalar(string.Concat(new string[]
            {
                "SELECT COUNT(*) AS count FROM pragma_table_info('",
                table,
                "') WHERE name='",
                column,
                "'"
            })) > 0;
        }
        public static bool AddColumnsIntoTable(string table, string columnName, int typeColumnData)
        {
            bool result = false;
            try
            {
                bool flag = Connector.Instance.ExecuteNonQuery(string.Concat(new string[]
                {
                    "ALTER TABLE ",
                    table,
                    " ADD COLUMN '",
                    columnName,
                    "' ",
                    (typeColumnData == 0) ? "INT" : "TEXT",
                    ";"
                })) > 0;
                if (flag)
                {
                    result = true;
                }
            }
            catch
            {
            }
            return result;
        }

        public static bool UpdateFieldToAccount(List<string> lstId, string fieldName, string fieldValue)
        {
            bool result = false;
            try
            {
                int num = 100;
                int num2 = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal((double)lstId.Count * 1.0 / (double)num)));
                List<string> list = new List<string>();
                string text = "";
                bool flag = fieldName == "pass";
                if (flag)
                {
                    text = ", pass_old=pass";
                }
                for (int i = 0; i < num2; i++)
                {
                    string item = string.Concat(new string[]
                    {
                        "update accounts set ",
                        fieldName,
                        " = '",
                        fieldValue.Replace("'", "''"),
                        "'",
                        text,
                        " where id IN (",
                        string.Join(",", lstId.GetRange(num * i, (num * i + num <= lstId.Count) ? num : (lstId.Count % num))),
                        ")"
                    });
                    list.Add(item);
                }
                bool flag2 = Connector.Instance.ExecuteNonQuery(list) > 0;
                result = flag2;
            }
            catch
            {
            }
            return result;
        }
        public static DataTable GetAccFromId(List<string> lstId)
        {
            DataTable result = new DataTable();
            try
            {
                int num = 100;
                int num2 = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal((double)lstId.Count * 1.0 / (double)num)));
                List<string> list = new List<string>();
                for (int i = 0; i < num2; i++)
                {
                    string item = "SELECT uid, pass, token, cookie1,email, passmail, fa2 FROM accounts WHERE id IN ('" + string.Join("','", lstId.GetRange(num * i, (num * i + num <= lstId.Count) ? num : (lstId.Count % num))) + "')";
                    list.Add(item);
                }
                result = Connector.Instance.ExecuteQuery(list);
            }
            catch (Exception ex)
            {
                Common.ExportError(null, ex, "GetAccFromFile");
            }
            return result;
        }
        public static bool UpdateFieldToAccount(string id, string fieldName, string fieldValue)
        {
            bool result = false;
            try
            {
                string text = "";
                bool flag = fieldName == "pass";
                if (flag)
                {
                    text = ", pass_old=pass";
                }
                string query = string.Concat(new string[]
                {
                    "update accounts set ",
                    fieldName,
                    " = '",
                    fieldValue.Replace("'", "''"),
                    "'",
                    text,
                    " where id=",
                    id
                });
                bool flag2 = Connector.Instance.ExecuteNonQuery(query) > 0;
                result = flag2;
            }
            catch
            {
            }
            return result;
        }
        public static bool DeleteAccountToDatabase(List<string> lstId, bool isReallyDelete = false)
        {
            if (isReallyDelete)
            {
                List<string> list = new List<string>();
                DataTable accFromId = CommonSQL.GetAccFromId(lstId);
                for (int i = 0; i < accFromId.Rows.Count; i++)
                {
                    string text = "";
                    for (int j = 0; j < accFromId.Columns.Count; j++)
                    {
                        text = text + accFromId.Rows[i][j].ToString() + "|";
                    }
                    text = text.Substring(0, text.Length - 1);
                    list.Add(text);
                }
                File.AppendAllText("bin.txt", "======" + DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + "======\r\n");
                File.AppendAllLines("bin.txt", list);
            }
            bool result = true;
            try
            {
                int num = 100;
                int num2 = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal((double)lstId.Count * 1.0 / (double)num)));
                List<string> list2 = new List<string>();
                for (int k = 0; k < num2; k++)
                {
                    string item;
                    if (isReallyDelete)
                    {
                        item = "delete from accounts where id IN (" + string.Join(",", lstId.GetRange(num * k, (num * k + num <= lstId.Count) ? num : (lstId.Count % num))) + ")";
                    }
                    else
                    {
                        item = string.Concat(new string[]
                        {
                            "UPDATE accounts SET active=0, dateDelete='",
                            DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy"),
                            "' where id IN (",
                            string.Join(",", lstId.GetRange(num * k, (num * k + num <= lstId.Count) ? num : (lstId.Count % num))),
                            ")"
                        });
                    }
                    list2.Add(item);
                }
                for (int l = 0; l < list2.Count; l++)
                {
                    result = (Connector.Instance.ExecuteNonQuery(list2[l]) > 0);
                }
            }
            catch (Exception ex)
            {
                Common.ExportError(null, ex, "DeleteAccountToDatabase");
            }
            return result;
        }

        public static DataTable GetAllAccountFromDatabase(bool isGetActive = true)
        {
            DataTable result = new DataTable();
            try
            {
                string query = string.Format("select uid from accounts where active={0};", isGetActive ? 1 : 0);
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }
        public static bool UpdateMultiFieldToAccount(string id, string lstFieldName, string lstFieldValue, bool isAllowEmptyValue = true)
        {
            bool result = false;
            try
            {
                bool flag = lstFieldName.Split(new char[]
                {
                    '|'
                }).Length == lstFieldValue.Split(new char[]
                {
                    '|'
                }).Length;
                if (flag)
                {
                    int num = lstFieldName.Split(new char[]
                    {
                        '|'
                    }).Length;
                    string text = "";
                    for (int i = 0; i < num; i++)
                    {
                        bool flag2 = !isAllowEmptyValue && lstFieldValue.Split(new char[]
                        {
                            '|'
                        })[i].Trim() == "";
                        if (!flag2)
                        {
                            text = string.Concat(new string[]
                            {
                                text,
                                lstFieldName.Split(new char[]
                                {
                                    '|'
                                })[i],
                                "='",
                                lstFieldValue.Split(new char[]
                                {
                                    '|'
                                })[i].Replace("'", "''"),
                                "',"
                            });
                        }
                    }
                    text = text.TrimEnd(new char[]
                    {
                        ','
                    });
                    string query = "update accounts set " + text + " where id=" + id;
                    result = (Connector.Instance.ExecuteNonQuery(query) > 0);
                }
            }
            catch
            {
            }
            return result;
        }
        public static bool UpdateMultiFieldToAccount(List<string> lstId, string lstFieldName, string lstFieldValue)
        {
            bool result = false;
            try
            {
                bool flag = lstFieldName.Split(new char[]
                {
                    '|'
                }).Length == lstFieldValue.Split(new char[]
                {
                    '|'
                }).Length;
                if (flag)
                {
                    int num = lstFieldName.Split(new char[]
                    {
                        '|'
                    }).Length;
                    string text = "";
                    for (int i = 0; i < num; i++)
                    {
                        text = string.Concat(new string[]
                        {
                            text,
                            lstFieldName.Split(new char[]
                            {
                                '|'
                            })[i],
                            "='",
                            lstFieldValue.Split(new char[]
                            {
                                '|'
                            })[i].Replace("'", "''"),
                            "',"
                        });
                    }
                    text = text.TrimEnd(new char[]
                    {
                        ','
                    });
                    int num2 = 100;
                    int num3 = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal((double)lstId.Count * 1.0 / (double)num2)));
                    List<string> list = new List<string>();
                    for (int j = 0; j < num3; j++)
                    {
                        string item = string.Concat(new string[]
                        {
                            "update accounts set ",
                            text,
                            " where id IN (",
                            string.Join(",", lstId.GetRange(num2 * j, (num2 * j + num2 <= lstId.Count) ? num2 : (lstId.Count % num2))),
                            ")"
                        });
                        list.Add(item);
                    }
                    bool flag2 = Connector.Instance.ExecuteNonQuery(list) > 0;
                    result = flag2;
                }
            }
            catch
            {
            }
            return result;
        }
        public static string GetIdFileFromIdAccount(string id)
        {
            try
            {
                return Connector.Instance.ExecuteScalar("SELECT idFile FROM accounts WHERE id='" + id + "'").ToString();
            }
            catch
            {
            }
            return "";
        }
        public static bool UpdateFieldToFile(List<string> lstId, string fieldName, string fieldValue)
        {
            bool result = true;
            try
            {
                string query = string.Concat(new string[]
                {
                    "update files set ",
                    fieldName,
                    " = '",
                    fieldValue,
                    "' where id IN (",
                    string.Join(",", lstId),
                    ")"
                });
                bool flag = Connector.Instance.ExecuteNonQuery(query) > 0;
                result = flag;
            }
            catch
            {
            }
            return result;
        }
        public static bool DeleteFileToDatabaseIfEmptyAccount()
        {
            bool result = false;
            try
            {
                result = (Connector.Instance.ExecuteNonQuery("delete from files where id NOT IN (SELECT DISTINCT idfile FROM accounts)") > 0);
            }
            catch
            {
            }
            return result;
        }
        public static DataTable GetInvitePageGop(bool isGetFull = true)
        {
            DataTable result = new DataTable();
            try
            {
                string query = "";
                if (isGetFull)
                {
                    query = "SELECT uid_clone, GROUP_CONCAT(uid_page) AS uid_pages FROM invitePages WHERE active = 0 GROUP BY uid_clone;";

                }
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }
        public static DataTable GetInvitePage(bool isGetFull = true)
        {
            DataTable result = new DataTable();
            try
            {
                string query = "";
                if (isGetFull)
                {
                    query = "SELECT * FROM invitePages ORDER BY id";

                }
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }
        public static DataTable GetAccFromUid(List<string> lstUid)
        {
            DataTable result = new DataTable();
            try
            {
                int num = 100;
                int num2 = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal((double)lstUid.Count * 1.0 / (double)num)));
                List<string> list = new List<string>();
                for (int i = 0; i < num2; i++)
                {
                    string item = "SELECT t1.*, t2.name AS nameFile FROM accounts t1 JOIN files t2 ON t1.idfile=t2.id WHERE t1.uid IN ('" + string.Join("','", lstUid.GetRange(num * i, (num * i + num <= lstUid.Count) ? num : (lstUid.Count % num))) + "') and t1.active=1 ORDER BY t1.uid";
                    list.Add(item);
                }
                result = Connector.Instance.ExecuteQuery(list);
            }
            catch (Exception ex)
            {
                Common.ExportError(null, ex, "GetAccFromFile");
            }
            return result;
        }

        public static DataTable GetInfoProfileById(string id)
        {
            DataTable result = new DataTable();
            try
            {
                string query = "select * from accounts where uid='"+ id + "';";
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }

        public static bool UpdateInvitePage(string idClone, string idPage,string namePage = "", string note = "OK", string active = "1")
        {
            bool result = true;
            try
            {
                bool flag = Connector.Instance.ExecuteNonQuery("UPDATE invitePages SET active='" + active + "', note='" + note + "', name_page='" + namePage + "', interactEnd='" + DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + "' where uid_clone=" + idClone + " and uid_page=" + idPage) > 0;
                result = flag;
            }
            catch
            {
            }
            return result;
        }

        public static bool DeleteInvitePageToDatabase(List<string> lstId)
        {
            bool result = true;
            try
            {
                int num = 100;
                int num2 = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal((double)lstId.Count * 1.0 / (double)num)));
                List<string> list2 = new List<string>();
                for (int k = 0; k < num2; k++)
                {
                    string item = "delete from invitePages where uid_clone IN (" + string.Join(",", lstId.GetRange(num * k, (num * k + num <= lstId.Count) ? num : (lstId.Count % num))) + ")";
                    list2.Add(item);
                }
                for (int l = 0; l < list2.Count; l++)
                {
                    result = (Connector.Instance.ExecuteNonQuery(list2[l]) > 0);
                }
            }
            catch (Exception ex)
            {
                Common.ExportError(null, ex, "DeleteInvitePageToDatabase");
            }
            return result;
        }

        public static DataTable GetPageFromFile(List<string> lstIdFile = null, string info = "", bool isGetActive = true)
        {
            DataTable result = new DataTable();
            try
            {
                string query = "SELECT DISTINCT pagess.*, accounts.name AS uidName FROM pagess LEFT JOIN accounts ON pagess.uid = accounts.uid;";
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }

        //page

        public static List<string> ConvertToSqlInsertPage(List<string> lstSqlStatement)
        {
            List<string> list = new List<string>();
            try
            {
                int num = 100;
                int num2 = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal((double)lstSqlStatement.Count * 1.0 / (double)num)));
                for (int i = 0; i < num2; i++)
                {
                    string item = "INSERT INTO pagess(pageId,pageName,like,follow,uid,lastInteract,categoryId,tiepcan,tuongtac,ghichu,status,datecreated,idbusiness,interactEnd,info,avatar,token,countgroup) VALUES " + string.Join(",", lstSqlStatement.GetRange(num * i, (num * i + num <= lstSqlStatement.Count) ? num : (lstSqlStatement.Count % num)));
                    list.Add(item);
                }
            }
            catch
            {
            }
            return list;
        }

        public static string ConvertToSqlInsertPage(string pageId, string pageName, string like, string follow, string uid, string lastInteract, string categoryId, string tiepcan, string tuongtac, string ghichu, string status, string datecreated, string idbusiness, string interactEnd, string info, string avatar, string token, string countgroup)
        {
            string text = "";
            try
            {
                text = "('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}')";
                text = string.Format(text, new object[]
                {
                    pageId,
                    pageName.Replace("'", "''"),
                    like,
                    follow,
                    uid,
                    lastInteract,
                    categoryId,
                    tiepcan,
                    tuongtac,
                    ghichu,
                    status,
                    datecreated,
                    idbusiness,
                    interactEnd,
                    info,
                    avatar,
                    token,
                    countgroup
                });
            }
            catch
            {
            }
            return text;
        }

        public static DataTable GetPageFromId(List<string> lstId)
        {
            DataTable result = new DataTable();
            try
            {
                int num = 100;
                int num2 = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal((double)lstId.Count * 1.0 / (double)num)));
                List<string> list = new List<string>();
                for (int i = 0; i < num2; i++)
                {
                    string item = "SELECT * FROM pagess WHERE id IN ('" + string.Join("','", lstId.GetRange(num * i, (num * i + num <= lstId.Count) ? num : (lstId.Count % num))) + "')";
                    list.Add(item);
                }
                result = Connector.Instance.ExecuteQuery(list);
            }
            catch (Exception ex)
            {
                Common.ExportError(null, ex, "GetPageFromId");
            }
            return result;
        }

        public static bool DeletePageToDatabase(List<string> lstId, bool isReallyDelete = false)
        {

            List<string> list = new List<string>();
            DataTable pageFromId = CommonSQL.GetPageFromId(lstId);
            for (int i = 0; i < pageFromId.Rows.Count; i++)
            {
                string text = "";
                for (int j = 0; j < pageFromId.Columns.Count; j++)
                {
                    text = text + pageFromId.Rows[i][j].ToString() + "|";
                }
                text = text.Substring(0, text.Length - 1);
                list.Add(text);
            }
            File.AppendAllText("binpage.txt", "======" + DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy") + "======\r\n");
            File.AppendAllLines("binpage.txt", list);

            bool result = true;
            try
            {
                int num = 100;
                int num2 = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal((double)lstId.Count * 1.0 / (double)num)));
                List<string> list2 = new List<string>();
                for (int k = 0; k < num2; k++)
                {
                    string item = "delete from pagess where id IN (" + string.Join(",", lstId.GetRange(num * k, (num * k + num <= lstId.Count) ? num : (lstId.Count % num))) + ")";

                    list2.Add(item);
                }
                for (int l = 0; l < list2.Count; l++)
                {
                    result = (Connector.Instance.ExecuteNonQuery(list2[l]) > 0);
                }
            }
            catch (Exception ex)
            {
                Common.ExportError(null, ex, "DeletePageToDatabase");
            }
            return result;
        }

        public static bool UpdateFieldToPage(string id, string fieldName, string fieldValue)
        {
            bool result = false;
            try
            {
                string text = "";
                bool flag = fieldName == "pass";
                if (flag)
                {
                    text = ", pass_old=pass";
                }
                string query = string.Concat(new string[]
                {
                    "update pagess set ",
                    fieldName,
                    " = '",
                    fieldValue.Replace("'", "''"),
                    "'",
                    text,
                    " where id=",
                    id
                });
                bool flag2 = Connector.Instance.ExecuteNonQuery(query) > 0;
                result = flag2;
            }
            catch
            {
            }
            return result;
        }

    }
}
