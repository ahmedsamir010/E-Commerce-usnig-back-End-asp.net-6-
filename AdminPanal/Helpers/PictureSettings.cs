namespace AdminPanal.Helpers
{
    public class PictureSettings
    {
        public static string UploadFile(IFormFile file, string FolderName)
        {
            //1-Get Folder Path
            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", FolderName);
            //2-Set FileName Unique
            var fileName = Guid.NewGuid() + file.FileName;
            //3-GetFilePath
            var filePath = Path.Combine(FolderPath, fileName);
            //4-Save File As stream
            var FileStream = new FileStream(filePath, FileMode.Create);
            //5-copy file into stream
            file.CopyTo(FileStream);
            //6-return fileName
            return Path.Combine("images\\products", fileName);
        }

        public static void DeleteFile(string folderName, string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", folderName, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
