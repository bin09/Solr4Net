using System;
using System.Collections.Generic;
using System.Data;
using Solr.Log;
using Solr.MODEL;
using Solr.Config;
using Solr.Helper;

namespace Solr.DBUtility
{
    public class DalProBest : BaseSQL
    {
        public int GetCount(int id)
        {
            try
            {
                string sql = string.Format("SELECT COUNT(1) FROM Products_SellsBest WHERE Id>{0}", id);
                return this.ExecuteScalar(sql);
            }
            catch (Exception ex)
            {
                Log4Helper.AddError(ex);
                return 0;
            }
        }
        private DataTable GetData(int id)
        {
            try
            {
                string sql = string.Format("SELECT TOP " + config.TopCount + " Products_SellsBest.Id,Model,Title,MemberGrade,classParPath,[Content] FROM Products_SellsBest,Mb_Member WHERE Products_SellsBest.Id>{0} AND Mb_Member.Id=Products_SellsBest.MemberId ORDER BY Id ASC", id);
                return this.ExecuteDataTable(sql);
            }
            catch (Exception ex)
            {
                Log4Helper.AddError(ex);
                return null;
            }
        }
        public List<ModProduct> GetList()
        {
            int id = Convert.ToInt32(ConfigSetting.GetValue("LastBestId"));
            DataTable dt = this.GetData(id);
            List<ModProduct> list = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                ModProduct mod = null;
                list = new List<ModProduct>();
                foreach (DataRow dr in dt.Rows)
                {
                    mod = new ModProduct();
                    mod.Id = Convert.ToInt32(dr["Id"]);
                    mod.SolrId = string.Concat("b", dr["Id"]);                    //区分普通产品和优势产品在solr的唯一标识
                    mod.Title = dr["Title"].ToString();
                    mod.ClassParPath = dr["classParPath"].ToString();
                    mod.Content = Utils.ClearHtml(dr["Content"].ToString());
                    mod.MemberGrade = Convert.ToInt32(dr["MemberGrade"]);
                    mod.Model = dr["Model"].ToString();
                    if (mod.MemberGrade == 2)
                        mod.ProBoost = config.SilverBoost;
                    else if (mod.MemberGrade == 3)
                        mod.ProBoost = config.GoldBoost;
                    else if (mod.MemberGrade == 4)
                        mod.ProBoost = config.VipBoost;
                    list.Add(mod);
                }
            }
            return list;
        }
    }
}
