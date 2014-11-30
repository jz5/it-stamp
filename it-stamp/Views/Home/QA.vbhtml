@Code
    ViewBag.Title = "Q & A"

    Dim qa1 = New Dictionary(Of String, String) From {
        {"""IT勉強会""とは？",
         "IT勉強会スタンプでの""IT勉強会""とは、ITに関するイベントです。"},
        {"""コミュニティ""とは？",
         "IT勉強会スタンプでの""コミュニティ""とは、IT勉強会の主催者です。"},
        {"""チェックイン""とは？",
         "IT勉強会スタンプでは、""IT勉強会に参加を記録すること""を""チェックイン""と呼んでいます。IT勉強会スタンプは、チェックインして、IT勉強会の参加を記録できるWebサービスです。"},
        {"""スタンプ""とは？",
         "チェックインすると、実績として""スタンプ""が手に入ります（実装予定）。コレクションして楽しんでください。"},
        {"""IT勉強会スタンプラリー 2015""とは？",
         "対象のIT勉強会に参加してスタンプを集める無料のオフラインイベントです。対象のIT勉強会は、IT勉強会スタンプのサイトで調べられます。"}
    }

End Code

<div class="row">
    <div class="col-sm-12 col-md-8">
        <h1>@ViewBag.Title</h1>

        @For Each i In qa1
            @<p>🇶 @i.Key</p>
            @<p style="margin-bottom:30px;">🇦 @Html.Raw(i.Value)</p>
        Next

    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>
