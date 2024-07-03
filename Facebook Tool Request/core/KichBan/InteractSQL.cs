using Facebook_Tool_Request.core.KichBan;
using System;
using System.Collections.Generic;
using System.Data;

namespace core.KichBan
{
    public class InteractSQL
    {
        public static bool CheckColumnIsExistInTable(string table, string column)
        {
            bool result = false;
            try
            {
                int num = Connector.Instance.ExecuteScalar(string.Concat(new string[]
                {
                    "SELECT COUNT(*) AS count FROM pragma_table_info('",
                    table,
                    "') WHERE name='",
                    column,
                    "'"
                }));
                bool flag = num > 0;
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

        public static DataTable GetKichBanById(string id)
        {
            DataTable result = new DataTable();
            try
            {
                string query = "SELECT * FROM Kich_Ban WHERE Id_KichBan = " + id;
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }

        public static string GetTenKichBanById(string id)
        {
            string result = "";
            try
            {
                string query = "SELECT TenKichBan FROM Kich_Ban WHERE Id_KichBan = " + id;
                result = Connector.Instance.ExecuteQuery(query).Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public static DataTable GetAllKichBan()
        {
            DataTable result = new DataTable();
            try
            {
                string query = "SELECT * FROM Kich_Ban";
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }

        public static DataTable GetKichBanMoi()
        {
            DataTable result = new DataTable();
            try
            {
                string query = "SELECT * FROM Kich_Ban ORDER BY Id_KichBan DESC LIMIT 1";
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }

        public static string GetCauHinhFromKichBan(string Id_KichBan)
        {
            string result = "";
            try
            {
                string query = "SELECT CauHinh FROM Kich_Ban WHERE Id_KichBan = " + Id_KichBan;
                result = Connector.Instance.ExecuteQuery(query).Rows[0]["CauHinh"].ToString();
            }
            catch
            {
            }
            return result;
        }

        public static bool SaveCauHinhFromKichBan(string Id_KichBan, string cauHinh)
        {
            try
            {
                string query = "UPDATE Kich_Ban SET CauHinh = '" + cauHinh + "' WHERE Id_KichBan = " + Id_KichBan;
                return Connector.Instance.ExecuteNonQuery(query) > 0;
            }
            catch
            {
            }
            return false;
        }

        public static bool CheckExistTenKichBan(string tenKichBan)
        {
            try
            {
                string query = "SELECT Id_KichBan FROM Kich_Ban WHERE TenKichBan = '" + tenKichBan + "'";
                bool flag = Connector.Instance.ExecuteQuery(query).Rows.Count > 0;
                if (flag)
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public static bool InsertKichBan(string tenKichBan)
        {
            bool result = false;
            try
            {
                string query = "INSERT INTO Kich_Ban (TenKichBan) VALUES ('" + tenKichBan + "')";
                bool flag = Connector.Instance.ExecuteNonQuery(query) > 0;
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

        public static bool UpdateKichBan(string id, string tenKichBanMoi)
        {
            bool result = false;
            try
            {
                string query = "UPDATE Kich_Ban SET TenKichBan = '" + tenKichBanMoi + "' WHERE Id_KichBan = " + id;
                bool flag = Connector.Instance.ExecuteNonQuery(query) > 0;
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

        public static bool DeleteKichBan(string id)
        {
            bool result = false;
            try
            {
                string query = "DELETE FROM Kich_Ban WHERE Id_KichBan = " + id;
                bool flag = Connector.Instance.ExecuteNonQuery(query) > 0;
                if (flag)
                {
                    InteractSQL.DeleteHanhDongByIdKichBan(id);
                    result = true;
                }
            }
            catch
            {
            }
            return result;
        }

        public static bool DuplicateKichBan(string Id_KichBanCu, string TenKichBanMoi)
        {
            bool result = false;
            try
            {
                bool flag = InteractSQL.InsertKichBan(TenKichBanMoi);
                if (flag)
                {
                    string text = InteractSQL.GetKichBanMoi().Rows[0]["Id_KichBan"].ToString();
                    DataTable dataTable = Connector.Instance.ExecuteQuery("SELECT * FROM Hanh_Dong WHERE Id_KichBan = " + Id_KichBanCu);
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        string query = string.Concat(new string[]
                        {
                            "INSERT INTO Hanh_Dong (STT,Id_KichBan, TenHanhDong, Id_TuongTac, CauHinh) VALUES (",
                            dataTable.Rows[i]["STT"].ToString(),
                            ",",
                            text,
                            ", '",
                            dataTable.Rows[i]["TenHanhDong"].ToString(),
                            "', ",
                            dataTable.Rows[i]["Id_TuongTac"].ToString(),
                            ", '",
                            dataTable.Rows[i]["CauHinh"].ToString(),
                            "')"
                        });
                        Connector.Instance.ExecuteNonQuery(query);
                    }
                    result = true;
                }
            }
            catch
            {
            }
            return result;
        }

        public static DataTable GetTuongTac(string id = "", string ten = "")
        {
            DataTable result = new DataTable();
            try
            {
                string text = "";
                bool flag = id != "";
                if (flag)
                {
                    text = text + "Id_TuongTac = " + id + " AND ";
                }
                bool flag2 = ten != "";
                if (flag2)
                {
                    text = text + "TenTuongTac = '" + ten + "'";
                }
                bool flag3 = text != "";
                if (flag3)
                {
                    bool flag4 = text.EndsWith(" AND ");
                    if (flag4)
                    {
                        text = text.Replace(" AND ", "");
                    }
                    string query = "SELECT * FROM Tuong_Tac WHERE " + text;
                    result = Connector.Instance.ExecuteQuery(query);
                }
            }
            catch
            {
            }
            return result;
        }

        public static DataTable GetAllHanhDongByKichBan(string idKichBan)
        {
            DataTable result = new DataTable();
            try
            {
                string query = "SELECT t1.Id_HanhDong, t1.TenHanhDong,t2.Id_TuongTac,t2.TenTuongTac, t2.MoTa FROM Hanh_Dong t1 JOIN Tuong_Tac t2 ON t1.Id_TuongTac = t2.Id_TuongTac WHERE t1.Id_KichBan = " + idKichBan + " ORDER BY t1.STT";
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }

        public static DataTable GetHanhDongById(string id)
        {
            DataTable result = new DataTable();
            try
            {
                string query = "SELECT t1.TenHanhDong,t1.CauHinh,t2.Id_TuongTac,t2.TenTuongTac, t2.MoTa FROM Hanh_Dong t1 JOIN Tuong_Tac t2 ON t1.Id_TuongTac = t2.Id_TuongTac WHERE t1.Id_HanhDong = " + id;
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }

        public static DataTable GetHanhDongMoi()
        {
            DataTable result = new DataTable();
            try
            {
                string query = "SELECT t1.Id_HanhDong,t1.TenHanhDong,t1.CauHinh,t2.Id_TuongTac,t2.TenTuongTac, t2.MoTa FROM Hanh_Dong t1 JOIN Tuong_Tac t2 ON t1.Id_TuongTac = t2.Id_TuongTac ORDER BY Id_HanhDong DESC LIMIT 1";
                result = Connector.Instance.ExecuteQuery(query);
            }
            catch
            {
            }
            return result;
        }

        public static bool CheckExistTenHanhDong(string tenHanhDong)
        {
            try
            {
                string query = "SELECT Id_HanhDong FROM Hanh_Dong WHERE TenHanhDong = '" + tenHanhDong + "'";
                bool flag = Connector.Instance.ExecuteQuery(query).Rows.Count > 0;
                if (flag)
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public static bool InsertHanhDong(string Id_KichBan, string TenHanhDong, string Id_TuongTac, string CauHinh)
        {
            bool result = false;
            try
            {
                string text = "";
                try
                {
                    text = Connector.Instance.ExecuteQuery("SELECT STT FROM Hanh_Dong WHERE Id_KichBan = " + Id_KichBan + " ORDER BY STT DESC LIMIT 1").Rows[0]["STT"].ToString();
                }
                catch
                {
                }
                text = ((text == "") ? "1" : (Convert.ToInt32(text) + 1).ToString());
                CauHinh = CauHinh.Replace("'", "''");
                string query = string.Concat(new string[]
                {
                    "INSERT INTO Hanh_Dong (STT,Id_KichBan, TenHanhDong, Id_TuongTac, CauHinh) VALUES (",
                    text,
                    ",",
                    Id_KichBan,
                    ", '",
                    TenHanhDong,
                    "', ",
                    Id_TuongTac,
                    ", '",
                    CauHinh,
                    "')"
                });
                bool flag = Connector.Instance.ExecuteNonQuery(query) > 0;
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

        public static bool DuplicateHanhDong(string Id_HanhDong, string TenHanhDongMoi)
        {
            bool result = false;
            try
            {
                DataTable dataTable = Connector.Instance.ExecuteQuery("SELECT * FROM Hanh_Dong WHERE Id_HanhDong = " + Id_HanhDong);
                string text = "";
                try
                {
                    text = Connector.Instance.ExecuteQuery("SELECT STT FROM Hanh_Dong WHERE Id_KichBan = " + dataTable.Rows[0]["Id_KichBan"].ToString() + " ORDER BY STT DESC LIMIT 1").Rows[0]["STT"].ToString();
                }
                catch
                {
                }
                text = ((text == "") ? "1" : (Convert.ToInt32(text) + 1).ToString());
                string query = string.Concat(new string[]
                {
                    "INSERT INTO Hanh_Dong (STT,Id_KichBan, TenHanhDong, Id_TuongTac, CauHinh) VALUES (",
                    text,
                    ",",
                    dataTable.Rows[0]["Id_KichBan"].ToString(),
                    ", '",
                    TenHanhDongMoi,
                    "', ",
                    dataTable.Rows[0]["Id_TuongTac"].ToString(),
                    ", '",
                    dataTable.Rows[0]["CauHinh"].ToString(),
                    "')"
                });
                bool flag = Connector.Instance.ExecuteNonQuery(query) > 0;
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

        public static bool UpdateHanhDong(string Id_HanhDong, string TenHanhDong = "", string CauHinh = "")
        {
            bool result = false;
            try
            {
                string text = "";
                CauHinh = CauHinh.Replace("'", "''");
                bool flag = TenHanhDong != "";
                if (flag)
                {
                    text = text + "TenHanhDong = '" + TenHanhDong + "',";
                }
                bool flag2 = CauHinh != "";
                if (flag2)
                {
                    text = text + "CauHinh = '" + CauHinh + "'";
                }
                bool flag3 = text != "";
                if (flag3)
                {
                    text = text.TrimEnd(new char[]
                    {
                        ','
                    });
                    string query = "UPDATE Hanh_Dong SET " + text + " WHERE Id_HanhDong = " + Id_HanhDong;
                    bool flag4 = Connector.Instance.ExecuteNonQuery(query) > 0;
                    if (flag4)
                    {
                        result = true;
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        public static bool UpdateThuTuHanhDong(string Id_HanhDong1, string Id_HanhDong2)
        {
            bool result = false;
            try
            {
                string str = Connector.Instance.ExecuteQuery("SELECT STT FROM Hanh_Dong WHERE Id_HanhDong = " + Id_HanhDong1).Rows[0]["STT"].ToString();
                string str2 = Connector.Instance.ExecuteQuery("SELECT STT FROM Hanh_Dong WHERE Id_HanhDong = " + Id_HanhDong2).Rows[0]["STT"].ToString();
                string query = "UPDATE Hanh_Dong SET STT = " + str2 + " WHERE Id_HanhDong = " + Id_HanhDong1;
                string query2 = "UPDATE Hanh_Dong SET STT = " + str + " WHERE Id_HanhDong = " + Id_HanhDong2;
                bool flag = Connector.Instance.ExecuteNonQuery(query) > 0 && Connector.Instance.ExecuteNonQuery(query2) > 0;
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

        public static bool DeleteHanhDongByIdHanhDong(string id)
        {
            bool result = false;
            try
            {
                string query = "DELETE FROM Hanh_Dong WHERE Id_HanhDong = " + id;
                bool flag = Connector.Instance.ExecuteNonQuery(query) > 0;
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

        public static bool DeleteHanhDongByIdKichBan(string id)
        {
            bool result = false;
            try
            {
                string query = "DELETE FROM Hanh_Dong WHERE Id_KichBan = " + id;
                bool flag = Connector.Instance.ExecuteNonQuery(query) > 0;
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

        public static List<string> GetIdHanhDongByIdKichBanAndTenTuongTac(string Id_KichBan, string tenTuongTac)
        {
            List<string> list = new List<string>();
            try
            {
                string query = string.Concat(new string[]
                {
                    "SELECT t1.Id_HanhDong FROM Hanh_Dong t1 JOIN Tuong_Tac t2 ON t1.Id_TuongTac = t2.Id_TuongTac WHERE t1.Id_KichBan = ",
                    Id_KichBan,
                    " AND t2.TenTuongTac = '",
                    tenTuongTac,
                    "'"
                });
                DataTable dataTable = Connector.Instance.ExecuteQuery(query);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    list.Add(dataTable.Rows[i]["Id_HanhDong"].ToString());
                }
            }
            catch
            {
            }
            return list;
        }

        public static string GetCauHinhFromHanhDong(string Id_HanhDong)
        {
            string result = "";
            try
            {
                string query = "SELECT CauHinh FROM Hanh_Dong WHERE Id_HanhDong = " + Id_HanhDong;
                result = Connector.Instance.ExecuteQuery(query).Rows[0]["CauHinh"].ToString();
            }
            catch
            {
            }
            return result;
        }
    }

}
