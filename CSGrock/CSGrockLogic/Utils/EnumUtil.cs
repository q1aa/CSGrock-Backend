namespace CSGrock.CSGrockLogic.Utils
{
    public class EnumUtil
    {
        public static Enums.RequestEnum.RequestType ParseRequestMethodeEnum(string requestMethode)
        {
            switch (requestMethode)
            {
                case "GET":
                    return Enums.RequestEnum.RequestType.GET;
                case "POST":
                    return Enums.RequestEnum.RequestType.POST;
                case "PUT":
                    return Enums.RequestEnum.RequestType.PUT;
                case "DELETE":
                    return Enums.RequestEnum.RequestType.DELETE;
                case "PATCH":
                    return Enums.RequestEnum.RequestType.PATCH;
                case "HEAD":
                    return Enums.RequestEnum.RequestType.HEAD;
                case "OPTIONS":
                    return Enums.RequestEnum.RequestType.OPTIONS;
                default:
                    return Enums.RequestEnum.RequestType.GET;
            }
        }
    }
}
