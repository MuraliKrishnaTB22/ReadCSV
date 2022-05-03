using System;
using System.IO;

namespace MonitorCSV.Utility
{
    /// <summary>
    /// CSV Function
    /// </summary>
    public class CSV
    {
        /// <summary>
        /// Load the content into array
        /// </summary>
        /// <param name="filename">Specify the csv filename</param>
        /// <returns></returns>
        public string[,] LoadCSV(string filename)
        {           
            //read the filecontent
            string filecontent = File.ReadAllText(filename);
            filecontent = filecontent.Replace(Constant.nterminator,Constant.rterminator);

            string[] lines = filecontent.Split(new char[] { Constant.rterminator },
                StringSplitOptions.RemoveEmptyEntries);
            
            //find out the rows and columns
            int rows = lines.Length,cols = lines[0].Split(',').Length;
            //allocate the memory in the array
            string[,] values = new string[rows, cols];
                        
            //assign the value into the array
            for (int r = 0; r < rows; r++)
            {
                string[] line_r = lines[r].Split(',');
                for (int c = 0; c < cols; c++)
                {
                    values[r, c] = line_r[c];
                }
            }
                       
            return values;
        }
    }
}
