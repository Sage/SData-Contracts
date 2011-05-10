#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;

#endregion

namespace Sage.Sis.Common.Data.OleDb
{
    public static class JetHelpers
    {
        public static bool TableExists(string tableName, IJetTransaction transaction)
        {
            string sql = string.Format("SELECT COUNT(*) FROM {0};", tableName);
            OleDbCommand oleDbCmd = transaction.CreateOleCommand();

            oleDbCmd.CommandText = sql;
            oleDbCmd.CommandType = CommandType.Text;

            try
            {
                oleDbCmd.ExecuteNonQuery();
            }
            catch (OleDbException)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Liest sequenziell den Inhalt eines binären Datenfeldes und gibt ihn als object zurück.
        /// </summary>
        /// <param name="reader">Der DatenReader.</param>
        /// <param name="ordinal">0-basierter Spaltenindex.</param>
        /// <returns>Binärer Inhalt des Datenfeldes als Objekttyp.</returns>
        /// <remarks>
        /// Die Methode ist für sequenzielles Einlesen (CommandBehavior.SequentialAccess) ausgelegt.
        /// </remarks>
        public static byte[] ReadBlob(DbDataReader reader, int ordinal)
        {
            byte[] blobContent;                     // Inhalt des BLOB
            int bufferSize = 1024;                  // Größe des BLOB buffer.
            byte[] outByte = new byte[bufferSize];  // Der BLOB byte[] buffer, der mit Hilfe von GetBytes gefüllt wird.
            long retval;                            // Anzahl bytes, welche von GetBytes zurückgegeben wird.
            long startIndex = 0;                    // Startposition der BLOB-Ausgabe.

            //if (!reader.IsDBNull(ordinal))
            //{
            MemoryStream memStream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(memStream))
            {
                // Lese die Bytes und erhalte die Anzahl zurückgegebener Bytes.
                retval = reader.GetBytes(ordinal, startIndex, outByte, 0, bufferSize);

                // Lese solange zusätzliche Bytes bis der Buffer nicht mehr komplett gefüllt wurde.
                while (retval == bufferSize)
                {
                    writer.Write(outByte);
                    writer.Flush();

                    // Setze den nächsten start index an das Ende des letzen Buffers und fülle den Buffer erneut.
                    startIndex += bufferSize;
                    retval = reader.GetBytes(ordinal, startIndex, outByte, 0, bufferSize);
                }

                // Schreibe den letzen Bufferinhalt.
                //writer.Write(outByte, 0, (int)retval - 1);
                writer.Write(outByte, 0, (int)retval);
                writer.Flush();

                memStream.Position = 0;
                blobContent = memStream.ToArray();
            }

           
            return blobContent;
        }
    }
}
