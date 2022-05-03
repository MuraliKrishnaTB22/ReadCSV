using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;


namespace MonitorCSV
{
    /// <summary>
    /// Main Program .Initially Point for execute the application 
    /// </summary>
    class MainProgram
    {
        static void Main(string[] args)
        {
            Utility.CSV csv= new Utility.CSV();
            Model.Model model;
            List<Model.Model> lstmodel=new List<Model.Model>();
            //Get the filename
            string filename = Path.GetFullPath(Constant.Manifest);
            //Load the data into array
            string[,] values= csv.LoadCSV(filename);
            //Load the data into model
            for (int i = 1; i <= values.GetUpperBound(0); i++)
            {
                model = new Model.Model()
                {
                    OrderNo = values[i, 0],
                    ConsignmentNo = values[i, 1],
                    ParcelCode = values[i, 2],
                    ConsigneeName = values[i, 3],
                    Address1 = values[i, 4],
                    Address2 = values[i, 5],
                    City = values[i, 6],
                    State = values[i, 7],
                    CountryCode = values[i, 8],
                    ItemQuantity = Convert.ToInt32(values[i, 9]),
                    ItemValue = Convert.ToDouble(values[i, 10]),
                    ItemWeight = Convert.ToDouble(values[i, 11]),
                    ItemDescription = values[i, 12],
                    ItemCurrency = String.IsNullOrEmpty(values[i, 13])?"GBP":values[i, 13]
                };               
                    
                    lstmodel.Add(model);
            }
            //group  by for Order no,consignment no,parcel no
            var groupbyorders = lstmodel.GroupBy(x => new { x.OrderNo, x.ConsignmentNo, x.ParcelCode });
            //Xml Creation
            using (XmlWriter writer = XmlWriter.Create("orders.xml"))
            {
                writer.WriteStartElement("Orders");
                foreach (var groupbyorder in groupbyorders)
                {                    
                    writer.WriteElementString("Order", groupbyorder.Key.OrderNo);
                    writer.WriteStartElement("Consignments");
                    writer.WriteElementString("Consignment", groupbyorder.Key.ConsignmentNo);
                    
                    writer.WriteStartElement("Parcels");
                    writer.WriteElementString("Parcel", groupbyorder.Key.ParcelCode);
                    
                    writer.WriteStartElement("ParcelItems");
                    string parcelItem = "";
                    foreach (var group in groupbyorder)
                    {
                        parcelItem += group.ItemDescription;                        
                        
                    }
                    writer.WriteElementString("ParcelItem",parcelItem);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                   
                }
                double itemvalue = lstmodel.Sum(x => x.ItemValue);
                double itemWeight = lstmodel.Sum(x => x.ItemWeight);
                writer.WriteStartElement("TotalValue",itemvalue.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("TotalWeight", itemWeight.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
            
            Console.ReadLine();
        }
    }
}
