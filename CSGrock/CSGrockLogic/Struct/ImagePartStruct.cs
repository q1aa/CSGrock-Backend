namespace CSGrock.CSGrockLogic.Struct
{
    public class ImagePartStruct
    {
        public int partID { get; set; }
        public string partContent { get; set; }
        public int partsLenght { get; set; }

        public string requestID { get; set; }

        public ImagePartStruct(int partID, string partContent, int partsLenght, string requestID)
        {
            this.partID = partID;
            this.partContent = partContent;
            this.partsLenght = partsLenght;
            this.requestID = requestID;
        }
    }
}
