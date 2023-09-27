using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBotDemo
{
    public class FileSaver
    {
        private Stream stream;
        private string path;
        public FileSaver(string onlinePath, Stream stream) 
        {
            this.stream = stream;
            path= onlinePath;
            
        }
        public void Save()
        {
            //path += ".mp4";
            using(FileStream file = File.Create(path))
            {
                stream.Seek(0,SeekOrigin.Begin);
                stream.CopyTo(file);

            }
        }
    }
}
