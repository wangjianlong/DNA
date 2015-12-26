using DNA.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DNA
{
    class Program
    {
        static void Main(string[] args)
        {
            //MDBHelper.ReadOne("Select XZJDMC from YDDW GROUP BY XZJDMC");
            var list = MDBHelper.GetAllDistrict();
            if (list.Count != 0)
            {
                var dict = list.GetExcelOneData();
                Console.ReadLine();

            }
           
            Console.ReadLine();
        }
    }
}
