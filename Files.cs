namespace DownloadFiles; 
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

    public class Files
    {
    
        public async Task<string> upload(IFormFile arrayBuffer, string filePath, string targetDir)
        {
        if(arrayBuffer == null || arrayBuffer.Length == 0)
        {
            throw new ArgumentException("No File uploaded");
        }
        var fullPath = Path.Combine(targetDir, "deposits", filePath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        using(var stream = new FileStream(fullPath, FileMode.Create))
        {
            await arrayBuffer.CopyToAsync(stream);
        }
        return fullPath;
        }
    }


