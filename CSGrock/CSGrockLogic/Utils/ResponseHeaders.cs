using System.Net.Mime;

namespace CSGrock.CSGrockLogic.Utils
{
    public class ResponseHeaders
    {
        private static readonly List<string> headersList = new List<string>()
        {
            // Existing headers (relevant for files)
            "text/html", // Web page
            "image/jpeg", // JPEG image
            "image/png", // PNG image
            "image/gif", // GIF image
            "image/svg+xml", // Scalable Vector Graphics
            "video/mp4", // MP4 video
            "video/webm", // WebM video
            "video/ogg", // Ogg video
            "audio/mpeg", // MP3 audio
            "application/octet-stream", // Generic binary data (often used for downloads)
            "application/pdf", // PDF document
            "font/ttf", // TrueType font
            "font/woff", // Web Open Font Format
            "font/woff2", // Web Open Font Format 2
            "application/zip", // ZIP archive
            "application/x-gzip", // GZIP compressed archive
            "application/x-bzip2", // BZIP2 compressed archive
            "application/x-rar-compressed", // RAR compressed archive
            "image/bmp", // Bitmap image
            "image/vnd.microsoft.icon", // ICO icon

            // Content-type variations (file extensions)
            "text/plain", // Plain text
            "text/csv", // Comma-Separated Values
            "application/vnd.ms-excel", // Microsoft Excel (.xls)
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", // Microsoft Excel (.xlsx)
            "text/markdown", // Markdown document
            "application/vnd.ms-powerpoint", // Microsoft PowerPoint (.ppt)
            "application/vnd.openxmlformats-officedocument.presentationml.presentation", // Microsoft PowerPoint (.pptx)
            "application/msword", // Microsoft Word (.doc)
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document", // Microsoft Word (.docx)
            // Audio/Video variations
            "audio/flac", // FLAC lossless audio
            "audio/wav", // WAV audio
            "audio/webm", // WebM audio
            "video/x-flv", // Flash video
            "video/quicktime", // QuickTime movie
            "video/x-matroska", // Matroska video

            // Document formats
            "application/vnd.oasis.opendocument.text", // OpenDocument Text (.odt)
            "application/vnd.oasis.opendocument.spreadsheet", // OpenDocument Spreadsheet (.ods)
            "application/vnd.oasis.opendocument.presentation", // OpenDocument Presentation (.odp)
            "application/vnd.apple.pages", // Apple Pages document
            "application/vnd.ms-powerpoint", // Microsoft PowerPoint (.ppt) (already included, but mentioned again for completeness)

            // Archives
            "application/x-7z-compressed", // 7z archive
            "application/x-tar", // Tar archive

            // Images (less common)
            "image/webp", // WebP image format
            "image/tiff", // Tagged Image File Format

            // Executables (use with caution)
            "application/x-executable", // Generic executable file

            // Font formats (less common)
            "font/otf", // OpenType font
            "application/x-font-ttf", // Alternative for TrueType font

            // Audio formats (less common)
            "audio/ogg", // Ogg Vorbis audio (already included, but mentioned again for completeness)
            "audio/midi", // MIDI audio

            // Video formats (less common)
            "video/x-msvideo", // AVI video
            "video/3gpp", // 3GPP video (used in mobile phones)
            "video/x-ms-wmv", // Windows Media Video

            // Image formats (less common)
            "image/x-icon", // Generic icon format (may overlap with ICO)
            "image/vnd.adobe.photoshop", // Photoshop (.psd)
            "image/x-xcf", // GIMP image format

            // Document formats (less common)
            "text/x-c", // C source code
            "text/x-java", // Java source code
            "application/vnd.oasis.opendocument.graphics", // OpenDocument Drawing (.odg)
            "application/vnd.oasis.opendocument.formula", // OpenDocument Formula (.odf)

            // Executables (specific types, use with caution)
            "application/x-executable; charset=binary", // Executable with binary charset
            "application/x-msdownload", // Windows executable (EXE)

            // Web-related (less common)
            "application/json+patch", // JSON Patch document (for updating JSON data)
        };
        public static Task AddFileExtensionHeaders(HttpContext context, string fileExtension)
        {
            string contentType = headersList.FirstOrDefault(x => x.Contains(fileExtension));
            if (contentType != null)
            {
                context.Response.Headers.Add("Content-Type", contentType);
            }
            return Task.CompletedTask;
        }
    }
}
