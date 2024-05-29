namespace CSGrock.CSGrockLogic.Struct
{
    public class ImagePartStruct
    {
        public int partID { get; set; }
        public string partContent { get; set; }
        public int partsLenght { get; set; }
        public string fileName { get; set; }
        public string fileType { get; set; }
        public string requestID { get; set; }

        public ImagePartStruct(int partID, string partContent, int partsLenght, string fileName, string fileType, string requestID)
        {
            this.partID = partID;
            this.partContent = partContent;
            this.partsLenght = partsLenght;
            this.fileName = fileName;
            this.fileType = fileType;
            this.requestID = requestID;
        }
    }
}
