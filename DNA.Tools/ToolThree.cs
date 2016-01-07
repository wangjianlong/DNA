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
            StartRow2 = 23;
            Init(mdbFilePath);
        }
        public void Working()
        {
            foreach (var region in Regions)
            {
                AParcel aprcel = new AParcel();
                SQLText = string.Format("Select COUNT(*),SUM(WFKTDMJ) from GYYD where WKFTDMJ >0 AND XZJDMC={0}", region);//未开发总规模
                aprcel.HE = ExecuteReader(SQLText);
                SQLText = string.Format("Select COUNT(*),SUM(WFKTDMJ) from GYYD where WKFTDMJ=YDZMJ AND WKFTDMJ>0 AND XZJDMC={0}", region);//整宗未开发
                aprcel.WKF = ExecuteReader(SQLText);
                SQLText = string.Format("Select COUNT(*),SUM(WFKTDMJ) from GYYD where WKFTDMJ>0 AND YKFTDMJ>0 AND XZJDMC={0}", region);
                aprcel.BFWKF = ExecuteReader(SQLText);
                ParcelDict.Add(region, aprcel);
                RegionSum = RegionSum + aprcel;
            }
            foreach (var terrace in Terraces)
            {
                AParcel aprcel = new AParcel();
                SQLText = string.Format("Select COUNT(*),SUM(WFKTDMJ) from GYYD where WKFTDMJ >0 AND CYPTMC Like '%{0}%'", terrace);//未开发总规模
                aprcel.HE = ExecuteReader(SQLText);
                SQLText = string.Format("Select COUNT(*),SUM(WFKTDMJ) from GYYD where WKFTDMJ=YDZMJ AND WKFTDMJ>0 AND CYPTMC Like '%{0}%'", terrace);//整宗未开发
                aprcel.WKF = ExecuteReader(SQLText);
                SQLText = string.Format("Select COUNT(*),SUM(WFKTDMJ) from GYYD where WKFTDMJ>0 AND YKFTDMJ>0 AND CYPTMC Like '%{0}%'", terrace);
                aprcel.BFWKF = ExecuteReader(SQLText);
                TerraceDict.Add(terrace, aprcel);
                TerraceSum = TerraceSum + aprcel;
            }

        }
        private Parcel ExecuteReader(string SQLCommandText)
        {
            Parcel parcel=null;
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
                            Number = int.Parse(reader[0].ToString()),
                            Area = double.Parse(reader[1].ToString())
                        };
                    }
                }
                Connection.Close();
            }
            return parcel;
        }
        public void WriteHelper(AParcel Aparcel, ISheet Sheet, int Row, int Line)
        {
            WriteBase(Aparcel.HE, Sheet, StartRow, StartCell);
            WriteBase(Aparcel.WKF, Sheet, StartRow, StartCell + 2);
            WriteBase(Aparcel.BFWKF, Sheet, StartRow, StartCell + 4);
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
            WriteHelper(TerraceSum, Sheet, 31, StartCell);
        }
    }
}
