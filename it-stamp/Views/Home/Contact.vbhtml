@Code
    ViewBag.Title = "お問い合わせ"
End Code

<div class="row">
    <div class="col-sm-12 col-md-8">

        <h1>@ViewBag.Title</h1>

        <h2>メール</h2>
        <p>Webサイトに関する内容は <mark>pronama-store+itstamp@@outlook.jp</mark> までお問い合わせください。</p>

        <h2>サイト管理者の情報</h2>
        <ul>
            <li>@@jz5： IT・開発系コミュニティ <a href="http://pronama.jp/">プログラミング生放送</a> 代表</li>
            <li><a href="https://pronama.stores.jp/#!/tokushoho">プロ生ストア</a></li>
        </ul>

        <h2>コミュニティ管理者の申請</h2>
        <p>コミュニティ管理者の申請は、メールでご連絡ください。原則、コミュニティのWebサイトのドメインのメールまたは記載されている連絡先からご連絡お願いします。</p>


    </div>
    <div class="col-sm-12 col-md-4">
        @Html.Partial("_SidebarPartial")
    </div>
</div>
