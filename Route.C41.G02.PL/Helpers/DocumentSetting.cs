using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Route.C41.G02.PL.Helpers
{
    public static class DocumentSetting
    {
        public static async Task <string> UploadFile(IFormFile file, string folderName)
        {

            // 1. get located folder path

           
            string folderPath =Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);
        
           if(!Directory.Exists(folderPath))
            {
             Directory.CreateDirectory(folderPath); 
            }


           // 2. get file name and make it unique

           string fileName=$"{Guid .NewGuid()}{Path.GetExtension(file.FileName)}";
           
            //3. get file path
           string filePath=Path.Combine(folderPath, fileName);

            //4. save file as streams [data per time]
            
          using  var  fileStream=new FileStream(filePath,FileMode.Create);
        
            

           await file.CopyToAsync(fileStream); 


            return fileName;
        
         
         
        }

        public static void DeleteFile(string fileName, string folderName)
        {
            if(! string.IsNullOrEmpty(fileName)&& !string.IsNullOrEmpty(folderName))
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath); 
                }
            }    
            
        }

    }
}
