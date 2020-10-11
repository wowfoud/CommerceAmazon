namespace Commerce.Amazon.Domain.Config
{
    public class Setting
    {

        public Setting()
        {
        }

        public string FolderComments { get; }
        public string MessageBodyNotify { get; }
        public string MessageSubjectNotify { get; }
        public string MessageBodyWarning { get; }
        public string MessageSubjectWarning { get; }
    }
}
