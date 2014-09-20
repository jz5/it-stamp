Imports System.Web.Optimization

Public Module BundleConfig
    ' バンドルの詳細については、http://go.microsoft.com/fwlink/?LinkId=301862 を参照してください
    Public Sub RegisterBundles(ByVal bundles As BundleCollection)

        bundles.Add(New ScriptBundle("~/bundles/jquery").Include(
                    "~/Scripts/jquery-{version}.js"))

        bundles.Add(New ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.validate*",
                    "~/Scripts/jquery.unobtrusive-ajax*"))

        ' 開発と学習には、Modernizr の開発バージョンを使用します。次に、実稼働の準備が
        ' できたら、http://modernizr.com にあるビルド ツールを使用して、必要なテストのみを選択します。
        bundles.Add(New ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"))

        bundles.Add(New ScriptBundle("~/bundles/script").Include(
                  "~/Scripts/icheck.min.js",
                  "~/Scripts/jquery.fs.selecter.min.js",
                  "~/Scripts/jquery.fs.stepper.min.js",
                  "~/Scripts/jquery.bxslider.min.js",
                  "~/Scripts/respond.js"))

        bundles.Add(New StyleBundle("~/Content/css").Include(
                  "~/Content/bootflat.css",
                  "~/Content/jquery.bxslider.css",
                  "~/Content/site.css"))

        ' デバッグを行うには EnableOptimizations を false に設定します。詳細については、
        ' http://go.microsoft.com/fwlink/?LinkId=301862 を参照してください
        BundleTable.EnableOptimizations = True
    End Sub
End Module

