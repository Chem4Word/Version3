// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.com.chemspider.www;
using Chem4Word.Core.Helpers;
using IChem4Word.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace Chem4Word.WebServices
{
    public class Chemspider
    {
        private IChem4WordTelemetry Telemetry { get; set; }

        public Chemspider(IChem4WordTelemetry telemetry)
        {
            Telemetry = telemetry;
        }

        public string GetInchiKey(string molfile)
        {
            string module = "GetInchiKey()";
            string result = null;
            DateTime started = DateTime.Now;

            try
            {
                //Log.Debug("Calling ChemSpider WebService");
                Telemetry.Write(module, "Verbose", "Calling WebService");
                InChI i = new InChI();
                i.Url = Globals.Chem4WordV3.SystemOptions.ChemSpiderWebServiceUri + "InChI.asmx";
                i.UserAgent = "Chem4Word";
                i.Timeout = 5000;
                result = i.MolToInChIKey(molfile);
                Debug.WriteLine("ChemSpider Result: " + result);
                //Log.Debug("ChemSpider Result: " + result);
            }
            catch (WebException wex)
            {
                Telemetry.Write(module, "Exception", wex.Message);
                //if (wex.Status != WebExceptionStatus.Timeout)
                //{
                //    Telemetry.Write(module, "Exception(Data)", molfile);
                //}
            }
            catch (Exception ex)
            {
                Telemetry.Write(module, "Exception", ex.Message);
                //Telemetry.Write(module, "Exception(Data)", molfile);
            }

            TimeSpan ts = DateTime.Now - started;
            Telemetry.Write(module, "Timing", "Took " + ts.TotalMilliseconds.ToString("#,###.0", CultureInfo.InvariantCulture) + "ms");

            return result;
        }

        public Dictionary<string, string> GetSynonyms(string inchiKey)
        {
            string module = "GetSynonyms()";

            Dictionary<string, string> result = new Dictionary<string, string>();

            DateTime started = DateTime.Now;

            if (!string.IsNullOrEmpty(inchiKey))
            {
                try
                {
                    //Log.Debug("Getting Chemspider RDF Page");
                    Telemetry.Write(module, "Verbose", "Calling WebService");
                    string url = Globals.Chem4WordV3.SystemOptions.ChemSpiderRdfServiceUri + inchiKey;
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    request.Timeout = 5000;
                    request.UserAgent = "Chem4Word";
                    HttpWebResponse response;

                    response = (HttpWebResponse)request.GetResponse();
                    if (HttpStatusCode.OK.Equals(response.StatusCode))
                    {
                        XmlDocument document = new XmlDocument();
                        document.Load(response.GetResponseStream());
                        XmlNamespaceManager manager = new XmlNamespaceManager(document.NameTable);
                        manager.AddNamespace("foaf", "http://xmlns.com/foaf/0.1/");
                        manager.AddNamespace("chemdomain", "http://www.polymerinformatics.com/ChemAxiom/ChemDomain.owl#");

                        try
                        {
                            string chemspiderUrl = document.DocumentElement.Attributes[0].InnerText;
                            // Url == http://www.chemspider.com/Chemical-Structure.236.rdf
                            string[] parts = chemspiderUrl.Split('/');
                            string chemspiderId = parts[3].Split('.')[1];
                            result.Add(Constants.ChemspiderIdName, chemspiderId);
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                        }

                        //XmlNode imageNode = document.SelectSingleNode("//foaf:Image", manager);

                        XmlNode smilesNode = document.SelectSingleNode("//chemdomain:SMILES", manager);
                        if (smilesNode != null)
                        {
                            result.Add(Constants.ChemSpiderSmilesName, smilesNode.InnerText);
                        }

                        XmlNode synonymsNode = document.SelectSingleNode("//chemdomain:Synonym", manager);
                        if (synonymsNode != null)
                        {
                            result.Add(Constants.ChemSpiderSynonymName, synonymsNode.InnerText);
                        }

                        XmlNode formulaNode = document.SelectSingleNode("//chemdomain:MolecularFormula", manager);
                        if (formulaNode != null)
                        {
                            result.Add(Constants.ChemspiderFormulaName, formulaNode.InnerText);
                        }
                    }
                    else
                    {
                        Telemetry.Write(module, "Error", "Http Status: " + response.StatusCode);
                        //Log.Debug("Chemspider RDF Page - Error - Status code: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    //Log.Error("Chemspider RDF Page - Exception - " + ex.Message);
                    if (ex.Message.Contains("404"))
                    {
                        // Do Nothing
                    }
                    else
                    {
                        Telemetry.Write(module, "Exception", ex.Message);
                    }
                }
            }

            if (!result.ContainsKey(Constants.ChemSpiderSmilesName))
            {
                result.Add(Constants.ChemSpiderSmilesName, "Unknown");
            }
            if (!result.ContainsKey(Constants.ChemSpiderSynonymName))
            {
                result.Add(Constants.ChemSpiderSynonymName, "Unknown");
            }
            if (!result.ContainsKey(Constants.ChemspiderFormulaName))
            {
                result.Add(Constants.ChemspiderFormulaName, "Unknown");
            }

            TimeSpan ts = DateTime.Now - started;
            Telemetry.Write(module, "Timing", "Took " + ts.TotalMilliseconds.ToString("#,###.0", CultureInfo.InvariantCulture) + "ms");

            return result;
        }

        public string GetSynonym(string inchiKey)
        {
            string module = "GetSynonym()";
            string result = null;
            DateTime started = DateTime.Now;

            if (!string.IsNullOrEmpty(inchiKey))
            {
                try
                {
                    //Log.Debug("Getting Chemspider RDF Page");
                    Telemetry.Write(module, "Verbose", "Calling WebService");
                    string url = Globals.Chem4WordV3.SystemOptions.ChemSpiderRdfServiceUri + inchiKey;
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    request.Timeout = 5000;
                    request.UserAgent = "Chem4Word";
                    HttpWebResponse response;

                    response = (HttpWebResponse)request.GetResponse();
                    if (HttpStatusCode.OK.Equals(response.StatusCode))
                    {
                        result = "Unknown";
                        XmlDocument document = new XmlDocument();
                        document.Load(response.GetResponseStream());
                        XmlNamespaceManager manager = new XmlNamespaceManager(document.NameTable);
                        manager.AddNamespace("foaf", "http://xmlns.com/foaf/0.1/");
                        manager.AddNamespace("chemdomain", "http://www.polymerinformatics.com/ChemAxiom/ChemDomain.owl#");

                        string chemspiderUrl = document.DocumentElement.Attributes[0].InnerText;

                        XmlNode imageNode = document.SelectSingleNode("//foaf:Image", manager);
                        XmlNode smilesNode = document.SelectSingleNode("//chemdomain:SMILES", manager);
                        XmlNode synonymsNode = document.SelectSingleNode("//chemdomain:Synonym", manager);
                        result = synonymsNode.InnerText;
                        XmlNode formulaNode = document.SelectSingleNode("//chemdomain:MolecularFormula", manager);
                    }
                    else
                    {
                        Telemetry.Write(module, "Error", "Http Status: " + response.StatusCode);
                        //Log.Debug("Chemspider RDF Page - Error - Status code: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    //Log.Error("Chemspider RDF Page - Exception - " + ex.Message);
                    if (ex.Message.Contains("404"))
                    {
                        result = "Not Found";
                    }
                    else
                    {
                        Telemetry.Write(module, "Exception", ex.Message);
                    }
                }
            }

            TimeSpan ts = DateTime.Now - started;
            Telemetry.Write(module, "Timing", "Took " + ts.TotalMilliseconds.ToString("#,###.0", CultureInfo.InvariantCulture) + "ms");

            return result;
        }

        private string StreamToString(Stream stream)
        {
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private Stream StringToStream(string src)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(src);
            return new MemoryStream(byteArray);
        }
    }
}