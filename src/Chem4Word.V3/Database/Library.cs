// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.Helpers;
using Chem4Word.Core.UI.Forms;
using Chem4Word.Model.Converters.CML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace Chem4Word.Database
{
    public class Library
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public Library()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            string libraryTarget = Path.Combine(Globals.Chem4WordV3.AddInInfo.ProgramDataPath, Constants.LibraryFileName);

            if (!File.Exists(libraryTarget))
            {
                Globals.Chem4WordV3.Telemetry.Write(module, "Information", "Copying initial Library database");
                ResourceHelper.WriteResource(Assembly.GetExecutingAssembly(), "Data.Library.db", libraryTarget);
            }
        }

        private SQLiteConnection LibraryConnection()
        {
            string path = Path.Combine(Globals.Chem4WordV3.AddInInfo.ProgramDataPath, Constants.LibraryFileName);
            // Source https://www.connectionstrings.com/sqlite/
            var conn = new SQLiteConnection($"Data Source={path};Synchronous=Full");
            return conn.OpenAndReturn();
        }

        public Dictionary<string, int> GetLibraryNames()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Dictionary<string, int> allNames = new Dictionary<string, int>();

                using (SQLiteConnection conn = LibraryConnection())
                {
                    using (SQLiteDataReader names = GetAllNames(conn))
                    {
                        while (names.Read())
                        {
                            string name = names["Name"] as string;
                            if (!string.IsNullOrEmpty(name) && name.Length > 3)
                            {
                                int id = int.Parse(names["ChemistryId"].ToString());
                                if (!allNames.ContainsKey(name))
                                {
                                    allNames.Add(name, id);
                                }
                            }
                        }
                    }
                }

                return allNames;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }

        public void DeleteAllChemistry()
        {
            using (SQLiteConnection conn = LibraryConnection())
            {
                DeleteAllChemistry(conn);
            }
        }

        public bool ImportCml(string cmlFile)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            bool result = false;

            try
            {
                var converter = new CMLConverter();
                Model.Model model = converter.Import(cmlFile);

                if (model.AllAtoms.Count > 0)
                {
                    double before = model.MeanBondLength;
                    if (before < Constants.MinimumBondLength - Constants.BondLengthTolerance
                        || before > Constants.MaximumBondLength + Constants.BondLengthTolerance)
                    {
                        model.ScaleToAverageBondLength(Constants.StandardBondLength);
                        double after = model.MeanBondLength;
                        Globals.Chem4WordV3.Telemetry.Write(module, "Information", $"Structure rescaled from {before.ToString("#0.00")} to {after.ToString("#0.00")}");
                    }

                    // Ensure each molecule has a Consise Formula set
                    foreach (var molecule in model.Molecules)
                    {
                        if (string.IsNullOrEmpty(molecule.ConciseFormula))
                        {
                            molecule.ConciseFormula = molecule.CalculatedFormula();
                        }
                    }

                    CMLConverter cmlConverter = new CMLConverter();
                    model.CustomXmlPartGuid = "";
                    var cml = cmlConverter.Export(model);

                    string chemicalName = model.ConciseFormula;
                    var mol = model.Molecules[0];
                    if (mol.ChemicalNames.Count > 0)
                    {
                        foreach (var name in mol.ChemicalNames)
                        {
                            long temp;
                            if (!long.TryParse(name.Name, out temp))
                            {
                                chemicalName = name.Name;
                                break;
                            }
                        }
                    }

                    using (SQLiteConnection conn = LibraryConnection())
                    {
                        var id = AddChemistry(conn, cml, chemicalName, model.ConciseFormula);
                        foreach (var name in mol.ChemicalNames)
                        {
                            AddChemicalName(conn, id, name.Name, name.DictRef);
                        }
                    }

                    result = true;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }

            return result;
        }

        public void UpdateChemistry(long id, string name, string xml, string formula)
        {
            using (SQLiteConnection conn = LibraryConnection())
            {
                UpdateChemistry(conn, id, name, xml, formula);
            }
        }

        private void UpdateChemistry(SQLiteConnection conn, long id, string name, string xml, string formula)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                Byte[] blob = Encoding.UTF8.GetBytes(xml);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("UPDATE GALLERY");
                sb.AppendLine("SET Name = @name, Chemistry = @blob, Formula = @formula");
                sb.AppendLine("WHERE ID = @id");

                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                command.Parameters.Add("@id", DbType.Int64).Value = id;
                command.Parameters.Add("@blob", DbType.Binary, blob.Length).Value = blob;
                command.Parameters.Add("@name", DbType.String, name?.Length ?? 0).Value = name ?? "";
                command.Parameters.Add("@formula", DbType.String, formula?.Length ?? 0).Value = formula ?? "";

                using (SQLiteTransaction tr = conn.BeginTransaction())
                {
                    command.ExecuteNonQuery();
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        public void DeleteChemistry(long chemistryId)
        {
            using (SQLiteConnection conn = LibraryConnection())
            {
                DeleteChemistry(conn, chemistryId);
            }
        }

        private void DeleteChemistry(SQLiteConnection conn, long chemistryId)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                StringBuilder sb = new StringBuilder();

                using (SQLiteTransaction tr = conn.BeginTransaction())
                {
                    sb.AppendLine("DELETE FROM Gallery");
                    sb.AppendLine("WHERE ID = @id");

                    SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                    command.Parameters.Add("@id", DbType.Int64, 20).Value = chemistryId;
                    command.ExecuteNonQuery();

                    sb = new StringBuilder();
                    sb.AppendLine("DELETE FROM ChemicalNames");
                    sb.AppendLine("WHERE ChemistryId = @id");

                    SQLiteCommand nameCommand = new SQLiteCommand(sb.ToString(), conn);
                    nameCommand.Parameters.Add("@id", DbType.Int64, 20).Value = chemistryId;
                    nameCommand.ExecuteNonQuery();

                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        public string GetChemistryByID(long id)
        {
            string result = null;
            using (SQLiteConnection conn = LibraryConnection())
            {
                SQLiteDataReader chemistry = GetChemistryByID(conn, id);
                while (chemistry.Read())
                {
                    var byteArray = (Byte[])chemistry["Chemistry"];
                    result = CmlFromBytes(byteArray, id);
                    break;
                }

                chemistry.Close();
                chemistry.Dispose();
            }

            return result;
        }

        private SQLiteDataReader GetChemistryByID(SQLiteConnection conn, long id)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Chemistry, Name, Formula");
                sb.AppendLine("FROM Gallery");
                sb.AppendLine("WHERE ID = @id");
                sb.AppendLine("ORDER BY NAME");

                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                command.Parameters.Add("@id", DbType.Int64).Value = id;
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }

        public long AddChemistry(string chemistryXml, string chemistryName, string formula)
        {
            long result;
            using (SQLiteConnection conn = LibraryConnection())
            {
                result = AddChemistry(conn, chemistryXml, chemistryName, formula);
            }

            return result;
        }

        private long AddChemistry(SQLiteConnection conn, string chemistryXml, string chemistryName, string formula)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                long lastId;
                StringBuilder sb = new StringBuilder();

                Byte[] blob = Encoding.UTF8.GetBytes(chemistryXml);

                sb.AppendLine("INSERT INTO GALLERY");
                sb.AppendLine(" (Chemistry, Name, Formula)");
                sb.AppendLine("VALUES");
                sb.AppendLine(" (@blob,@name, @formula)");

                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                command.Parameters.Add("@blob", DbType.Binary, blob.Length).Value = blob;
                command.Parameters.Add("@name", DbType.String, chemistryName.Length).Value = chemistryName;
                command.Parameters.Add("@formula", DbType.String, formula.Length).Value = formula;

                using (SQLiteTransaction tr = conn.BeginTransaction())
                {
                    command.ExecuteNonQuery();
                    string sql = "SELECT last_insert_rowid()";
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    lastId = (Int64)cmd.ExecuteScalar();
                    tr.Commit();
                }

                return lastId;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return -1;
            }
        }

        private long AddChemicalName(SQLiteConnection conn, long id, string name, string dictRef)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                long lastID;
                var refs = dictRef.Split(':');

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("DELETE FROM ChemicalNames");
                sb.AppendLine("WHERE ChemistryID = @chemID");
                sb.AppendLine(" AND Namespace = @namespace");
                sb.AppendLine(" AND Tag = @tag");

                SQLiteCommand delcommand = new SQLiteCommand(sb.ToString(), conn);
                delcommand.Parameters.Add("@namespace", DbType.String, refs[0].Length).Value = refs[0];
                delcommand.Parameters.Add("@tag", DbType.String, refs[1].Length).Value = refs[1];
                delcommand.Parameters.Add("@chemID", DbType.Int32).Value = id;

                sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ChemicalNames");
                sb.AppendLine(" (ChemistryID, Name, Namespace, tag)");
                sb.AppendLine("VALUES");
                sb.AppendLine("(@chemID, @name,@namespace, @tag)");

                SQLiteCommand insertCommand = new SQLiteCommand(sb.ToString(), conn);
                insertCommand.Parameters.Add("@name", DbType.String, name.Length).Value = name;
                insertCommand.Parameters.Add("@namespace", DbType.String, refs[0].Length).Value = refs[0];
                insertCommand.Parameters.Add("@tag", DbType.String, refs[1].Length).Value = refs[1];
                insertCommand.Parameters.Add("@chemID", DbType.Int32).Value = id;

                using (SQLiteTransaction tr = conn.BeginTransaction())
                {
                    delcommand.ExecuteNonQuery();
                    insertCommand.ExecuteNonQuery();
                    string sql = "SELECT last_insert_rowid()";
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    lastID = (Int64)cmd.ExecuteScalar();
                    tr.Commit();
                }

                return lastID;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return -1;
            }
        }

        private void DeleteAllChemistry(SQLiteConnection conn)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SQLiteCommand command = new SQLiteCommand("DELETE FROM ChemistryByTags", conn);
                SQLiteCommand command2 = new SQLiteCommand("DELETE FROM Gallery", conn);
                SQLiteCommand command3 = new SQLiteCommand("DELETE FROM ChemicalNames", conn);

                using (SQLiteTransaction tr = conn.BeginTransaction())
                {
                    command.ExecuteNonQuery();
                    command2.ExecuteNonQuery();
                    command3.ExecuteNonQuery();
                    tr.Commit();
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private SQLiteDataReader GetAllNames(SQLiteConnection conn)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT DISTINCT Name, ChemistryId");
                sb.AppendLine("FROM ChemicalNames");
                sb.AppendLine(" UNION");
                sb.AppendLine("SELECT DISTINCT Name, Id");
                sb.AppendLine("FROM Gallery");

                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }

        public List<ChemistryDTO> GetAllChemistry(string filter)
        {
            List<ChemistryDTO> results = new List<ChemistryDTO>();
            using (SQLiteConnection conn = LibraryConnection())
            {
                SQLiteDataReader chemistry = GetAllChemistry(conn, filter);
                while (chemistry.Read())
                {
                    var dto = new ChemistryDTO();

                    dto.Id = (long)chemistry["ID"];
                    var byteArray = (Byte[])chemistry["Chemistry"];
                    dto.Cml = CmlFromBytes(byteArray, dto.Id);
                    dto.Name = chemistry["name"] as string;
                    dto.Formula = chemistry["formula"] as string;

                    results.Add(dto);
                }

                chemistry.Close();
                chemistry.Dispose();
            }

            return results;
        }

        private string CmlFromBytes(byte[] byteArray, long id)
        {
            string cml = Encoding.UTF8.GetString(byteArray);
            CMLConverter cc = new CMLConverter();
            Model.Model m = cc.Import(cml);
            double before = m.MeanBondLength;
            if (before < Constants.MinimumBondLength - Constants.BondLengthTolerance
                || before > Constants.MaximumBondLength + Constants.BondLengthTolerance)
            {
                m.ScaleToAverageBondLength(Constants.StandardBondLength);
                double after = m.MeanBondLength;
                Debug.WriteLine($"Structure Id: {id} rescaled from {before.ToString("#0.00")} to {after.ToString("#0.00")}");
            }

            return cc.Export(m);
        }

        public List<ChemistryTagDTO> GetChemistryByTags()
        {
            List<ChemistryTagDTO> results = new List<ChemistryTagDTO>();
            using (SQLiteConnection conn = LibraryConnection())
            {
                SQLiteDataReader allTags = GetChemistryByTags(conn);
                while (allTags.Read())
                {
                    ChemistryTagDTO dto = new ChemistryTagDTO();

                    dto.Id = (long)allTags["ID"];
                    dto.GalleryId = (long)allTags["GalleryID"];
                    dto.TagId = (long)allTags["TagID"];

                    results.Add(dto);
                }

                allTags.Close();
                allTags.Dispose();
            }

            return results;
        }

        public List<UserTagDTO> GetAllUserTags()
        {
            List<UserTagDTO> results = new List<UserTagDTO>();
            using (SQLiteConnection conn = LibraryConnection())
            {
                SQLiteDataReader allTags = GetAllUserTags(conn);

                while (allTags.Read())
                {
                    var tag = new UserTagDTO();
                    tag.Id = (long)allTags["ID"];
                    tag.Text = (string)allTags["UserTag"];
                    tag.Lock = (long)allTags["Lock"];
                    results.Add(tag);
                }

                allTags.Close();
                allTags.Dispose();
            }

            return results;
        }

        private SQLiteDataReader GetAllUserTags(SQLiteConnection conn)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT Id, UserTag, Lock");
                sb.AppendLine("FROM UserTags");

                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }

        public List<UserTagDTO> GetAllUserTags(int chemistryID)
        {
            List<UserTagDTO> results = new List<UserTagDTO>();
            using (SQLiteConnection conn = LibraryConnection())
            {
                SQLiteDataReader allTags = GetAllUserTags(conn, chemistryID);

                while (allTags.Read())
                {
                    var tag = new UserTagDTO();
                    tag.Id = (long)allTags["ID"];
                    tag.Text = (string)allTags["UserTag"];
                    tag.Lock = (long)allTags["Lock"];
                    results.Add(tag);
                }

                allTags.Close();
                allTags.Dispose();
            }

            return results;
        }

        public static SQLiteDataReader GetChemistryByTags(SQLiteConnection conn)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT Id, GalleryID, TagID");
                sb.AppendLine("FROM ChemistryByTags");
                sb.AppendLine("ORDER BY GalleryID, TagID");

                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }

        private SQLiteDataReader GetAllUserTags(SQLiteConnection conn, int chemistryId)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT Id, UserTag, Lock");
                sb.AppendLine("FROM UserTags");
                sb.AppendLine("WHERE ID IN");
                sb.AppendLine(" (SELECT TagID");
                sb.AppendLine("  FROM ChemistryByTags");
                sb.AppendLine("  WHERE GalleryID = @id)");

                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                command.Parameters.Add("@id", DbType.Int64).Value = chemistryId;
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }

        private SQLiteDataReader GetAllChemistry(SQLiteConnection conn, string filter)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                StringBuilder sb = new StringBuilder();

                if (string.IsNullOrWhiteSpace(filter))
                {
                    sb.AppendLine("SELECT Id, Chemistry, Name, Formula");
                    sb.AppendLine("FROM Gallery");
                    sb.AppendLine("ORDER BY NAME");

                    SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                    return command.ExecuteReader();
                }
                else
                {
                    sb.AppendLine("SELECT Id, Chemistry, Name, Formula");
                    sb.AppendLine("FROM Gallery");
                    sb.AppendLine("WHERE Name LIKE @filter");
                    sb.AppendLine("OR");
                    sb.AppendLine("Id IN");
                    sb.AppendLine("(SELECT ChemistryID");
                    sb.AppendLine(" FROM ChemicalNames");
                    sb.AppendLine(" WHERE Name LIKE @filter)");
                    sb.AppendLine("ORDER BY Name");

                    SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                    command.Parameters.Add("@filter", DbType.String).Value = $"%{filter}%";
                    return command.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }

        private SQLiteDataReader GetAllChemistryWithTags(SQLiteConnection conn)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Chemistry, Name, Formula, UserTag");
                sb.AppendLine("FROM GetAllChemistryWithTags");
                sb.AppendLine("ORDER BY NAME");

                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }

        private SQLiteDataReader GetChemicalNames(SQLiteConnection conn, int id)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT ChemicalNameID, Name, namespace, Tag");
                sb.AppendLine("FROM ChemicalNames");
                sb.AppendLine("WHERE ChemistryID = @id");
                sb.AppendLine("ORDER BY NAME");

                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                command.Parameters.Add("@id", DbType.Int64).Value = id;
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }
    }

    public class ChemistryDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Formula { get; set; }
        public string Cml { get; set; }
    }

    public class ChemistryTagDTO
    {
        public long Id { get; set; }
        public long GalleryId { get; set; }
        public long TagId { get; set; }
    }

    public class UserTagDTO
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public long Lock { get; set; }
    }
}