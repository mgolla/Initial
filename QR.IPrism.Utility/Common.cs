//------------------------------------------------------------------------------
// <copyright file="Common.cs" company="Qatar Airways">
//     Copyright (c) Qatar Airways.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace QR.IPrism.Utility
{
    #region Namespaces
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Security.Cryptography;
    using System.Net;
    using System.IO;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
using Oracle.DataAccess.Types;
    using System.Web.Security;
    using System.Runtime.Serialization.Formatters.Binary;
    #endregion
    /// <summary>
    /// Class with constants used in service
    /// </summary>
    public class Common
    {

        private static string _crewImage = "CrewImage";
        /// <summary>
        /// Common Class to call generic Methods  
        /// </summary>
        #region Methods
        /// <summary>
        /// This method is used to convert the plain text to Encrypted/Un-Readable Text format.
        /// </summary>
        /// <param name="PlainText">Plain Text to Encrypt before transferring over the network.</param>
        /// <returns>Cipher Text</returns>
        public static string Encrypt(string PlainText, string Key)
        {
            //Getting the bytes of Input String.
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key));

            //De-allocatinng the memory after doing the Job.
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

            //Assigning the Security key to the TripleDES Service Provider.
            objTripleDESCryptoService.Key = securityKeyArray;

            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;

            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();

            //Transform the bytes array to resultArray
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);

            //Releasing the Memory Occupied by TripleDES Service Provider for Encryption.
            objTripleDESCryptoService.Clear();

            //Convert and return the encrypted data/byte into string format.
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }


        /// <summary>
        /// This method is used to convert the Cipher/Encypted text to Plain Text.
        /// </summary>
        /// <param name="CipherText">Encrypted Text</param>
        /// <returns>Plain/Decrypted Text</returns>
        public static string Decrypt(string CipherText, string Key)
        {
            byte[] toEncryptArray = Convert.FromBase64String(CipherText);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key));

            //De-allocatinng the memory after doing the Job.
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

            //Assigning the Security key to the TripleDES Service Provider.
            objTripleDESCryptoService.Key = securityKeyArray;

            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;

            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();

            //Transform the bytes array to resultArray
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            //Releasing the Memory Occupied by TripleDES Service Provider for Decryption.          
            objTripleDESCryptoService.Clear();

            //Convert and return the decrypted data/byte into string format.
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
       
        #endregion

        public static string OracleDateFormate(DateTime? date)
        {

            return date != null && date.Value != DateTime.MinValue ? Convert.ToDateTime(date).ToString("dd-MMM-yyyy HH:mm:ss") : string.Empty;

        }
        public static string OracleDateFormateddMMMyyyyHHmm(DateTime? date)
        {

            return date != null && date.Value != DateTime.MinValue ? Convert.ToDateTime(date).ToString("dd-MMM-yyyy HH:mm") : string.Empty;

        }

        public static string OracleDateFormateddMMMyyyy(DateTime? date)
        {
            return date != null && date.Value != DateTime.MinValue ? Convert.ToDateTime(date).ToString("dd-MMM-yyyy") : string.Empty;
        }
        public static string DateFormateddMMMyyyyWithSpace(DateTime? date)
        {
            return date != null && date.Value != DateTime.MinValue ? Convert.ToDateTime(date).ToString("dd MMM yyyy") : string.Empty;
        }
        public static string DateFormateddMMMyyyyhhmmWithSpace(DateTime? date)
        {
            return date != null && date.Value != DateTime.MinValue ? Convert.ToDateTime(date).ToString("dd MMM yyyy hh:mm") : string.Empty;
        }
        public static OracleDate ToOracleDate(DateTime? date)
        {
            return date != null && date.Value != DateTime.MinValue ? OracleDate.Parse(Convert.ToDateTime(date).ToString("dd-MMM-yyyy")) : OracleDate.Null;
        }
        public static OracleTimeStamp ToOracleDateTimeStamp(DateTime? date)
        {
            return date != null && date.Value != DateTime.MinValue ? OracleTimeStamp.Parse(Convert.ToDateTime(date).ToString("dd-MMM-yy hh.mm.ss.ff tt")) : OracleTimeStamp.Null;
        }
        public static OracleDate ToOracleDate(string date)
        {
            return !string.IsNullOrEmpty(date) ? OracleDate.Parse(Convert.ToDateTime(date).ToString("dd-MMM-yyyy")) : OracleDate.Null;
        }
        public static OracleTimeStamp ToOracleDateTimeStamp(string date)
        {
            return !string.IsNullOrEmpty(date) ? OracleTimeStamp.Parse(Convert.ToDateTime(date).ToString("dd-MMM-yy hh.mm.ss.ff tt")) : OracleTimeStamp.Null;
        }

        #region keywordSearch
        /// <summary>
        /// !---- Edocs maintained-----!
        /// !important  For Use only in downlading the files from Edocs. for other use DecryptString /EncryptString in this file.
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">use hashing 2 encrypt this data?pass true is  yes</param>
        /// <returns></returns>
        public static string EdocsDownloadDecrypt(string cipherString, bool useHashing, string key)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            //string key = "QatarAirways";
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string EncryptWithMachineKey(string PlainText)
        {
            var bytes = Convert.FromBase64String(PlainText);
            byte[] output = MachineKey.Unprotect(bytes, _crewImage);
            string originalData = Encoding.UTF8.GetString(output);
            return Base64ToObject(originalData).ToString();
        }
        private static object Base64ToObject(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return new BinaryFormatter().Deserialize(ms);
            }
        }
        private static string ObjectToBase64(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        #endregion KeywordSearch

        #region Kafou BO
        #region Constants

        public const string CONTENTTYPE = "CCRAttachment";
        public const string ISACTIVE = "Y";

        #endregion

        #region Enum

        public enum RecognitionType
        {
            Group,
            Individual
        }
        #endregion
        #endregion
    }
}
