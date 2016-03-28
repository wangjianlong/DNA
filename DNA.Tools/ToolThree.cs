using DNA.Models;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DNA.Tools
{
    public class ToolThree:ToolRegion,ITool
    {
        public static string CurrentName
        {
            get
            {
                return "表3工业用地（未开发）基本情况汇总表.xls";
            }
        }
        public Dictionary<string, AParcel> ParcelDict { get; set; }
        public Dictionary<string, AParcel> TerraceDict { get; set; }
        public AParcel RegionSum { get; set; }
        public AParcel TerraceSum { get; set; }
        public ToolThree(string mdbFilePath)
        {
            ParcelDict = new Dictionary<string, AParcel>();
            TerraceDict = new Dictionary<string, AParcel>();
            RegionSum = new AParcel()
            {
                WKF = new Parcel(),
                BFWKF = new Parcel(),
                HE = new Parcel()
            };
            TerraceSum = new AParcel()
            {
                WKF = new Parcel(),
                BFWKF = new Parcel(),
                HE = new Parcel()
            };
            SheetName = "表3";
            StartRow = 5;
            StartCell = 2;
            StartRow2 = 31;
            Init(mdbFilePath);
        }
        public void Doing()
        {
            Working();
        }
        public void Working()
        {
            foreach (var region in Regions)
            {
                AParcel aprcel = new AParcel();
                SQLText = string.Format("Select COUNT(*),SUM(WKFTDMJ) from GYYD where XZJDMC='{0}' AND TDSYQK<>'1'", region);//未开发总规模
                aprcel.HE = ExecuteReader(SQLText);
                SQLText = string.Format("Select COUNT(*),SUM(WKFTDMJ) from GYYD where XZJDMC='{0}' AND TDSYQK='2'", region);//整宗未开发
                aprcel.WKF = ExecuteReader(SQLText);
                SQLText = string.Format("Select COUNT(*),SUM(WKFTDMJ) from GYYD where  XZJDMC='{0}' AND TDSYQK='3'", region);
                aprcel.BFWKF = ExecuteReader(SQLText);
                aprcel = aprcel / 10000;
                ParcelDict.Add(region, aprcel);
                RegionSum = RegionSum + aprcel;
            }
            foreach (var terrace in Terraces)
            {
                AParcel aprcel = new AParcel();
                SQLText = string.Format("Select COUNT(*),SUM(WKFTDMJ) from GYYD where CYPTMC Like '%{0}%' AND TDSYQK<>'1'", terrace);//未开发总规模
                aprcel.HE = ExecuteReader(SQLText);
                SQLText = string.Format("Select COUNT(*),SUM(WKFTDMJ) from GYYD where CYPTMC Like '%{0}%' AND TDSYQK='2'", terrace);//整宗未开发
                aprcel.WKF = ExecuteReader(SQLText);
                SQLText = string.Format("Select COUNT(*),SUM(WKFTDMJ) from GYYD where CYPTMC Like '%{0}%' AND TDSYQK='3'", terrace);
                aprcel.BFWKF = ExecuteReader(SQLText);
                aprcel = aprcel / 10000;
                TerraceDict.Add(terrace, aprcel);
                TerraceSum = TerraceSum + aprcel;
            }

        }
        private Parcel ExecuteReader(string SQLCommandText)
        {
            Parcel parcel=null;
            int a = 0;
            double b = .0;
            using (OleDbConnection Connection = new OleDbConnection(ConnectionString))
            {
                Connection.Open();
                using (OleDbCommand Command = Connection.CreateCommand())
                {
                    Command.CommandText = SQLCommandText;
                    var reader = Command.ExecuteReader();
                    if (reader.Read())
                    {
                        parcel = new Parcel()
                        {
                            Number = int.TryParse(reader[0].ToString(),out a)?a:0,
                            Area = double.TryParse(reader[1].ToString(),out b)?b:.0
                        };
                    }
                }
                Connection.Close();
            }
            return parcel;
        }
        public void WriteHelper(AParcel Aparcel, ISheet Sheet, int Row, int Line)
        {
            WriteBase(Aparcel.HE, Sheet, Row, Line);
            WriteBase(Aparcel.WKF, Sheet, Row, Line + 2);
            WriteBase(Aparcel.BFWKF, Sheet, Row, Line + 4);
        }

        public void Write(ref ISheet Sheet)
        {
            foreach (var region in ParcelDict.Keys)
            {
                Sheet.GetRow(StartRow).GetCell(1).SetCellValue(region);
                var parcel = ParcelDict[region];
                WriteHelper(parcel,Sheet, StartRow++, StartCell);
            }
            WriteHelper(RegionSum, Sheet, StartRow2 - 1, StartCell);
            foreach (var terrace in TerraceDict.Keys)
            {
                Sheet.GetRow(StartRow2).GetCell(1).SetCellValue(terrace);
                WriteHelper(TerraceDict[terrace], Sheet, StartRow2++, StartCell);
            }
            WriteHelper(TerraceSum, Sheet, 51, StartCell);
        }
        public string GetCurrentName()
        {
            return CurrentName;
        }
    }
}
