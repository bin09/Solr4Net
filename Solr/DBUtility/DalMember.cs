using System;
using System.Collections.Generic;
using System.Data;
using Solr.Log;
using Solr.MODEL;
using Solr.Config;
using Solr.Helper;

namespace Solr.DBUtility
{
    public class DalMember : BaseSQL
    {
        public int GetCount(int id)
        {
            try
            {
                string sql = string.Format("SELECT COUNT(1) FROM Mb_Member WHERE Id>{0}", id);
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
                string sql = string.Format("SELECT TOP " + config.TopCount + " Id,CompanyName,[Profile] FROM Mb_Member WHERE Id>{0} ORDER BY Id ASC", id);
                return this.ExecuteDataTable(sql);
            }
            catch (Exception ex)
            {
                Log4Helper.AddError(ex);
                return null;
            }
        }
        public List<ModMember> GetList()
        {
            int id = Convert.ToInt32(ConfigSetting.GetValue("LastMemberId"));
            DataTable dt = this.GetData(id);
            List<ModMember> list = null;
            if (dt != null && dt.Rows.Count > 0)
            {
                ModMember mod = null;
                list = new List<ModMember>();
                foreach (DataRow dr in dt.Rows)
                {
                    mod = new ModMember();
                    mod.Id = Convert.ToInt32(dr["Id"]);
                    mod.CompanyName = dr["companyName"].ToString();
                    mod.Content = Utils.ClearHtml(dr["Profile"].ToString());
                    list.Add(mod);
                }
            }
            return list;
        }
        public ModMember GetModel(int id)
        {
            try
            {
                string sql = string.Format("SELECT Id,CompanyName,[Profile] FROM Mb_Member WHERE Id={0}",id);
                DataTable dt = this.ExecuteDataTable(sql);
                ModMember mod = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    mod = new ModMember();
                    mod.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                    mod.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                    mod.Content = Utils.ClearHtml(dt.Rows[0]["Content"].ToString());
                }
                return mod;
            }
            catch (Exception ex)
            {
                Log4Helper.AddError(ex);
                return null;
            }
        }
    }
}
