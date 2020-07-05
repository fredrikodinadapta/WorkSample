using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using WorkSampleExperis.Models;

namespace WorkSampleExperis.Services
{
    public class ConvertService : IConvertService
    {
        public ConvertResult Convert(IFormFile file)
        {

            var result = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.Add(reader.ReadLine());
            }

            var error = "";

            var checkedList = checkList(result);

            if (!checkedList) {
                return new ConvertResult
                {
                    XML = null,
                    Error = "Datan i filen följer inte XMLstandarden."
                };
            }
            else
            {
                var xml = new XElement("people");
                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i].StartsWith("P"))
                    {
                        var itemPerson = result[i].Remove(0, 2);
                        string[] dataPerson = itemPerson.Split("|");

                        xml.Add(new XElement("person",
                            dataPerson.Select((data, index) =>
                             index == 0 ? new XElement("firstname", data) : new XElement("lastname", data)
                            )));

                        var person = xml.Elements("person").LastOrDefault();

                        for (i = i + 1; i < result.Count; i++)
                        {
                            var next = result[i];
                            if (next.StartsWith("A") || next.StartsWith("F") || next.StartsWith("T"))
                            {
                                if (next.StartsWith("A"))
                                {
                                    var itemAdress = next.Remove(0, 2);
                                    string[] dataAdress = itemAdress.Split("|");
                                    person.Add(new XElement("adress",
                                        dataAdress.Select((data, index) =>
                                            index == 0 ? new XElement("street", data) : index == 1 ? new XElement("town", data) : new XElement("postal", data)
                                        )));
                                }

                                if (next.StartsWith("T"))
                                {
                                    var itemTelephone = next.Remove(0, 2);
                                    string[] dataTelephone = itemTelephone.Split("|");

                                    person.Add(new XElement("phone",
                                        dataTelephone.Select((data, index) =>
                                            index == 0 ? new XElement("mobile", data) : new XElement("home", data)
                                        )));
                                }

                                if (next.StartsWith("F"))
                                {

                                    var itemFamily = next.Remove(0, 2);
                                    string[] dataFamily = itemFamily.Split("|");

                                    person.Add(new XElement("family",
                                        dataFamily.Select((data, index) =>
                                            index == 0 ? new XElement("name", data) : new XElement("born", data)
                                        )));

                                    var family = person.Elements("family").LastOrDefault();

                                    for (i = i + 1; i < result.Count; i++)
                                    {

                                        var nextFamily = result[i];
                                        if (nextFamily.StartsWith("A") || nextFamily.StartsWith("T"))
                                        {
                                            if (nextFamily.StartsWith("A"))
                                            {
                                                var itemAdress = nextFamily.Remove(0, 2);
                                                string[] dataAdress = itemAdress.Split("|");
                                                family.Add(new XElement("adress",
                                                    dataAdress.Select((data, index) =>
                                                        index == 0 ? new XElement("street", data) : index == 1 ? new XElement("town", data) : new XElement("postal", data)
                                                    )));
                                            }

                                            if (nextFamily.StartsWith("T"))
                                            {
                                                var itemTelephone = nextFamily.Remove(0, 2);
                                                string[] dataTelephone = itemTelephone.Split("|");

                                                family.Add(new XElement("phone",
                                                    dataTelephone.Select((data, index) =>

                                                    index == 0 ? new XElement("mobile", data) : new XElement("home", data)
                                                    )));
                                            }
                                        }
                                        else
                                        {
                                            i -= 1;
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                i -= 1;
                                break;
                            }

                        }
                    }
                };
           
                return new ConvertResult
                {
                    XML = xml.ToString(),
                    Error = error
                };
            }
        }
            public bool checkList(List<string> result)
            {
                result = result.Select(p => !string.IsNullOrEmpty(p) ? p.Substring(0, 1) : p).ToList();
                bool check = true;
                foreach (var item in result)
                {
                    if (item == "P")
                    {
                        var next = result[result.IndexOf(item) + 1];
                        if (item == next) { check = false; } else { check = true; }
                    }
                    if (item == "F")
                    {
                        var next = result[result.IndexOf(item) + 1];
                        if (next == item || next == "P") { check = false; break; } else { check = true; }
                    }
                }

                return check;
            }
        }
        
    }


