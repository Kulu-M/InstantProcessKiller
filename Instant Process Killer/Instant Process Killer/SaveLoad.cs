using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Instant_Process_Killer
{
    public class SaveLoad
    {
        //SPEICHERN
        public static void writeBinary<T>(string file, T zuSpeichern)
        {
            FileStream fs = null;
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                fs = new FileStream(file, FileMode.Create);
                bf.Serialize(fs, zuSpeichern);
                fs.Close();
            }
            catch (Exception)
            {
                if (fs != null)
                    fs.Close();
            }
        }

        //LADEN
        internal static T readBinary<T>(string file)
        {
            T output;
            BinaryFormatter bf = new BinaryFormatter();
            if (File.Exists(file))
            {
                using (FileStream fs = new FileStream(file, FileMode.Open))
                {
                    try
                    {
                        output = (T)bf.Deserialize(fs);
                    }
                    catch (Exception)
                    {
                        output = default(T);
                    }
                }
                return output;
            }
            return default(T);
        }
    }
}
