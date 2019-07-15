using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace multithread
{
    public partial class Form1 : Form
    {
        
        
        public Form1()
        {
            
            InitializeComponent();
        }



        public static int satirGlobal = 0,sutunGlobal=0;
        
        public static int r=0, kararIndis = 0;
        double InfoSol=0.0, InfoSag=0.0;
        int enbBIndex=0,enbKIndex=0;
        double infoTreeDeger = 0;
        Global cls;
       

        public void dosyaMat()
        {
            int i = 0, j = 0;
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Txt dosyası  |*.txt";
            file.Title = "Aç";
            file.ShowDialog();
            string DosyaYolu = file.FileName;
            //Okuma işlem yapacağımız dosyanın yolunu belirtiyoruz.
            FileStream fs = new FileStream(DosyaYolu, FileMode.Open, FileAccess.Read);
            //Bir file stream nesnesi oluşturuyoruz. 1.parametre dosya yolunu,
            //2.parametre dosyanın açılacağını,
            //3.parametre dosyaya erişimin veri okumak için olacağını gösterir.
            StreamReader sw = new StreamReader(fs);
            string dene = sw.ReadLine();
            
            int sutunSay=0,satirSay = 0;
            string a;
            while (dene != null)
            {
                
                string[] words = dene.Split(',');
                sutunSay = 0;
                foreach (string word in words)
                {
                    a= word;
                    sutunSay++ ;
                }
                
                satirSay++;
                dene = sw.ReadLine();
            }

            sutunGlobal = sutunSay;
            satirGlobal = satirSay;
            sw.Close();
            fs.Close();
            cls = new Global();
             
          
             
            for (int k = 0; k < satirGlobal; k++)
            {
                for (int l = 0; l < sutunGlobal; l++)
                {
                    
                    cls.haberMat[k, l] = 0;
                }
            }
            FileStream fst = new FileStream(DosyaYolu, FileMode.Open, FileAccess.Read);
            StreamReader swt = new StreamReader(fst);
            string satir = swt.ReadLine();
            j = 0;
            while (satir != null)
            {

                j = 0;
                // Split string on spaces.
                // ... This will separate all the words.
                string[] words = satir.Split(',');
                foreach (string word in words)
                {
                   cls.haberMat[i, j] = Convert.ToInt16(word);
                    j++;
                }

                
                i++;
                satir = swt.ReadLine();
            }

            
            swt.Close();
            fst.Close();
        }


        public void sonucMat()
        {
            for(int i=0;i<satirGlobal;i++){
                    cls.sonucMat[i]=cls.haberMat[i,sutunGlobal-1];
            }
            sutunGlobal = sutunGlobal - 1;
        }
        

        public void infoTre()
        {
            double infoSay1 = 0, infoSay2 = 0;
            for (int i = 0; i < satirGlobal;i++ )
            {
                if (cls.sonucMat[i] == 1)
                {
                    infoSay1++;
                }
                else if (cls.sonucMat[i] == 2)
                {
                    infoSay2++;
                }
                
            }

            infoTreeDeger = infoSay1 / satirGlobal * Math.Log(infoSay1 / satirGlobal) + infoSay2 / satirGlobal * Math.Log(infoSay2 / satirGlobal);
            infoTreeDeger=Math.Abs(infoTreeDeger);
            //Console.WriteLine(infoTreeDeger);
        }

        int sayac = 0;
        TreeNode root ;
        public void button1_Click(object sender, EventArgs e)
        {
            
      
            dosyaMat();
            Console.WriteLine("SONUC MAT Bitti");
            sonucMat();
            int k = 0;
            infoTre();
            r = 0;
            while(k < 3){
                
                kararAgac();
                Thread.Sleep(100);
                if(k==3){
                    k = 0;
                }
                else
                {
                    r++;
                }
                k++;
            }
            k = 0;

            cls.kararSutun[0,0] = 0;
            for (int i = 0; i < sutunGlobal; i++)
            {
                for (int j = 0; j < sutunGlobal; j++)
                {

                    if (cls.kararSutun[i,0] < cls.EkolonInfo[i, j])
                    {
                        cls.kararSutun[i, 1] = cls.aralikMat[i,j];
                        cls.kararSutun[i,0] = cls.EkolonInfo[i, j];
                        
                    }
                }
            }

            double enb=0;

            for (int i = 0; i < sutunGlobal;i++ )
            {
                if (enb < cls.kararSutun[i,0])
                {

                    enb = cls.kararSutun[i, 0];
                    
                    kararIndis = i ;
                }
            }
            Console.WriteLine(kararIndis + " " + cls.kararSutun[kararIndis, 1]);

            root = new TreeNode(cls.sutunAdi[kararIndis] + " " + cls.kararSutun[kararIndis, 1]);
            treeView1.Nodes.Add(root);
            startCocukThreads();
           
        }
        public void agacCiz(){
            

    }

        public void kararAgac(){
            
                startKararThreads();
      
        
         
            //Console.WriteLine(karar1[kararIndis, 0] + " " + karar1[kararIndis, 1]);
           
        }
     
        public void startCocukThreads()
        {
            Thread cocukSol = new Thread(new ThreadStart(cocukOlusturSol));
            Thread cocukSag = new Thread(new ThreadStart(cocukOlusturSag));
            cocukSol.Start();
            cocukSag.Start();
            cocukSol.Join();
            cocukSag.Join();
            int kucukYuzde = 0, kucukYuzde1 = 0, buyukYuzde = 0, buyukYuzde1=0;

            // Küçük çocuk ekle
            double max = -1;
            int maxIndis = 0;
            for (int i = 0; i < 3;i++)
            {
                if(max<cls.solInfo[i])
                {
                    max = cls.solInfo[i];
                    maxIndis = i;
                }
            }
            Console.WriteLine(max + " " + maxIndis);
            TreeNode child = new TreeNode(cls.sutunAdi[maxIndis]+" "+cls.kararSutun[maxIndis,1]);
            root.Nodes.Add(child);
            kucukYuzde = Convert.ToInt16(100*(cls.kucukSayac[maxIndis, 0] / (cls.kucukSayac[maxIndis, 0] + cls.kucukSayac[maxIndis, 1])));
            kucukYuzde1 = Convert.ToInt16(100*(cls.kucukSayac[maxIndis, 2] / (cls.kucukSayac[maxIndis, 2] + cls.kucukSayac[maxIndis, 3]))); 

            child.Nodes.Add(new TreeNode(cls.kucukSayac[maxIndis,0]+" "+cls.kucukSayac[maxIndis,1])+ "-- %"+kucukYuzde +" "+cls.sutunAdi[4]);
            child.Nodes.Add(new TreeNode(cls.kucukSayac[maxIndis,2] + " " + cls.kucukSayac[maxIndis,3])+ "-- %"+kucukYuzde1 +" "+cls.sutunAdi[4]);
            // Büyük çocuk ekle
            max = cls.sagInfo[0];
             maxIndis = 0;
            for (int i = 0; i < 3; i++)
            {
                if(cls.sagInfo[i]!=0)
                {
                    if (max < cls.sagInfo[i])
                    {
                        max = cls.sagInfo[i];
                        maxIndis = i;
                    }
                }
            }
            Console.WriteLine(max + " " + maxIndis);
            TreeNode child1 = new TreeNode(cls.sutunAdi[maxIndis] + " " + cls.kararSutun[maxIndis, 1]);
            root.Nodes.Add(child1);
            //Yüzdesel olarak 1 ve 2 değerlerinin hesaplanması            
            buyukYuzde = Convert.ToInt16(100 * (cls.buyukSayac[maxIndis, 0] / (cls.buyukSayac[maxIndis, 0] + cls.buyukSayac[maxIndis, 1])));
            buyukYuzde1 = Convert.ToInt16(100 * (cls.buyukSayac[maxIndis, 2] / (cls.buyukSayac[maxIndis, 2] + cls.buyukSayac[maxIndis, 3])));

            child1.Nodes.Add(new TreeNode(cls.buyukSayac[maxIndis, 0] + " " + cls.buyukSayac[maxIndis, 1] + "-- %" + buyukYuzde + " " + cls.sutunAdi[4]));
            child1.Nodes.Add(new TreeNode(cls.buyukSayac[maxIndis, 2] + " " + cls.buyukSayac[maxIndis, 3] + "-- %" + buyukYuzde1 + " " + cls.sutunAdi[4]));

            /*
             Console.WriteLine(max + " " + maxIndis);
            TreeNode child = new TreeNode(cls.sutunAdi[maxIndis]+" "+cls.kararSutun[maxIndis,1]);
            root.Nodes.Add(child);
            child.Nodes.Add(new TreeNode(cls.kucukSayac[maxIndis,0]+" "+cls.kucukSayac[maxIndis,1])+ "-- %"+kucukYuzde +" "+cls.sutunAdi[4]);
            child.Nodes.Add(new TreeNode(cls.kucukSayac[maxIndis,2] + " " + cls.kucukSayac[maxIndis,3])+ "-- %"+kucukYuzde1 +" "+cls.sutunAdi[4]);
           
             */
        }



        private void cocukOlusturSol(){
            double enb=0;
            double enbSol;
            int enbIndex = 0;
            for(int i=0;i<3;i++)
            {
                if(i!=kararIndis)
                {
                    double sayacK1 = 0;
                    double sayacK2 = 0;
                    double sayacB1 = 0;
                    double sayacB2 = 0;
                    for (int j = 0; j < cls.haberMat.GetLength(0); j++)
                    {
                        if (cls.haberMat[j, kararIndis] < Convert.ToInt32(cls.kararSutun[kararIndis, 1]))
                        {
                            if (cls.haberMat[j, i] < Convert.ToInt32(cls.kararSutun[i,1]))
                            {
                                if (cls.haberMat[j, 3] == 1)
                                {
                                    sayacK1++;
                                }
                                else
                                {
                                    sayacK2++;
                                }
                            }
                            else
                            {
                                if (cls.haberMat[j, 3] == 1)
                                {
                                    sayacB1++;
                                }
                                else
                                {
                                    sayacB2++;
                                }
                            }
                        }

                    }
                    cls.kucukSayac[i, 0] = sayacK1;
                    cls.kucukSayac[i, 1] = sayacK2;
                    cls.kucukSayac[i, 2] = sayacB1;
                    cls.kucukSayac[i, 3] = sayacB2;
                    cls.solInfo[i] = infoTreeDeger - ((sayacK1 + sayacK2) / (sayacB1 + sayacB2 + sayacK2 + sayacK1) * (Math.Abs(sayacK1 / (sayacK1 + sayacK2) * Math.Log(sayacK1 / (sayacK1 + sayacK2)) + sayacK1 / (sayacK1 + sayacK2) * Math.Log(sayacK1 / (sayacK1 + sayacK2)))) + (sayacB1 + sayacB2) / (sayacB1 + sayacB2 + sayacK1 + sayacK2) * (Math.Abs(sayacB1 / (sayacB1 + sayacB2) * Math.Log(sayacB1 / (sayacB1 + sayacB2)) + sayacB1 / (sayacB1 + sayacB2) * Math.Log(sayacB1 / (sayacB1 + sayacB2)))));
                   

                    
                    
                }
            }
             
        }

        public void cocukOlusturSag()
        {
            double enb = 0;
            double enbSol;
            int enbIndex = 0;

            for (int i = 0; i < 3; i++)
            {
                if (i != kararIndis)
                {
                    double sayacK1 = 0;
                    double sayacK2 = 0;
                    double sayacB1 = 0;
                    double sayacB2 = 0;
                    for (int j = 0; j < cls.haberMat.GetLength(0); j++)
                    {
                        if (cls.haberMat[j, kararIndis] >= Convert.ToInt32(cls.kararSutun[kararIndis, 1]))
                        {
                            if (cls.haberMat[j, i] < Convert.ToInt32(cls.kararSutun[i, 1]))
                            {
                                if (cls.haberMat[j, 3] == 1)
                                {
                                    sayacK1++;
                                }
                                else
                                {
                                    sayacK2++;
                                }
                            }
                            else
                            {
                                if (cls.haberMat[j, 3] == 1)
                                {
                                    sayacB1++;
                                }
                                else
                                {
                                    sayacB2++;
                                }
                            }
                        }

                    }
                    cls.buyukSayac[i, 0] = sayacK1;
                    cls.buyukSayac[i, 1] = sayacK2;
                    cls.buyukSayac[i, 2] = sayacB1;
                    cls.buyukSayac[i, 3] = sayacB2;
                    cls.sagInfo[i] = infoTreeDeger - ((sayacK1 + sayacK2) / (sayacB1 + sayacB2 + sayacK2 + sayacK1) * (Math.Abs(sayacK1 / (sayacK1 + sayacK2) * Math.Log(sayacK1 / (sayacK1 + sayacK2)) + sayacK1 / (sayacK1 + sayacK2) * Math.Log(sayacK1 / (sayacK1 + sayacK2)))) + (sayacB1 + sayacB2) / (sayacB1 + sayacB2 + sayacK1 + sayacK2) * (Math.Abs(sayacB1 / (sayacB1 + sayacB2) * Math.Log(sayacB1 / (sayacB1 + sayacB2)) + sayacB1 / (sayacB1 + sayacB2) * Math.Log(sayacB1 / (sayacB1 + sayacB2)))));
                    
                    Console.WriteLine(cls.sagInfo[i]);

                }
            }

        }
    
            public void startKararThreads()
        { 
            Thread th1 = new Thread(new ThreadStart(optimumHesap1));
            th1.Name = "1";
            Thread th2 = new Thread(new ThreadStart(optimumHesap2));
            th2.Name = "2";
            Thread th3 = new Thread(new ThreadStart(optimumHesap3));
            th3.Name = "3";
            th1.Start();
            th2.Start();
            th3.Start();
            th1.Join();
            th2.Join();
            th3.Join();
        }

            public void optimumHesap1()
            {
                double IK=0,IB=0,topSaya;
                for (int i = 0; i < 3; i++)
                {
                    double sayacK1 = 0;
                    double sayacK2 = 0;
                    double sayacB1 = 0;
                    double sayacB2 = 0;
                    for (int j = 0; j < cls.haberMat.GetLength(0); j++)
                    {

                        if (cls.haberMat[j,0] < cls.aralikMat[0,i])
                        {
                            if (cls.haberMat[j,3] == 1)
                            {
                                sayacK1++;
                            }
                            else
                            {
                                sayacK2++;
                            }
                        }
                        else
                        {
                            if (cls.haberMat[j,3] == 1)
                            {
                                sayacB1++;
                            }
                            else
                            {
                                sayacB2++;
                            }
                        }

                    }



                    cls.EkolonInfo[0, i] = infoTreeDeger - ((sayacK1 + sayacK2) / (sayacB1 + sayacB2 + sayacK2 + sayacK1) * (Math.Abs(sayacK1 / (sayacK1 + sayacK2) * Math.Log(sayacK1 / (sayacK1 + sayacK2)) + sayacK1 / (sayacK1 + sayacK2) * Math.Log(sayacK1 / (sayacK1 + sayacK2)))) + (sayacB1 + sayacB2) / (sayacB1 + sayacB2 + sayacK1 + sayacK2) * (Math.Abs(sayacB1 / (sayacB1 + sayacB2) * Math.Log(sayacB1 / (sayacB1 + sayacB2)) + sayacB1 / (sayacB1 + sayacB2) * Math.Log(sayacB1 / (sayacB1 + sayacB2)))));


                }
            }
            private void optimumHesap2()
            {

                for (int i = 0; i < 3; i++)
                {
                    double sayacK1 = 0;
                    double sayacK2 = 0;
                    double sayacB1 = 0;
                    double sayacB2 = 0;
                    for (int j = 0; j < cls.haberMat.GetLength(0); j++)
                    {
                        if (cls.haberMat[j, 1] < cls.aralikMat[1, i])
                        {
                            if (cls.haberMat[j, 3] == 1)
                            {
                                sayacK1++;
                            }
                            else
                            {
                                sayacK2++;
                            }
                        }
                        else
                        {
                            if (cls.haberMat[j, 3] == 1)
                            {
                                sayacB1++;
                            }
                            else
                            {
                                sayacB2++;
                            }
                        }

                    }

                    cls.EkolonInfo[1, i] = infoTreeDeger - ((sayacK1 + sayacK2) / (sayacB1 + sayacB2 + sayacK2 + sayacK1) * (Math.Abs(sayacK1 / (sayacK1 + sayacK2) * Math.Log(sayacK1 / (sayacK1 + sayacK2)) + sayacK1 / (sayacK1 + sayacK2) * Math.Log(sayacK1 / (sayacK1 + sayacK2)))) + (sayacB1 + sayacB2) / (sayacB1 + sayacB2 + sayacK1 + sayacK2) * (Math.Abs(sayacB1 / (sayacB1 + sayacB2) * Math.Log(sayacB1 / (sayacB1 + sayacB2)) + sayacB1 / (sayacB1 + sayacB2) * Math.Log(sayacB1 / (sayacB1 + sayacB2)))));


                }
            }

            private void optimumHesap3()
            {

                for (int i = 0; i < 3; i++)
                {
                    double sayacK1 = 0;
                    double sayacK2 = 0;
                    double sayacB1 = 0;
                    double sayacB2 = 0;
                    for (int j = 0; j < cls.haberMat.GetLength(0); j++)
                    {
                        if (cls.haberMat[j, 2] < cls.aralikMat[2, i])
                        {
                            if (cls.haberMat[j, 3] == 1)
                            {
                                sayacK1++;
                            }
                            else
                            {
                                sayacK2++;
                            }
                        }
                        else
                        {
                            if (cls.haberMat[j, 3] == 1)
                            {
                                sayacB1++;
                            }
                            else
                            {
                                sayacB2++;
                            }
                        }

                    }

                    cls.EkolonInfo[2, i] = infoTreeDeger - ((sayacK1 + sayacK2) / (sayacB1 + sayacB2 + sayacK2 + sayacK1) * (Math.Abs(sayacK1 / (sayacK1 + sayacK2) * Math.Log(sayacK1 / (sayacK1 + sayacK2)) + sayacK1 / (sayacK1 + sayacK2) * Math.Log(sayacK1 / (sayacK1 + sayacK2)))) + (sayacB1 + sayacB2) / (sayacB1 + sayacB2 + sayacK1 + sayacK2) * (Math.Abs(sayacB1 / (sayacB1 + sayacB2) * Math.Log(sayacB1 / (sayacB1 + sayacB2)) + sayacB1 / (sayacB1 + sayacB2) * Math.Log(sayacB1 / (sayacB1 + sayacB2)))));


                }   
            }
         
 
            
    }
}
