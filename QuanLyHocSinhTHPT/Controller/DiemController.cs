using Dapper;
using QuanLyHocSinhTHPT.Helper;
using QuanLyHocSinhTHPT.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyHocSinhTHPT.Controller
{
    public class DiemController
    {
        public static List<Diem> getAllDataDiem(string MaHS)
        {
            string query = string.Format("EXEC dbo.GetPointByID @MaHS = '{0}' ", MaHS);
            using (var db = setupConection.ConnectionFactory())
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                return db.Query<Diem>(query).ToList();
            }
        }
        public static List<Diem> getAllDataDiem()
        {
            string query = "EXEC dbo.GetPoint";
            using (var db = setupConection.ConnectionFactory())
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                return db.Query<Diem>(query).ToList();
            }
        }

        public static bool ThemDiem(string _MaMH, string _MaHS, double _DiemMieng, double _Diem15p, double _Diem1h, double _DiemHK)
        {
            if (checkInputGV(_DiemMieng, _Diem15p, _Diem1h, _DiemHK))
            {
                int kt = -1;
                using (var db = setupConection.ConnectionFactory())
                {
                    try
                    {
                        if (db.State == ConnectionState.Closed)
                            db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                             kt = db.Execute("InsDiem",
                                new
                                {
                                    MaHS = _MaHS,
                                    MaMH=_MaMH,
                                    DiemMieng = _DiemMieng,
                                    Diem15p = _Diem15p,
                                    Diem1h = _Diem1h,
                                    DiemHK = _DiemHK
                                },
                                commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            transaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return false;
                    }
                }
                if (kt >= 1) return true;
                else return false;
            }
            return false;
        }

        public static bool SuaDiem(string _MaMH, string _MaHS, double _DiemMieng, double _Diem15p, double _Diem1h, double _DiemHK)
        {
            if (checkInputGV(_DiemMieng, _Diem15p, _Diem1h, _DiemHK))
            {
                int kt = -1;
                using (var db = setupConection.ConnectionFactory())
                {
                    try
                    {
                        if (db.State == ConnectionState.Closed)
                            db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            kt = db.Execute("EditDiem",
                                new
                                {
                                    MaHS = _MaHS,
                                    MaMH = _MaMH,
                                    DiemMieng = _DiemMieng,
                                    Diem15p = _Diem15p,
                                    Diem1h = _Diem1h,
                                    DiemHK = _DiemHK
                                },
                                commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            transaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return false;
                    }
                }
                if (kt >= 1) return true;
                else return false;
            }
            return false;
        }

        public static bool XoaDiem(string _MaHS,string _MaMH)
        {
            if (_MaHS != ""&& _MaMH!="")
            {
                int del = -1;
                using (var db = setupConection.ConnectionFactory())
                {
                    try
                    {
                        if (db.State == ConnectionState.Closed)
                            db.Open();
                        using (var transaction = db.BeginTransaction())
                        {
                            del = db.Execute("DelDiem",
                                new
                                {
                                    MaHS =_MaHS,
                                    MaMH=_MaMH
                                },
                                commandType: CommandType.StoredProcedure,
                                transaction: transaction);
                            transaction.Commit();
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return false;
                    }
                }
                if (del >= 1) return true;
                else return false;
            }
            return false;
        }

        public static bool checkInputGV(double _DiemMieng, double _Diem15p, double _Diem1h, double _DiemHK)
        {
            string errMS = "";
            if (_DiemMieng > 10 || _DiemMieng < 0) { errMS = "Sai Điểm miệng"; }
            if (_Diem15p > 10 || _Diem15p < 0) { errMS = "Sai Điểm 15p"; }
            if (_Diem1h > 10 || _Diem1h < 0) { errMS = "Sai Điểm 1 tiết"; }
            if (_DiemHK > 10 || _DiemHK < 0) { errMS = "Sai Điểm Học kỳ"; }

            if (errMS != "")
            {
                MessageBox.Show(errMS, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
    }
}