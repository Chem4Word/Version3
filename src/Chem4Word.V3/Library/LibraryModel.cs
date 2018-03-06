// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.Helpers;
using Chem4Word.Core.UI.Forms;
using Chem4Word.Model.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Text;

namespace Chem4Word.Library
{
    public static class LibraryModel
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public static SQLiteConnection LibraryConnection
        {
            get
            {
                string path = Path.Combine(Globals.Chem4WordV3.AddInInfo.ProgramDataPath, Constants.LibraryFileName);
                // Source https://www.connectionstrings.com/sqlite/
                var conn = new SQLiteConnection($"Data Source={path};Synchronous=Full");
                return conn.OpenAndReturn();
            }
        }

        public static Dictionary<string, int> GetLibraryNames()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            try
            {
                Dictionary<string, int> allNames = new Dictionary<string, int>();

                SQLiteDataReader names = LibraryModel.GetAllNames();
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

                names.Close();
                names.Dispose();

                return allNames;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }

        public static SQLiteDataReader GetAllChemistryWithTags()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Chemistry, Name, Formula, UserTag");
                sb.AppendLine("FROM GetAllChemistryWithTags");
                sb.AppendLine("ORDER BY NAME");

                SQLiteConnection conn = LibraryConnection;
                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }

        private static SQLiteDataReader GetAllNames()
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

                SQLiteConnection conn = LibraryConnection;
                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }

        public static SQLiteDataReader GetAllChemistry(string filter)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                StringBuilder sb = new StringBuilder();

                SQLiteConnection conn = LibraryConnection;
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

        public static SQLiteDataReader GetChemistryByTags()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT Id, GalleryID, TagID");
                sb.AppendLine("FROM ChemistryByTags");
                sb.AppendLine("ORDER BY GalleryID, TagID");

                SQLiteConnection conn = LibraryConnection;
                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }

        public static SQLiteDataReader GetAllUserTags()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT Id, UserTag, Lock");
                sb.AppendLine("FROM UserTags");

                SQLiteConnection conn = LibraryConnection;
                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }

        public static SQLiteDataReader GetAllUserTags(int chemistryID)
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

                SQLiteConnection conn = LibraryConnection;
                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                command.Parameters.Add("@id", DbType.Int64).Value = chemistryID;
                return command.ExecuteReader();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return null;
            }
        }

        public static SQLiteDataReader GetChemistryByID(int id)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT Id, Chemistry, Name, Formula");
                sb.AppendLine("FROM Gallery");
                sb.AppendLine("WHERE ID = @id");
                sb.AppendLine("ORDER BY NAME");

                SQLiteConnection conn = LibraryConnection;
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

        public static void DeleteChemistry(long chemistryId)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SQLiteConnection conn = LibraryConnection;
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

                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        public static long AddChemistry(string chemistryXml, string chemistryName, string formula)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                long lastID;
                StringBuilder sb = new StringBuilder();

                Byte[] blob = System.Text.Encoding.UTF8.GetBytes(chemistryXml);

                sb.AppendLine("INSERT INTO GALLERY");
                sb.AppendLine(" (Chemistry, Name, Formula)");
                sb.AppendLine("VALUES");
                sb.AppendLine(" (@blob,@name, @formula)");

                SQLiteConnection conn = LibraryConnection;
                SQLiteCommand command = new SQLiteCommand(sb.ToString(), conn);
                command.Parameters.Add("@blob", DbType.Binary, blob.Length).Value = blob;
                command.Parameters.Add("@name", DbType.String, chemistryName.Length).Value = chemistryName;
                command.Parameters.Add("@formula", DbType.String, formula.Length).Value = formula;

                using (SQLiteTransaction tr = conn.BeginTransaction())
                {
                    command.ExecuteNonQuery();
                    string sql = "SELECT last_insert_rowid()";
                    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                    lastID = (Int64)cmd.ExecuteScalar();
                    tr.Commit();
                }

                conn.Close();
                conn.Dispose();

                return lastID;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return -1;
            }
        }

        public static void UpdateChemistry(long id, string name, string xml, string formula)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                Byte[] blob = System.Text.Encoding.UTF8.GetBytes(xml);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("UPDATE GALLERY");
                sb.AppendLine("SET Name = @name, Chemistry = @blob, Formula = @formula");
                sb.AppendLine("WHERE ID = @id");

                SQLiteConnection conn = LibraryConnection;
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

                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        public static bool ImportCml(string cmlFile)
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

                    var id = AddChemistry(cml, chemicalName, model.ConciseFormula);
                    foreach (var name in mol.ChemicalNames)
                    {
                        LibraryModel.AddChemicalName(id, name.Name, name.DictRef);
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

        private static long AddChemicalName(long id, string name, string dictRef)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                long lastID;
                var refs = dictRef.Split(':');
                SQLiteConnection conn = LibraryConnection;

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

                conn.Close();
                conn.Dispose();

                return lastID;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                return -1;
            }
        }

        public static SQLiteDataReader GetChemicalNames(int id)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT ChemicalNameID, Name, namespace, Tag");
                sb.AppendLine("FROM ChemicalNames");
                sb.AppendLine("WHERE ChemistryID = @id");
                sb.AppendLine("ORDER BY NAME");

                SQLiteConnection conn = LibraryConnection;
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

        public static void DeleteAllChemistry()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SQLiteConnection conn = LibraryConnection;

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

                conn.Close();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }
    }
}