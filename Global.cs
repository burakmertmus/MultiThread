using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace multithread
{
    public class Global
    {
        public string[] sutunAdi = { "Age of patient", "Patient's year", "Number of positive", "Survived 5 years", "Died within 5 year" };
        public double[,] aralikDeger;
        public int[,] haberMat;
        public int[] acıkla;
        public int[] sonucMat;
        public int[,] aralikMat = { { 50, 60, 70 }, { 62, 63, 64 }, { 5, 10, 19 } };
        public double[,] EkolonInfo;
        public double[] sutunInfo = new double[3];
        public double[,] treeDugum;
        public double[,] karar1;
        public double[] sagInfo = new double[3];
        public double[] solInfo = new double[3];
        public double[,] kucukSayac = new double[2,4];
        public double[,] buyukSayac = new double[2,4];
        public double[,] kararSutun;
        public int [,]cocuklar=new int[2,5];
        public double[] childDeger = new double[2];

        public Global()
        {
            EkolonInfo = new double[3, 3];
            kararSutun = new double[3,2];
            karar1 = new double[Form1.sutunGlobal, 2];
            treeDugum = new double[Form1.satirGlobal, Form1.sutunGlobal];
            aralikDeger = new double[Form1.sutunGlobal, Form1.sutunGlobal];
            haberMat = new int [Form1.satirGlobal,Form1.sutunGlobal];
            sonucMat = new int[Form1.satirGlobal];
        }
       
    }
}
