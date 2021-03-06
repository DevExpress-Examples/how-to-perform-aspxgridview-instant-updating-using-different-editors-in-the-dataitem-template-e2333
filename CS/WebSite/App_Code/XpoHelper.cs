using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

/// <summary>
/// Summary description for XpoHelper
/// </summary>
public static class XpoHelper
{
    static XpoHelper()
    {
        CreateDefaultObjects();
    }

    public static Session GetNewSession()
    {
        return new Session(DataLayer);
    }

    public static UnitOfWork GetNewUnitOfWork()
    {
        return new UnitOfWork(DataLayer);
    }

    private readonly static object lockObject = new object();

    static IDataLayer fDataLayer;
    static IDataLayer DataLayer
    {
        get
        {
            if (fDataLayer == null)
            {
                lock (lockObject)
                {
                    fDataLayer = GetDataLayer();
                }
            }
            return fDataLayer;
        }
    }

    private static IDataLayer GetDataLayer()
    {
        XpoDefault.Session = null;

        InMemoryDataStore ds = new InMemoryDataStore();
        XPDictionary dict = new ReflectionDictionary();
        dict.GetDataStoreSchema(typeof(MyObject).Assembly);

        return new ThreadSafeDataLayer(dict, ds);
    }

    static void CreateDefaultObjects()
    {
        using (UnitOfWork uow = GetNewUnitOfWork())
        {
            MyObject obj = new MyObject(uow);
            obj.Title = "Item 1";
            obj.Item = 0;
            obj.SomeDate = DateTime.Now;

            obj = new MyObject(uow);
            obj.Title = "Item 2";
            obj.Item = 1;
            obj.SomeDate = DateTime.Now.AddMonths(1);

            obj = new MyObject(uow);
            obj.Title = "Item 3";
            obj.Item = 2;
            obj.SomeDate = DateTime.Now.AddMonths(2);

            uow.CommitChanges();
        }
    }
}