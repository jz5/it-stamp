﻿@Code
    ViewBag.Title = "Q & A"

    Dim qa1 = New Dictionary(Of String, String) From {
        {"IT勉強会スタンプラリーって何？",
         "IT勉強会に参加してスタンプを集めるイベントです。対象のIT勉強会に参加すると台紙にスタンプを押してもらえます。スタンプを集めると参加記念品がもらえます。"},
        {"参加は無料？",
         "無料です。各IT勉強会の参加費などについては、各IT勉強会の案内を参照してください。"},
        {"参加方法は？",
         "対象のIT勉強会に参加すると、スタンプを押す台紙がもらえます。2回目以降は台紙を持参してください。"},
        {"誰でも参加できる？",
         "できます。"},
        {"台紙を忘れたら？",
         "台紙を忘れても続けられます。IT勉強会で新しい台紙とその回のスタンプをもらってください。記念品の受け渡し時、複数の台紙がある場合はスタンプを合算して計算します。台紙のデザインが違う場合、新しい台紙をもらってコレクションしても構いません。"},
        {"台紙をなくしたら？",
         "新しい台紙をもらってください。なくした分のスタンプを押すことはできません。"},
        {"詳しいスタンプラリーのルールは？",
         "<a href=""" & Href("~/Stamprally/2015/") & """>IT勉強会スタンプラリー</a>の「スタンプラリーのルール」を参照してください。"},
        {"記念品の内容や受け取り方法方法は？",
         "<a href=""" & Href("~/Stamprally/2015/") & """>IT勉強会スタンプラリー</a>の「参加記念品」・「記念品の受け取り方法」を参照してください。"},
        {"IT勉強会の多い地域と少ない地域で何か考慮はある？（スタンプを取得する機会が異なる）",
         "ありません。"},
        {"台紙の代わりに「IT勉強会スタンプ」のチェックインや、スマートフォンのアプリは使わないの？",
         "今回は（前回も）、実際の紙で集める楽しさと誰でも参加できることを重視しました。次回の開催は、Web利用を考えています（開催は未定です）。"}
    }

    Dim qa2 = New Dictionary(Of String, String) From {
        {"コミュニティの参加は無料？",
         "無料です。スタンプまたはシール（オリジナル・既製品問わず）は、用意してください。"},
        {"対象のIT勉強会は？",
         "広く一般に募集している勉強会が対象です。内容やテーマ、勉強会のスタイル（セミナー形式・ディスカッション形式・ハッカソンなど）は限定しません。"},
        {"コミュニティの参加条件は？",
         "スタンプラリー運営に関わる作業を実施・協力していただけることが条件です。<a href=""" & Href("~/Stamprally/2015/Join") & """>コミュニティ参加</a>をご確認ください。"}
    }

End Code

<div class="row">
    <div class="col-sm-12 col-md-8">
        <a href="@Href("~/Stamprally/2015")"><img src="@Href("~/images/stamprally2015/stamprally-logo.png")" alt="" class="img-responsive" /></a>

        <h1>@ViewBag.Title</h1>

        <h2>一般参加者向け</h2>
        @For Each i In qa1
            @<p>🇶 @i.Key</p>
            @<p style="margin-bottom:30px;">🇦 @Html.Raw(i.Value)</p>
        Next

        <h2>コミュニティ向け</h2>
        @For Each i In qa2
            @<p>🇶 @i.Key</p>
            @<p style="margin-bottom:30px;">🇦 @Html.Raw(i.Value)</p>
        Next

        @Html.Partial("_SocialButtons")
    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>
