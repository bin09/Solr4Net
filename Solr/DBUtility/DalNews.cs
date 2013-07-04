using System;
using System.Collections.Generic;
using System.Data;
using Solr.Log;
using Solr.MODEL;
using Solr.Config;
using Solr.Helper;

namespace Solr.DBUtility
{
    public class DalNews : BaseSQL
    {
        public int GetCount(int id)
        {
            try
            {
                string sql = string.Format("SELECT COUNT(1) FROM News_Info WHERE Id>{0}", id);
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
                string sql = string.Format("SELECT TOP " + config.TopCount + " Id,Title,[Content] FROM News_Info WHERE Id>{0} ORDER BY Id ASC", id);
                return this.ExecuteDataTable(sql);
            }
            catch (Exception ex)
            {
                Log4Helper.AddError(ex);
                return null;
            }
        }
        public List<ModNews> GetList()
        {
            int id = Convert.ToInt32(ConfigSetting.GetValue("LastNewsId"));
            DataTable dt = this.GetData(id);
            List<ModNews> list = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                ModNews mod = null;
                list = new List<ModNews>();
                foreach (DataRow dr in dt.Rows)
                {
                    mod = new ModNews();
                    mod.Id = Convert.ToInt32(dr["Id"]);
                    mod.Title = dr["Title"].ToString();
                    mod.Content = Utils.ClearHtml(dr["Content"].ToString());
                    list.Add(mod);
                }
            }
            return list;
        }
    }
}
