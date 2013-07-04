using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solr.Log;
using Solr.MODEL;
using Solr.Config;

namespace Solr.DBUtility
{
    public class DalKeyword : BaseSQL
    {
        public int GetCount(int id)
        {
            try
            {
                string sql = string.Format("SELECT COUNT(1) FROM Keywords_Info WHERE Id>{0}",id);
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
                string sql = string.Format("SELECT TOP " + config.TopCount + " Id,Keyword FROM Keywords_Info WHERE Id>{0} ORDER BY Id ASC", id);
                return this.ExecuteDataTable(sql);
            }
            catch (Exception ex)
            {
                Log4Helper.AddError(ex);
                return null;
            }
        }
        public List<ModKeyword> GetList()
        {
            int id = Convert.ToInt32(ConfigSetting.GetValue("LastKeywordId"));
            DataTable dt = this.GetData(id);
            List<ModKeyword> list = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                ModKeyword mod = null;
                list = new List<ModKeyword>();
                foreach (DataRow dr in dt.Rows)
                {
                    mod = new ModKeyword();
                    mod.Id = Convert.ToInt32(dr["Id"]);
                    mod.Keyword = dr["Keyword"].ToString();
                    list.Add(mod);
                }
            }
            return list;
        }
    }
}
