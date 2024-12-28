using Microsoft.AspNetCore.Http;
using System;
using System.IO;


namespace DEMO.PL.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file , string FolderName )
        {
            //Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\" + FolderName
            // Located Folder Path
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory() , "wwwroot\\Files" , FolderName) ; 
            //Get Unique Name
            string FileName =$"{Guid.NewGuid()}{file.FileName}";
            //Get file Path
            string FilePath = Path.Combine(FolderPath, FileName); 
            // Save File As stream
            using var Fs = new FileStream(FilePath , FileMode.Create);
            file.CopyTo( Fs );
            //Return FileName
            return FileName ;


        }

        public static void DeleteFile(string FileName, string FolderName) 
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Files", FolderName , FileName) ;
            if (File.Exists(FilePath)) 
            {
                File.Delete(FilePath);
            }
        }
    }
}
