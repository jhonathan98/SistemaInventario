using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventario.Data
{
    internal class DataAccess
    {
        private string DataFile;

        public DataAccess(string file)
        {
            DataFile = file;
            if(!File.Exists(DataFile))
            {
                //Si no existe archivo lo crea
                FileStream fs = File.Create(DataFile);
                fs.Close();
            }
        }

        public void Save(string values) 
        {
            //borro archivo anterior
            if(File.Exists(DataFile))
            {
                File.Delete(DataFile);
            }
            //creo archivo con nueva información
            using (FileStream fs = File.Create(DataFile))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(values);
                //Añade informacion al archivo
                fs.Write(info, 0, info.Length);
            }
        }

        public string Read()
        {
            string s = "";
            using(StreamReader sr = new StreamReader(DataFile))
            {
                s = sr.ReadToEnd();
            }
            return s;
        }
    }
}
