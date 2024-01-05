namespace CSGrock.CSGrockLogic.Struct
{
    public class RequestUserStruct
    {
        public string username { get; set; }
        public Guid UUID { get; set; }

        public RequestUserStruct(string username, Guid UUID)
        {
            this.username = username;
            this.UUID = UUID;
        }
    }
}
