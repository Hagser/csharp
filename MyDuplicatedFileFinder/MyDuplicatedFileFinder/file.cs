namespace MyDuplicatedFileFinder
{
    public class file
    {
        public file()
        { }
        public string name { get; set; }
        public long len { get; set; }
        public string path { get; set; }
        public string hash {
            get { return name.GetHashCode() + "_" + len.GetHashCode(); }
            set { }
        }
    }
}
