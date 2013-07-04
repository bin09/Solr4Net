
namespace Solr.MODEL
{
    public class ModUserLog
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private byte type;
        public byte Type
        {
            get { return type; }
            set { type = value; }
        }
        private int linkId;
        public int LinkId
        {
            get { return linkId; }
            set { linkId = value; }
        }
        private byte linkType;
        public byte LinkType
        {
            get { return linkType; }
            set { linkType = value; }
        }
        private string typeProId;
        public string TypeProId
        {
            get { return typeProId; }
            set { typeProId = value; }
        }
    }
}
