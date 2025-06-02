using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Xml;

namespace FlowLedger.Utils
{
    internal static class FileHelper
    {
        internal static (bool, string, string) SelectFileForSaving(string fileType)
        {
            fileType = fileType.ToLower();
            var fileName = string.Empty;
            var saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Filter = $"Files|*.{fileType}",
                Title = "Create a file to continue ...",
                FileName = $"NewFile.{fileType}",
                OverwritePrompt = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                fileName = saveFileDialog.FileName;
                File.WriteAllText(fileName, string.Empty);
            }

            string err = string.Empty;

            var invalidChars = Path.GetInvalidPathChars();
            if (string.IsNullOrWhiteSpace(fileName) || fileName.Any(c => invalidChars.Contains(c)))
            {
                err = "File path isn't valid.";
                return (false, fileName, err);
            }
            else if (!File.Exists(fileName))
            {
                err = $"File doesn't exist at:\n{fileName}";
                return (false, fileName, err);
            }
            else if (fileName.Length > 260)
            {
                err = "File path is too long.";
                return (false, fileName, err);
            }
            return (true, fileName, err);
        }

        internal static (TResult, string) RunWithFile<TResult>(string fileName, Func<string, TResult> fileAction)
        {
            string error = string.Empty;
            try
            {
                return (fileAction(fileName), error);
            }
            catch (System.Security.SecurityException ex)
            {
                // MessageBox.Show("App running without enough security (role) right.");
                error = "App running without enough security (role) right.";
                Console.WriteLine($"security (role) right related error: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                // MessageBox.Show("File permission error occurred");
                error = "File permission error occurred";
                Console.WriteLine($"file permission error occurred: {ex.Message}");
            }
            catch (IOException ex)
            {
                // MessageBox.Show("File access denied");
                error = "File access denied";
                Console.WriteLine($"file locked or not enough space: {ex.Message}");
            }
            catch (Exception ex) when (IsSerializationRelatedEx(ex))
            {
                // MessageBox.Show("Serialization/deserialization related Error");
                error = "Serialization/deserialization related Error";
                Console.WriteLine($"(de)serialization error: {ex.Message}");
            }
            catch (Exception ex) when (IsMemoryRelatedEx(ex))
            {
                // MessageBox.Show("Memory performance issue");
                error = "Memory performance issue";
                Console.WriteLine($"Run out of memory/stack size exceeded: {ex.Message}");
            }
            catch (Exception ex)
            {
                // MessageBox.Show("uncategorized error");
                error = "uncategorized error";
                Console.WriteLine($"Error: {ex}");
            }
            return (default, error);
        }

        private static bool IsSerializationRelatedEx(Exception ex)
        {
            return ex is JsonSerializationException
                || ex is JsonWriterException
                || ex is XmlException;
        }

        private static bool IsMemoryRelatedEx(Exception ex)
        {
            return ex is StackOverflowException
                || ex is OutOfMemoryException;
        }
    }
}
