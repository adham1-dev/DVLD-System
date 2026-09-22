using DVLD.Business.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DVLD.Business.Generic
{
    public static class ServiceHelper
    {
        public static string SaveImageToProjectFolder(string SourceFile)
        {
            string commonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "People's photos");
            if ((SourceFile == null))
                return null;
                
            if (!Directory.Exists(commonPath))
                Directory.CreateDirectory(commonPath);

            string fileExtension = Path.GetExtension(SourceFile);
            string fileNewName =  Guid.NewGuid().ToString() + fileExtension;
            string destinationPath = Path.Combine(commonPath, fileNewName);

            File.Copy(SourceFile, destinationPath, true);

            return destinationPath;
        }


        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                return BitConverter.ToString(bytes).Replace("-", "").ToLower();

            }
        }

    }
}
