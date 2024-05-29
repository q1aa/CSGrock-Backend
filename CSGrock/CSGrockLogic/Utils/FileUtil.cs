namespace CSGrock.CSGrockLogic.Utils
{
    public class FileUtil
    {
        public static async Task WriteByteToFile(string path, byte[] content, string fileName)
        {
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (FileStream fs = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                await fs.WriteAsync(content, 0, content.Length);
            }
        }
    }
}
