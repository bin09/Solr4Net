
namespace Solr.Helper
{
    public enum ECore
    {
        ProCore,
        MemberCore,
        KeywordCore,
        BuyCore,
        NewsCore
    }
    public enum EProcessState
    {
        Run,                      //全部开始
        Stop                         //全部停止
    }
    public enum LogType
    {
        DELETE,
        UPDATE
    }
    public enum UserLogType
    {
        proSellsDel=2,                  
        proBestDel=4,
        buyDel=6,
        memberDel=8,
        newsDel=11,
        keywordDel=13,
        proSellsEdit=14,
        proBestEdit=15,
        buyEdit=16,
        memberEdit=17
    }
}
