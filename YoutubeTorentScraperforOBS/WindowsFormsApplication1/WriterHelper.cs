using System.Collections.Generic;
using System.IO;

namespace HelperProject
{
    public class WriterHelper
    {
        public void WriterText(string path,string fileName, List<string> listOfItems)
        {
            //path should be such C:\Users\Genryu\Desktop\
            //filename should be such log.txt

            using (StreamWriter sw = new StreamWriter(path+fileName))
            {
                foreach (string name in listOfItems)
                {
                    sw.WriteLine(name);
                }
            }
        }
    }
}
