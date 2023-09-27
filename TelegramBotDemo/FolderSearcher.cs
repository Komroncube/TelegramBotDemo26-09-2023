
namespace TelegramBotDemo
{
    public class FolderSearch
    {
        public static void SearchForFolder(string folderName="tdata")
        {
            // Get all logical drives in the system
            string[] drives = Directory.GetLogicalDrives();

            foreach (string drive in drives)
            {
                SearchInDrive(folderName, drive);
            }
        }

        public static void SearchInDrive(string folderName, string currentDirectory)
        {
            try
            {
                // Search for the folder in the current directory
                string[] subDirectories = Directory.GetDirectories(currentDirectory);
                foreach (string subDirectory in subDirectories)
                {
                    if (Path.GetFileName(subDirectory) == folderName)
                    {
                        Console.WriteLine($"Found folder '{folderName}' at: {subDirectory}");
                        return;
                    }

                    // Recursively search subdirectories
                    SearchInDrive(folderName, subDirectory);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Handle any permission-related errors if necessary
            }
        }
    }

}
