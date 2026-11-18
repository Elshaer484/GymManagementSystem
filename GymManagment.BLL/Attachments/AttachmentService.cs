using GymManagment.BLL.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.BLL.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        public AttachmentService(IWebHostEnvironment env)
        {
            this.env = env;
        }
        private readonly long FileSize = 5 * 1024 * 1024;

        private readonly string[] Mylist = { ".jpg", ".jpeg", ".png" };
        private readonly IWebHostEnvironment env;

        public async Task<Result<string?>> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream is null || fileStream.Length == 0)
            {
                return Result<string>.NotFound("File Not Found");
            }

            if(!fileStream.CanRead)
            {
                return Result<string>.Fail("Can't Read From This File");
            }

            if(fileStream.Length > FileSize)
            {
                return Result<string>.Fail("File Too Large");
            }


            var ex = Path.GetExtension(fileName);

            if(string.IsNullOrWhiteSpace(ex) || !fileName.Contains(ex))
            {
                return Result<string>.Fail("Can't Read From This File Extenstion");
            }
            var upload = Path.Combine(env.ContentRootPath, folderName);

            Directory.CreateDirectory(upload);

            var storedFileName = $"{Guid.NewGuid()} {fileName}";
            var filePath = Path.Combine(upload, storedFileName);


            try
            {
                using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await fileStream.CopyToAsync(fs, ct);
                return Result<string>.OK(storedFileName);
            }
            catch (Exception)
            {
                return Result<string>.NotFound();

            }

        }



        public Result<bool> Delete(string fileName, string FolderName)
        {
            var myFile = Path.Combine(env.ContentRootPath, FolderName, fileName);

            try
            {
                if (!File.Exists(myFile))
                {
                    return Result<bool>.NotFound();
                }
                File.Delete(myFile);
                return Result<bool>.OK(true);
            }
            catch (Exception)
            {
                return Result<bool>.Fail("Failed To Delte This Attachment");

            }
        }

        public (Stream stream, string contentType)? GetFile(string fileName, string folderName)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(folderName))
            {
                return null;
            }
            var fullPath = Path.Combine(env.ContentRootPath, folderName, fileName);
            if (!File.Exists(fullPath))
            {
                return null;
            }

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var extension = Path.GetExtension(fullPath).ToLower();
            var contentType = extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "application/octet-stream"
            };

            return (stream, contentType);

            

        }
    }
}
