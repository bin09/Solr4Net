using SolrNet.Attributes;

namespace Solr.MODEL
{
    public class ModProduct
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string solrId;
        [SolrUniqueKey("id")]
        public string SolrId
        {
            get { return solrId; }
            set { solrId = value; }
        }
        private string title;
        [SolrField("title")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string content;
        [SolrField("proContent")]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        private string classParPath;
        [SolrField("classParPath")]
        public string ClassParPath
        {
            get { return classParPath; }
            set { classParPath = value; }
        }
        private string model;
        [SolrField("model")]
        public string Model
        {
            get { return model; }
            set { model = value; }
        }
        private int provinceId;
        [SolrField("provinceId")]
        public int ProvinceId
        {
            get { return provinceId; }
            set { provinceId = value; }
        }
        private int cityId;
        [SolrField("cityId")]
        public int CityId
        {
            get { return cityId; }
            set { cityId = value; }
        }
        private int memberGrade;
        public int MemberGrade
        {
            get { return memberGrade; }
            set { memberGrade = value; }
        }
        private double proBoost;
        public double ProBoost
        {
            get { return proBoost; }
            set { proBoost = value; }
        }
    }
}
